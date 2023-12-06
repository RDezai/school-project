using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls.Primitives;
using projectmanagement.src; 

namespace projectmanagement
{
    public partial class ProjekteList : Window
    {
        private object newProjekt;

        public ProjekteList()
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

        private void AddProjektButton_Click(object sender, RoutedEventArgs e)
        {
            Project newProject = new Project
            {
                ProjektBezeichnung = "New Project",
                VerantwortlichePersonalnummer = 1,
                VonDatum = DateTime.Now,
                BisDatum = DateTime.Now.AddDays(10)
            };

            // New Window for adding new project
            AddProjektWindow addProjektWindow = new AddProjektWindow(newProjekt);
            addProjektWindow.Owner = this; // Uncomment if needed

            addProjektWindow.Show();

            LoadProjectData();
        }

        private AddProjektPhaseWindow AddProjektPhaseWindow(Projektphasen newPhase)
        {
            throw new NotImplementedException();
        }

        private void EditProjektButton_Click(object sender, RoutedEventArgs e)
        {
            Project selectedProject = dataGrid.SelectedItem as Project;

            if (selectedProject != null)
            {
                //Open a new window to edit the selected project
                EditProjektWindow editProjektWindow = new EditProjektWindow(selectedProject);
                editProjektWindow.Owner = this;
                editProjektWindow.Show();

                LoadProjectData();
            }
            else
            {
                MessageBox.Show("Please select a phase to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteProjekt_Click(object sender, RoutedEventArgs e)
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
