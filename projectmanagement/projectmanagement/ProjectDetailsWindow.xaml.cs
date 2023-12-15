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
                DatePickerStartdatum.SelectedDate = project.VonDatum;
                DatePickerEnddatum.SelectedDate = project.BisDatum;
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
                    VerantwortlichePersonalnummer = TextBoxVerantwortlicher.Text,
                    VonDatum = DatePickerStartdatum.SelectedDate.GetValueOrDefault(DateTime.Now), // or handle null differently
                    BisDatum = DatePickerEnddatum.SelectedDate.GetValueOrDefault(DateTime.Now)

                };

                Backend.SaveProjectToDatabase(newProject);

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
            bool validMainDetails = !string.IsNullOrWhiteSpace(TextBoxBezeichnung.Text) &&
                                    !string.IsNullOrWhiteSpace(TextBoxVerantwortlicher.Text) &&
                                    DatePickerStartdatum.SelectedDate.HasValue &&
                                    DatePickerEnddatum.SelectedDate.HasValue;
            if (!validMainDetails)
            {
                return false; // Return false immediately if main details are not valid
            }
            // Assuming the start date should be before or equal to the end date
            bool validDateRange = DatePickerStartdatum.SelectedDate <= DatePickerEnddatum.SelectedDate;

            return validDateRange;
        }
    }
}
