using System;
using System.Net;
using System.Text;

namespace FnoSharp.Extensions
{
    public static class WebRequestExtensions
    {
        public static WebRequest GetRequestWithBasicAuthorization(this WebRequest request, Uri uri)
        {
            var networkCredentials = request.Credentials.GetCredential(uri, "Basic");
            byte[] credentialBuffer = new UTF8Encoding().GetBytes(networkCredentials.UserName + ":" + networkCredentials.Password);
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);
            return request;
        }
    }
}
