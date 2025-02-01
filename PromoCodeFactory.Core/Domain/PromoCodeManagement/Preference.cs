namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    /// <summary>
    /// Клиентское Предпочтение
    /// </summary>
    public class Preference
        : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
    }
}