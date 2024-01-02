using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static projectmanagement.Backend;

namespace projectmanagement
{
 
    public partial class GanttChartWindow : Window
    {
        public int ProjectId { get; private set; }

        public GanttChartWindow()
        {
            InitializeComponent();
        }

        public GanttChartWindow(int projectId) : this()
        {
            ProjectId = projectId;
            this.Loaded += GanttChartWindow_Loaded; // Add Loaded event handler
        }
        private void GanttChartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowGanttDiagram(ProjectId); // Call to display the Gantt chart
        }
        private void DrawGanttChart(List<GanttEntry> ganttEntries)
        {
            // Define a starting point for the Y-axis
            double currentTop = 10; // 10 pixels from the top of the canvas
            double barHeight = 20; // The height of each Gantt bar

            foreach (var entry in ganttEntries)
            {
                // Create a new rectangle to represent the Gantt bar
                Rectangle rect = new Rectangle
                {
                    Width = entry.duration * 10, // Assuming 10 pixels per time unit for width
                    Height = barHeight,
                    Fill = Brushes.Blue, // Fill color for the Gantt bar
                    Stroke = Brushes.Black // Border color for the Gantt bar
                };

                // Position the rectangle on the canvas
                Canvas.SetLeft(rect, entry.startTime * 10); // Assuming 10 pixels per time unit for positioning
                Canvas.SetTop(rect, currentTop);

                // Add the rectangle to the canvas
                GanttChartCanvas.Children.Add(rect);

                // Add text to display the phase name
                TextBlock txt = new TextBlock
                {
                    Text = entry.phaseName,
                    Foreground = Brushes.White
                };
                Canvas.SetLeft(txt, entry.startTime * 10);
                Canvas.SetTop(txt, currentTop);
                GanttChartCanvas.Children.Add(txt);

                // Increment the top position for the next bar
                currentTop += barHeight + 5; // 5 pixels space between bars
            }

            // Update the canvas size to fit all Gantt bars
            GanttChartCanvas.Width = ganttEntries.Max(e => e.startTime + e.duration) * 10;
            GanttChartCanvas.Height = currentTop;
        }

        // Call this method when you want to display the Gantt chart
        public void ShowGanttDiagram(int projectID)
        {
            var ganttList = GetGanttList(projectID);
            DrawGanttChart(ganttList);
        }
    }
}
