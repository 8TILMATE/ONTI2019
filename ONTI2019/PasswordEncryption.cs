using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTI2019
{
    public class PasswordEncryption
    {
        public static string EncryptPassword(string password)
        {
            string newpas = "";
            foreach(char c in password)
            {
                if (Char.IsLetterOrDigit(c))
                {
                    if (Char.IsDigit(c))
                    {
                        newpas += (9 - Int32.Parse(c.ToString())).ToString();
                    }
                    if(Char.IsLetter(c))
                    {
                        if(c!='A'&&c!='Z'&&c!='a'&&c!='z')
                        {
                            int index = Convert.ToInt32(c);
                            index++;
                            Char x = Convert.ToChar(index);
                            newpas += x;
                        }
                        else
                        {
                            switch (c)
                            {
                                case 'A':
                                    newpas += "Z";
                                    break;
                                case 'Z':
                                    newpas += "A";
                                    break;
                                case 'a':
                                    newpas += "z";
                                    break;
                                case 'z':
                                    newpas += "a";
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    newpas += c;
                }
            }
            return newpas;
        }
    }
}
