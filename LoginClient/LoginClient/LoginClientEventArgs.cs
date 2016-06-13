using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginClient
{
    public delegate void LoginClientEventHandler(object sender, LoginClientEventArgs args);
    public class LoginClientEventArgs : EventArgs
    {
        //autoimplemented property for Username
        public string Username { get; set; }

        //autoimplemented property for Password
        public string Password { get; set; }

        //autoimplemented property for selecting the registration rights 
        public int ClientOrEmployee { get; set; }

        //general constructor
        public LoginClientEventArgs(string userName, string pass, int clOrEmpl)
        {
            Username = userName;
            Password = pass;
            ClientOrEmployee = clOrEmpl;
        }
    }
}
