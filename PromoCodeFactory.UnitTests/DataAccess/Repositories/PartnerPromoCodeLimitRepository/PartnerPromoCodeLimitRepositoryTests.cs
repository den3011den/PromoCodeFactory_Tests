using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PromoCodeFactory.UnitTests.DataAccess.Repositories.PartnerPromoCodeLimitRepository
{
    //public class GetActivePartnerPromoCodeLimitListAsync_SelectFromPromoCodeLimit_ReturnsListWithMoreThenOneActivePartnerPromoCodeLimit
    public class PartnerPromoCodeLimitRepositoryTests : IDisposable
    {

        private readonly DataContext _db;

        public PartnerPromoCodeLimitRepositoryTests()
        {
            _db = InMemoryDatabaseContextFactory.GetDbContext();
        }

        [Fact]
        public void GetActivePartnerPromoCodeLimitListAsync_SelectFromPromoCodeLimit_ReturnsListWithTwoActivePartnerPromoCodeLimit()
        {

            var partnerRepositoryMock = new Mock<IRepository<Partner>>();

            var partnerId1 = Guid.NewGuid();
            FillPartnerPromoCodeLimitData.FillPartnerPromoCodeLimitListWithTwoActiveLimitsForPartnerId(partnerId1, _db);

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);
            var partnerPromoCodeLimitList = partnerPromoCodeLimitRepository.GetActivePartnerPromoCodeLimitListAsync(partnerId1).Result;

            Assert.NotNull(partnerPromoCodeLimitList);
            Assert.IsAssignableFrom<IEnumerable<PartnerPromoCodeLimit>>(partnerPromoCodeLimitList);
            Assert.Equal(2, partnerPromoCodeLimitList.Count());

        }
        public void Dispose()
        {
        }
    }
}
