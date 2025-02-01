using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    /// <summary>
    /// Интерфейс для работы с сущностью Partner (Партнёры)
    /// </summary>
    public interface IPartnerPromoCodeLimitRepository : IRepository<PartnerPromoCodeLimit>
    {
        /// <summary>
        /// Установить Партнёру активный лимит на промокоды
        /// </summary>
        /// <param name="partnerId">ИД партнёра (Guid))</param>
        /// <param name="setPartnerPromoCodeLimitRequest">Данные устанавливаемого лимита (объект типа SetPartnerPromoCodeLimitRequest)</param>
        /// <returns>Возвращает объект добавленого лимита (объект типа PartnerPromoCodeLimit)</returns>
        public Task<PartnerPromoCodeLimit> SetPartnerPromoCodeLimitAsync(Guid partnerId, SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest);

        /// <summary>
        /// Получить список активных на текущий момент лимитов партнёра
        /// </summary>
        /// <param name="partnerId">ИД партнёра (Guid)</param>
        /// <returns>Список активных лимитов партнёра</returns>
        public Task<IEnumerable<PartnerPromoCodeLimit>> GetActivePartnerPromoCodeLimitListAsync(Guid partnerId);

        /// <summary>
        /// Отключить лимит (сделать неактивным) для партнёра
        /// </summary>
        /// <param name="partner">Партнёр - объект Partner</param>
        /// <param name="partnerPromoCodeLimit">Активный в данный момент лимит партнёра - объект PartnerPromoCodeLimit</param>
        /// <returns>Всегда возвращает true если нет исключения. Если не найден партнёр или лимит - выбрасывает исключения </returns>
        public Task<bool> TurnOffPartnerPromoCodeLimitAsync(Partner partner, PartnerPromoCodeLimit partnerPromoCodeLimit);

    }
}
