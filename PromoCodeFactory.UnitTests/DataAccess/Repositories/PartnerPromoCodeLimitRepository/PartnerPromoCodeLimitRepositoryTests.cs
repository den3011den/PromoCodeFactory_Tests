using Moq;
using Newtonsoft.Json;
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
        public void GetActivePartnerPromoCodeLimitListAsync_SelectActivePromoCodeLimit_ReturnsListWithTwoActivePartnerPromoCodeLimit()
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

        [Fact]
        public void GetActivePartnerPromoCodeLimitListAsync_SelectActivePromoCodeLimit_ReturnsListWithOneActivePartnerPromoCodeLimit()
        {

            var partnerRepositoryMock = new Mock<IRepository<Partner>>();

            var partnerPromoCodeLimitId = Guid.NewGuid();
            var partnerId = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;

            PartnerPromoCodeLimit partnerPromoCodeLimitToAdd = new PartnerPromoCodeLimit
            {
                Id = partnerPromoCodeLimitId,
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            };

            PartnerPromoCodeLimit partnerPromoCodeLimitToCompare = new PartnerPromoCodeLimit
            {
                Id = partnerPromoCodeLimitId,
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            };

            FillPartnerPromoCodeLimitData.FillPartnerPromoCodeLimitListWithOneActiveLimit(partnerPromoCodeLimitToAdd, _db);

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);
            var partnerPromoCodeLimitList = partnerPromoCodeLimitRepository.GetActivePartnerPromoCodeLimitListAsync(partnerPromoCodeLimitToAdd.PartnerId).Result;

            Assert.NotNull(partnerPromoCodeLimitList);
            Assert.IsAssignableFrom<IEnumerable<PartnerPromoCodeLimit>>(partnerPromoCodeLimitList);
            Assert.Single(partnerPromoCodeLimitList);

            //partnerPromoCodeLimitToCompare.Limit = 101;

            var expectedPartnerPromoCodeLimitStr = JsonConvert.SerializeObject(partnerPromoCodeLimitToCompare);
            var actualPartnerPromoCodeLimitStr = JsonConvert.SerializeObject(partnerPromoCodeLimitList.FirstOrDefault());
            Assert.Equal(expectedPartnerPromoCodeLimitStr, actualPartnerPromoCodeLimitStr);

        }


        [Fact]
        public void GetActivePartnerPromoCodeLimitListAsync_SelectActivePromoCodeLimit_ReturnsEmptyList()
        {

            var partnerRepositoryMock = new Mock<IRepository<Partner>>();

            var partnerPromoCodeLimitId = Guid.NewGuid();
            var partnerId = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;

            PartnerPromoCodeLimit partnerPromoCodeLimitToAdd = new PartnerPromoCodeLimit
            {
                Id = partnerPromoCodeLimitId,
                PartnerId = partnerId,
                CreateDate = dateTimeNow.AddDays(-30),
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(-20),
                Limit = 100
            };

            FillPartnerPromoCodeLimitData.FillPartnerPromoCodeLimitListWithOneActiveLimit(partnerPromoCodeLimitToAdd, _db);

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);
            var partnerPromoCodeLimitList = partnerPromoCodeLimitRepository.GetActivePartnerPromoCodeLimitListAsync(partnerPromoCodeLimitToAdd.PartnerId).Result;

            Assert.IsAssignableFrom<IEnumerable<PartnerPromoCodeLimit>>(partnerPromoCodeLimitList);
            Assert.Empty(partnerPromoCodeLimitList);
        }


        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
