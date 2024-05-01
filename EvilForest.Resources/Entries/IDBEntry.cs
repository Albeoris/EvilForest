using System;

namespace EvilForest.Resources
{
    public interface IDBEntry<T>
    {
        public T EntryId { get; }
    }
}