using System;

namespace FF8.Core
{
    public interface IAwaiter
    {
        Boolean IsCompleted { get; }
        void GetResult();
    }
}