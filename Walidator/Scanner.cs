
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Walidator
{
    public class scanner
    {

        private static Regex numberRegex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?$"); // double, int i postaci wykladnicze
        private static char[] numberChars = { 'e', 'E', '-', '.', '+' };//tablica z akceptowanymi znakami dla liczby nie liczac cyfr
        public static string jsonf; //kopia tekstu json
        public static List<Error> ErrorList = new List<Error>();

        /// <summary>
        /// Analiza leksykalna
        /// </summary>
        /// <param name="json"></param>
        /// <param name="TokensList"></param>
        /// <returns></returns>
        public bool lexer(string json, ref List<Token> TokensList, ref string errorList)
        {
            jsonf = json; // kopia textu json   
            string cString;
            StringBuilder currentString = new StringBuilder();

            int line = 1;//licznik linii
            int n = 0; //licznik char

            char lastChar = ' '; //przechowuje przedostatni znak
            char c; //zmienna do iteracji po znakach

            string currentLine = ""; // akumulowana obecna linia
            string tmp = ""; //tymczasowa pomocnicza zmienna

            bool isString = false;// zmienna do sprawdzania czy jest w srodku czy poza napisem


            // Wyłapuje pusty pliki JSON
            if (json.Length == 0)
            {
                ErrorList.Add(new Error(line, "JSON file is empty."));
            }

            for (; n < json.Length; n++)
            {
                c = json[n]; //pobiera znak dla kolejnej iteracji
                currentLine += c; //dodaje znak do aktualnej lini
                cString = c.ToString();

                if (Char.IsWhiteSpace(c))
                {
                    if ((lastChar == ':') && (!(c == '\n')))
                    {
                        TokensList.Add(new Token(Token.WhiteSpace, line, cString));
                    }
                    if (c == '\n')
                    {
                        line++;
                    }

                }
                else if (c == '{')// CHAR lub objectStart
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.objectStart, line, cString));
                    }
                    else
                    {
                        currentString.Append(c);
                    }
                }
                else if (c == '}')// CHAR lub objectEnd
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.objectEnd, line, cString));
                    }
                    else
                    {
                        currentString.Append(c);
                    }
                }
                else if (c == '[')// CHAR lub arrayStart
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.arrayStart, line, cString));
                    }
                    else
                    {
                        currentString.Append(c);
                    }
                }
                else if (c == ']') // CHAR lub arrayEnd
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.arrayEnd, line, cString));
                    }
                    else
                    {
                        currentString.Append(c);
                    }
                }
                else if (c == '"')
                {
                    if (isString && lastChar == '\\')// Sprawdzamy cz jest znak '\' przed '"' 
                    {
                        currentString.Append(cString);
                    }
                    else if (isString && lastChar != '\\')//rozpoczęty string + '"'(bez'\') == koniec string
                    {
                        isString = false;
                        string tmp1 = currentString.ToString();
                        int tmp2 = keywords(tmp1);

                        if (tmp2 == 0)//nie wykrytosłów kluczowych
                        {
                            TokensList.Add(new Token(Token.stringToken, line, tmp1));
                        }
                        else
                        {
                            TokensList.Add(new Token(tmp2, line, tmp1));
                        }
                        currentString.Length = 0;
                    }
                    else// rozpoczynamy stringa isString=true
                    {
                        isString = true;
                    }
                }
                else if (c == ':')
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.colon, line, cString));

                    }
                    else
                    {
                        currentString.Append(cString);
                    }
                }
                else if (c == ',')
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.comma, line, cString));
                    }
                    else
                    {
                        currentString.Append(cString);
                    }
                }
                else if (c == '.')
                {
                    if (isString)
                    {
                        currentString.Append(cString);
                    }
                }
                else if (isString)
                {
                    if (lastChar != '\\')
                    {
                        currentString.Append(cString);
                    }
                    else
                    {
                        ErrorList.Add(new Error(line, "Got unexpected char '\\'"));
                    }

                }
                else if (!isString)
                {

                    //NUMBER
                    if (c == '-' || c == '+' || c == 'E' || c == 'e' || c == '.' || Char.IsDigit(c))
                    {
                        //iterujemy i laczymy cyfery/znaki a potem patrzymy czy dobry regex
                        while (numberChars.Contains(json[n]) || Char.IsDigit(json[n]))
                        {
                            tmp += json[n].ToString();
                            n++;
                            currentString.Append(cString);
                        }
                        n--;
                        if (numberRegex.IsMatch(tmp))
                        {
                            TokensList.Add(new Token(Token.number, line, currentString.ToString()));
                            currentString.Length = 0;
                        }
                        else
                        {
                            ErrorList.Add(new Error(line, "Number, logic value or string not in quotes!"));
                        }
                        tmp = "";
                    }
                    //TRUE
                    else if (c == 't' || c == 'T')
                    {
                        tmp = (json[n].ToString() + json[n + 1].ToString() + json[n + 2].ToString() + json[n + 3].ToString()).ToLower();
                        if (String.Equals(tmp, "true"))
                        {
                            TokensList.Add(new Token(Token.TRUE, line, "true"));
                            n += 3;
                            tmp = "";
                            c = json[n];
                        }
                        else
                        {
                            ErrorList.Add(new Error(line, "Number, logic value or string not in quotes!"));
                        }
                    }
                    //FALSE
                    else if (c == 'f' || c == 'F')
                    {
                        tmp = (json[n].ToString() + json[n + 1].ToString() + json[n + 2].ToString() + json[n + 3].ToString() + json[n + 4]).ToLower();
                        if (String.Equals(tmp, "false"))
                        {
                            TokensList.Add(new Token(Token.FALSE, line, "false"));
                            n += 4;
                            tmp = "";
                            c = json[n];
                        }
                        else
                        {
                            ErrorList.Add(new Error(line, "Number, logic value or string not in quotes!"));
                        }
                    }
                    //NULL
                    else if (c == 'n' || c == 'N')
                    {
                        tmp = (json[n].ToString() + json[n + 1].ToString() + json[n + 2].ToString() + json[n + 3].ToString()).ToLower();
                        if (String.Equals(tmp, "null"))
                        {
                            TokensList.Add(new Token(Token.NULL, line, "null"));
                            n += 3;
                            tmp = "";
                            c = json[n];
                        }
                        else
                        {
                            ErrorList.Add(new Error(line, "Number, logic value or string not in quotes!"));
                        }
                    }
                    //else jest zle
                    else
                    {
                        ErrorList.Add(new Error(line, "Number, logic value or string not in quotes!"));
                    }


                }


                lastChar = c;
            }
            TokensList.Add(new Token(Token.END, line, "END"));
            errorList = ListToString(ErrorList);
            if (ErrorList.Count == 0)
            {
                return true;
            }
            else
            { return false; }

        }

        public string ListToString(List<Error> error)
        {
            string retval = "";
            StringBuilder errList = new StringBuilder();
            errList.AppendFormat("+Scaner: \n\t-Error({0}\n)", error.Count.ToString());

            foreach (var er in error)
            {
                errList.AppendFormat("\t-line{0} error:{1}", er.GetLine(), er.GetDescription());
            }
            return retval;
        }

        public int keywords(string parametr)
        {
            int retVal = 0;
            switch (parametr)
            {
                case "$id":
                    retVal = 51;
                    break;
                case "$schema":
                    retVal = 52;
                    break;
                case "title":
                    retVal = 53;
                    break;
                case "type":
                    retVal = 54;
                    break;
                case "properties":
                    retVal = 55;
                    break;
                case "description":
                    retVal = 56;
                    break;
                case "required":
                    retVal = 57;
                    break;
                case "minimum":
                    retVal = 58;
                    break;
                case "maximum":
                    retVal = 59;
                    break;
                case "minLength﻿":
                    retVal = 60;
                    break;
                case "definitions":
                    retVal = 61;
                    break;
                case "maxLength﻿":
                    retVal = 62;
                    break;
                case "enum":
                    retVal = 63;
                    break;
                case "$ref":
                    retVal = 64;
                    break;
            }
            return retVal;

        }
    }
}
