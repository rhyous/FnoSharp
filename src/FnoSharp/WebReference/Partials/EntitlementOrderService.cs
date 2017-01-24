using System;
using System.Net;

using FnoSharp.Extensions;
using FnoSharp.Model;

namespace FnoSharp
{
    public partial class EntitlementOrderService : IDisposable
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
    }
}
