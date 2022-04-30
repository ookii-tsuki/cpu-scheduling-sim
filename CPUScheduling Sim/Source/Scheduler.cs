using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUScheduling_Sim.Source
{
    internal enum Algorithm
    {
        FCFS = 1,
        SJF_PREEMPTIVE,
        SJF_NONPREEMPTIVE,
        PRIORITY,
        ROUND_ROBIN
    }
    internal class Scheduler
    {
        public static Processes Processes { get; set; } = new Processes();
        public static Processes ScheduleProcesses(Algorithm algorithm) => algorithm switch
        {
            Algorithm.FCFS => FCFSSchedule(),
            Algorithm.SJF_PREEMPTIVE => SJFPreSchedule(),
            Algorithm.SJF_NONPREEMPTIVE => SJFNonPreSchedule(),
            Algorithm.PRIORITY => PrioritySchedule(),
            Algorithm.ROUND_ROBIN => RoundRobinSchedule(),
            _ => throw new NotImplementedException("Please select a valid algorithm")
        };

        private static Processes FCFSSchedule()
        {
            Processes processes = new Processes();

            processes.AddRange(Processes.OrderBy(p => p.ArriveTime));

            var avgTrTime = TimeSpan.Zero;
            var avgWtTime = TimeSpan.Zero;
            for (int i = 0; i < processes.Count; i++)
            {
                var ct = FindCompletionTime(processes, i);
                var arrival = processes[i].ArriveTime;
                var cpuTime = processes[i].CPUTime;

                avgTrTime += ct - arrival;
                avgWtTime += ct - arrival - cpuTime;
            }

            avgTrTime /= processes.Count;
            avgWtTime /= processes.Count;

            processes.AverageTurnAroundTime = avgTrTime;
            processes.AverageWaitingTime = avgWtTime;

            return processes;
        }

        private static Processes SJFPreSchedule()
        {
            Processes processes = new Processes();
            // do the sjf preemtive sort and calculate properties
            return processes;
        }
        private static Processes SJFNonPreSchedule()
        {
            Processes processes = new Processes();
            // do the sjf non-preemtive sort and calculate properties
            return processes;
        }
        private static Processes PrioritySchedule()
        {
            Processes processes = new Processes();
            // do the priority sort and calculate properties
            return processes;
        }
        private static Processes RoundRobinSchedule()
        {
            Processes processes = new Processes();
            // do the roundrobin sort sort and calculate properties
            return processes;
        }

        public static TimeSpan FindCompletionTime(Processes processes, int index)
        {
            TimeSpan completionTime = processes[0].ArriveTime;
            int i = 0;
            while(i <= index)
            {
                var arrive = processes[i].ArriveTime;
                if(arrive > completionTime)
                    completionTime += arrive - completionTime;
                else
                {
                    completionTime += processes[i].CPUTime;
                    i++;
                }
            }
            return completionTime;
        }
    }
}
