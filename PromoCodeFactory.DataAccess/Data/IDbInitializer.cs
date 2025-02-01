namespace PromoCodeFactory.DataAccess.Data
{

    /// <summary>
    /// Инициализация БД - создание и наполнение начальными данными
    /// </summary>
    public interface IDbInitializer
    {
        public void InitializeDb();
    }
}