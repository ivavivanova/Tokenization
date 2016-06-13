using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace TokenizationSOAPServer
{
    [ServiceContract]
    public interface ITokenServer
    {

        [OperationContract]
        bool ValidAndLuhnTest(string id);

        [OperationContract]
        string MakeTokenByID(string id);

        [OperationContract]
        void UsersSave(string userName, string password, int access);

        [OperationContract]
        int ValidationLog(string userName, string password);

        [OperationContract]
        int AreInSystem(string userName);

        [OperationContract]
        void TokensSave(string token,string id);

        [OperationContract]
        string LoadID (string token);

        [OperationContract]
        bool IsTokenInSystem(string token);

        [OperationContract]
        void SortedByIDAndSaveInFile();

        [OperationContract]
        void SortedByTokenAndSaveInFile();
    }


    //class Token, represents the Tokens (with ID and Token number) in the system
    [DataContract]
    public class Token
    {
        private string iD; //number of Card ID
        private string tokenID; //number of Token

        //property for ID
        [DataMember]
        public string ID
        {
            get
            {
                return iD;
            }//end get
            set
            {
                if (value != null)
                    iD = value;
                else
                    iD = "";
            }//end set
        }//end property

        //property for TokenID
        [DataMember]
        public string TokenID
        {
            get
            {
                return tokenID;
            }//end get
            set
            {
                if (value != null)
                    tokenID = value;
                else
                    tokenID = "";
            }//end set
        }//end property

        //general constructor
        public Token(string idNum, string tokenNum)
        {
            ID = idNum;
            TokenID = tokenNum;
        }//end constructor

        //default constructor
        public Token() : this("4563960122019991", "1234243434269991") { }

        //method ToString
        public override string ToString()
        {
            return string.Format("Token: {1} | CardID: {0}",iD,tokenID);
        }//end method
    }//end class Token

    //Access level for each register User
    [DataContract]
    public enum UserAccess
    {
        ClIENT = 0,
        EMPLOYEE = 1
    };

    //class User, represents the users in the system 
    [DataContract]
    public class User
    {
        private string name; //name of user
        private string password; //password of user
        private int userAcc; //access of user

        //property for name of User
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }//end get
            set
            {
                if (value != null)
                    name = value;
                else
                    name = "";
            }//end set
        }//end property

        //property for User password
        [DataMember]
        public string Password
        {
            get
            {
                return password;
            }//end get
            set
            {
                if (value != null)
                    password = value;
                else
                    password = "";
            }//end set
        }//end property

        //property for User access in system
        [DataMember]
        public UserAccess UserAcc
        {
            get
            {
                return (UserAccess)userAcc;
            }//end get
            set
            {
                if (value == UserAccess.ClIENT || value == UserAccess.EMPLOYEE)
                    userAcc = (int)value;
                else
                    userAcc = (int)UserAccess.ClIENT;
            }//end set
        }//end property

        //general constructor
        public User(string uName, string pass, UserAccess acc)
        {
            Name = uName;
            Password = pass;
            UserAcc = acc;
        }//end constructor

        //default constructor
        public User() : this("root", "root", (UserAccess)1) { }

    }
}
