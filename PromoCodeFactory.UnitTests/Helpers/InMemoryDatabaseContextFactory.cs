using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using PromoCodeFactory.DataAccess;

namespace PromoCodeFactory.UnitTests.Helpers
{
    public static class InMemoryDatabaseContextFactory
    {
        private static DbContextOptions<DataContext> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
               .AddEntityFrameworkInMemoryDatabase()
               .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase("InMemoryDb", new InMemoryDatabaseRoot())
                    .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        public static DataContext GetDbContext() => new DataContext(CreateNewContextOptions());
    }
}
