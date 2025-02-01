using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PromoCodeFactory.DataAccess.Repositories.Exceptions
{
    /// <summary>
    /// Исключение Партнёр имеет более одного действующего лимита на промокоды
    /// </summary>
    /// 

    [Serializable]
    public class PartnerHasMoreThenOneActivePartnerPromoCodeLimitException : Exception
    {
        public PartnerHasMoreThenOneActivePartnerPromoCodeLimitException()
        {
        }

        public PartnerHasMoreThenOneActivePartnerPromoCodeLimitException(IEnumerable<PartnerPromoCodeLimit> partnerPromoCodeLimitList)
                : base($"Партнёр имеет более одного действующего лимита на выдачу промокодов")
        {
        }

        public PartnerHasMoreThenOneActivePartnerPromoCodeLimitException(SerializationInfo serializationInfo, StreamingContext streamingContext)
                : base(serializationInfo, streamingContext)
        {
        }
    }
}
