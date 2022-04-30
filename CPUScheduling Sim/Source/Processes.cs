using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUScheduling_Sim.Source
{
    public class Processes : List<Process>
    {
        public TimeSpan AverageWaitingTime { get; set; }
        public TimeSpan AverageTurnAroundTime { get; set; }
    }
    public class Process
    {
        public int PID { get; set; }
        public TimeSpan ArriveTime { get; set; }
        public TimeSpan CPUTime { get; set; }
        public int Priority { get; set; }
    }
}
