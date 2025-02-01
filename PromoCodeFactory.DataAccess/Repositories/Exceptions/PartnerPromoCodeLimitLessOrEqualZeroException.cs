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
    public class PartnerPromoCodeLimitLessOrEqualZeroException : Exception
    {

        public string MyProperty { get; set; }
        public PartnerPromoCodeLimitLessOrEqualZeroException()
        {

        }

        public PartnerPromoCodeLimitLessOrEqualZeroException(SetPartnerPromoCodeLimitRequest partnerPromoCodeLimit)
                : base($"Лимит равен {partnerPromoCodeLimit.Limit.ToString()}, что меньше или равно нулю. Должен быть положительным числом")
        {
        }

        public PartnerPromoCodeLimitLessOrEqualZeroException(SerializationInfo serializationInfo, StreamingContext streamingContext)
                : base(serializationInfo, streamingContext)
        {
        }
    }
}
