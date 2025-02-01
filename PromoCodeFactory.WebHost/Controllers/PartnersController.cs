using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories.Exceptions;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Партнеры
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PartnersController
        : ControllerBase
    {
        private readonly IRepository<Partner> _partnersRepository;
        private readonly IPartnerPromoCodeLimitRepository _partnerPromoCodeLimitRepository;

        public PartnersController(IRepository<Partner> partnersRepository, IPartnerPromoCodeLimitRepository partnerPromoCodeLimitRepository)
        {
            _partnersRepository = partnersRepository;
            _partnerPromoCodeLimitRepository = partnerPromoCodeLimitRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PartnerResponse>>> GetPartnersAsync()
        {
            var partners = await _partnersRepository.GetAllAsync();

            var response = partners.Select(x => new PartnerResponse()
            {
                Id = x.Id,
                Name = x.Name,
                NumberIssuedPromoCodes = x.NumberIssuedPromoCodes,
                IsActive = x.IsActive,
                PartnerLimits = x.PartnerLimits
                    .Select(y => new PartnerPromoCodeLimitResponse()
                    {
                        Id = y.Id,
                        PartnerId = y.PartnerId,
                        Limit = y.Limit,
                        CreateDate = y.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        EndDate = y.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                        CancelDate = y.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
                    }).ToList()
            });

            return Ok(response);
        }

        [HttpGet("{id}/limits/{limitId}")]
        public async Task<ActionResult<PartnerPromoCodeLimit>> GetPartnerLimitAsync(Guid id, Guid limitId)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);

            if (partner == null)
                return NotFound();

            var limit = partner.PartnerLimits
                .FirstOrDefault(x => x.Id == limitId);

            var response = new PartnerPromoCodeLimitResponse()
            {
                Id = limit.Id,
                PartnerId = limit.PartnerId,
                Limit = limit.Limit,
                CreateDate = limit.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                EndDate = limit.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                CancelDate = limit.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
            };

            return Ok(response);
        }


        /// <summary>
        /// Добавление нового действующего лимита партнёру
        /// </summary>
        /// <param name="id">ИД партнёра</param>
        /// <param name="request">Добавляемый лимит - объект SetPartnerPromoCodeLimitRequest</param>
        /// <returns></returns>
        /// <response code="201">Успешное выполнение. Лимит добавлен партнёру</response>
        /// <response code="400">Проблема при добавлении лимита. Смотрите пояснение к ответу. Лимит не добавлен партнёру</response>  
        /// <response code="404">Партнёр с указанным ИД не найден</response>  
        [HttpPost("{id}")]
        public async Task<IActionResult> SetPartnerPromoCodeLimitAsync(Guid id, SetPartnerPromoCodeLimitRequest request)
        {

            PartnerPromoCodeLimit newPartnerPromoCodeLimit = null;

            try
            {
                newPartnerPromoCodeLimit = await _partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(id, request);
            }
            catch (PartnerPromoCodeLimitLessOrEqualZeroException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PartnerPromoCodeLimitNotActiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PartnerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (PartnerNotActiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PartnerHasMoreThenOneActivePartnerPromoCodeLimitException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var routVar = "";
            if (Request != null)
            {
                routVar = new UriBuilder(Request.Scheme, Request.Host.Host, (int)Request.Host.Port, Request.Path.Value).ToString()
                    + "/limits/" + newPartnerPromoCodeLimit.Id.ToString();
            }

            return Created(routVar, new PartnerPromoCodeLimitResponse
            {
                Id = newPartnerPromoCodeLimit.Id,
                PartnerId = newPartnerPromoCodeLimit.PartnerId,
                CreateDate = newPartnerPromoCodeLimit.CreateDate.ToString(),
                CancelDate = newPartnerPromoCodeLimit.CancelDate.ToString(),
                EndDate = newPartnerPromoCodeLimit.EndDate.ToString(),
                Limit = newPartnerPromoCodeLimit.Limit
            });

        }


        [HttpPost("canceledLimits/{id}")]
        public async Task<IActionResult> CancelPartnerPromoCodeLimitAsync(Guid id)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);

            if (partner == null)
                return NotFound();

            //Если партнер заблокирован, то нужно выдать исключение
            if (!partner.IsActive)
                return BadRequest("Данный партнер не активен");

            //Отключение лимита
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x =>
                !x.CancelDate.HasValue);

            if (activeLimit != null)
            {
                activeLimit.CancelDate = DateTime.Now;
            }

            await _partnersRepository.UpdateAsync(partner);

            return NoContent();
        }
    }
}