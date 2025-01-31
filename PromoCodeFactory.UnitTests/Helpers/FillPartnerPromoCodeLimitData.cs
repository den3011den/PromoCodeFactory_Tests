using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess;
using System;

namespace PromoCodeFactory.UnitTests.Helpers
{
    public static class FillPartnerPromoCodeLimitData
    {

        public static void FillPartnerPromoCodeLimitListWithTwoActiveLimitsForPartnerId(Guid partnerId, DataContext _db)
        {
            var partnerId2 = Guid.NewGuid();
            var partnerId3 = Guid.NewGuid();
            var dateTimeNow = DateTime.Now;


            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = dateTimeNow,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId2,
                CreateDate = dateTimeNow,
                CancelDate = dateTimeNow,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId2,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId2,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId3,
                CreateDate = dateTimeNow,
                CancelDate = dateTimeNow,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId3,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });

            _db.PartnerPromoCodeLimit.Add(new PartnerPromoCodeLimit
            {
                PartnerId = partnerId3,
                CreateDate = dateTimeNow,
                CancelDate = null,
                EndDate = dateTimeNow.AddDays(30),
                Limit = 100
            });
            _db.SaveChanges();
        }
    }
}
