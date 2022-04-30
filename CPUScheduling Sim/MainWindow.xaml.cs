using CPUScheduling_Sim.Source;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPUScheduling_Sim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Scheduler.Processes.Add(new Process { PID = 1, ArriveTime = TimeSpan.FromMinutes(4), CPUTime = TimeSpan.FromMinutes(5), Priority = 1 });
            Scheduler.Processes.Add(new Process { PID = 2, ArriveTime = TimeSpan.FromMinutes(6), CPUTime = TimeSpan.FromMinutes(4), Priority = 3 });
            Scheduler.Processes.Add(new Process { PID = 3, ArriveTime = TimeSpan.Zero, CPUTime = TimeSpan.FromMinutes(3), Priority = 2 });
            Scheduler.Processes.Add(new Process { PID = 4, ArriveTime = TimeSpan.FromMinutes(6), CPUTime = TimeSpan.FromMinutes(2), Priority = 5 });
            Scheduler.Processes.Add(new Process { PID = 5, ArriveTime = TimeSpan.FromMinutes(5), CPUTime = TimeSpan.FromMinutes(4), Priority = 4 });

            
            DataContext = this;
            processTable.ItemsSource = Scheduler.Processes;
        }

        void Calculate()
        {
            if (algothimsCB.SelectedIndex == 0)
                return;

            if (Scheduler.Processes.Count == 0)
                return;

            var processes = Scheduler.ScheduleProcesses((Algorithm)algothimsCB.SelectedIndex);

            if(processStack.Children.Count > 0)
                processStack.Children.Clear();

            var chart = Chart.GenerateChart(processStack, processes);
            foreach (var item in chart)
                processStack.Children.Add(item);

            avgWaitingTime.Text = $"Average Waiting Time: {processes.AverageWaitingTime}";
            avgTurnAroundTime.Text = $"Average Turn Around Time: { processes.AverageTurnAroundTime}";
        }

        private void addProcess_Click(object sender, RoutedEventArgs e)
        {
            addDialogBox.IsOpen = true;
        }

        private void dialog_AddProcess_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process
            {
                PID = Scheduler.Processes.Count + 1,
                ArriveTime = TimeSpan.FromMinutes(float.Parse(arriveTimeTB.Text)),
                CPUTime = TimeSpan.FromMinutes(float.Parse(cpuTimeTB.Text)),
                Priority = int.Parse(priorityTB.Text)
            };
            Scheduler.Processes.Add(process);

            processTable.Items.Refresh();
            Calculate();

            addDialogBox.IsOpen = false;
        }

        private void dialog_Cancel_Click(object sender, RoutedEventArgs e)
        {
            addDialogBox.IsOpen = false;
        }

        private void algothimsCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            Scheduler.Processes.Clear();

            processTable.Items.Refresh();

            if (processStack.Children.Count > 0)
                processStack.Children.Clear();

            avgWaitingTime.Text = "Average Waiting Time:";
            avgTurnAroundTime.Text = "Average Turn Around Time:";
        }

        private void processTable_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var selectedIndex = processTable.SelectedIndex;
            if (e.Key != Key.Delete || selectedIndex < 0)
                return;

            Scheduler.Processes.RemoveAt(selectedIndex);

            processTable.Items.Refresh();
            Calculate();
        }
    }
}
