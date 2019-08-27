using System;
using System.Collections.Generic;

namespace imbSCI.BusinessData.Models.Organization
{
  public class PostAddress
    {
        public PostAddress()
        {
        }

        public PostAddressType Type { get; set; } = PostAddressType.Unknown;

        public String StreetAndNumber { get; set; } = "";

        public String ZipCode { get; set; } = "";

        public String Town { get; set; } = "";

        public String Country { get; set; } = "";

        public String AddressCode { get; set; } = "";
    }
}