using System;

namespace PromoCodeFactory.Core.Domain
{
    /// <summary>
    /// Базовый класс сущностей
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// ИД сущности
        /// </summary>
        public Guid Id { get; set; }
    }
}