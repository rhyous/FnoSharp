using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FnoSharp.Extensions;
using FnoSharp.Model;

namespace FnoSharp
{
    public partial class ProductPackagingService : IDisposable
    {
        protected override WebRequest GetWebRequest(Uri uri = null)
        {
            Timeout = new Timeout().Milliseconds;
            var actualUri = uri ?? new Uri(Url);
            var request = (HttpWebRequest)base.GetWebRequest(actualUri);
            return (PreAuthenticate) ? request.GetRequestWithBasicAuthorization(actualUri) : request;
        }


        void IDisposable.Dispose()
        {
            SoapHeaderInjectionExtension.EnabledServices.Remove(Url);
            SoapHeaderInjectionExtension.UserAndPassword.Remove(Url);
        }

        public createProductResponseType CreateProducts(IEnumerable<createProductDataType> products)
        {
            return createProduct(products.ToArray());
        }

        public createProductResponseType CreateProduct(createProductDataType product)
        {
            return createProduct(new[] { product });
        }
    }
}
