using System;
using System.Collections.Generic;
using System.Windows;

namespace projectmanagement
{
    public partial class ProjektDetailsWindow : Window
    {
        //  list of employees for the "Verantwortlicher" selection
        private List<Mitarbeiter> mitarbeiter;

        public ProjektDetailsWindow(Project project)
        {
            InitializeComponent();
            InitializeEmployeeList(); // Initialize the list of employees

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

        private void InitializeEmployeeList()
        {
            // Initialize the list of employees (replace this with your actual data retrieval logic)
            mitarbeiter = new List<Mitarbeiter>
            {
                new mitarbeiter { Name = "Employee1" },
                new Employee { Name = "Employee2" },
                // Add more employees as needed
            };
        }

        private void AuswaehlenButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the employee selection window
            EmployeeSelectionWindow employeeSelectionWindow = new EmployeeSelectionWindow(employees);
            employeeSelectionWindow.Owner = this;

            if (employeeSelectionWindow.ShowDialog() == true)
            {
                // Set the selected employee in the "Verantwortlicher" TextBox
                TextBoxVerantwortlicher.Text = employeeSelectionWindow.SelectedEmployee.Name;
            }
        }

        private void HinzufuegenButton_Click(object sender, RoutedEventArgs e)
        {
            // Add logic to handle the addition of phases (Phasen) here
            // You may open a new window or use a dialog to input phase details
            // and update the UI accordingly.
        }
    }
}
