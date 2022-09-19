using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagementLibrary.Models;

namespace TimeManagementLibrary
{
    public class Context
    {
        //Properties to hold users data.
        public User User { get; set; }

        //Observable collections already implement the INotifyPropertyChanged interface which
        //allows the datagrid to be dynamically updated as the values within the collection change.
        public ObservableCollection<Module> Modules { get; set; }
        public ObservableCollection<StudySession> StudySessions { get; set; }

        public ObservableCollection<SelfStudyHours> SelfStudyHours { get; set; }

        public Context()
        {
            User = new User();
            Modules = new ObservableCollection<Module>();
            StudySessions = new ObservableCollection<StudySession>();

            SelfStudyHours = new ObservableCollection<SelfStudyHours>();
        }

        //Adds module to collection.
        public void AddModule(string moduleCode, string moduleName, int numberOfCredits, int weeklyClassHours)
        {
            Module module = new Module(moduleCode, moduleName, numberOfCredits, weeklyClassHours);
            Modules.Add(module);
        }

        //Adds a study session to collection.
        public void AddStudySession(string moduleCode, DateTime sessionDate, int numberOfHours)
        {
            StudySession studySession = new StudySession(moduleCode, sessionDate, numberOfHours);
             StudySessions.Add(studySession);
        }

        //Calculates the weekly self study hours required for a module and returns it.
        public double CalculateWeeklySelfStudyHours(int numberOfCredits, int weeklyClassHours, int numberOfSemesterWeeks)
        {
            double weeklySelfStudyHours = ((numberOfCredits * 10) / numberOfSemesterWeeks) - weeklyClassHours;
            return weeklySelfStudyHours;
        }

        //Calculates the remaining hours left of self-study for a selected module.
        public double CalculateRemainingSelfStudyHours(double weeklySelfStudyHours, string moduleCode)
        {
            DateTime startOfWeek = DateTime.Now.Date;

            //Subtracts a day from 'startOfWeek' until it is the start of the current week.
            while (startOfWeek.DayOfWeek.ToString() != "Monday")
            {
                startOfWeek = startOfWeek.Date.AddDays(-1);
            }

            DateTime endOfWeek = startOfWeek.AddDays(6);

            //Returns study sessions that only occurred during the current week.
            var selectedModule = StudySessions.Where(s => s.ModuleCode == moduleCode);
            var currentWeekStudySessions = selectedModule.Where(s => s.SessionDate.Date >= startOfWeek.Date && s.SessionDate.Date <= endOfWeek.Date);
            double weekSelfStudyHours = currentWeekStudySessions.Sum(s => s.NumberOfHours);

            return weekSelfStudyHours;
        }

        //Signs the user up.
        public void SignUp(string username, int numberOfSemesterWeeks, DateTime semesterStartDate)
        {
            User.Username = username;
            User.NumberOfSemesterWeeks = numberOfSemesterWeeks;
            User.SemesterStartDate = semesterStartDate;
        }
        //Allows user to update their modules.
        public void UpdateModule(Module module,string moduleCode, string moduleName, int numberOfCredits, int weeklyClassHours)
        {
            //loop goes through StudySessions and changes the module code of the updated module.
            foreach (StudySession session in StudySessions)
            {
                if (session.ModuleCode == module.ModuleCode)
                {
                    session.ModuleCode = moduleCode;
                }
            }

            //Updates module details.
            foreach (Module mod in Modules)
            {
                if (mod.ModuleCode == module.ModuleCode)
                {
                    mod.ModuleCode = moduleCode;
                    mod.ModuleName = moduleName;
                    mod.NumberOfCredits = numberOfCredits;
                    mod.WeeklyClassHours = weeklyClassHours;
                }
            }
        }

        //Returns true or false based on whether or not the module code exists within the collection.
        public bool FoundModuleCode(string moduleCode)
        {
            //Searches the collection to find if the module code exists or not. If it does not, null is returned.
            var foundMod = Modules.FirstOrDefault(m => m.ModuleCode == moduleCode);
            if (foundMod == null)
            {
                return false;
            }
            return true;
        }

        //Removes module from collection.
        public void RemoveModule(Module module)
        {
            Modules.Remove(module);
            foreach (StudySession session in StudySessions.ToList())
            {
                if (session.ModuleCode == module.ModuleCode)
                {
                    StudySessions.Remove(session);
                }
            }
        }

        //Assigns the selfstudysession collection corresponding values based on the modules currently entered.
        public void LoadSelfStudySessions()
        {
            SelfStudyHours.Clear();
            foreach (Module mod in Modules)
            {
                //Calculates the required study time, and the remaining study time for each module and adds it to the corresponding list.
                double weeklyStudyHours = CalculateWeeklySelfStudyHours(mod.NumberOfCredits, mod.WeeklyClassHours, User.NumberOfSemesterWeeks);
                double remainingWeeklyStudyHours = CalculateRemainingSelfStudyHours(weeklyStudyHours, mod.ModuleCode);
                SelfStudyHours selfStudyHours = new SelfStudyHours(mod.ModuleCode, weeklyStudyHours, weeklyStudyHours - remainingWeeklyStudyHours);
                SelfStudyHours.Add(selfStudyHours);
            }
        }
    }
}
