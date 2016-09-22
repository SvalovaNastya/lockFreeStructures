using System.Threading;

namespace LockFreeStructures
{
    public class Queue<T>
    {
        public class Node
        {
            public T Value;
            public Node Next;
        }

        private Node head;
        private Node tail;

        public Queue()
        {
            var node = new Node();
            head = tail = node;
        }

        private void ActualizeTail()
        {
            while (true)
            {
                Node oldTail = tail;
                if (oldTail.Next == null)
                    break;
                Interlocked.CompareExchange(ref tail, oldTail.Next, oldTail);
            }
        }

        public void Enqueue(T value)
        {
            var newNode = new Node { Value = value };
            do
            {
                ActualizeTail();
            } while (Interlocked.CompareExchange(ref tail.Next, newNode, null) != null);
        }

        public bool TryDequeue(out T result)
        {
            while (true)
            {
                Node oldHead = head;
                if (oldHead.Next == null)
                    break;
                if (Interlocked.CompareExchange(ref head, oldHead.Next, oldHead) == oldHead)
                {
                    result = oldHead.Next.Value;
                    return true;
                }
            }
            result = default(T);
            return false;
        }
    }
}