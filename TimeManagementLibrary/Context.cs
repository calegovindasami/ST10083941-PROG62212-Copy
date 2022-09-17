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

        public Context()
        {
            User = new User();
            Modules = new ObservableCollection<Module>();
            StudySessions = new ObservableCollection<StudySession>();
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
            var currentWeekStudySessions = StudySessions.Where(s => s.SessionDate.Date >= startOfWeek.Date && s.SessionDate.Date <= endOfWeek.Date);
            double remainingSelfStudyHours = currentWeekStudySessions.Sum(s => s.NumberOfHours);

            return remainingSelfStudyHours;
        }

        public void SignUp(string username, int numberOfSemesterWeeks, DateTime semesterStartDate)
        {
            User.Username = username;
            User.NumberOfSemesterWeeks = numberOfSemesterWeeks;
            User.SemesterStartDate = semesterStartDate;
        }


    }
}
