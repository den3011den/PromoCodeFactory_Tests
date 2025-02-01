using System;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{

    /// <summary>
    /// Связка клиента и его предпочтений
    /// </summary>
    public class CustomerPreference
    {

        /// <summary>
        /// ИД клиента
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// ИД предпочтения
        /// </summary>
        public Guid PreferenceId { get; set; }

        /// <summary>
        /// Предпочтение
        /// </summary>
        public virtual Preference Preference { get; set; }
    }
}