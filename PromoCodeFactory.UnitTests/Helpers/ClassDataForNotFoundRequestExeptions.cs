using PromoCodeFactory.DataAccess.Repositories.Exceptions;
using System.Collections;
using System.Collections.Generic;

namespace PromoCodeFactory.UnitTests.Helpers
{
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
