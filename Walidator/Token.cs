
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

        

        public const int JsonFile = 0;
        public const int ArrayStart = 1;//[
        public const int ArrayEnd = 2;//]
        public const int Qute = 3;//"
        public const int Colon = 4;//:
        public const int ObjectStart = 5;//{
        public const int ObjectEnd = 6;//}
        public const int Coma = 7;//,
        public const int Dot = 8;//.
        public const int Number = 9;
        public const int END = 10;
        public const int StringToken = 20;

        public const int WhiteSpace = 30; //spacja
        public const int NewLine = 31;//nowa linia 

        public const int TRUE = 36;
        public const int FALSE = 40;
        public const int NULL = 100;

        public const int Id = 51;
        public const int Schema= 52;
        public const int Title= 53;
        public const int Type = 54;
        public const int Properties = 55;
        public const int Description = 56;
        public const int Required = 57;
        public const int Minimum = 58;
        public const int Maximum = 59;
        public const int MinLength = 60;
        public const int Definitions = 61;
        public const int MaxLength = 62;
        public const int EnumToken = 63;
        public const int RefToken = 64;
                    



       
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
