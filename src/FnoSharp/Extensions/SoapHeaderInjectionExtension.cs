using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Services.Protocols;
using FnoSharp.Model;

namespace FnoSharp.Extensions
{
    public class SoapHeaderInjectionExtension : SoapExtension
    {
        public static Dictionary<string, bool> EnabledServices = new Dictionary<string, bool>();
        public static Dictionary<string, NetworkCredential> UserAndPassword = new Dictionary<string, NetworkCredential>();

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null; // Unused
        }

        public override object GetInitializer(Type serviceType)
        {
            return null; // Unused
        }

        public override void Initialize(object initializer)
        {
            // Unused
        }

        public override void ProcessMessage(SoapMessage message)
        {
            if (!IsEnabledForUrl(message.Url))
                return;
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    NetworkCredential creds;
                    if (UserAndPassword.TryGetValue(message.Url, out creds))
                    {
                        message.Headers.Add(new UserIdSoapHeader { Value = creds.UserName });
                        message.Headers.Add(new PasswordSoapHeader { EncodedPassword = creds.Password });
                    }
                    break;
                case SoapMessageStage.AfterSerialize:
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
            }
        }

        public bool IsEnabledForUrl(string url)
        {
            bool isEnabled;
            EnabledServices.TryGetValue(url, out isEnabled);
            return isEnabled;
        }
    }
}