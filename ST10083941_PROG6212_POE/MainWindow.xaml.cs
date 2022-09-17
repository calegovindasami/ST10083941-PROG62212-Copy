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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeManagementLibrary;

namespace ST10083941_PROG6212_POE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Context Context { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Context = new Context();
            ucUser.btnSubmit.Click += BtnSubmit_Click;
            ucModules.btnAddModule.Click += BtnAddModule_Click;
            DataContext = Context;

        }

        private void BtnAddModule_Click(object sender, RoutedEventArgs e)
        {
            string moduleCode = ucModules.ModuleCode;
            string moduleName = ucModules.ModuleName;
            int numberOfCredits = ucModules.NumberOfCredits;
            int weeklyClassHours = ucModules.WeeklyClassHours;

            Context.AddModule(moduleCode, moduleName, numberOfCredits, weeklyClassHours);
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //Prevents user from leaving fields empty.
            if (ucUser.Username == "" || ucUser.SemesterStartDate.ToString() == "")
            {
                userSnackbar.IsActive = true;
                userSnackbar.MessageQueue?.Enqueue("Values cannot be left empty.", null, null, null, false, true, TimeSpan.FromSeconds(3));
                
            }
            else
            {
                Context.SignUp(ucUser.Username, ucUser.NumberOfSemesterWeeks, ucUser.SemesterStartDate.Value);
                ucUser.btnSubmit.Command = MaterialDesignThemes.Wpf.Transitions.Transitioner.MoveNextCommand;
            }
        }
    }
}
