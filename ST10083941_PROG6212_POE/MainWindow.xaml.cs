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

            //Adds events for the buttons in the user controls.
            ucUser.btnSubmit.Click += BtnSubmit_Click;
            ucModules.btnAddModule.Click += BtnAddModule_Click;
            ucSessions.cmbModuleCode.DropDownClosed += CmbModuleCode_DropDownClosed;
            ucSessions.btnAddSession.Click += BtnAddSession_Click;
            
            //Assigns the proper context for current class and corresponding user control class. This allows the DataGrid to be linked to the ObservableCollections.
            DataContext = Context;
            ucSessions.DataContext = Context;

            //Limits the users semester start date to current year only.
            ucUser.dpSemesterStartDate.DisplayDateStart = new DateTime(DateTime.Now.Year, 1, 1);
            ucUser.dpSemesterStartDate.DisplayDateEnd = new DateTime(DateTime.Now.Year, 12, 31);

        }

        //Notifies and prevents user from inputting a study session for a module which they have already completed its required study time.
        private void CmbModuleCode_DropDownClosed(object sender, EventArgs e)
        {
            if (ucSessions.cmbModuleCode.Text != "")
            {
                string moduleCode = ucSessions.cmbModuleCode.Text;
                var module = Context.SelfStudyHours.FirstOrDefault(s => s.ModuleCode == moduleCode);
                ucSessions.nudStudyHours.Maximum = module.RemainingWeeklyStudyHours;

                //Prevents user from adding a session for a modules required hours they have completed.
                if (module.RemainingWeeklyStudyHours == 0)
                {
                    //The MessageQueue code below calls a custom controls method which allows a snackbar to be used as a notification tool. This is from the MaterialDesign NuGet package.
                    snackSessionsSuccess.MessageQueue?.Enqueue("Required hours has been completed for this module.", null, null, null, false, true, TimeSpan.FromSeconds(3));
                    ucSessions.btnAddSession.IsEnabled = false;
                }
                else
                {
                    ucSessions.btnAddSession.IsEnabled = true;
                }
            }
        }

        //Does basic validation and adds a study session to the StudySessions collection located in the "Context.cs" file.
        private void BtnAddSession_Click(object sender, RoutedEventArgs e)
        {
            //Ensures all fields have correct input.
            if (ucSessions.dpSessionDate.SelectedDate == null || ucSessions.cmbModuleCode.Text == "" || ucSessions.nudStudyHours.Value == null || ucSessions.nudStudyHours.Value == 0)
            {
                snackSessions.MessageQueue?.Enqueue("Invalid details.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else
            {
                string moduleCode = ucSessions.ModuleCode;
                DateTime sessionDate = ucSessions.SessionDate;
                int numberOfHours = ucSessions.NumberOfHours;

                Context.AddStudySession(moduleCode, sessionDate, numberOfHours);
                Context.LoadSelfStudySessions();

                //Resets input field.
                ucSessions.cmbModuleCode.SelectedItem = null;
                ucSessions.dpSessionDate.SelectedDate = null;
                ucSessions.nudStudyHours.Value = 1;
            }
        }

        //Adds module to the Modules collection and performs basic validation and notifies the user if their input was incorrect.
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
            //Checks if module code already exists.
            else if (!Context.FoundModuleCode(moduleCode))
            {
                Context.AddModule(moduleCode, moduleName, numberOfCredits, weeklyClassHours);
                snackModulesSuccess.MessageQueue?.Enqueue("Module has been added.", null, null, null, false, true, TimeSpan.FromSeconds(3));
                Context.LoadSelfStudySessions();
                ucModules.ClearFields();
            }
            else
            {
                snackModules.MessageQueue?.Enqueue("Module already exists.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
        }

        //Signs the user up and moves them to the next page if they have passed the validation.
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

                //This uses the MaterialDesign package which moves the user from the sign up page to the modules page. By using this command, a little animation is done.
                ucUser.btnSubmit.Command = MaterialDesignThemes.Wpf.Transitions.Transitioner.MoveNextCommand;

                //Only allows user to enter a study session for their current semester.
                ucSessions.dpSessionDate.DisplayDateStart = Context.User.SemesterStartDate;
                ucSessions.dpSessionDate.DisplayDateEnd = Context.User.SemesterStartDate.AddDays(Context.User.NumberOfSemesterWeeks * 7);
            }
        }

        //Validates and notifies user if their input is incorrect otherwise it updates the corresponding collections.
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string moduleCode = ucModules.ModuleCode;
            string moduleName = ucModules.ModuleName;
            int numberOfCredits = ucModules.NumberOfCredits;
            int weeklyClassHours = ucModules.WeeklyClassHours;
            var moduleToBeUpdated = dgModules.SelectedItem as Module;
            if (moduleCode == "" || moduleName == "")
            {
                snackModules.MessageQueue?.Enqueue("Fields cannot be empty.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else if (dgModules.SelectedIndex != -1)
            {
                //Calls the update method in the Context.cs class.
                Context.UpdateModule(moduleToBeUpdated, moduleCode, moduleName, numberOfCredits, weeklyClassHours);
                Context.LoadSelfStudySessions();
                dgModules.Items.Refresh();
                dgSessions.Items.Refresh();
                ucSessions.cmbModuleCode.Items.Refresh();
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

        //Deletes the selected module, if no module is selected, the user is notified.
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgModules.SelectedIndex != -1)
            {
                Module module = dgModules.SelectedItem as Module;
                Context.RemoveModule(module);
                Context.LoadSelfStudySessions();
                dgSessions.Items.Refresh();
                ucSessions.cmbModuleCode.Items.Refresh();
                snackModulesSuccess.MessageQueue?.Enqueue("Module has been deleted.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
            else
            {
                snackModules.MessageQueue?.Enqueue("Please select a module to delete by clicking on the module code.", null, null, null, false, true, TimeSpan.FromSeconds(3));
            }
        }
        
        //Clears the input field for the module form.
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ucModules.ClearFields();
            snackModulesSuccess.MessageQueue?.Enqueue("Fields have been cleared.", null, null, null, false, true, TimeSpan.FromSeconds(3));
        }

    }
}
