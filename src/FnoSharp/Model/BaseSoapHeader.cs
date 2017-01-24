using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace FnoSharp.Model
{
    class BaseSoapHeader : SoapHeader
    {
        public BaseSoapHeader()
        {
            Actor = "http://schemas.xmlsoap.org/soap/actor/next";
            MustUnderstand = false;
        }

        [XmlText]
        public virtual string Value { get; set; }
    }
}