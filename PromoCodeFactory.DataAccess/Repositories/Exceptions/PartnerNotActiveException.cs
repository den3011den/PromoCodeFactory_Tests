using System;
using System.Runtime.Serialization;

namespace PromoCodeFactory.DataAccess.Repositories.Exceptions
{
    /// <summary>
    /// Исключение Партнёр не найден
    /// </summary>
    /// 

    [Serializable]
    public class PartnerNotFoundException : Exception
    {

        public PartnerNotFoundException(Guid partnerId)
                : base($"Партнёр с ИД  {partnerId} не найден")
        {
        }
        public PartnerNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
