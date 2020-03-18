
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Walidator
{
    public class Token
    {

        public int token;       //token
        public int lineNumber;        //number line 
        public int charNumber;     //char number from beggining

        public const int jsonFile = 0;
        public const int arrayStart = 1;//[
        public const int arrayEnd = 2;//]
        public const int qute = 3;//"
        public const int colon = 4;//:
        public const int objectStart = 5;//{
        public const int objectEnd = 6;//}
        public const int comma = 7;//,
        public const int dot = 8;//.
        public const int number = 9;
        public const int END = 10;
        public const int CHAR = 20;
        public const int TRUE = 30;
        public const int FALSE = 40;
        public const int NULL = 100;

       
        public Token(int Token, int Column, int Line)
        {
            this.token = Token;
            this.charNumber = Column;
            this.lineNumber = Line;
        }

        public int GetToken()
        {
            return this.token;
        }

        public int GetLine()
        {
            return this.lineNumber;
        }

        public int GetCnumber()
        {
            return this.charNumber;
        }

        public override string ToString()
        {
            switch (this.token)
            {
                case 1:
                    return "[";
                case 2:
                    return "]";
                case 3:
                    return "\"";
                case 4:
                    return ":";
                case 5:
                    return "{";
                case 6:
                    return "}";
                case 7:
                    return ",";
                case 8:
                    return ".";
                case 10:
                    return "5";
                case 20:
                    return "b";
                case 30:
                    return "true";
                case 40:
                    return "false";
                case 50:
                    return "null";
                case 100:
                    return "EOF";
                default:
                    break;
            }

            return token.ToString();
        }

    }
}
