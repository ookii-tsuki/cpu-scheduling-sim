using CPUScheduling_Sim.Source;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CPUScheduling_Sim
{
    internal class Chart
    {
        private static Dictionary<int, Color> chartColors = new Dictionary<int, Color>();

        /// <summary>
        /// Generates gantt chart from processes
        /// </summary>
        /// <param name="panel">panel in UI</param>
        /// <param name="processes">processes to generate chart from</param>
        /// <returns>List of grids representing the gantt chart</returns>
        public static List<Grid> GenerateChart(StackPanel panel, Processes processes)
        {
            List<Grid> grids = new List<Grid>(processes.Count);
            double totalTime = Scheduler.FindCompletionTime(processes, processes.Count - 1).TotalMilliseconds;
            var offset = Scheduler.FindCompletionTime(processes, 0);

            if(processes[0].ArriveTime != TimeSpan.Zero)
            {
                var width = (processes[0].ArriveTime.TotalMilliseconds / totalTime) * (panel.Width - 5);
                grids.Add(BlankBlock(panel.Height, width));
            }

            for (int i = 0; i < processes.Count; i++)
            {
                Process? process = processes[i];
                Grid grid = new Grid();

                var thisOffset = Scheduler.FindCompletionTime(processes, i);

                if (i > 0 && thisOffset - offset != process.CPUTime)
                {
                    var width = ((thisOffset - offset - process.CPUTime).TotalMilliseconds / totalTime) * (panel.Width - 5);
                    grids.Add(BlankBlock(panel.Height, width));
                }

                Border block = new Border();

                block.Height = panel.Height;
                block.Width = (process.CPUTime.TotalMilliseconds / totalTime) * (panel.Width - 5);
                offset = thisOffset;

                GenerateColors();
                block.Background = new SolidColorBrush(chartColors[process.PID]);

                if (grids.Count == 0)
                    block.CornerRadius = new CornerRadius(10, 0, 0, 10);
                else if(i == processes.Count - 1)
                    block.CornerRadius = new CornerRadius(0, 10, 10, 0);


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

                grid.Children.Add(block);
                grid.Children.Add(cpuTime);
                grid.Children.Add(ct);
                grid.Children.Add(pid);

                grids.Add(grid);
            }
            return grids;
        }


        static double? currentHue = null;
        /// <summary>
        /// Generates random color palette using the golden ratio scheme
        /// </summary>
        private static void GenerateColors()
        {
            var processes = Scheduler.Processes;
            var processCount = processes.Count;

            double goldenRatioConjugate = 0.618033988749895;
            currentHue ??= new Random().NextDouble();

            for (int i = 0; i < processCount; i++)
            {
                if (chartColors.ContainsKey(processes[i].PID))
                    continue;

                chartColors.Add(processes[i].PID, HSLToRGB((double)currentHue, .9, .75));
                currentHue += goldenRatioConjugate;
                currentHue %= 1;
            }
        }

        private static Color HSLToRGB(double h, double s, double l)
        {
            double[] t = new double[] { 0, 0, 0 };
            byte r = 0;
            byte g = 0;
            byte b = 0;

            if (s == 0)
            {
                r = g = b = (byte)(l * 255);
                return Color.FromRgb(r, g, b);
            }

            double q, p;

            q = l < 0.5 ? l * (1 + s) : l + s - (l * s);
            p = 2 * l - q;

            t[0] = h + (1.0 / 3.0);
            t[1] = h;
            t[2] = h - (1.0 / 3.0);

            for (byte i = 0; i < 3; i++)
            {
                t[i] = t[i] < 0 ? t[i] + 1.0 : t[i] > 1 ? t[i] - 1.0 : t[i];

                if (t[i] * 6.0 < 1.0)
                    t[i] = p + ((q - p) * 6 * t[i]);
                else if (t[i] * 2.0 < 1.0)
                    t[i] = q;
                else if (t[i] * 3.0 < 2.0)
                    t[i] = p + ((q - p) * 6 * ((2.0 / 3.0) - t[i]));
                else
                    t[i] = p;
            }
            return Color.FromRgb((byte)(t[0] * 255), (byte)(t[1] * 255), (byte)(t[2] * 255));
        }
        private static Grid BlankBlock(double height, double width)
        {
            Rectangle[] rec = new Rectangle[2];
            for (int j = 0; j < rec.Length; j++)
            {
                var r = new Rectangle();
                r.Height = height;
                r.Width = width;
                rec[j] = r;
            }
            rec[0].Fill = PatternBrush();
            rec[1].Fill = new SolidColorBrush(Color.FromArgb(20, 0, 0, 0));

            var blank = new Grid();
            blank.Children.Add(rec[0]);
            blank.Children.Add(rec[1]);

            return blank;
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
