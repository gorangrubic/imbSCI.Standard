namespace imbSCI.BusinessData.Core
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>'
    public class ReferencedWrapperForRecordsCollection<TCollection, TReference> : IRecordWithGetUID, IReferencedWrapperForRecordsCollection
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>'
        where TCollection : class, IRecordsWithUIDCollection, new()
        where TReference : class, IRecordWithGetUID

    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>.UID_NOREFERENCE'
        public const string UID_NOREFERENCE = "NO_REFERENCE_SET";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>.UID_NOREFERENCE'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>.reference'
        public TReference reference { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>.reference'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>.records'
        public TCollection records { get; set; } = new TCollection();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>.records'

        IRecordWithGetUID IReferencedWrapperForRecordsCollection.reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value as TReference;
            }
        }

        IRecordsWithUIDCollection IReferencedWrapperForRecordsCollection.records
        {
            get
            {
                return records;
            }
            set
            {
                records = value as TCollection;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>.GetUID()'
        public string GetUID()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReferencedWrapperForRecordsCollection<TCollection, TReference>.GetUID()'
        {
            if (reference != null)
            {
                return reference.GetUID();
            }
            return UID_NOREFERENCE;
        }
    }
}