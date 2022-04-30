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

                TextBlock textBlock = new TextBlock();

                textBlock.Text = $"{Scheduler.FindCompletionTime(processes, i).TotalMinutes}min\n{process.CPUTime.ToString(@"mm\:ss")}";
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.TextAlignment = TextAlignment.Center;

                TextBlock pid = new TextBlock();
                pid.HorizontalAlignment = HorizontalAlignment.Left;
                pid.VerticalAlignment = VerticalAlignment.Top;
                pid.Margin = new Thickness(3, 3, 0, 0);
                pid.Text = $"P{process.PID}";

                grid.Children.Add(rectangle);
                grid.Children.Add(textBlock);
                grid.Children.Add(pid);

                grids.Add(grid);
            }
            return grids;
        }
    }
}
