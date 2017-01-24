using System;
using System.Net;
using System.Web.Services.Protocols;

namespace FnoSharp.Extensions
{
    public static class WebClientProtocolExtensions
    {
        public static void SetNetworkCredentials(this WebClientProtocol client, string username, string password, string customUrl = null, bool preAuthenticate = true)
        {
            client.PreAuthenticate = preAuthenticate;
            client.Url = string.IsNullOrWhiteSpace(customUrl) ? client.Url : customUrl;
            var netCredential = new NetworkCredential(username, password);
            if (preAuthenticate)
            {
                ICredentials credentials = netCredential.GetCredential(new Uri(client.Url), "Basic");
                client.Credentials = credentials;
            }
            else
            {
                SoapHeaderInjectionExtension.EnabledServices[client.Url] = true;
                SoapHeaderInjectionExtension.UserAndPassword[client.Url] = netCredential;
            }
        }

        public static void EnableSoapXmlLogging(this WebClientProtocol client)
        {
            SoapLoggerExtension.EnabledServices[client.Url] = true;
        }
    }
}
