
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
        public string val;

        

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
        public const int stringToken = 20;

        public const int WhiteSpace = 30; //spacja
        public const int NewLine = 31;//nowa linia 

        public const int TRUE = 36;
        public const int FALSE = 40;
        public const int NULL = 100;

        public const int id = 51;
        public const int schema= 52;
        public const int title= 53;
        public const int type = 54;
        public const int properties = 55;
        public const int description = 56;
        public const int required = 57;
        public const int minimum = 58;
        public const int maximum = 59;
        public const int minLength﻿ = 60;
        public const int definitions﻿ = 61;
        public const int maxLength﻿ = 62;
        public const int enumToken = 63;
        public const int refToken = 64;
                    



       
        public Token(int Token, int Line, string valString)
        {
            this.token = Token;
            this.lineNumber = Line;
            this.val = valString;
        }

        public int GetToken()
        {
            return this.token;
        }

        public int GetLine()
        {
            return this.lineNumber;
        }


        public string GetValString()
        {
            return this.val;
        }

        //prawdopodobnie zbędne
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
