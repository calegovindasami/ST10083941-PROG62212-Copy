using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementLibrary.Models
{
    public class Module
    {
        //Class representing the Module attributes.
        public Module(string moduleCode, string moduleName, int numberOfCredits, int weeklyClassHours)
        {
            ModuleCode = moduleCode;
            ModuleName = moduleName;
            NumberOfCredits = numberOfCredits;
            WeeklyClassHours = weeklyClassHours;
        }

        public string ModuleCode{ get; set; }
        public string ModuleName{ get; set; }
        public int NumberOfCredits { get; set; }
        public int WeeklyClassHours { get; set; }

        public string GetCode()
        {
            return ModuleCode;
        }
    }

}
