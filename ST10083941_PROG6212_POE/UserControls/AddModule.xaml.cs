using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10083941_PROG6212_POE.UserControls
{
    /// <summary>
    /// Interaction logic for AddModule.xaml
    /// </summary>
    public partial class AddModule : UserControl
    {
        //Regex of allowed input characters
        Regex regex = new Regex("[^a-zA-Z0-9]+");

        //Getters and setters for this controls input fields.
        public string ModuleCode
        {
            get => txbModuleCode.Text;
            private set => txbModuleCode.Text = value;
        }
        public string ModuleName
        {
            get => txbModuleName.Text;
            private set => txbModuleName.Text = value;
        }
        public int NumberOfCredits
        {
            get => Convert.ToInt32(nudNumberOfCredits.Value);
            private set => nudNumberOfCredits.Value = value;
        }
        public int WeeklyClassHours
        {
            get => Convert.ToInt32(nudWeeklyClassHours.Value);
            private set => nudWeeklyClassHours.Value = value;
        }
        public AddModule()
        {
            InitializeComponent();
        }

        //Only allows textbox input to allow alphanumeric characters.
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }
        //Clears the input fields.
        public void ClearFields()
        {
            txbModuleCode.Text = "";
            txbModuleName.Text = "";
            nudNumberOfCredits.Value = 1;
            nudWeeklyClassHours.Value = 1;
            
        }
    }
}
