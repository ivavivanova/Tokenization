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

namespace TheTokenizer
{
    /// <summary>
    /// Interaction logic for TokenizerUserControl.xaml
    /// </summary>
    public partial class TokenizerUserControl : UserControl
    {
        public event TokenizerEventHandler TokenByID; //event for Token
        public event TokenizerEventHandler IDForToken; //event for ID
        public event TokenizerEventHandler ClearAll; //event for Clear
        //default constructor
        public TokenizerUserControl()
        {
            InitializeComponent();
        }//end constructor

        //property for the client input
        public string TokenOrID
        {
            get
            {
                return txtTokenOrID.Text;
            }//end get
            set
            {
                if (value != null)
                    txtTokenOrID.Text = value;
                else
                    txtTokenOrID.Text = "";
            }//end set
        }//end property

        

        //clear all inputs
        private void btnClr_Click(object sender, RoutedEventArgs e)
        {
            if (ClearAll != null)
            {
                txtTokenOrID.Text = "";
                ClearAll(this, new TokenizerEventArgs(TokenOrID));
            }
            
        }

        //event handler for generating Token
        private void btnGenerateToken_Click(object sender, RoutedEventArgs e)
        {
            if (TokenByID != null)
            {
                TokenByID(this, new TokenizerEventArgs(TokenOrID));
            }
        }//end event handler

        //event handler for showing ID
        private void btnViewID_Click(object sender, RoutedEventArgs e)
        {
            if (IDForToken != null)
            {
                IDForToken(this, new TokenizerEventArgs(TokenOrID));
            }
        }//end event handler
    }//end class TokenizerUserControl
}//end namespace
