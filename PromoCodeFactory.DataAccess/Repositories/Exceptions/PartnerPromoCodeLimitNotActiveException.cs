using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Runtime.Serialization;

namespace PromoCodeFactory.DataAccess.Repositories.Exceptions
{
    /// <summary>
    /// Исключение Партнёрский лимит на промокоды не активен
    /// </summary>
    /// 

    [Serializable]
    public class PartnerPromoCodeLimitNotActiveException : Exception
    {
        public PartnerPromoCodeLimitNotActiveException()
        {
        }

        public PartnerPromoCodeLimitNotActiveException(SetPartnerPromoCodeLimitRequest partnerPromoCodeLimit)
                : base($"Лимит не активен. Дата окончания лимита должна быть больше текущих даты/времени (устанавливаемый лимит должен быть действующим на момент добавления)")
        {
        }
        public PartnerPromoCodeLimitNotActiveException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
};

