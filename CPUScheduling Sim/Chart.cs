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
        private static Dictionary<int, Color> chartColors = new Dictionary<int, Color>();
        public static List<Grid> GenerateChart(StackPanel panel, Processes processes)
        {
            List<Grid> grids = new List<Grid>(processes.Count);
            double totalTime = Scheduler.FindCompletionTime(processes, processes.Count - 1).TotalMilliseconds;
            var offset = Scheduler.FindCompletionTime(processes, 0);
            for (int i = 0; i < processes.Count; i++)
            {
                Process? process = processes[i];
                Grid grid = new Grid();

                var thisOffset = Scheduler.FindCompletionTime(processes, i);

                if (i > 0 && thisOffset - offset != process.CPUTime)
                {
                    Rectangle rec = new Rectangle();
                    rec.Height = panel.Height;
                    rec.Width = ((thisOffset - offset - process.CPUTime).TotalMilliseconds / totalTime) * (panel.Width - 5);
                    rec.Fill = PatternBrush();

                    var blank = new Grid();
                    blank.Children.Add(rec);
                    grids.Add(blank);
                }

                Rectangle rectangle = new Rectangle();

                rectangle.Height = panel.Height;
                rectangle.Width = (process.CPUTime.TotalMilliseconds / totalTime) * (panel.Width - 5);
                offset = thisOffset;

                GenerateColors();
                rectangle.Fill = new SolidColorBrush(chartColors[process.PID]);


                TextBlock cpuTime = new TextBlock();

                cpuTime.Text = $"{process.CPUTime.TotalMilliseconds}ms";
                cpuTime.HorizontalAlignment = HorizontalAlignment.Center;
                cpuTime.VerticalAlignment = VerticalAlignment.Center;
                cpuTime.TextAlignment = TextAlignment.Center;


                TextBlock ct = new TextBlock();
                ct.HorizontalAlignment = HorizontalAlignment.Right;
                ct.VerticalAlignment = VerticalAlignment.Top;
                ct.Margin = new Thickness(0, 3, 5, 0);
                ct.Text = $"{thisOffset.Milliseconds}";

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
        private static void GenerateColors()
        {
            var processes = Scheduler.Processes;
            var processCount = processes.Count;
            for (int i = chartColors.Count; i < processCount; i++)
            {
                var random = new Random();
                Color c = Color.FromRgb((byte)random.Next(200, 255), (byte)random.Next(150, 255), (byte)random.Next(150, 255));
                chartColors.Add(processes[i].PID, c);
            }
        }
        private static Brush PatternBrush()
        {
            VisualBrush vb = new VisualBrush();

            vb.TileMode = TileMode.Tile;
            vb.Viewport = new Rect(0, 0, 10, 10);
            vb.ViewportUnits = BrushMappingMode.Absolute;

            vb.Viewbox = new Rect(0, 0, 10, 10);
            vb.ViewboxUnits = BrushMappingMode.Absolute;

            Line line = new Line();
            line.Stroke = Brushes.Gray;
            var rotation = new RotateTransform();
            rotation.Angle = 45;
            line.RenderTransform = rotation;
            line.X1 = 0;
            line.Y1 = 0;
            line.X2 = 10;
            line.Y2 = 0;

            vb.Visual = line;

            return vb;
        }
    }
}
