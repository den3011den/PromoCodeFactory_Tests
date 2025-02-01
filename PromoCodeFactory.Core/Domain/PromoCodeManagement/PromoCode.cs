using PromoCodeFactory.Core.Domain.Administration;
using System;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    /// <summary>
    /// Прмокод
    /// </summary>
    public class PromoCode
        : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Сервисная информация
        /// </summary>
        public string ServiceInfo { get; set; }

        /// <summary>
        /// Дата/время начала действия промокода
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Дата/время окончания действия промокода
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Наименование партнёра
        /// </summary>
        public string PartnerName { get; set; }

        /// <summary>
        /// Менеджер партнёра
        /// </summary>
        public virtual Employee PartnerManager { get; set; }

        /// <summary>
        /// Предпочтение
        /// </summary>
        public virtual Preference Preference { get; set; }
    }
}