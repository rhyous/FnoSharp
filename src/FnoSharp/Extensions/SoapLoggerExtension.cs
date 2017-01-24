using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Services.Protocols;
using log4net;
using FnoSharp.Model;

namespace FnoSharp.Extensions
{
    public class SoapLoggerExtension : SoapExtension
    {
        private static ILog Logger
        {
            get { return _Logger ?? (_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)); }
        } private static ILog _Logger;

        public static Dictionary<string, bool> EnabledServices = new Dictionary<string, bool>();

        private Stream _OldStream;
        private Stream _NewStream;

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

        public override Stream ChainStream(Stream stream)
        {
            _OldStream = stream;
            _NewStream = new MemoryStream();
            return _NewStream;
        }

        public override void ProcessMessage(SoapMessage message)
        {
            if (!IsEnabledForUrl(message.Url))
                return;
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    Log(message, "AfterSerialize");
                    CopyStream(_NewStream, _OldStream);
                    _NewStream.Position = 0;
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(_OldStream, _NewStream);
                    Log(message, "BeforeDeserialize");
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
            }
        }

        private bool IsEnabledForUrl(string url)
        {
            bool isEnabled;
            EnabledServices.TryGetValue(url, out isEnabled);
            return isEnabled;
        }

        private void Log(SoapMessage message, string stage)
        {
            _NewStream.Position = 0;
            Logger.Debug(stage);
            var reader = new StreamReader(_NewStream);
            string requestXml = reader.ReadToEnd();
            _NewStream.Position = 0;
            if (!string.IsNullOrWhiteSpace(requestXml))
                Logger.Debug(new Xml(requestXml).PrettyXml);
        }

        private void CopyStream(Stream fromStream, Stream toStream)
        {
            try
            {
                StreamReader sr = new StreamReader(fromStream);
                StreamWriter sw = new StreamWriter(toStream);
                sw.WriteLine(sr.ReadToEnd());
                sw.Flush();
            }
            catch (Exception ex)
            {
                string message = String.Format("CopyStream failed because: {0}", ex.Message);
                Logger.Error(message, ex);
            }
        }
    }
}