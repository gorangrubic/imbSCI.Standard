namespace imbSCI.BusinessData.Core
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'IReferencedWrapperForRecordsCollection'
    public interface IReferencedWrapperForRecordsCollection : IRecordWithGetUID
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'IReferencedWrapperForRecordsCollection'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'IReferencedWrapperForRecordsCollection.reference'
        IRecordWithGetUID reference { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'IReferencedWrapperForRecordsCollection.reference'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'IReferencedWrapperForRecordsCollection.records'
        IRecordsWithUIDCollection records { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'IReferencedWrapperForRecordsCollection.records'
    }
}