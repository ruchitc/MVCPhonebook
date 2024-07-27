using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookClientTests.Utilities
{
    [ExcludeFromCodeCoverage]
    public class TempDataDictionaryFactory
    {
        private readonly ITempDataProvider _tempDataProvider;

        public TempDataDictionaryFactory(ITempDataProvider tempDataProvider)
        {
            _tempDataProvider = tempDataProvider;
        }

        public ITempDataDictionary GetTempData(HttpContext context)
        {
            return CreateTempData(context);
        }

        private ITempDataDictionary CreateTempData(HttpContext context)
        {
            if (_tempDataProvider == null)
            {
                throw new InvalidOperationException($"No {nameof(ITempDataDictionary)} was set");
            }

            return new TempDataDictionary(context, _tempDataProvider);
        }
    }
}
