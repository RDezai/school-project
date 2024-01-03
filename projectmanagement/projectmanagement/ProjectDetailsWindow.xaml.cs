using projectmanagement.src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace projectmanagement
{
    public partial class ProjectDetailsWindow : Window
    {
        public ObservableCollection<Projectphases> ProjectPhasesCollection { get; set; }

        public Project currentProject { get; set; } // Initialize currentProject at class level

        //  list of employees for the "Verantwortlicher" selection
        private List<Employee> mitarbeiter;

        public ProjectDetailsWindow(Project project)
        {
            InitializeComponent();
            currentProject = project;
            // Set data context or populate fields based on the provided project
            // For simplicity, let's assume the Project class has properties Bezeichnung, Startdatum, Enddatum, etc.
            if (project != null)
            {
                TextBoxBezeichnung.Text = currentProject.Name;
                TextBoxVerantwortlicher.Text = currentProject.Verantwortlicher;
                DatePickerStartdatum.SelectedDate = currentProject.Startdatum;
                DatePickerEnddatum.SelectedDate = currentProject.Enddatum;

                // Set other fields accordingly
            }

            ProjectPhasesCollection = new ObservableCollection<Projectphases>();
            PhasesDataGrid.ItemsSource = ProjectPhasesCollection;
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

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInput())
            {
                bool isEditing = currentProject != null && currentProject.Proj_ID > 0;

                // Update the project's properties from the input fields
                currentProject.Name = TextBoxBezeichnung.Text + currentProject.Proj_ID;
                currentProject.Verantwortlicher = TextBoxVerantwortlicher.Text;
                currentProject.Startdatum = DatePickerStartdatum.SelectedDate.GetValueOrDefault(DateTime.Now);
                currentProject.Enddatum = DatePickerEnddatum.SelectedDate.GetValueOrDefault(DateTime.Now.AddDays(10));

                // Save or update the project
                if (isEditing)
                {
                    Backend.UpdateProject(currentProject);
                }
                else
                {
                    Backend.SaveProjectToDatabase(currentProject);
                }

                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool IsValidInput()
        {
            // Use && for all conditions to ensure all fields are valid
            return !string.IsNullOrWhiteSpace(TextBoxBezeichnung.Text) &&
                   DatePickerStartdatum.SelectedDate.HasValue &&
                   DatePickerEnddatum.SelectedDate.HasValue &&
                   !string.IsNullOrWhiteSpace(TextBoxVerantwortlicher.Text);
        }
        private void AddPhases_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of Projectphases from the input fields.
            Projectphases newPhase = new Projectphases
            {
                Kennung = TextBoxNummer.Text,
                Bezeichnung = TextBoxPhase.Text,
    };

            // Validate and parse the Dauer input field.
            if (!int.TryParse(TextBoxDauer.Text, out int dauer))
            {
                MessageBox.Show("Bitte geben Sie die Dauer in ganzen Zahlen ein.");
                return;
            }
            newPhase.Dauer = dauer;

            // Assuming Vorgänger is optional, parse if not empty.
            newPhase.Vorgaenger = string.IsNullOrWhiteSpace(TextBoxVorgaenger.Text) ? null : TextBoxVorgaenger.Text;

            // insert newPhase into the database using the backend method.
            Backend.AddNewProjectPhase(newPhase);

            // Clear the textboxes after adding.
            ClearInputFields();

            // Refresh the DataGrid view if necessary.
            RefreshPhasesDataGrid();
        }

        private void ClearInputFields()
        {
            TextBoxNummer.Clear();
            TextBoxPhase.Clear();
            TextBoxDauer.Clear();
            TextBoxVorgaenger.Clear();
        }

        private void RefreshPhasesDataGrid()
        {
            var updatedPhases = Backend.GetAllProjectPhases(currentProject.Proj_ID);

            // Clear the existing collection
            ProjectPhasesCollection.Clear();

            // Add the updated phases to the collection
            foreach (var phase in updatedPhases)
            {
                ProjectPhasesCollection.Add(phase);
            }
        }

    }
}
