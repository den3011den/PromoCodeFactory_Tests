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
    /// <summary>
    /// Класс тестирования методов класса-репозитория для работы с лимитами
    /// </summary>
    public class PartnerPromoCodeLimitRepositoryTests : IDisposable
    {

        private readonly DataContext _db;

        public PartnerPromoCodeLimitRepositoryTests()
        {
            // БД в памяти
            _db = InMemoryDatabaseContextFactory.GetDbContext();
        }

        /// <summary>
        /// Выборка активных лимитов партнёра с получением сразу двух действующих на данных момент лимитов
        /// </summary>
        [Fact]
        public void GetActivePartnerPromoCodeLimitListAsync_SelectActivePromoCodeLimit_ReturnsListWithTwoActivePartnerPromoCodeLimit()
        {

            //Arrange
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();

            var partnerId1 = Guid.NewGuid();
            FillPartnerPromoCodeLimitData.FillPartnerPromoCodeLimitListWithTwoActiveLimitsForPartnerId(partnerId1, _db);

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);

            // Act
            var partnerPromoCodeLimitList = partnerPromoCodeLimitRepository.GetActivePartnerPromoCodeLimitListAsync(partnerId1).Result;

            // Assert
            Assert.NotNull(partnerPromoCodeLimitList);
            Assert.IsAssignableFrom<IEnumerable<PartnerPromoCodeLimit>>(partnerPromoCodeLimitList);
            Assert.Equal(2, partnerPromoCodeLimitList.Count());

        }

        /// <summary>
        /// Выборка активных лимитов партнёра с получением одного действующего на данных момент лимита
        /// </summary>

        [Fact]
        public void GetActivePartnerPromoCodeLimitListAsync_SelectActivePromoCodeLimit_ReturnsListWithOneActivePartnerPromoCodeLimit()
        {

            //Arrange
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

            // Act
            var partnerPromoCodeLimitList = partnerPromoCodeLimitRepository.GetActivePartnerPromoCodeLimitListAsync(partnerPromoCodeLimitToAdd.PartnerId).Result;

            // Assert
            Assert.NotNull(partnerPromoCodeLimitList);
            Assert.IsAssignableFrom<IEnumerable<PartnerPromoCodeLimit>>(partnerPromoCodeLimitList);
            Assert.Single(partnerPromoCodeLimitList);

            var expectedPartnerPromoCodeLimitStr = JsonConvert.SerializeObject(partnerPromoCodeLimitToCompare);
            var actualPartnerPromoCodeLimitStr = JsonConvert.SerializeObject(partnerPromoCodeLimitList.FirstOrDefault());
            Assert.Equal(expectedPartnerPromoCodeLimitStr, actualPartnerPromoCodeLimitStr);

        }

        /// <summary>
        /// Выборка активных лимитов партнёра с получением пустого списка (ноль элементов) действующих на данных момент лимитов
        /// </summary>
        [Fact]
        public void GetActivePartnerPromoCodeLimitListAsync_SelectActivePromoCodeLimit_ReturnsEmptyList()
        {

            //Arrange
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

            // Act
            var partnerPromoCodeLimitList = partnerPromoCodeLimitRepository.GetActivePartnerPromoCodeLimitListAsync(partnerPromoCodeLimitToAdd.PartnerId).Result;

            // Assert
            Assert.IsAssignableFrom<IEnumerable<PartnerPromoCodeLimit>>(partnerPromoCodeLimitList);
            Assert.Empty(partnerPromoCodeLimitList);
        }


        /// <summary>
        /// Лимит партнёра был успешно сделан неактивным
        /// </summary>
        [Fact]
        public void TurnOffPartnerPromoCodeLimitAsync_TurnOffPartnerPromoCodeLimit_PartnerPromocodeLimitHasBeenTurnedOff()
        {
            // Arrange
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

            // Act
            var tempVar = partnerPromoCodeLimitRepository.TurnOffPartnerPromoCodeLimitAsync(gotPartnerFromDb, gotLimitFromDb);

            // Assert
            var gotLimitFromDbAfterAction = _db.PartnerPromoCodeLimit.FirstOrDefault(u => u.Id == partnerPromoCodeLimitId);
            Assert.NotNull(gotLimitFromDbAfterAction.CancelDate);
        }

        /// <summary>
        /// Количество выданных промокодов у партнёра было успешно обнулено
        /// </summary>
        [Fact]
        public void TurnOffPartnerPromoCodeLimitAsync_SetPartnerNumberIssuedPromoCodesToZero_PartnerNumberIssuedPromoCodesEqualZero()
        {
            // Assert
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

            // Act
            var tempVar = partnerPromoCodeLimitRepository.TurnOffPartnerPromoCodeLimitAsync(gotPartnerFromDb, gotLimitFromDb);

            // Assert
            var gotPartnerFromDbAfterAction = partnerRepository.GetByIdAsync(partnerId).Result;
            Assert.Equal(0, gotPartnerFromDbAfterAction.NumberIssuedPromoCodes);
        }


        /// <summary>
        /// При попытке отключения активного лимита партнёра не удалось найти партнёра
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TurnOffPartnerPromoCodeLimitAsync_PartnerNotFound_ThrowsPartnerNotFoundException()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var partnerPromoCodeLimitId = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;
            var partner = new Partner
            {
                Id = partnerId,
                Name = "SomeName",
                NumberIssuedPromoCodes = 100,
                IsActive = true
            };

            var partnerPromoCodeLimit = new PartnerPromoCodeLimit
            {
                Id = partnerPromoCodeLimitId,
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 1000
            };

            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            partnerRepositoryMock.Setup(u => u.GetByIdAsync(partnerId)).ReturnsAsync(() => null);

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<PartnerNotFoundException>(async () => await partnerPromoCodeLimitRepository.TurnOffPartnerPromoCodeLimitAsync(partner, partnerPromoCodeLimit));
        }


        /// <summary>
        /// При попытке отключения активного лимита партнёра не удалось найти отключаемый лимит
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TurnOffPartnerPromoCodeLimitAsync_PartnerPromoCodeLimitNotFound_ThrowsPartnerPromoCodeLimitNotFoundException()
        {

            // Arrange
            var partnerId = Guid.NewGuid();
            var partnerPromoCodeLimitId = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;
            var partner = new Partner
            {
                Id = partnerId,
                Name = "SomeName",
                NumberIssuedPromoCodes = 100,
                IsActive = true
            };

            var partnerPromoCodeLimit = new PartnerPromoCodeLimit
            {
                Id = partnerPromoCodeLimitId,
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 1000
            };

            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            partnerRepositoryMock.Setup(u => u.GetByIdAsync(partnerId)).ReturnsAsync(() => partner);

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db, partnerRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<PartnerPromoCodeLimitNotFoundException>(async () => await partnerPromoCodeLimitRepository.TurnOffPartnerPromoCodeLimitAsync(partner, partnerPromoCodeLimit));
        }


        /// <summary>
        /// Проверка попытки добавить патрнёру лимит с нудевым или отрицательным значением лимита
        /// </summary>
        /// <param name="limit">Значение лимита</param>
        /// <returns></returns>
        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        public async Task SetPartnerPromoCodeLimitAsync_PropertyLimitLessOrEqualZero_ThrowsPartnerPromoCodeLimitLessOrEqualZeroException(int limit)
        {

            // Arrange
            var _db = new Mock<DataContext>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerId = Guid.NewGuid();
            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now.AddDays(30),
                Limit = limit
            };

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db.Object, partnerRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<PartnerPromoCodeLimitLessOrEqualZeroException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }


        /// <summary>
        /// Попытка установить партнёру лимит с датой окончания меньше текущих даты/времени
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PropertyEndDateLessThenActualDatetime_ThrowsPartnerPromoCodeLimitNotActiveException()
        {

            // Arrange
            var _db = new Mock<DataContext>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerId = Guid.NewGuid();
            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = DateTime.Now.AddDays(-1),
                Limit = 100
            };

            var partnerPromoCodeLimitRepository = new PromoCodeFactory.DataAccess.Repositories.PartnerPromoCodeLimitRepository(_db.Object, partnerRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<PartnerPromoCodeLimitNotActiveException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }

        /// <summary>
        /// Попытка установить активный лимит партнёру, которого не удаётся найти в БД
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerNotFound_ThrowsPartnerNotFoundException()
        {

            // Arrange
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

            // Act & Assert
            await Assert.ThrowsAsync<PartnerNotFoundException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }


        /// <summary>
        /// Попытка установить активный лимит не активному партнёру
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerNotActive_ThrowsPartnerNotActiveException()
        {

            // Arrange
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

            // Act & Assert
            await Assert.ThrowsAsync<PartnerNotActiveException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }


        /// <summary>
        /// Попытка установить активный лимит когда у партнёра уже есть 2 активных лимита (должен быть в норме максимум один активный лимит)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerHasMoreThenOneActivePromocodeLimit_ThrowsPartnerHasMoreThenOneActivePartnerPromoCodeLimitException()
        {
            // Arrange
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

            // Act & Assert
            await Assert.ThrowsAsync<PartnerHasMoreThenOneActivePartnerPromoCodeLimitException>(async () => await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest));
        }


        /// <summary>
        /// Проверка успешного добавления в БД активного лимита партнёру 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_AddToDbPartnerPromocodeLimit_PartnerPromocodeLimitAddedToDb()
        {

            // Arrange
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

            // Act
            var addedLimit = await partnerPromoCodeLimitRepository.SetPartnerPromoCodeLimitAsync(partnerId, setPartnerPromoCodeLimitRequest);


            // Assert
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
