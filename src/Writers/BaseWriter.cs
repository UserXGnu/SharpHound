using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharphound.Writers
{
    public abstract class BaseWriter<T>
    {
        protected readonly string DataType;
        protected readonly List<T> Queue;
        protected bool FileCreated;
        protected int Count;
        protected bool NoOp;

        internal BaseWriter(string dataType)
        {
            DataType = dataType;
            Queue = new List<T>();
        }

        internal async System.Threading.Tasks.Task AcceptObject(T item)
        {
            if (NoOp)
                return;
            if (!FileCreated)
            {
                CreateFile();
                FileCreated = true;
            }

            Queue.Add(item);
            Count++;
            if (Count % 30 == 0)
            {
                await WriteData();
                Queue.Clear();
            }
        }

        protected abstract System.Threading.Tasks.Task WriteData();

        internal abstract System.Threading.Tasks.Task FlushWriter();

        protected abstract void CreateFile();
    }
}