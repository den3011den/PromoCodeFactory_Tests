using Moq;
using Newtonsoft.Json;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.DataAccess.Repositories.Exceptions;
using PromoCodeFactory.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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



        [Fact]
        public void TurnOffPartnerPromoCodeLimitAsync_TurnOffPartnerPromoCodeLimit_PartnerPromocodeLimitHasBeenTurnedOff()
        {
            var partnerId = Guid.NewGuid();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            partnerRepositoryMock.Setup(u => u.GetByIdAsync(partnerId)).ReturnsAsync(() =>
            new Partner
            {
                Id = partnerId,
                Name = "SomeName",
                NumberIssuedPromoCodes = 100,
                IsActive = true
            });

            var partnerPromoCodeLimitId = Guid.NewGuid();
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

            FillPartnerPromoCodeLimitData.FillPartnerPromoCodeLimitListWithOneActiveLimit(partnerPromoCodeLimitToAdd, _db);

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);

            var gotLimitFromDb = _db.PartnerPromoCodeLimit.FirstOrDefault(u => u.Id == partnerPromoCodeLimitId);
            var gotPartnerFromDb = partnerRepositoryMock.Object.GetByIdAsync(partnerId).Result;

            var tempVar = partnerPromoCodeLimitRepository.TurnOffPartnerPromoCodeLimitAsync(gotPartnerFromDb, gotLimitFromDb);

            var gotLimitFromDbAfterAction = _db.PartnerPromoCodeLimit.FirstOrDefault(u => u.Id == partnerPromoCodeLimitId);

            Assert.NotNull(gotLimitFromDbAfterAction.CancelDate);
        }


        [Fact]
        public void TurnOffPartnerPromoCodeLimitAsync_SetPartnerNumberIssuedPromoCodesToZero_PartnerNumberIssuedPromoCodesEqualZero()
        {
            var partnerId = Guid.NewGuid();
            var partnerRepository = new PromoCodeFactory.DataAccess.Repositories.EfRepository<Partner>(_db);
            partnerRepository.AddAsync(new Partner
            {
                Id = partnerId,
                Name = "SomeName",
                NumberIssuedPromoCodes = 100,
                IsActive = true
            }).GetAwaiter();

            var partnerPromoCodeLimitId = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepository);
            partnerPromoCodeLimitRepository.AddAsync(
             new PartnerPromoCodeLimit
             {
                 Id = partnerPromoCodeLimitId,
                 PartnerId = partnerId,
                 CreateDate = dateTimeNow,
                 CancelDate = null,
                 EndDate = dateTimeNow.AddDays(30),
                 Limit = 100
             }).GetAwaiter();


            var gotLimitFromDb = partnerPromoCodeLimitRepository.GetByIdAsync(partnerPromoCodeLimitId).Result;
            var gotPartnerFromDb = partnerRepository.GetByIdAsync(partnerId).Result;

            var tempVar = partnerPromoCodeLimitRepository.TurnOffPartnerPromoCodeLimitAsync(gotPartnerFromDb, gotLimitFromDb);

            var gotPartnerFromDbAfterAction = partnerRepository.GetByIdAsync(partnerId).Result;

            Assert.Equal(0, gotPartnerFromDbAfterAction.NumberIssuedPromoCodes);
        }


        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        public async Task SetPartnerPromoCodeLimitAsync_PropertyLimitLessOrEqualZero_ThrowsPartnerPromoCodeLimitLessOrEqualZeroException(int limit)
        {

            var _db = new Mock<DataContext>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerId = Guid.NewGuid();
            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now.AddDays(30),
                Limit = limit
            };

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db.Object, partnerRepositoryMock.Object);
            await Assert.ThrowsAsync<PartnerPromoCodeLimitLessOrEqualZeroException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PropertyEndDateLessLessThenActualDatetime_ThrowsPartnerPromoCodeLimitNotActiveException()
        {

            var _db = new Mock<DataContext>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerId = Guid.NewGuid();
            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now.AddDays(-1),
                Limit = 100
            };

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db.Object, partnerRepositoryMock.Object);
            await Assert.ThrowsAsync<PartnerPromoCodeLimitNotActiveException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }


        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerNotFound_ThrowsPartnerNotFoundException()
        {

            var _db = new Mock<DataContext>();
            var partnerId = Guid.NewGuid();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            partnerRepositoryMock.Setup(u => u.GetByIdAsync(partnerId)).ReturnsAsync(() => null);

            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now.AddDays(30),
                Limit = 100
            };

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db.Object, partnerRepositoryMock.Object);
            await Assert.ThrowsAsync<PartnerNotFoundException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerNotActive_ThrowsPartnerNotActiveException()
        {

            var _db = new Mock<DataContext>();
            var partnerId = Guid.NewGuid();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            partnerRepositoryMock.Setup(u => u.GetByIdAsync(partnerId)).ReturnsAsync(() =>
            new Partner
            {
                Id = partnerId,
                Name = "SomeName",
                NumberIssuedPromoCodes = 100,
                IsActive = false
            });

            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now.AddDays(30),
                Limit = 100
            };

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db.Object, partnerRepositoryMock.Object);
            await Assert.ThrowsAsync<PartnerNotActiveException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }


        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerHasMoreThenOneActivePromocodeLimit_ThrowsPartnerHasMoreThenOneActivePartnerPromoCodeLimitException()
        {
            var partnerId = Guid.NewGuid();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            partnerRepositoryMock.Setup(u => u.GetByIdAsync(partnerId)).ReturnsAsync(() =>
            new Partner
            {
                Id = partnerId,
                Name = "SomeName",
                NumberIssuedPromoCodes = 100,
                IsActive = true
            });

            FillPartnerPromoCodeLimitData.FillPartnerPromoCodeLimitListWithTwoActiveLimitsForPartnerId(partnerId, _db);

            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now.AddDays(30),
                Limit = 100
            };

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);
            await Assert.ThrowsAsync<PartnerHasMoreThenOneActivePartnerPromoCodeLimitException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_AddToDbPartnerPromocodeLimit_PartnerPromocodeLimitAddedToDb()
        {
            var partnerId = Guid.NewGuid();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            partnerRepositoryMock.Setup(u => u.GetByIdAsync(partnerId)).ReturnsAsync(() =>
            new Partner
            {
                Id = partnerId,
                Name = "SomeName",
                NumberIssuedPromoCodes = 100,
                IsActive = true
            });

            var partnerPromoCodeLimitId = Guid.NewGuid();
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

            FillPartnerPromoCodeLimitData.FillPartnerPromoCodeLimitListWithOneActiveLimit(partnerPromoCodeLimitToAdd, _db);

            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now.AddDays(30),
                Limit = 100
            };

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);

            var addedLimit = await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest);


            var gotFromDBLimit = _db.PartnerPromoCodeLimit.FirstOrDefault(u => u.Id == addedLimit.Id);
            var expectedPartnerPromoCodeLimitStr = JsonConvert.SerializeObject(new
            {
                addedLimit.Id,
                addedLimit.PartnerId,
                addedLimit.CreateDate,
                addedLimit.CancelDate,
                addedLimit.EndDate,
                addedLimit.Limit,
            });
            var actualPartnerPromoCodeLimitStr = JsonConvert.SerializeObject(
                new
                {
                    gotFromDBLimit.Id,
                    gotFromDBLimit.PartnerId,
                    gotFromDBLimit.CreateDate,
                    gotFromDBLimit.CancelDate,
                    gotFromDBLimit.EndDate,
                    gotFromDBLimit.Limit,
                });
            Assert.Equal(expectedPartnerPromoCodeLimitStr, actualPartnerPromoCodeLimitStr);

            Assert.Equal(setPartnerPromoCodeLimitRequest.EndDate, gotFromDBLimit.EndDate);
            Assert.Equal(setPartnerPromoCodeLimitRequest.Limit, gotFromDBLimit.Limit);


        }


        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
