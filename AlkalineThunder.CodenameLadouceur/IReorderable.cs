using System;
using System.Collections.Generic;
using System.Text;

namespace AlkalineThunder.Nucleus
{
    public interface IReorderable<T> : ICollection<T>
    {
        void SendToBack(T item);
        void BringToFront(T item);
    }
}
