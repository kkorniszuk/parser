
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Walidator
{
    public class Error
    {
        int LineNumber;
        string ErrorDescription;
        //keywords
        public const string ErrorRequired = "Incorrect required";
        public const string ErrorSchema= "Incorrect $schema";
        public const string ErrorTitle = "Incorrect title";
        public const string ErrorProperties = "Incorrect properties";
        public const string ErrorID = "Incorrect $id";
        public const string ErrorDefinitions = "Incorrect definitions";
        public const string ErrorType = "Incorrect type";

        //other
        public const string ErrorStartSymbol = "Starting symbol '{' required.";
        public const string ErrorString = "Not found string";
        public const string ErrorComma = "Not found ','";
        public const string ErrorColon = "Not found Colon!";
        public const string ErrorObjectStart = "Not found '{'";
        public const string ErrorObjectSstop = "Not found '}'";
        public const string ErrorArraytStart = "Not found '['";
        public const string ErrorArrayEnd = "Not found ']'";
        public const string ErrorNumber = "Not found Number'";
        public const string WhiteSpace = "Not found WhiteSpace!";
        public const string ErrorUrl = "Not found Url!";
        public const string ErrorStringType = "Unknow type?";
        public const string ErrorRef = "Invalid path to ref";

        public Error(int Line, string Description)
        {
            this.LineNumber = Line;
            this.ErrorDescription = Description;
        }

        public int GetLine()
        {
            return this.LineNumber;
        }

        public string GetDescription()
        {
            return this.ErrorDescription;
        }
    }
}
