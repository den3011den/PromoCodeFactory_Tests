using System;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class SetPartnerPromoCodeLimitRequest
    {
        public DateTime EndDate { get; set; }
        public int Limit { get; set; }
    }
}
