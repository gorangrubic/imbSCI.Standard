using imbSCI.BusinessData.Core;
using System.Collections;

namespace imbSCI.BusinessData.Models.Organization
{
    /// <summary>
    /// Collection of <see cref="Person"/> instances, not referenced
    /// </summary>
    /// <seealso cref="imbSCI.BusinessData.Core.RecordsWithUIDCollection{imbSCI.BusinessData.Models.Organization.Person}" />
    public class PersonCollection : RecordsWithUIDCollection<Person>
    {
        public PersonCollection()
        {
        }
    }
}