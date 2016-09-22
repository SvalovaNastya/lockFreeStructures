using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LockFreeStructures
{
    [TestFixture]
    public class StackTests
    {
        [Test]
        public void StackTest()
        {
            var stack = new Stack<int>();

            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            Console.WriteLine(stack.Pop());
            Console.WriteLine(stack.Pop());
            Console.WriteLine(stack.Pop());
        }

        [Test]
        public void MultithreadingStackTest()
        {
            var stack = new Stack<int>();

            var tasks = new Task[3];
            for (var j = 0; j < 3; j++)
            {
                var k = j;
                tasks[j] = Task.Run(() =>
                {
                    for (var i = 0; i < 10; i++)
                        stack.Push(i*3 + k);
                });
            }

            Task.WaitAll(tasks);

            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine(stack.Pop());
            }
        }
    }
}