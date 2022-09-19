using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementLibrary.Models
{
    public class StudySession
    {
        //Class containing the study session attributes of the user.
        public StudySession(string moduleCode, DateTime sessionDate, int numberOfHours)
        {
            ModuleCode = moduleCode;
            SessionDate = sessionDate;
            NumberOfHours = numberOfHours;
        }

        public string ModuleCode { get; set; }
        public DateTime SessionDate { get; set; }
        public int NumberOfHours { get; set; }

    }
}
