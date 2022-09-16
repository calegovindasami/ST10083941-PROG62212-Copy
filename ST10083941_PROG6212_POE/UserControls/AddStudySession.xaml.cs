using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10083941_PROG6212_POE.UserControls
{
    /// <summary>
    /// Interaction logic for AddStudySession.xaml
    /// </summary>
    public partial class AddStudySession : UserControl
    {
        ObservableCollection<string> ModuleCodes = new ObservableCollection<string>();
        public string ModuleCode 
        {
            get => cmbModuleCode.Text;
            private set => cmbModuleCode.Text = value; 
        }
        public DateTime? SessionDate 
        { 
            get => dpSessionDate.SelectedDate; 
            set => dpSessionDate.SelectedDate = value; 
        }
        public int NumberOfHours 
        { 
            get => Convert.ToInt32(nudStudyHours.Value); 
            set => nudStudyHours.Value = value; 
        }
        public AddStudySession()
        {
            InitializeComponent();
        }

        public void LoadModules(ObservableCollection<string> moduleCodes)
        {
            ModuleCodes = moduleCodes;
            cmbModuleCode.ItemsSource = ModuleCodes;
            DataContext = this;
        }
    }
}
