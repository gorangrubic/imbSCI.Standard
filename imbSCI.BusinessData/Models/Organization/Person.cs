using imbSCI.BusinessData.Core;
using System;

namespace imbSCI.BusinessData.Models.Organization
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person'
    public class Person : IRecordWithGetUID
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person.Person()'
        public Person()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person.Person()'
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person.Genre'
        public PersonGenre Genre { get; set; } = PersonGenre.Unknown;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person.Genre'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person.Name'
        public String Name { get; set; } = "";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person.Name'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person.Lastname'
        public String Lastname { get; set; } = "";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person.Lastname'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person.PhoneNumber'
        public String PhoneNumber { get; set; } = "";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person.PhoneNumber'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person.EMailAddress'
        public String EMailAddress { get; set; } = "";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person.EMailAddress'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person.Position'
        public PersonPosition Position { get; set; } = PersonPosition.Unknown;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person.Position'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Person.GetUID()'
        public string GetUID()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Person.GetUID()'
        {
            return Name + Lastname + Position;
        }
    }
}