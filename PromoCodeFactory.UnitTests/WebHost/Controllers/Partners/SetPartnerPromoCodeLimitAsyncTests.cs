using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.UnitTests.Helpers;
using PromoCodeFactory.WebHost.Controllers;
using System;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{

    /// <summary>
    /// Класс с тестами метода SetPartnerPromoCodeLimitAsync класса PartnersController
    /// </summary>
    public class SetPartnerPromoCodeLimitAsyncTests : IDisposable
    {

        /// <summary>
        /// Проверка случаев, когда должен вернуться BadRequest
        /// </summary>
        /// <param name="exception">Объект исключения</param>
        [Theory]
        [ClassData(typeof(ClassDataForBadRequestExeptions))]
        public void SetPartnerPromoCodeLimitAsync_ThrownExceptionsForBadRequest_ReturnsBadRequest(Exception exception)
        {
            //Arrange
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerPromoCodeLimitRepositoryMock = new Mock<IPartnerPromoCodeLimitRepository>();

            partnerPromoCodeLimitRepositoryMock.Setup(u =>
                u.SetPartnerPromoCodeLimitAsync(It.IsAny<Guid>(), It.IsAny<SetPartnerPromoCodeLimitRequest>()).Result)
                .Throws(exception);

            PartnersController partnersController = new PartnersController(partnerRepositoryMock.Object, partnerPromoCodeLimitRepositoryMock.Object);

            // Act
            var result = partnersController.SetPartnerPromoCodeLimitAsync(Guid.NewGuid(), new SetPartnerPromoCodeLimitRequest()).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        /// <summary>
        /// Проверка случаев, когда должен вернуться NotFound
        /// </summary>
        /// <param name="exception">Объект исключения</param>
        [Theory]
        [ClassData(typeof(ClassDataForNotFoundRequestExeptions))]
        public void SetPartnerPromoCodeLimitAsync_ThrownExceptionsForNotFoundRequest_ReturnsNotFoundRequest(Exception exception)
        {

            // Arrange
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerPromoCodeLimitRepositoryMock = new Mock<IPartnerPromoCodeLimitRepository>();

            partnerPromoCodeLimitRepositoryMock.Setup(u =>
                u.SetPartnerPromoCodeLimitAsync(It.IsAny<Guid>(), It.IsAny<SetPartnerPromoCodeLimitRequest>()).Result)
                .Throws(exception);

            PartnersController partnersController = new PartnersController(partnerRepositoryMock.Object, partnerPromoCodeLimitRepositoryMock.Object);

            // Act
            var result = partnersController.SetPartnerPromoCodeLimitAsync(Guid.NewGuid(), new SetPartnerPromoCodeLimitRequest()).Result;

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Проверка случая, когда всё прошло успешно и вернулся Created
        /// </summary>
        [Fact]
        public void SetPartnerPromoCodeLimitAsync_CheckForCreatedAtActionResult_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerPromoCodeLimitRepositoryMock = new Mock<IPartnerPromoCodeLimitRepository>();

            var partnerPromoCodeLimitId = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;
            var partnerId = Guid.NewGuid();

            var partnerPromoCodeLimit = new PartnerPromoCodeLimit
            {
                Id = partnerPromoCodeLimitId,
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                EndDate = dateTimeNow.AddDays(30),
                CancelDate = null,
                Limit = 100
            };


            partnerPromoCodeLimitRepositoryMock.Setup(u =>
                u.SetPartnerPromoCodeLimitAsync(It.IsAny<Guid>(), It.IsAny<SetPartnerPromoCodeLimitRequest>()).Result)
                .Returns(partnerPromoCodeLimit);

            SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                EndDate = partnerPromoCodeLimit.EndDate,
                Limit = partnerPromoCodeLimit.Limit
            };
            PartnersController partnersController = new PartnersController(partnerRepositoryMock.Object, partnerPromoCodeLimitRepositoryMock.Object);

            // Act
            var result = partnersController.SetPartnerPromoCodeLimitAsync(Guid.NewGuid(), setPartnerPromoCodeLimitRequest).Result;

            // Assert
            Assert.IsType<CreatedResult>(result);

        }

        public void Dispose()
        {

        }
    }

}