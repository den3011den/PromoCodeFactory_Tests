using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{

    /// <summary>
    /// Клиент
    /// </summary>
    public class Customer
        : BaseEntity
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Полное имя
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Список предпочтений клиента
        /// </summary>
        public virtual ICollection<CustomerPreference> Preferences { get; set; }
    }
}