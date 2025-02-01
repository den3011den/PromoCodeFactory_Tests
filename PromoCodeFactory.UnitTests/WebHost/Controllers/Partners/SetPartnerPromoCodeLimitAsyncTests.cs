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
    public class SetPartnerPromoCodeLimitAsyncTests : IDisposable
    {


        [Theory]
        [ClassData(typeof(ClassDataForBadRequestExeptions))]
        public void SetPartnerPromoCodeLimitAsync_ThrownExceptionsForBadRequest_ReturnsBadRequest(Exception exception)
        {

            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerPromoCodeLimitRepositoryMock = new Mock<IPartnerPromoCodeLimitRepository>();

            partnerPromoCodeLimitRepositoryMock.Setup(u =>
                u.SetPartnerPromoCodeLimitAsync(It.IsAny<Guid>(), It.IsAny<SetPartnerPromoCodeLimitRequest>()).Result)
                .Throws(exception);

            PartnersController partnersController = new PartnersController(partnerRepositoryMock.Object, partnerPromoCodeLimitRepositoryMock.Object);

            var result = partnersController.SetPartnerPromoCodeLimitAsync(Guid.NewGuid(), new SetPartnerPromoCodeLimitRequest()).Result;

            Assert.IsType<BadRequestObjectResult>(result);

        }

        [Theory]
        [ClassData(typeof(ClassDataForNotFoundRequestExeptions))]
        public void SetPartnerPromoCodeLimitAsync_ThrownExceptionsForNotFoundRequest_ReturnsNotFoundRequest(Exception exception)
        {

            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerPromoCodeLimitRepositoryMock = new Mock<IPartnerPromoCodeLimitRepository>();

            partnerPromoCodeLimitRepositoryMock.Setup(u =>
                u.SetPartnerPromoCodeLimitAsync(It.IsAny<Guid>(), It.IsAny<SetPartnerPromoCodeLimitRequest>()).Result)
                .Throws(exception);

            PartnersController partnersController = new PartnersController(partnerRepositoryMock.Object, partnerPromoCodeLimitRepositoryMock.Object);

            var result = partnersController.SetPartnerPromoCodeLimitAsync(Guid.NewGuid(), new SetPartnerPromoCodeLimitRequest()).Result;

            Assert.IsType<NotFoundObjectResult>(result);

        }


        [Fact]
        public void SetPartnerPromoCodeLimitAsync_CheckForCreatedAtActionResult_ReturnsCreatedAtActionResult()
        {

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

            var result = partnersController.SetPartnerPromoCodeLimitAsync(Guid.NewGuid(), setPartnerPromoCodeLimitRequest).Result;

            Assert.IsType<CreatedResult>(result);

        }

        public void Dispose()
        {

        }
    }

}