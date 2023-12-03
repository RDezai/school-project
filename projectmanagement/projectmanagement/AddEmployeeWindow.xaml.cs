// AddEmployeeWindow.xaml.cs

using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace projectmanagement
{
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow(Mitarbeiter newEmployee)
        {
            InitializeComponent();

            // Set initial values if provided
            txtName.Text = newEmployee.Nachname;
            txtVorname.Text = newEmployee.Vorname;
            txtAbteilung.Text = newEmployee.Abteilung;
            txtTelefon.Text = newEmployee.Tel_Nr;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input and save the new employee to the database
            if (IsValidInput())
            {
                // Create a new Mitarbeiter object with the entered data
                Mitarbeiter newEmployee = new Mitarbeiter
                {
                    Vorname = txtVorname.Text,
                    Nachname = txtName.Text,
                    Abteilung = txtAbteilung.Text,
                    Tel_Nr = txtTelefon.Text
                };

                Backend.SaveEmployeeToDatabase(newEmployee);

                if (Owner is EmployeeWindow mainWindow)
                {
                    mainWindow.LoadEmployeeData();
                }
                // Close the window after saving
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidInput()
        {
            // Add validation logic here (e.g., checking if required fields are not empty)
            return !string.IsNullOrWhiteSpace(txtVorname.Text) &&
                   !string.IsNullOrWhiteSpace(txtName.Text) &&
                   !string.IsNullOrWhiteSpace(txtAbteilung.Text) &&
                   !string.IsNullOrWhiteSpace(txtTelefon.Text);
        }

        // Optional: Add TextChanged event handlers for further validation or dynamic updates
        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Add custom logic if needed
        }

        private void txtTelefon_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Add custom logic if needed
        }
    }
}
