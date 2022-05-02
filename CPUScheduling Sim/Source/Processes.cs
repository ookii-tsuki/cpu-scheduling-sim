using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUScheduling_Sim.Source
{
    public class Processes : List<Process>, ICloneable
    {
        public TimeSpan AverageWaitingTime { get; set; }
        public TimeSpan AverageTurnAroundTime { get; set; }

        public object Clone()
        {
            Processes processes = new Processes();
            foreach (var process in this)
            {
                processes.Add((Process)process.Clone());
            }
            return processes;
        }
    }
    public class Process : ICloneable
    {
        public int PID { get; set; }
        public TimeSpan ArriveTime { get; set; }
        public TimeSpan CPUTime { get; set; }
        public int Priority { get; set; }

        public object Clone()
        {
            var process = (Process)MemberwiseClone();
            return process;
        }
    }
}
