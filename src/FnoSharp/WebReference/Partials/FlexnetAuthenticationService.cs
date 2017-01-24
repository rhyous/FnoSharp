using System;
using System.Net;
using FnoSharp.Extensions;

namespace FnoSharp
{
    public partial class FlexnetAuthenticationService : IDisposable
    {
        protected override WebRequest GetWebRequest(Uri uri = null)
        {
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
