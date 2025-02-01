using System;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{

    /// <summary>
    /// Запрос на установку нового лимита на выдачу промокодов для партнёра
    /// </summary>
    public class SetPartnerPromoCodeLimitRequest
    {
        /// <summary>
        /// Дата/время окончания лимита
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Лимит
        /// </summary>
        public int Limit { get; set; }
    }
}
