using System;
using System.Threading;

namespace LockFreeStructures
{
    public class Stack<T>
    {
        public class Node
        {
            public T Value;
            public Node Next;
        }

        private Node head;

        public Stack()
        {
            head = null;
        }

        public void Push(T value)
        {
            var newNode = new Node {Value = value};
            do
            {
                newNode.Next = head;
            } while (Interlocked.CompareExchange(ref head, newNode, newNode.Next) != newNode.Next);
        }

        public T Pop()
        {
            Node node;
            do
            {
                node = head;
            } while (node != null && Interlocked.CompareExchange(ref head, node.Next, node) != node);

            if (node != null)
                return node.Value;
            throw new Exception();
        }
    }
}