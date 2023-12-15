using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Linq;

namespace projectmanagement
{
    public partial class ProjectDetailsWindow : Window
    {
        //  list of employees for the "Verantwortlicher" selection
        private List<Employee> mitarbeiter;

        public ProjectDetailsWindow(Project project)
        {
            InitializeComponent();
           
            // Set data context or populate fields based on the provided project
            // For simplicity, let's assume the Project class has properties Bezeichnung, Startdatum, Enddatum, etc.
            if (project != null)
            {
                TextBoxBezeichnung.Text = project.ProjektBezeichnung;
                DatePickerStartdatum.SelectedDate = project.Startdatum;
                DatePickerEnddatum.SelectedDate = project.Enddatum;
                // Set other fields accordingly
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeSelectionWindow selectionWindow = new EmployeeSelectionWindow();
            selectionWindow.Owner = this;

            if (selectionWindow.ShowDialog() == true)
            {
                var selectedEmployee = selectionWindow.SelectedEmployee;
                if (selectedEmployee != null)
                {
                    TextBoxVerantwortlicher.Text = $"{selectedEmployee.Vorname} {selectedEmployee.Nachname}";
                }
            }
        }



        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInput())
            {
                // Create a new Project object with the entered data
                Project newProject = new Project
                {
                    ProjektBezeichnung = TextBoxBezeichnung.Text,
                    Verantwortlicher = TextBoxVerantwortlicher.Text,
                    Startdatum = DatePickerStartdatum.SelectedDate.GetValueOrDefault(DateTime.Now), // or handle null differently
                    Enddatum = DatePickerEnddatum.SelectedDate.GetValueOrDefault(DateTime.Now)

                };

                Backend.UpdateProject(newProject);

                /*if (Owner is ProjectsList mainWindow)
                {
                    mainWindow.LoadProjectData();
                }
                // Close the window after saving*/
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidInput()
        {
            return !string.IsNullOrWhiteSpace(TextBoxBezeichnung.Text) ||
           DatePickerStartdatum.SelectedDate.HasValue ||
           DatePickerEnddatum.SelectedDate.HasValue ||
           !string.IsNullOrWhiteSpace(TextBoxVerantwortlicher.Text);
        }
    }
}
