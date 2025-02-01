namespace PromoCodeFactory.Core.Domain.Administration
{
    /// <summary>
    /// Роль
    /// </summary>
    public class Role
        : BaseEntity
    {
        /// <summary>
        /// Наименование роли
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание роли
        /// </summary>
        public string Description { get; set; }
    }
}