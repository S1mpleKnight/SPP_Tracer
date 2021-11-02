using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TracerLib
{
    [Serializable]
    [DataContract]
    public sealed class ThreadInfo
    {
        private int id;
        private long time;
        private List<MethodInfo> methods;
        private Stack<MethodInfo> callMethods;

        [DataMember(Name = "id", Order = 0)]
        public int Id
        {
            get { return id; }
            private set { }
        }
        [DataMember(Name = "time", Order = 1)]
        public string Time
        {
            get { return time.ToString() + "ms"; }
            private set { }
        }
        [XmlIgnore]
        public long TimeInt
        {
            get { return time; }
        }
        [DataMember(Name = "methods", Order = 2)]
        public List<MethodInfo> Methods
        {
            get { return methods; }
            private set { }
        }

        public ThreadInfo()
        {
            time = 0;
            methods = new List<MethodInfo>();
            callMethods = new Stack<MethodInfo>();
        }

        public ThreadInfo(int threadID)
        {
            id = threadID;
            time = 0;
            methods = new List<MethodInfo>();
            callMethods = new Stack<MethodInfo>();
        }

        public void StartTrace(MethodInfo method)
        {
            if (callMethods.Count == 0)
            {
                methods.Add(method);
            }
            else
            {
                callMethods.Peek().AddNestedMethod(method);
            }

            callMethods.Push(method);
            method.StartTrace();
        }

        public void StopTrace()
        {
            MethodInfo lastMethod = callMethods.Peek();
            lastMethod.StopTrace();
            if (callMethods.Count == 1)
            {
                time += lastMethod.TimeInt;
            }

            callMethods.Pop();
        }
    }
}
