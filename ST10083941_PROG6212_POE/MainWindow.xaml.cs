using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using TimeManagementLibrary.Models;

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
            ucSessions.DataContext = Context;
            ucSessions.btnAddSession.Click += BtnAddSession_Click;
            ucSessions.cmbModuleCode.SelectionChanged += CmbModuleCode_SelectionChanged;
            Context.LoadSelfStudySessions();

        }

        private void CmbModuleCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string moduleCode = ucSessions.ModuleCode;
        }

        private void BtnAddSession_Click(object sender, RoutedEventArgs e)
        {
            if (ucSessions.dpSessionDate.SelectedDate == null)
            {
                snackSessions.MessageQueue?.Enqueue("Please pick a date.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else
            {
                string moduleCode = ucSessions.ModuleCode;
                DateTime sessionDate = ucSessions.SessionDate;
                int numberOfHours = ucSessions.NumberOfHours;

                Context.AddStudySession(moduleCode, sessionDate, numberOfHours);
                Context.LoadSelfStudySessions();

                ucSessions.cmbModuleCode.SelectedIndex = 0;
                ucSessions.dpSessionDate.SelectedDate = null;
                ucSessions.nudStudyHours.Value = 1;
            }
        }

        private void BtnAddModule_Click(object sender, RoutedEventArgs e)
        {
            string moduleCode = ucModules.ModuleCode;
            string moduleName = ucModules.ModuleName;
            int numberOfCredits = ucModules.NumberOfCredits;
            int weeklyClassHours = ucModules.WeeklyClassHours;


            if (moduleName == "" && moduleName == "")
            {
                snackModules.MessageQueue?.Enqueue("Fields cannot be empty.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else if (!Context.FoundModuleCode(moduleCode))
            {
                Context.AddModule(moduleCode, moduleName, numberOfCredits, weeklyClassHours);
                snackModulesSuccess.MessageQueue?.Enqueue("Module has been added.", null, null, null, false, true, TimeSpan.FromSeconds(3));
                Context.LoadSelfStudySessions();
            }
            else
            {
                snackModules.MessageQueue?.Enqueue("Module already exists.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //Prevents user from leaving fields empty.
            if (ucUser.Username == "" || ucUser.SemesterStartDate.ToString() == "")
            {
                userSnackbar.MessageQueue?.Enqueue("Values cannot be left empty.", null, null, null, false, true, TimeSpan.FromSeconds(3));
                
            }
            else
            {
                Context.SignUp(ucUser.Username, ucUser.NumberOfSemesterWeeks, ucUser.SemesterStartDate.Value);
                ucUser.btnSubmit.Command = MaterialDesignThemes.Wpf.Transitions.Transitioner.MoveNextCommand;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string moduleCode = ucModules.ModuleCode;
            string moduleName = ucModules.ModuleName;
            int numberOfCredits = ucModules.NumberOfCredits;
            int weeklyClassHours = ucModules.WeeklyClassHours;
            var moduleToBeUpdated = dgModules.SelectedItem as Module;
            if (moduleCode == "" && moduleName == "")
            {
                snackModules.MessageQueue?.Enqueue("Fields cannot be empty.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else if (moduleToBeUpdated.ModuleCode != moduleCode)
            {
                snackModules.MessageQueue?.Enqueue("Module already exists. Please select the correct module.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else if (dgModules.SelectedIndex != -1)
            {
                Context.UpdateModule(moduleToBeUpdated, moduleCode, moduleName, numberOfCredits, weeklyClassHours);
                Context.LoadSelfStudySessions();
                dgModules.Items.Refresh();
                snackModulesSuccess.MessageQueue?.Enqueue("Module has been updated.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else
            {
                snackModules.MessageQueue?.Enqueue("Please select a module by double clicking on the module code.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
        }

        //Displays selected datagrid item value into the input fields for user to update.
        private void dgModules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgModules.SelectedItem != null)
            {
                Module module = dgModules.SelectedItem as Module;
                ucModules.txbModuleCode.Text = module.ModuleCode;
                ucModules.txbModuleName.Text = module.ModuleName;
                ucModules.nudNumberOfCredits.Value = module.NumberOfCredits;
                ucModules.nudWeeklyClassHours.Value = module.WeeklyClassHours;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgModules.SelectedIndex != -1)
            {
                Module module = dgModules.SelectedItem as Module;
                Context.RemoveModule(module);
                snackModulesSuccess.MessageQueue?.Enqueue("Module has been deleted.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else
            {
                snackModules.MessageQueue?.Enqueue("Please select a module to delete by clicking on the module code.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ucModules.ClearFields();
            snackModulesSuccess.MessageQueue?.Enqueue("Fields have been cleared.", null, null, null, false, true, TimeSpan.FromSeconds(3));
        }

    }
}
