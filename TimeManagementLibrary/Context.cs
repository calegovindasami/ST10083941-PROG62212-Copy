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
        //Variables to hold users data.
        public User User { get; set; }
        public ObservableCollection<Module> Modules { get; set; }
        public ObservableCollection<StudySession> StudySessions { get; set; }

        public ObservableCollection<SelfStudyHours> SelfStudyHours { get; set; }

        public Context()
        {
            User = new User("Cassper", 8, DateTime.Today.Date);

            //Temp values
            Modules = new ObservableCollection<Module>()
            {
                new Module("PROG6212", "Programming 2B", 15, 8),
                new Module("SOEN6222", "Software Engineering", 15, 6),
                new Module("CLDV6212", "Cloud Development", 15, 4)
            };
            StudySessions = new ObservableCollection<StudySession>()
            {
                new StudySession("PROG6212", DateTime.Today.Date, 2)
            };

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

        public void SignUp(string username, int numberOfSemesterWeeks, DateTime semesterStartDate)
        {
            User.Username = username;
            User.NumberOfSemesterWeeks = numberOfSemesterWeeks;
            User.SemesterStartDate = semesterStartDate;
        }

        public void UpdateModule(Module module,string moduleCode, string moduleName, int numberOfCredits, int weeklyClassHours)
        {
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

        public bool FoundModuleCode(string moduleCode)
        {
            var foundMod = Modules.FirstOrDefault(m => m.ModuleCode == moduleCode);
            if (foundMod == null)
            {
                return false;
            }
            return true;
        }

        public void RemoveModule(Module module)
        {
            Modules.Remove(module);
        }

        public void LoadSelfStudySessions()
        {
            SelfStudyHours.Clear();
            foreach (Module mod in Modules)
            {
                double weeklyStudyHours = CalculateWeeklySelfStudyHours(mod.NumberOfCredits, mod.WeeklyClassHours, User.NumberOfSemesterWeeks);
                double remainingWeeklyStudyHours = CalculateRemainingSelfStudyHours(weeklyStudyHours, mod.ModuleCode);
                SelfStudyHours selfStudyHours = new SelfStudyHours(mod.ModuleCode, weeklyStudyHours, weeklyStudyHours - remainingWeeklyStudyHours);
                SelfStudyHours.Add(selfStudyHours);
            }
        }
    }
}
