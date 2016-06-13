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
using System.Text.RegularExpressions;

namespace TokenizationSOAPClient
{

    public partial class TokenClient : Window
    {
        private ServiceReference.TokenServerClient client; //object to add client in server

        //default constructor
        public TokenClient()
        {
            InitializeComponent();
            client = new ServiceReference.TokenServerClient();
        }

        
        // Username validation
        //username must starts with some leter, and can contain leters, digits and _
        public bool IsUsernameValid(string username)
        {
            return Regex.Match(username, "^[a-zA-Z][a-zA-Z0-9_]*$").Success;
        }


        private void LogOrReg_Login(object sender, LoginClient.LoginClientEventArgs args)
        {
            if (!(IsUsernameValid(args.Username)))
            {
                MessageBox.Show("Invalid username!");
            }
            else if (args.Password=="")
            {
                MessageBox.Show("You must insert some password!");
            }
            else
            {
                if (client.ValidationLog(args.Username, args.Password) == 1)
                {
                    MessageBox.Show("You are logged in!");
                    LogOrReg.Visibility = Visibility.Hidden;
                    Token.Visibility = Visibility.Visible;
                    btnSaveSortedInFiles.Visibility = Visibility.Visible;
                }
                
                else
                {
                    MessageBox.Show("Incorrect username or password!");
                }
             }
            
        }

        private void LogOrReg_Reg(object sender, LoginClient.LoginClientEventArgs args)
        {
           
            if (args.Password=="")
            {
                MessageBox.Show("You must fill password field!");
            }
            if (!(IsUsernameValid(args.Username)))
            {
                MessageBox.Show("Invalid username!");
            }
            else
            {
                if (client.AreInSystem(args.Username) == 1)
                {
                    MessageBox.Show("This username is already exists!");
                }
                else
                {
                    client.UsersSave(args.Username, args.Password, args.ClientOrEmployee);
                    MessageBox.Show("You have been registrated!");
                    LogOrReg.Visibility = Visibility.Hidden;
                    Token.Visibility = Visibility.Visible;
                    btnSaveSortedInFiles.Visibility = Visibility.Visible;
                   
                }
                
            }
        }

        private void Token_TokenByID(object sender, TheTokenizer.TokenizerEventArgs args)
        {
            if (!(client.ValidAndLuhnTest(args.TokenOrID))) MessageBox.Show("Invalid card ID!");
            else
            {
                lblResult.Visibility = Visibility.Visible;
                lblResultCaption.Visibility = Visibility.Visible;
                lblResultCaption.Content = string.Format("Generated token is:");
                lblResult.Content = (client.MakeTokenByID(args.TokenOrID)).ToString();
                while((client.IsTokenInSystem(lblResult.Content.ToString())))
                {
                    lblResult.Content = (client.MakeTokenByID(args.TokenOrID)).ToString();
                }
                client.TokensSave(lblResult.Content.ToString(), args.TokenOrID);
            }
        }

        private void Token_ClearAll(object sender, TheTokenizer.TokenizerEventArgs args)
        {
            lblResult.Content = "";
            lblResult.Visibility = Visibility.Hidden;
            lblResultCaption.Content = "";
            lblResultCaption.Visibility = Visibility.Hidden;
        }

        private void Token_IDForToken(object sender, TheTokenizer.TokenizerEventArgs args)
        {
            if (client.IsTokenInSystem(args.TokenOrID))
            {
                if ((client.LoadID(args.TokenOrID)) == null)
                {
                    MessageBox.Show("You don't have enough permissions!");
                }
                else
                {

                    lblResultCaption.Visibility = Visibility.Visible;
                    lblResultCaption.Content = "The ID for this token is: ";
                    lblResult.Visibility = Visibility.Visible;
                    lblResult.Content = (client.LoadID(args.TokenOrID)).ToString();
                }
            }
            else
            {
                MessageBox.Show("In the system there is no ID for this token!");
            }
        }

        private void btnSaveSortedInFiles_Click(object sender, RoutedEventArgs e)
        {
            client.SortedByIDAndSaveInFile();
            client.SortedByTokenAndSaveInFile();
            MessageBox.Show("The tokens in system are sorted (by token and by ID) and saved in two files!");
        }
    }
}
