using imbSCI.Core.interfaces;
using System;

namespace imbSCI.BusinessData.Models.Organization
{
public class EmailAddress : IFromString
    {
        public EmailAddress()
        {

        }

        public String Address { get; set; } = "";

        public void FromString(string input)
        {
            Address = input;
        }
    }
}