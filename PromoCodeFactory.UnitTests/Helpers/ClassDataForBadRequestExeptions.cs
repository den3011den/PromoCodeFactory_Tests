using PromoCodeFactory.DataAccess.Repositories.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PromoCodeFactory.UnitTests.Helpers
{

    /// <summary>
    /// Класс для перечисления всех исключений, вызывающих возврат BadRequest 
    /// в методе SetPartnerPromoCodeLimitAsync контроллера PartnersController
    /// </summary>
    public class ClassDataForBadRequestExeptions : IEnumerable<object[]>
    {

        private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {new PartnerPromoCodeLimitLessOrEqualZeroException()},
                new object[] {new PartnerPromoCodeLimitNotActiveException()},
                new object[] {new PartnerNotActiveException()},
                new object[] {new PartnerHasMoreThenOneActivePartnerPromoCodeLimitException()},
                new object[] {new Exception()},
            };


        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    }
}
