using PromoCodeFactory.DataAccess.Repositories.Exceptions;
using System.Collections;
using System.Collections.Generic;

namespace PromoCodeFactory.UnitTests.Helpers
{

    /// <summary>
    /// Класс для перечисления всех исключений, вызывающих возврат NotFound 
    /// в методе SetPartnerPromoCodeLimitAsync контроллера PartnersController
    /// </summary>
    public class ClassDataForNotFoundRequestExeptions : IEnumerable<object[]>
    {

        private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {new PartnerNotFoundException()},
            };


        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    }
}
