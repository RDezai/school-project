using projectmanagement.src;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projectmanagement
{
    /// <summary>
    /// Interaction logic for EditProjektPhaseWindow.xaml
    /// </summary>
    public partial class EditProjektWindow : Window
    {
        private Projektphasen selectedPhase;
        private Project selectedProject;

        public EditProjektWindow()
        {
            InitializeComponent();
        }

        public EditProjektWindow(Projektphasen selectedPhase)
        {
            this.selectedPhase = selectedPhase;
        }

        public EditProjektWindow(Project selectedProject)
        {
            this.selectedProject = selectedProject;
        }
    }
}
