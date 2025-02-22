﻿using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class PartnerPromoCodeLimitRepository : EfRepository<PartnerPromoCodeLimit>, IPartnerPromoCodeLimitRepository
    {

        private readonly IRepository<Partner> _partnerRepository;
        private readonly DataContext _db;
        public PartnerPromoCodeLimitRepository(DataContext db, IRepository<Partner> partnerRepository) : base(db)
        {
            _partnerRepository = partnerRepository;
            _db = db;
        }

        public async Task<IEnumerable<PartnerPromoCodeLimit>> GetActivePartnerPromoCodeLimitListAsync(Guid partnerId)
        {
            var retVar = _db.PartnerPromoCodeLimit.Where(x => x.PartnerId == partnerId && !x.CancelDate.HasValue && x.EndDate >= DateTime.Now).ToList();
            return retVar;

        }

        public async Task<PartnerPromoCodeLimit> SetPartnerPromoCodeLimitAsync(Guid partnerId, SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest)
        {
            if (setPartnerPromoCodeLimitRequest.Limit <= 0)
                throw new PartnerPromoCodeLimitLessOrEqualZeroException(setPartnerPromoCodeLimitRequest);
            if (setPartnerPromoCodeLimitRequest.EndDate < DateTime.Now)
                throw new PartnerPromoCodeLimitNotActiveException(setPartnerPromoCodeLimitRequest);

            var partner = await _partnerRepository.GetByIdAsync(partnerId);

            if (partner == null)
                throw new PartnerNotFoundException(partnerId);

            if (!partner.IsActive)
                throw new PartnerNotActiveException(partner);

            var partnerActivePromoCodeLimitList = await GetActivePartnerPromoCodeLimitListAsync(partnerId);

            if (partnerActivePromoCodeLimitList != null)
            {
                var partnerActivePromoCodeLimitListCount = partnerActivePromoCodeLimitList.Count();
                if (partnerActivePromoCodeLimitListCount > 1)
                {
                    throw new PartnerHasMoreThenOneActivePartnerPromoCodeLimitException(partnerActivePromoCodeLimitList);
                }
                if (partnerActivePromoCodeLimitListCount == 1)
                {
                    var activeLimit = partnerActivePromoCodeLimitList.FirstOrDefault();
                    if (activeLimit != null)
                    {
                        await TurnOffPartnerPromoCodeLimitAsync(partner, activeLimit);

                    }
                }
            }

            var newLimit = new PartnerPromoCodeLimit()
            {
                Id = Guid.NewGuid(),
                Limit = setPartnerPromoCodeLimitRequest.Limit,
                Partner = partner,
                PartnerId = partner.Id,
                CreateDate = DateTime.Now,
                EndDate = setPartnerPromoCodeLimitRequest.EndDate
            };
            await AddAsync(newLimit);

            return newLimit;

        }

        public async Task<bool> TurnOffPartnerPromoCodeLimitAsync(Partner partner, PartnerPromoCodeLimit partnerPromoCodeLimit)
        {
            var partnerFound = await _partnerRepository.GetByIdAsync(partner.Id);
            if (partnerFound == null)
                throw new PartnerNotFoundException(partner.Id);

            var partnerPromoCodeLimitFound = await GetByIdAsync(partnerPromoCodeLimit.Id);
            if (partnerPromoCodeLimitFound == null)
                throw new PartnerPromoCodeLimitNotFoundException(partnerPromoCodeLimit.Id);

            partner.NumberIssuedPromoCodes = 0;
            partnerPromoCodeLimitFound.CancelDate = DateTime.Now;
            await _partnerRepository.UpdateAsync(partner);
            await UpdateAsync(partnerPromoCodeLimitFound);
            return true;
        }
    }
}
