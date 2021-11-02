using System.Collections.Generic;
using System.Threading;
using TracerLib;

namespace LR1_SPP
{
    public class Examples
    {
        private ITracer tracer;

        public Examples(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void StartTest()
        {
            var threads = new List<Thread>();

            for (int i = 0; i < 5; i++)
            {
                Thread thread;
                switch (i)
                {
                    case 0:
                    case 1:
                        thread = new Thread(firstMethod);
                        break;
                    case 2:
                    case 3:
                        thread = new Thread(secondMethod);
                        break;
                    default:
                        thread = new Thread(sixthMethod);
                        break;
                }
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        public void firstMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            thirdMethod();
            tracer.StopTrace();
        }

        public void secondMethod()
        {
            tracer.StartTrace();
            var threads = new List<Thread>();

            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(fifthMethod);
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            Thread.Sleep(200);
            fourthMethod();
            fifthMethod();
            tracer.StopTrace();
        }

        public void thirdMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(300);
            tracer.StopTrace();
        }

        public void fourthMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(400);
            tracer.StopTrace();
        }

        public void fifthMethod()
        {
            tracer.StartTrace();
            var threads = new List<Thread>();

            for (int i = 0; i < 2; i++)
            {
                Thread thread = new Thread(sixthMethod);
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            Thread.Sleep(500);
            sixthMethod();
            tracer.StopTrace();
        }

        public void sixthMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(600);
            tracer.StopTrace();
        }
    }
}
