/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            const int value = 0;

            // a. Continuation task should be executed regardless of the result of the parent task.
            var exceptionTask = Task.Factory.StartNew(() => ParentTaskWithException(value));
            var normalTask = Task.Factory.StartNew(() => ParentTask());

            exceptionTask.ContinueWith(
                _ =>
                {
                    Console.WriteLine("a. Continuation task runs regardless! Parent was failed.");
                });
            normalTask.ContinueWith(
                _ =>
                {
                    Console.WriteLine("a. Continuation task runs regardless! Parent was succeded.");
                });

            // b. Continuation task should be executed when the parent task finished without success.");
            exceptionTask.ContinueWith(
                _ =>
            {
                Console.WriteLine("b. Continuation task runs when parent is faulted!");
            }, TaskContinuationOptions.OnlyOnFaulted);

            normalTask.ContinueWith(
                _ =>
                {
                    Console.WriteLine("b. Continuation task runs when parent is succeded!");
                }, TaskContinuationOptions.OnlyOnFaulted);

            // c.Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
            exceptionTask.ContinueWith(
               _ =>
           {
               Console.WriteLine($"c. Continuation task runs when parent is failed and the parent is: {Thread.CurrentThread.ManagedThreadId}!");
           }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            normalTask.ContinueWith(
              _ =>
              {
                  Console.WriteLine($"c. Continuation task runs when parent is failed and the parent is: {Thread.CurrentThread.ManagedThreadId}!");
              }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            // d. Continuation task should be executed outside of the thread pool when the parent task would be cancelled.            
            var cts = new CancellationTokenSource();
            {
                cts.Cancel();
                var token = cts.Token;
                var normalCancelTask = Task.Factory.StartNew(() => ParentTask(), token)
                .ContinueWith(
                   _ =>
                {
                    Console.WriteLine($"d. Continuation task runs when parent is cancelled! ThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
                }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);
            }

            Console.ReadLine();
            Task.WaitAll();
        }

        private static int ParentTaskWithException(int divisor)
        {
            Console.WriteLine($"Parent task with exception is running with id : {Thread.CurrentThread.ManagedThreadId}");

            return 10 / divisor;
        }

        private static void ParentTask()
        {
            Console.WriteLine($"Parent task is running with id : {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
