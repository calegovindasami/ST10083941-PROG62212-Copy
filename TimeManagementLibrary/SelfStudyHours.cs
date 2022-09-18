using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementLibrary
{
    public class SelfStudyHours
    {
        //This class is used to store the remaining weekly study hours for each module as well as required. I added this so that i can link it to a datagrid
        //using an observable collection.

        public SelfStudyHours(string moduleCode, double weeklySelfStudyHours, double remainingWeeklyStudyHours)
        {
            ModuleCode = moduleCode;
            WeeklySelfStudyHours = weeklySelfStudyHours;
            RemainingWeeklyStudyHours = remainingWeeklyStudyHours;
        }

        public string ModuleCode { get; set; }
        public double WeeklySelfStudyHours { get; set; }
        public double RemainingWeeklyStudyHours { get; set; }
    }
}
