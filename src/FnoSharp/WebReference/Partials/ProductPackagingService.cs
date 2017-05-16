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

        public deleteProductResponseType DeleteProducts(IEnumerable<deleteProductDataType> products)
        {
            var prodStates = products.Select(p => new productStateDataType { stateToSet = StateType.DRAFT, productIdentifier = p.productIdentifier });
            var changeStateResponse = setProductState(prodStates.ToArray());
            if (changeStateResponse.statusInfo.status == StatusType.FAILURE)
                return new deleteProductResponseType { statusInfo = changeStateResponse.statusInfo };
            var prodsToDelete = products.Select(p => new deleteProductDataType { productIdentifier = p.productIdentifier }).ToList();
            if (changeStateResponse.statusInfo.status == StatusType.FAILURE)
            {
                foreach (var item in changeStateResponse.failedData)
                {
                    prodsToDelete.RemoveAll(p => p.productIdentifier.primaryKeys.name == item.product.productIdentifier.primaryKeys.name
                                              && p.productIdentifier.primaryKeys.version == item.product.productIdentifier.primaryKeys.version);
                }
            }
            return deleteProduct(prodsToDelete.ToArray());

        }
    }
}
