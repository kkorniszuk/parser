
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
        public static List<string> lines = new List<string>(); // zbiór linii

        /// <summary>
        /// Analiza leksykalna
        /// </summary>
        /// <param name="json"></param>
        /// <param name="TokensList"></param>
        /// <returns></returns>
        public bool lexer(string json, ref List<Token> TokensList)
        {
            jsonf = json; // kopia textu json   
            lines.Clear();
            string cString;
            StringBuilder currentString = new StringBuilder();

            int line = 1;//licznik linii
            int n = 0; //licznik char

            char lastChar = ' '; //przechowuje przedostatni znak
            char c; //zmienna do iteracji po znakach

            string currentLine = ""; // akumulowana obecna linia
            string tmp = ""; //tymczasowa pomocnicza zmienna

            bool isString = false;// zmienna do sprawdzania czy jest w srodku czy poza napisem


            // Wyłapuje puste pliki JSON
            if (json.Length == 0)
            {
                throw new JSONException("JSON file is empty.\n");
            }
            else
            {
                TokensList.Add(new Token(Token.jsonFile, n, line, "START")); //Token startowy
            }


            for (; n < json.Length; n++)
            {
                c = json[n]; //pobiera znak dla kolejnej iteracji
                currentLine += c; //dodaje znak do aktualnej lini
                cString = c.ToString();

                if (Char.IsWhiteSpace(c))
                {
                    if (c == '\t')
                    {
                    }
                        if (c == '\n')
                    {
                        if (isString)
                        { currentString.Append('\n'); }
                        else
                        {
                            TokensList.Add(new Token(Token.NewLine, n, line, cString));
                        }
                    }
                    TokensList.Add(new Token(Token.WhiteSpace, n, line, cString));
                }
                else if (c == '{')// CHAR lub objectStart
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.objectStart, n, line, cString));
                    }
                    else
                    {
                        currentString.Append(c);
                        // TokensList.Add(new Token(Token.CHAR, n, currentString.Append(cString));
                    }
                }
                else if (c == '}')// CHAR lub objectEnd
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.objectEnd, n, line, cString));
                    }
                    else
                    {
                        currentString.Append(c);
                        //TokensList.Add(new Token(Token.CHAR, n, line));
                    }
                }
                else if (c == '[')// CHAR lub arrayStart
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.arrayStart, n, line, cString));
                    }
                    else
                    {
                        currentString.Append(c);
                        //TokensList.Add(new Token(Token.CHAR, n, line));
                    }
                }
                else if (c == ']') // CHAR lub arrayEnd
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.arrayEnd, n, line, cString));
                    }
                    else
                    {
                        currentString.Append(c);
                        //TokensList.Add(new Token(Token.CHAR, n, line));
                    }
                }

                /*
                 * Sprawdzamy:
                 * 1. Czy jestesmy w napisie i czy NIE ma \ przed znakiem => QUOTE
                 * 2. Czy jestesmy w napisie i czy jest \ przed znakiem => CHAR
                 * 3. Jesli nie jestesmy w napisie => QUOTE                 
                 */

                else if (c == '"')
                {
                    if (isString && lastChar == '\\')// Sprawdzamy cz jest znak '\' przed '"' 
                    {
                        currentString.Append(cString);
                        //TokensList.Add(new Token(Token.CHAR, n, line, cS));
                    }
                    else if (isString && lastChar != '\\')//rozpoczęty string + '"'(bez'\') == koniec string
                    {
                        isString = false;
                        string tmp1 = currentString.ToString();
                        int tmp2 = keywords(tmp1);


                        if (tmp2 == 0)//nie wykrytosłów kluczowych
                        {
                            TokensList.Add(new Token(Token.stringToken, n, line, tmp1));
                        }
                        else
                        {
                            TokensList.Add(new Token(tmp2, n, line, tmp1));
                        }
                        currentString.Length = 0;
                    }
                    else// rozpoczynamy stringa isString=true
                    {
                        //TokensList.Add(new Token(Token.qute, n, line,));
                        isString = true;
                    }
                }
                else if (c == ':')
                {
                    if (!isString)
                    {
                        TokensList.Add(new Token(Token.colon, n, line, cString));

                    }
                    else
                    {
                        currentString.Append(cString);
                        //TokensList.Add(new Token(Token.CHAR, n, line, ));
                    }
                }
                else if (c == ',')
                {
                    if (!isString)
                    {

                        TokensList.Add(new Token(Token.comma, n, line, cString));
                    }
                    else
                    {
                        currentString.Append(cString);
                        //TokensList.Add(new Token(Token.CHAR, n, line));
                    }
                }
                else if (c == '.')
                {
                    if (!isString)
                    {

                        //TokensList.Add(new Token(Token.CHAR, n, line));
                    }
                    else
                    {
                        currentString.Append(cString);
                        //TokensList.Add(new Token(Token.dot, n, line));
                    }
                }
                else if (c == '\n')
                {
                    if (isString)
                    {

                    }
                    else
                    {
                        lines.Add(currentLine);
                        line++;
                        currentLine = "";
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
                        throw new JSONException(ErrorMessage.errorMsg(line, n, "Got unexpected char '\\'"));
                    }

                }
                /* Jesli nie jestesmy w napisie a pojawia sie litery lub cyfry
                * 1. Jesli liczba  => NUMBER
                * 2. Sprawdzamy czy wartosc logiczna => TRUE v FALSE
                * 3. Sprawdzamy czy null => NULL
                * 
                * Jeśli żaden z powyższych zgłoś błąd 
                */
                else if (!isString)
                {
                    try
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
                                TokensList.Add(new Token(Token.number, n, line, currentString.ToString()));
                                currentString.Length = 0;
                            }

                            else
                                throw new JSONException(ErrorMessage.errorMsg(line, n, "Number, logic value or string not in quotes!"));
                            tmp = "";
                        }
                        //TRUE
                        else if (c == 't' || c == 'T')
                        {
                            tmp = (json[n].ToString() + json[n + 1].ToString() + json[n + 2].ToString() + json[n + 3].ToString()).ToLower();
                            if (String.Equals(tmp, "true"))
                            {
                                TokensList.Add(new Token(Token.TRUE, n, line, "true"));
                                n += 3;
                                tmp = "";
                                c = json[n];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, n, "Number, logic value or string not in quotes!"));
                            }
                        }
                        //FALSE
                        else if (c == 'f' || c == 'F')
                        {
                            tmp = (json[n].ToString() + json[n + 1].ToString() + json[n + 2].ToString() + json[n + 3].ToString() + json[n + 4]).ToLower();
                            if (String.Equals(tmp, "false"))
                            {
                                TokensList.Add(new Token(Token.FALSE, n, line, "false"));
                                n += 4;
                                tmp = "";
                                c = json[n];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, n, "Number, logic value or string not in quotes!"));
                            }
                        }
                        //NULL
                        else if (c == 'n' || c == 'N')
                        {
                            tmp = (json[n].ToString() + json[n + 1].ToString() + json[n + 2].ToString() + json[n + 3].ToString()).ToLower();
                            if (String.Equals(tmp, "null"))
                            {
                                TokensList.Add(new Token(Token.NULL, n, line, "null"));
                                n += 3;
                                tmp = "";
                                c = json[n];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, n, "Number, logic value or string not in quotes!"));
                            }
                        }
                        //else jest zle
                        else
                        {
                            throw new JSONException(ErrorMessage.errorMsg(line, n, "Number, logic value or string not in quotes!"));
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new JSONException(ErrorMessage.errorMsg(line, n, "Unexpected EOF!"));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new JSONException(ErrorMessage.errorMsg(line, n, "Unexpected EOF!"));
                    }
                }


                lastChar = c;
            }
            lines.Add(currentLine); //dodanie ostatniej linii
            TokensList.Add(new Token(Token.END, n, line, "END"));
            return false;
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
                case " title":
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
                case "definitions﻿":
                    retVal = 61;
                    break;
                case "maxLength﻿":
                    retVal = 62;
                    break;
                case "enum":
                    retVal = 51;
                    break;
                case "$ref":
                    retVal = 51;
                    break;
            }
            return retVal;

        }
    }
}
