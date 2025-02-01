using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess;
using System;

namespace PromoCodeFactory.UnitTests.Helpers
{

    /// <summary>
    /// Класс с методами для заполнения тестовыми данными БД для теста 
    /// метода SetPartnerPromoCodeLimitAsync контроллера PartnersController
    /// </summary>
    public static class FillPartnerPromoCodeLimitData
    {

        /// <summary>
        /// Для партнёра с указанным ИД заполняет данные лимитов так, что 2 из них одновременно активны на текущий момент
        /// </summary>
        /// <param name="partnerId">ИД партнёра</param>
        /// <param name="_db">Контекст БД</param>
        public static void FillPartnerPromoCodeLimitListWithTwoActiveLimitsForPartnerId(Guid partnerId, DataContext _db)
        {
            var partnerId2 = Guid.NewGuid();
            var partnerId3 = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;


            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = dateTimeNow,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId2,
                CreateDate = dateTimeNow,
                CancelDate = dateTimeNow,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId2,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId2,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId3,
                CreateDate = dateTimeNow,
                CancelDate = dateTimeNow,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId3,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId3,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });
            _db.SaveChanges();
        }


        /// <summary>
        /// Для партнёра с указанным ИД заполняет данные лимитов так, что только 1 из них активен на текущий момент
        /// </summary>
        /// <param name="partnerId">ИД партнёра</param>
        /// <param name="_db">Контекст БД</param>
        public static void FillPartnerPromoCodeLimitListWithOneActiveLimit(PartnerPromoCodeLimit partnerPromoCodeLimit, DataContext _db)
        {
            var partnerId1 = partnerPromoCodeLimit.PartnerId;
            var partnerId2 = Guid.NewGuid();
            var partnerId3 = Guid.NewGuid();


            _db.PartnerPromoCodeLimit.Add(partnerPromoCodeLimit);

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId1,
                CreateDate = DateTime.Now.AddDays(-60),
                CancelDate = null,
                EndDate = DateTime.Now.AddDays(-30),
                Limit = 110
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId1,
                CreateDate = DateTime.Now.AddDays(-20),
                CancelDate = null,
                EndDate = DateTime.Now.AddDays(-10),
                Limit = 120
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId1,
                CreateDate = DateTime.Now.AddDays(-20),
                CancelDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(120),
                Limit = 130
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId2,
                CreateDate = DateTime.Now.AddDays(-20),
                CancelDate = null,
                EndDate = DateTime.Now.AddDays(20),
                Limit = 140
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId2,
                CreateDate = DateTime.Now.AddDays(-20),
                CancelDate = null,
                EndDate = DateTime.Now.AddDays(-10),
                Limit = 120
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId2,
                CreateDate = DateTime.Now.AddDays(-20),
                CancelDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(120),
                Limit = 130
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId3,
                CreateDate = DateTime.Now.AddDays(-20),
                CancelDate = null,
                EndDate = DateTime.Now.AddDays(20),
                Limit = 140
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId3,
                CreateDate = DateTime.Now.AddDays(-20),
                CancelDate = null,
                EndDate = DateTime.Now.AddDays(-10),
                Limit = 120
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                PartnerId = partnerId3,
                CreateDate = DateTime.Now.AddDays(-20),
                CancelDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(120),
                Limit = 130
            });
            _db.SaveChanges();
        }

    }
}
