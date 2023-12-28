using projectmanagement.src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Linq;

namespace projectmanagement
{
    public partial class ProjectDetailsWindow : Window
    {
        public ObservableCollection<Projectphases> ProjectPhasesCollection { get; set; }

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
            int Vorgaenger = -1;
            if (!string.IsNullOrWhiteSpace(TextBoxVorgaenger.Text) && !int.TryParse(TextBoxVorgaenger.Text, out Vorgaenger))
            {
                MessageBox.Show("Bitte geben Sie eine gültige Vorgänger-ID ein oder lassen Sie das Feld leer, wenn es keine gibt.");
                return;
            }
            newPhase.Vorgaenger = string.IsNullOrWhiteSpace(TextBoxVorgaenger.Text) ? -1 : Vorgaenger;

            // Now you would insert newPhase into the database using the backend method.
            Backend.AddNewProjectPhase(newPhase);

            // Optionally, clear the textboxes after adding.
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
            var updatedPhases = Backend.GetAllProjectPhases();

            // Clear the existing collection
            ProjectPhasesCollection.Clear();

            // Add the updated phases to the collection
            foreach (var phase in updatedPhases)
            {
                ProjectPhasesCollection.Add(phase);
            }
        }



        private void SavePhaseToDatabase(Projectphases phase)
        {
            // Implement database insertion logic here.
            // This method will use ADO.NET, Entity Framework, Dapper, or another ORM/Data access tool to save 'phase' to the database.
        }



    }
}
