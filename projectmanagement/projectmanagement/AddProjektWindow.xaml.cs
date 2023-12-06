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
    /// Interaction logic for AddProjektPhaseWindow.xaml
    /// </summary>
    public partial class AddProjektWindow : Window
    {
        private object newProjekt;

        public AddProjektWindow()
        {
            InitializeComponent();
        }

        public AddProjektWindow(object newProjekt)
        {
            this.newProjekt = newProjekt;
        }
    }
}