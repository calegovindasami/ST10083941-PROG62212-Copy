using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
namespace ST10083941_PROG6212_POE.UserControls
{
    /// <summary>
    /// Interaction logic for UserSignUp.xaml
    /// </summary>
    public partial class UserSignUp : UserControl
    {

        //Properties to retrieve the data stored within the input fields for the user.
        public string Username
        {
            get => txbUsername.Text;
            set => txbUsername.Text = value;
        }

        public int NumberOfSemesterWeeks
        {
            get => Convert.ToInt32(nudNumberOfSemesterWeeks.Value);
            set => nudNumberOfSemesterWeeks.Value = value;
        }

        public DateTime? SemesterStartDate
        {
            get => dpSemesterStartDate.SelectedDate;
            set => dpSemesterStartDate.SelectedDate = value;
        }

        public UserSignUp()
        {
            InitializeComponent();
        }

        //Limits characters entered to alphabets only.
        private void txbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.A || e.Key > Key.Z)
            {
                e.Handled = true;
            }
        }



    }
}
