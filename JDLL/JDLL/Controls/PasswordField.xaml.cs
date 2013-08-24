using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JDLL.Controls
{
    /// <summary>
    /// Interaction logic for PasswordField.xaml
    /// </summary>
    public partial class PasswordField : UserControl
    {
        /// <summary>
        /// Username that the User entered
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// Password that the User entered
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// True if the Control should hide after the uses presses Submit
        /// </summary>
        public bool HideAfterSubmit { get; set; }

        /// <summary>
        /// Event called when user presses Submit
        /// </summary>
        public event EventHandler OnSubmit;

        public PasswordField()
        {
            InitializeComponent();
            Pass.PasswordChar = '*';
        }

        //Submit
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserName = User.Text;
            Password = Pass.Password;

            User.Text = "";
            Pass.Password = "";

            if (HideAfterSubmit)
                this.Visibility = Visibility.Hidden;

            OnSubmit(this, null);
        }

        //Clear
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User.Text = "";
            Pass.Password = "";
        }

        /// <summary>
        /// Deletes the Current Username and Password
        /// </summary>
        public void ClearData()
        {
            UserName = "";
            Password = "";
        }

        /// <summary>
        /// Hides the Control
        /// </summary>
        /// <param name="ClearData">True if the control should call ClearData()</param>
        public void Disable(bool ClearData)
        {
            if (ClearData)
                this.ClearData();

            User.Text = "";
            Pass.Password = "";

            this.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Makes the control Visible
        /// </summary>
        public void Enable()
        {
            this.Visibility = Visibility.Visible;
        }

        private void Button_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(this, null);
        }
    }
}
