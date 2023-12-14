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

        public ProjectsList()
        {
            InitializeComponent();
            DataContext = this;
            LoadProjectData();
        }

        private void LoadProjectData()
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
            Project newProject = new Project
            {
                ProjektBezeichnung = "New Project",
                VerantwortlichePersonalnummer = "1",
                VonDatum = DateTime.Now,
                BisDatum = DateTime.Now.AddDays(10)
            };

            ProjectDetailsWindow ProjectDetailsWindow = new ProjectDetailsWindow(new Project());
            ProjectDetailsWindow.Owner = this;

            ProjectDetailsWindow.ShowDialog();

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
                projectDetailsWindow.Owner = this;

                projectDetailsWindow.ShowDialog();

                LoadProjectData();
            }
            else
            {
                MessageBox.Show("Please select project to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
