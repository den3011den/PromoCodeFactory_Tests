using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IPartnerPromoCodeLimitRepository : IRepository<PartnerPromoCodeLimit>
    {
        public Task<PartnerPromoCodeLimit> SetPartnerPromoCodeLimitAsync(Guid partnerId, SetPartnerPromoCodeLimitRequest setPartnerPromoCodeLimitRequest);
        public Task<IEnumerable<PartnerPromoCodeLimit>> GetActivePartnerPromoCodeLimitListAsync(Guid partnerId);

    }
}
