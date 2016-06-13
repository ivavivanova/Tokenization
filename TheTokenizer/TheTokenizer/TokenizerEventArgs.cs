using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTokenizer
{
    public delegate void TokenizerEventHandler(object sender, TokenizerEventArgs args);
    public class TokenizerEventArgs : EventArgs
    {
        //autoimplemented property for Token or ID generated
        public string TokenOrID{ get; set; }


        //general constructor
        public TokenizerEventArgs(string tokenOrID)
        {
            TokenOrID = tokenOrID;
        }//end constructor
    }
}
