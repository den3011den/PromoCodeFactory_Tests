using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Runtime.Serialization;

namespace PromoCodeFactory.DataAccess.Repositories.Exceptions
{
    /// <summary>
    /// Исключение промокод не найден
    /// </summary>
    /// 

    [Serializable]
    public class PartnerPromoCodeLimitNotFoundException : Exception
    {
        public PartnerPromoCodeLimitNotFoundException(PartnerPromoCodeLimit partnerPromoCodeLimit)
                : base($"Партнёрский лимит на промокоды с ИД ${partnerPromoCodeLimit.Id} не найден")
        {
        }
        public PartnerPromoCodeLimitNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
        {
        }
    }
}
