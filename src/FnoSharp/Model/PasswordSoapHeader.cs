using System;
using System.Text;
using System.Xml.Serialization;

namespace FnoSharp.Model
{
    [XmlRoot("UserPassword", Namespace = "urn:com.macrovision:flexnet/platform")]
    class PasswordSoapHeader : BaseSoapHeader
    {
        [XmlIgnore]
        public string EncodedPassword
        {
            set { Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(value)); }
        }
    }
}
