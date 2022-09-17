using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementLibrary
{
    public class SelfStudyHours
    {
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
