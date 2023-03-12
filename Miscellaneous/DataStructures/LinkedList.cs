using System;
using System.Collections.Generic;
using System.Text;

namespace RandomUsefulThings.Misc.DataStructures
{
    public class LinkedList<T>
    {
        private class Element
        {
            public T value;
            public Element previous;
            public Element next;
        }

        private Element first;
        private Element last;
    }
}
