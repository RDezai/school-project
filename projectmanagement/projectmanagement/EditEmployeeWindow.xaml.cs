using projectmanagement;
using System.Data.SQLite;
using System.Windows;
using System.Xml.Linq;
using System;

public partial class EditEmployeeWindow : Window
{
    private employee existingEmployee;
    private object txtName;
    private object txtVorname;

    public EditEmployeeWindow(employee existingEmployee)
    {
        InitializeComponent();

        this.existingEmployee = existingEmployee;

        // Set initial values if provided
        txtName.Text = existingEmployee.Nachname;
        txtVorname.Text = existingEmployee.Vorname;
        txtAbteilung.Text = existingEmployee.Abteilung;
        txtTelefon.Text = existingEmployee.Tel_Nr;
    }

    private void InitializeComponent()
    {
        throw new NotImplementedException();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Validate input and save the employee changes to the database
        if (IsValidInput())
        {
            // Update the existing Mitarbeiter object with the entered data
            existingEmployee.Vorname = txtVorname.Text;
            existingEmployee.Nachname = txtName.Text;
            existingEmployee.Abteilung = txtAbteilung.Text;
            existingEmployee.Tel_Nr = txtTelefon.Text;

            SaveEmployeeToDatabase(existingEmployee);

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

}
