using CPUScheduling_Sim.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CPUScheduling_Sim
{
    internal class Chart
    {
        public static List<Grid> GenerateChart(StackPanel panel, Processes processes)
        {
            List<Grid> grids = new List<Grid>(processes.Count);
            double totalTime = processes.Select(x => x.CPUTime.TotalMilliseconds).Sum();

            for (int i = 0; i < processes.Count; i++)
            {
                Process? process = processes[i];
                Grid grid = new Grid();
                Rectangle rectangle = new Rectangle();

                rectangle.Height = panel.Height;
                rectangle.Width = (process.CPUTime.TotalMilliseconds / totalTime) * panel.Width;

                var random = new Random();
                Color c = Color.FromRgb((byte)random.Next(200, 255), (byte)random.Next(150, 255), (byte)random.Next(150, 255));

                rectangle.Fill = new SolidColorBrush(c);

                TextBlock cpuTime = new TextBlock();

                cpuTime.Text = $"{process.CPUTime.TotalMilliseconds}ms";
                cpuTime.HorizontalAlignment = HorizontalAlignment.Center;
                cpuTime.VerticalAlignment = VerticalAlignment.Center;
                cpuTime.TextAlignment = TextAlignment.Center;
                

                TextBlock ct = new TextBlock();
                ct.HorizontalAlignment = HorizontalAlignment.Right;
                ct.VerticalAlignment = VerticalAlignment.Top;
                ct.Margin = new Thickness(0, 3, 5, 0);
                ct.Text = $"{Scheduler.FindCompletionTime(processes, i).Milliseconds}";

                TextBlock pid = new TextBlock();
                pid.HorizontalAlignment = HorizontalAlignment.Left;
                pid.VerticalAlignment = VerticalAlignment.Bottom;
                pid.Margin = new Thickness(5, 0, 0, 3);
                pid.Text = $"P{process.PID}";
                pid.FontSize = 9;

                grid.Children.Add(rectangle);
                grid.Children.Add(cpuTime);
                grid.Children.Add(ct);
                grid.Children.Add(pid);

                grids.Add(grid);
            }
            return grids;
        }
    }
}
