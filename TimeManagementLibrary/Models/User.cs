using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementLibrary.Models
{
    public class User
    {
        //Class used for holding the Users values.
        public User(string username, int numberOfSemesterWeeks, DateTime semesterStartDate)
        {
            Username = username;
            NumberOfSemesterWeeks = numberOfSemesterWeeks;
            SemesterStartDate = semesterStartDate;
        }

        public string Username{ get; set; }
        public int NumberOfSemesterWeeks { get; set; }
        public DateTime SemesterStartDate { get; set; }

        public User()
        {

        }
    }
}
