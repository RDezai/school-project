// EmployeeWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using projectmanagement;

namespace projectmanagement
{
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            try
            {
                // Fetch data from the database
                List<Mitarbeiter> employees = GetEmployeesFromDatabase();

                // Bind data to the DataGrid
                dataGrid.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employee data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<Mitarbeiter> GetEmployeesFromDatabase()
        {
            // Get the connection string for the SQLite database
            string connectionString = GetConnectionString();

            // Construct the SELECT query to fetch specific columns from the Mitarbeiter table
            string selectQuery = "SELECT Vorname, Nachname, Tel_Nr, Abteilung FROM " + Mitarbeiter.GetTableName();

            // Using statement for automatic resource management (connection and command)
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                // Open the database connection
                connection.Open();

                // Create a list to store Mitarbeiter objects (employees)
                List<Mitarbeiter> employees = new List<Mitarbeiter>();

                // Using statement for automatic resource management (reader)
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Iterate through each row in the result set
                    while (reader.Read())
                    {
                        // Create a new Mitarbeiter object for each row
                        Mitarbeiter employee = new Mitarbeiter
                        {
                            // Assign properties based on database columns
                            Vorname = reader["Vorname"].ToString(),
                            Nachname = reader["Nachname"].ToString(),
                            Tel_Nr = reader["Tel_Nr"].ToString(),
                            Abteilung = reader["Abteilung"].ToString()
                        };

                        // Add the Mitarbeiter object to the list
                        employees.Add(employee);
                    }
                }

                // Close the database connection
                connection.Close();

                // Return the list of Mitarbeiter objects
                return employees;
            }
        }


        private static string GetConnectionString()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            int projectManagementPosition = path.IndexOf("projectmanagement");
            path = path.Substring(0, projectManagementPosition);
            return "Data Source=" + path + "projectmanagement\\database\\database.db";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            Mitarbeiter newEmployee = new Mitarbeiter
            {
                Vorname = "New",
                Nachname = "Employee",
                Tel_Nr = "123456789",
                Abteilung = "IT"
            };

            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow(newEmployee);
            addEmployeeWindow.Owner = this;
            addEmployeeWindow.ShowDialog();

            LoadEmployeeData();

        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //Handle editing selected employee
            //Implement the logic to open a new window and edit selected employee.
            Mitarbeiter selectedEmployee = dataGrid.SelectedItem as Mitarbeiter;

            if (selectedEmployee != null)
            {
                EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(selectedEmployee);
                editEmployeeWindow.Owner = this;
                editEmployeeWindow.ShowDialog();

                //Refresh the Data Grid after editing an employee.
                LoadEmployeeData();
            }
            else
            {
                MessageBox.Show("Please select an employee to edit.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //Handle deleting the selected Employee
            Mitarbeiter selectedEmployee = dataGrid.SelectedItem as Mitarbeiter;

            if (selectedEmployee != null)
            {
                //Delete selected employee from database.
                if (MessageBox.Show("Are you sure you want to delete this employee?", 
                    "Confirmation", 
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DeleteEmployee(selectedEmployee);
                    LoadEmployeeData();
                }
            } 
            else
            {
                MessageBox.Show("Please select an employee to delete.", 
                    "Information", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
            }
        }

        private void DeleteEmployee(Mitarbeiter employee)
        {
            try
            {
                string connectionString = GetConnectionString();
                string deleteQuery = $"DELETE FROM {Mitarbeiter.GetTableName()} WHERE Vorname = @Vorname AND Nachname = @Nachname";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Vorname", employee.Vorname);
                    command.Parameters.AddWithValue("@Vorname", employee.Nachname);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
