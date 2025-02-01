using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    /// <summary>
    /// Партнёр
    /// </summary>
    public class Partner
        : BaseEntity
    {
        /// <summary>
        /// Нименование партнёра
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество выпущенных промокодов
        /// </summary>
        public int NumberIssuedPromoCodes { get; set; }

        /// <summary>
        /// Признак активности партнёра (true - активен, false - не активен)
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Список лимитов партнёра на выдачу промокодов
        /// </summary>
        public virtual ICollection<PartnerPromoCodeLimit> PartnerLimits { get; set; }
    }
}