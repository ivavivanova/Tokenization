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

namespace LoginClient
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public event LoginClientEventHandler Login; //event for Login
        public event LoginClientEventHandler Reg; //event for Registration

        //default constructor
        public LoginUserControl()
        {
            InitializeComponent();
        }//end constructor

        //property for Username
        public string Username
        {
            get
            {
                return txtUsername.Text;
            }//end get
            set
            {
                if (value != null)
                    txtUsername.Text = value;
                else
                    txtUsername.Text = "";
            }//end set
        }//end property Username

        //property for Password
        public string Password
        {
            get
            {
                return passBoxPassword.Password;
            }//end get
            set
            {
                if (value != null)
                    passBoxPassword.Password = value;
                else
                    passBoxPassword.Password = "";
            }//end set
        }//end property for Password

        //get property for for selecting the registration rights
        public int ClientOrEmployee
        {
            get
            {
                ListBoxItem currentItem = lstChoose.SelectedItem as ListBoxItem;
                if (currentItem != null)
                    return Convert.ToInt32(currentItem.Tag);
                else
                    return 1;
            }//end get
        }//end property

        //event handler for Login
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Login != null)
            {
                Login(this, new LoginClientEventArgs(Username, Password, ClientOrEmployee));
            }
        }//end event handler

        //event handler for Registration
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            if(Reg != null)
            {
                Reg(this, new LoginClientEventArgs(Username, Password, ClientOrEmployee));
            }
        }//end event handler

        //if checked, then register account
        private void chBoxReg_Checked(object sender, RoutedEventArgs e)
        {
            lstChoose.Visibility = Visibility.Visible;
            btnReg.Visibility = Visibility.Visible;
        }

        //if unchecked, then login account
        private void chBoxReg_Unchecked(object sender, RoutedEventArgs e)
        {
            lstChoose.Visibility = Visibility.Hidden;
            btnReg.Visibility = Visibility.Hidden;
        }

    }//end class LoginUserControl
}//end namespace
