using imbSCI.BusinessData.Models.Core;
using imbSCI.BusinessData.Models.Organization;

namespace imbSCI.BusinessData.Models.Company
{
/// <summary>
    /// Collection of <see cref="Person"/> instances, referenced to a company model instance via <see cref="ModelRecordsCollectionBase{T}.name"/> identifier
    /// </summary>
    /// <seealso cref="imbSCI.BusinessData.Models.Core.ModelRecordsCollectionBase{imbSCI.BusinessData.Models.Organization.Person}" />
    public class CompanyPersonCollection: ModelRecordsCollectionBase<Person>
    {
        public CompanyPersonCollection()
        {

        }
    }
}