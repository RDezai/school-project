using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls.Primitives;
using projectmanagement.src; 

namespace projectmanagement
{
    public partial class ProjectsList : Window
    {
        private object newProject;
        private Project currentProject;

        public ProjectsList()
        {
            InitializeComponent();
            DataContext = this;
            LoadProjectData();
        }

        public void LoadProjectData()
        {
            try
            {
                // Fetch data from the database using Backend
                List<Project> projects = Backend.GetProjectList();

                // Bind data to the DataGrid
                dataGrid.ItemsSource = projects;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading phase data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            // Initialize a new project with default values
            currentProject = new Project
            {
                Name = "New Project",
                Verantwortlicher = "",
                Startdatum = DateTime.Now,
                Enddatum = DateTime.Now.AddDays(10)
            };

            // Open the ProjectDetailsWindow for the new project
            ProjectDetailsWindow projectDetailsWindow = new ProjectDetailsWindow(currentProject);
            projectDetailsWindow.Owner = this;
            projectDetailsWindow.ShowDialog();

            // After the window is closed, reload the project data
            LoadProjectData();
        }

        /*private AddProjektPhaseWindow AddProjektPhaseWindow(Projektphasen newPhase)
        {
            throw new NotImplementedException();
        }*/

        private void EditProjectButton_Click(object sender, RoutedEventArgs e)
        {
            Project selectedProject = dataGrid.SelectedItem as Project;

            if (selectedProject != null)
            {
                ProjectDetailsWindow projectDetailsWindow = new ProjectDetailsWindow(selectedProject);
                projectDetailsWindow.currentProject = selectedProject; // Pass the selected project to the details window
                projectDetailsWindow.Owner = this;
                projectDetailsWindow.ShowDialog();

                LoadProjectData(); // Refresh the project list
            }
            else
            {
                MessageBox.Show("Please select a project to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            // Handle deleting the selected project
            Project selectedProject = dataGrid.SelectedItem as Project;

            if (selectedProject != null)
            {
                // Delete the selected project from the database
                if (MessageBox.Show("Are you sure you want to delete this prject?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Backend.DeleteProject(selectedProject);
                    LoadProjectData();
                }
            }
            else
            {
                MessageBox.Show("Please select a project to delete.",
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void OpenGanttDiagram_Click(object sender, RoutedEventArgs e)
        {
            // Get the currently selected project from the dataGrid
            var selectedProject = dataGrid.SelectedItem as Project;

            if (selectedProject != null)
            {
                // Use the projectID of the selected project
                int projectId = selectedProject.Proj_ID;

                // Create and show the GanttChartWindow, passing the selected project's ID
                GanttChartWindow ganttWindow = new GanttChartWindow(projectId);
                ganttWindow.Show(); // or ShowDialog(), depending on your needs
            }
            else
            {
                MessageBox.Show("Please select a project first.");
            }
        }



    }
}
