using System;

namespace SharpTools
{
    public class InputPoint<T>
    {
        protected readonly object locker = new object();

        protected T read;

        public T Value
        {
            get { lock (locker) { return read; } }
            set { lock (locker) { read = value; } }
        }
    }

    public class OutputPoint<T> : InputPoint<T>
    {
        protected T write;

        public T Write
        {
            get { lock (locker) { return write; } }
            set { lock (locker) { write = value; } }
        }

        public T GetAndSet(T value)
        {
            lock (locker)
            {
                var current = write;
                write = value;
                return current;
            }
        }

        public bool Dirty
        {
            get
            {
                lock (locker)
                {
                    return !object.Equals(read, write);
                }
            }
        }
    }
}
