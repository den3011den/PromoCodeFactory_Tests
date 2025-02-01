using System;

namespace PromoCodeFactory.Core.Domain.Administration
{

    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employee
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
        /// ИД роли
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Количество применённых сотрудником кодов
        /// </summary>
        public int AppliedPromocodesCount { get; set; }
    }
}