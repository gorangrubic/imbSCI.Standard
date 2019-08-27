using imbSCI.Core.interfaces;
using System;

namespace imbSCI.BusinessData.Models.Organization
{
  public class InternetAddress
    {
        public String WebSite { get; set; }
        public String PrimaryEMailAddress { get; set; }

        public InternetAddressType Type { get; set; } = InternetAddressType.Unknown;

        public InternetAddress()
        {
        }
    }
}