using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace TracerLib
{
    [Serializable]
    [DataContract]
    public sealed class MethodInfo
    {
        private string methodName;
        private string className;
        private long time;

        private List<MethodInfo> methodsList;
        private Stopwatch stopWatch;

        [DataMember(Name = "name", Order = 0)]
        public string MethodName
        {
            get { return methodName; }
            private set { }
        }
        [DataMember(Name = "class", Order = 1)]
        public string ClassName
        {
            get { return className; }
            private set { }
        }

        [DataMember(Name = "time", Order = 2)]
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

        [DataMember(Name = "methods", Order = 3)]
        public List<MethodInfo> Methodlist
        {
            get { return methodsList; }
            private set { }
        }

        public MethodInfo()
        {
            methodsList = new List<MethodInfo>();
            stopWatch = new Stopwatch();
            time = 0;
        }

        public MethodInfo(MethodBase method)
        {
            methodsList = new List<MethodInfo>();
            stopWatch = new Stopwatch();
            methodName = method.Name;
            className = method.DeclaringType.Name;
            time = 0;
        }

        public void StartTrace()
        {
            stopWatch.Start();
        }

        public void StopTrace()
        {
            stopWatch.Stop();
            time = stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
        }

        public void AddNestedMethod(MethodInfo method)
        {
            methodsList.Add(method);
        }
    }
}
