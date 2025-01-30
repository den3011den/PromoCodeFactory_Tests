using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Runtime.Serialization;

namespace PromoCodeFactory.DataAccess.Repositories.Exceptions
{
    /// <summary>
    /// Исключение Партнёр не активен
    /// </summary>
    /// 

    [Serializable]
    public class PartnerNotActiveException : Exception
    {
        public PartnerNotActiveException(Partner partner)
                : base($"Партнёр с ИД ${partner.Id} не активен")
        {
        }
        public PartnerNotActiveException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
        {
        }
    }
}
