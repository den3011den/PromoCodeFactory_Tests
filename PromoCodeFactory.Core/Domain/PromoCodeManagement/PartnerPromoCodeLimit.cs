using System;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    /// <summary>
    /// Лимиты парнёров на выдачу промокодов
    /// </summary>
    public class PartnerPromoCodeLimit : BaseEntity
    {

        /// <summary>
        /// ИД партнёра
        /// </summary>
        public Guid PartnerId { get; set; }

        /// <summary>
        /// Партнёр
        /// </summary>
        public virtual Partner Partner { get; set; }

        /// <summary>
        /// Дата создания лимита
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Дата отмены лимита
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// Дата окончания действия лимита
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Лимит
        /// </summary>
        public int Limit { get; set; }
    }
}