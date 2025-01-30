using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Runtime.Serialization;

namespace PromoCodeFactory.DataAccess.Repositories.Exceptions
{
    /// <summary>
    /// Исключение Партнёрский лимит на промокоды меньше или равен нулю
    /// </summary>
    /// 

    [Serializable]
    public class PartnerPromoCodeLimitNotLessOrEqualZeroException : Exception
    {
        public PartnerPromoCodeLimitNotLessOrEqualZeroException(SetPartnerPromoCodeLimitRequest partnerPromoCodeLimit)
                : base($"Лимит равен {partnerPromoCodeLimit.Limit.ToString()}, что меньше или равно нулю. Должен быть положительным числом")
        {
        }

        public PartnerPromoCodeLimitNotLessOrEqualZeroException(SerializationInfo serializationInfo, StreamingContext streamingContext)
                : base(serializationInfo, streamingContext)
        {
        }
    }
}
