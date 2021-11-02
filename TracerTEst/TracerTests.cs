using Xunit;
using TracerLib;
using System.Threading;
using System.Collections.Generic;

namespace TracerTest
{
    
    public class TracerTest
    {
        private Tracer tracer;
        private int sleepTime;
        private TraceResult traceResult;
        private const int threadsCount = 3;
        private const int threadsNestedCount = 2;

        public void TestMethod(int sleepTime)
        {
            tracer.StartTrace();
            Thread.Sleep(sleepTime);
            tracer.StopTrace();
        }

        public void TestMethod2(int sleepTime)
        {
            tracer.StartTrace();
            Thread.Sleep(sleepTime * 2);
            tracer.StopTrace();
        }

        public void InnerMethod(int sleepTime)
        {
            tracer.StartTrace();
            Thread.Sleep(sleepTime);
            TestMethod(sleepTime);
            tracer.StopTrace();
        }

        public void NestedMethod(int sleepTime)
        {
            tracer.StartTrace();
            Thread.Sleep(sleepTime);
            InnerMethod(sleepTime);
            tracer.StopTrace();
        }

        public void MultipleThreadMethod(int sleepTime)
        {
            tracer.StartTrace();
            var threads = new List<Thread>();
            for (int i = 0; i < threadsNestedCount; i++)
            {
                Thread thread = new Thread(() => TestMethod(sleepTime));
                threads.Add(thread);
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            TestMethod(sleepTime);
            Thread.Sleep(sleepTime);
            tracer.StopTrace();
        }

        //time of single thread
        [Fact]
        public void TimeTestSingleThread()
        {
            tracer = new Tracer();
            sleepTime = 228;
            TestMethod(sleepTime);
            traceResult = tracer.GetTraceResult();
            Assert.True(traceResult.threads[0].TimeInt >= sleepTime);
        }

        //time of several threads
        [Fact]
        public void TimeTestMultiThread()
        {
            tracer = new Tracer();
            sleepTime = 111;

            var threads = new List<Thread>();
            for (int i = 0; i < threadsCount; i++)
            {
                Thread thread = new Thread(() => TestMethod(sleepTime));
                threads.Add(thread);
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            traceResult = tracer.GetTraceResult();
            long actualtime = 0;
            for (int i = 0; i < traceResult.threads.Count; i++)
            {
                actualtime += traceResult.threads[i].TimeInt;
            }
            Assert.True(actualtime >= sleepTime * threadsCount);
        }

        //time of nested methods
        [Fact]
        public void TimeTestNestedMethods()
        {
            tracer = new Tracer();
            sleepTime = 50;

            NestedMethod(sleepTime);
            traceResult = tracer.GetTraceResult();

            Assert.True(traceResult.threads[0].TimeInt >= sleepTime * 3);
        }

        //nested threads
        [Fact]
        public void TestNestedThreads()
        {
            tracer = new Tracer();
            sleepTime = 80;
            int singlemethods = 0, nestedmethods = 0;

            var threads = new List<Thread>();
            for (int i = 0; i < threadsCount; i++)
            {
                Thread thread = new Thread(() => MultipleThreadMethod(sleepTime));
                threads.Add(thread);
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            traceResult = tracer.GetTraceResult();
            Assert.Equal(threadsCount * threadsNestedCount + threadsCount, traceResult.threads.Count);
            for (int i = 0; i < traceResult.threads.Count; i++)
            {
                Assert.Equal(1, traceResult.threads[i].Methods.Count);
                Assert.Equal(nameof(TracerTest), traceResult.threads[i].Methods[0].ClassName);
                if (traceResult.threads[i].Methods[0].Methodlist.Count != 0)
                {
                    nestedmethods++;
                    Assert.Equal(nameof(MultipleThreadMethod), traceResult.threads[i].Methods[0].MethodName);
                    Assert.Equal(nameof(TestMethod), traceResult.threads[i].Methods[0].Methodlist[0].MethodName);
                }
                else
                    singlemethods++;
            }
            Assert.Equal(threadsCount, nestedmethods);
            Assert.Equal(threadsNestedCount * threadsCount, singlemethods);
        }

        //several methods in single thread
        [Fact]
        public void TestMultipleMethodsInSingleThread()
        {
            tracer = new Tracer();
            sleepTime = 400;

            TestMethod(sleepTime);
            TestMethod2(sleepTime);
            traceResult = tracer.GetTraceResult();
            Assert.Equal(1, traceResult.threads.Count);
            Assert.Equal(2, traceResult.threads[0].Methods.Count);
            Assert.True(traceResult.threads[0].TimeInt >= sleepTime * 3);
            Assert.Equal(nameof(TestMethod), traceResult.threads[0].Methods[0].MethodName);
            Assert.Equal(nameof(TestMethod2), traceResult.threads[0].Methods[1].MethodName);
        }

        //single method in single thread
        [Fact]
        public void TestSingleNestedMethod()
        {
            tracer = new Tracer();
            sleepTime = 1000;

            TestMethod(sleepTime);
            traceResult = tracer.GetTraceResult();

            Assert.Equal(1, traceResult.threads.Count);
            Assert.Equal(1, traceResult.threads[0].Methods.Count);
            Assert.True(traceResult.threads[0].TimeInt >= sleepTime);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId, traceResult.threads[0].Id);
            Assert.Equal(0, traceResult.threads[0].Methods[0].Methodlist.Count);
            Assert.True(traceResult.threads[0].Methods[0].TimeInt >= sleepTime);
            Assert.Equal(nameof(TestMethod), traceResult.threads[0].Methods[0].MethodName);
        }
    }
}

