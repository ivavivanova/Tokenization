using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections;
using wox.serial;
using System.IO;

namespace TokenizationSOAPServer
{
    
    public class TokenServer: ITokenServer
    {
        string usersXML= "E:\\registratedUsers.xml"; //XML file for registrated users in the system
        string tokensXML = "E:\\tokens.xml"; //XML file for generated tokens in the system
        string lastLoginUser = "E:\\lastLoginUser.xml"; //XML file to save the access of last login user
        ArrayList usersInSystem = new ArrayList(); //to save new registered user in xml
        private User[] usersHelper = new User[1000]; //helper array for users data access
        private UserAccess lastLoginAccess; //the access of last login user 
        ArrayList tokensInSystem = new ArrayList(); //to save new generated pair of token and ID
        private Token[] tokensHelper = new Token[1000]; //helper array for tokens data access

        

        //method verifying the validiti of the card
        public bool ValidAndLuhnTest(string id)
        {
            //number of digits must be 16
            if (id.Length < 16 || id.Length > 16) return false;

            int[] idArray = new int[id.Length]; //id number of Card
            int sumLuhn = 0; //variable for the Luhn algorithm

            for (int i = 0; i < id.Length; i++)
            {
                idArray[i] = Convert.ToInt32(id[i].ToString());
            }

            //first digit must be 3,4,5 or 6
            if (idArray[0] < 3 || idArray[0] > 6) return false;

            //Luhn test
            for (int i = 0; i<idArray.Length; i++)
            {
                if (i % 2 == 0)    idArray[i] *= 2;

                if (idArray[i] > 9)    idArray[i] = (idArray[i] / 10) + (idArray[i] % 10);

                sumLuhn += idArray[i];
            }
            return ((sumLuhn % 10) == 0);
        }//end method
        
        //method to make Token
        public string MakeTokenByID(string id)
        {
            int[] idArray = new int[id.Length]; //id number of Card
            int[] tokenArray = new int[id.Length]; //token number
            Random tempRand = new Random(); //generate random numbers for the token
            int sumToken = 0; //sum of token numbers must not be multiple of 10
            string tokenizationResult = ""; //the string result of tokenization

            for (int i = 0; i < id.Length; i++)
            {
                idArray[i] = Convert.ToInt32(id[i].ToString());
            }

            //generate first 11 digits in the token
            //first digit must not be 0,3,4,5 or 6
            //next digits must not be the same as the digit in the ID number
            for (int i = 0; i<tokenArray.Length-5; i++)
            {
                tokenArray[i] = tempRand.Next(0, 10);
                while(tokenArray[0]==0||(tokenArray[0]>=3 && tokenArray[0]<=6))
                {
                    tokenArray[0] = tempRand.Next(1, 10);
                }
                while(tokenArray[i] == idArray[i])
                {
                    tokenArray[i] = tempRand.Next(0, 10);
                }

                sumToken += tokenArray[i];
            }

            //last four digits must be the same as the digit in the ID number
            for (int i = tokenArray.Length-4; i<tokenArray.Length; i++)
            {
                tokenArray[i] = idArray[i];
                sumToken += tokenArray[i];
            }

            //twelve digit must not must be the same as twelve digit in the ID 
            //the sum of digits must not be multiple of 10
            tokenArray[tokenArray.Length-5] = tempRand.Next(0, 10);
            while((((sumToken % 10) + tokenArray[tokenArray.Length-5])==10) || (tokenArray[tokenArray.Length-5]==idArray[tokenArray.Length-5]))
            {
                tokenArray[tokenArray.Length - 5] = tempRand.Next(0, 10);
            }

            //write the array into string
            for(int i=0;i<tokenArray.Length;i++)
            {
                tokenizationResult += tokenArray[i].ToString();
            }

            return tokenizationResult;
        }

        //load users from registratedUsers.xml to ArrayList
        private void AddUsers()
        {
            /*usersInSystem.Add(new User("iva", "123", UserAccess.ClIENT));
            usersInSystem.Add(new User("ema", "5693", UserAccess.ClIENT));
            usersInSystem.Add(new User("boss", "ssob123", UserAccess.EMPLOYEE));
            usersInSystem.Add(new User("iva2", "1ala23", UserAccess.ClIENT));
            usersInSystem.Add(new User("eva", "56bala93", UserAccess.ClIENT));
            usersInSystem.Add(new User("employee1", "ssob123", UserAccess.EMPLOYEE));
            usersInSystem.Add(new User("employee2", "ssob123", UserAccess.EMPLOYEE));
            usersInSystem.Add(new User("employee3", "ssob123", UserAccess.EMPLOYEE));
            usersInSystem.Add(new User("employee4", "ssob123", UserAccess.EMPLOYEE));
            Easy.save(usersInSystem, usersXML);*/
            usersInSystem = (ArrayList)Easy.load(usersXML);
        }

        //if username is in the system, user must change it
        public int AreInSystem(string userName)
        {
            AddUsers();
            usersHelper = usersInSystem.ToArray(typeof(User)) as User[];
            
            for (int temp = 0; temp < usersInSystem.Count; temp++)
            {
                if (usersHelper[temp].Name == userName)
                {
                     return 1;
                }
            }
            
            return 0;
        }

        //method to save new registered user
        public void UsersSave(string userName, string password, int access)
        {
            AddUsers(); //load old users in usersInSystem ArrayList
            usersInSystem.Add(new User(userName, password, (UserAccess)access)); //add new user
            Easy.save(usersInSystem, usersXML); //save new user
            usersHelper = usersInSystem.ToArray(typeof(User)) as User[]; //get user data
            lastLoginAccess = (UserAccess)access; //get user access
            Easy.save((int)lastLoginAccess, lastLoginUser); //save the user access
        }

        //method to validation login
        public int ValidationLog(string userName, string password)
        {
            AddUsers(); //load old users in usersInSystem ArrayList
            usersHelper = usersInSystem.ToArray(typeof(User)) as User[]; //get user data
            for (int temp=0;temp<usersInSystem.Count;temp++)
            {
                if(usersHelper[temp].Name==userName) //if userName is in the xml for registered
                {
                    if (usersHelper[temp].Password == password) //if this is the password of user
                    {
                        lastLoginAccess = usersHelper[temp].UserAcc; //get user access
                        Easy.save((int)lastLoginAccess, lastLoginUser); //save the user access
                        return 1;
                    }
                }
            }
            return 0;
        }

        //method to load the tokens in the system
        private void TokensLoad()
        {
            /*tokensInSystem.Add(new Token("4563960122019991", "1234243434269991"));
            Easy.save(tokensInSystem, tokensXML);*/
            tokensInSystem = (ArrayList)Easy.load(tokensXML);
        }

        //if username is in the system, user must change it
        public bool IsTokenInSystem(string token)
        {
            TokensLoad(); //load old tokens in the system
            tokensHelper = tokensInSystem.ToArray(typeof(Token)) as Token[]; //get tokens data
            for (int temp = 0; temp < tokensInSystem.Count; temp++)
            {
                if (tokensHelper[temp].TokenID == token)
                {
                    return true;
                }
            }
            return false;
        }

        //method to save new registered user
        public void TokensSave(string tokenID, string iD)
        {
            TokensLoad(); //load old tokens in the system
            tokensInSystem.Add(new Token(iD, tokenID)); //add new token
            Easy.save(tokensInSystem, tokensXML); //save new user
            tokensHelper = tokensInSystem.ToArray(typeof(Token)) as Token[]; //get tokens data
        } 

        //if user have enough permissions and the token is almost generated, then return card
        //ID for this token
        public string LoadID(string token)
        {
            TokensLoad(); //load old tokens in the system
            tokensHelper = tokensInSystem.ToArray(typeof(Token)) as Token[]; //get tokens data
            lastLoginAccess = (UserAccess)Easy.load(lastLoginUser); //get the access of last login user
            if (lastLoginAccess == UserAccess.EMPLOYEE) //if user have enough permissions
            {
                for (int temp = 0; temp < tokensHelper.Length; temp++)
                {
                    if (tokensHelper[temp].TokenID == token) //if userName is in the xml for registered
                    {
                        return tokensHelper[temp].ID; //return card ID for this token
                    }
                }
            }
            
            return null; 
        }

        //sorted tokens by token and save them in file
        public void SortedByTokenAndSaveInFile()
        {
            string filename = "E:\\sortedByToken.txt"; //name of file
            TokensLoad(); //load old tokens in the system
            tokensHelper = tokensInSystem.ToArray(typeof(Token)) as Token[]; //get tokens data

            //sorted by Token
            var sorted =
                from token in tokensHelper
                orderby token.TokenID
                select token;

            //save data in file
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var sort in sorted)
                {
                    writer.WriteLine(sort.ToString());
                }
            }
         }

        //sorted tokens by ID and save them in file
        public void SortedByIDAndSaveInFile()
        {
            string filename = "E:\\sortedByID.txt"; //the name of file
            TokensLoad(); //load old tokens in the system
            tokensHelper = tokensInSystem.ToArray(typeof(Token)) as Token[]; //get tokens data

            //sorted by ID
            var sorted =
                from token in tokensHelper
                orderby token.ID
                select token;

            //safe data in file
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var sort in sorted)
                {
                    writer.WriteLine(sort.ToString());
                }
            }
        }
    }
}
