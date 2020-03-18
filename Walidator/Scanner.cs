
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

        /*Analiza leksykalna*/
        public bool lexer(string json, ref List<Token> TokensList)
        {
            jsonf = json; // kopia textu json   
            lines.Clear();

            int line = 1;//licznik linii
            int i = 0; //licznik char

            char lastChar = ' '; //przechowuje przedostatni znak
            char c; //zmienna do iteracji po znakach

            string currentLine = ""; // akumulowana obecna linia
            string tmp = ""; //tymczasowa pomocnicza zmienna

            bool isIn = false;// zmienna do sprawdzania czy jest w srodku czy poza napisem


            // Jeśli nic nie dostalismy => koniec
            if (json.Length == 0)
            {
                throw new JSONException("JSON file is empty.\n");
            }

            TokensList.Add(new Token(Token.jsonFile, i, line)); //dodajemy token startowy

            for (; i < json.Length; i++)
            {
                c = json[i];
                currentLine += c;
                if (c == '{')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.objectStart, i, line));
                }
                else if (c == '}')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.objectEnd, i, line));
                }
                else if (c == '[')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.arrayStart, i, line));
                }
                else if (c == ']')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.arrayEnd, i, line));
                }

                /*
                 * Sprawdzamy:
                 * 1. Czy jestesmy w napisie i czy NIE ma \ przed znakiem => QUOTE
                 * 2. Czy jestesmy w napisie i czy jest \ przed znakiem => CHAR
                 * 3. Jesli nie jestesmy w napisie => QUOTE                 
                 */

                else if (c == '"')
                {
                    if (isIn && lastChar != '\\')// (\")==(") 
                    {
                        isIn = false;
                        TokensList.Add(new Token(Token.qute, i, line));
                    }
                    else if (isIn && lastChar == '\\')
                    {
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    }
                    else
                    {
                        TokensList.Add(new Token(Token.qute, i, line));
                        isIn = true;
                    }
                }
                else if (c == ':')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.colon, i, line));
                }
                else if (c == ',')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.comma, i, line));
                }
                else if (c == '.')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.dot, i, line));
                }
                else if (c == '\n')
                {
                    lines.Add(currentLine);
                    currentLine = "";
                    line++;
                }
                else if (Char.IsWhiteSpace(c))
                {
                    continue;
                }
                /* Jesli nie jestesmy w napisie a pojawia sie litery lub cyfry
                * 1. Jesli liczba  => NUMBER
                * 2. Sprawdzamy czy wartosc logiczna => TRUE v FALSE
                * 3. Sprawdzamy czy null => NULL
                * 
                * Jeśli żaden z powyższych zgłoś błąd 
                */
                else if (!isIn)
                {
                    try
                    {
                        //NUMBER
                        if (c == '-' || c == '+' || c == 'E' || c == 'e' || c == '.' || Char.IsDigit(c))
                        {
                            //iterujemy i laczymy cyfery/znaki a potem patrzymy czy dobry regex
                            while (numberChars.Contains(json[i]) || Char.IsDigit(json[i]))
                            {
                                tmp += json[i].ToString();
                                i++;
                            }
                            i--;
                            if (numberRegex.IsMatch(tmp))
                                TokensList.Add(new Token(Token.number, i, line));
                            else
                                throw new JSONException(ErrorMessage.errorMsg(line, i, "Number, logic value or string not in quotes!"));
                            tmp = "";
                        }
                        //TRUE
                        else if (c == 't' || c == 'T')
                        {
                            tmp = (json[i].ToString() + json[i + 1].ToString() + json[i + 2].ToString() + json[i + 3].ToString()).ToLower();
                            if (String.Equals(tmp, "true"))
                            {
                                TokensList.Add(new Token(Token.TRUE, i, line));
                                i += 3;
                                tmp = "";
                                c = json[i];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, i, "Number, logic value or string not in quotes!"));
                            }
                        }
                        //FALSE
                        else if (c == 'f' || c == 'F')
                        {
                            tmp = (json[i].ToString() + json[i + 1].ToString() + json[i + 2].ToString() + json[i + 3].ToString() + json[i + 4]).ToLower();
                            if (String.Equals(tmp, "false"))
                            {
                                TokensList.Add(new Token(Token.FALSE, i, line));
                                i += 4;
                                tmp = "";
                                c = json[i];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, i, "Number, logic value or string not in quotes!"));
                            }
                        }
                        //NULL
                        else if (c == 'n' || c == 'N')
                        {
                            tmp = (json[i].ToString() + json[i + 1].ToString() + json[i + 2].ToString() + json[i + 3].ToString()).ToLower();
                            if (String.Equals(tmp, "null"))
                            {
                                TokensList.Add(new Token(Token.NULL, i, line));
                                i += 3;
                                tmp = "";
                                c = json[i];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, i, "Number, logic value or string not in quotes!"));
                            }
                        }
                        //else jest zle
                        else
                        {
                            throw new JSONException(ErrorMessage.errorMsg(line, i, "Number, logic value or string not in quotes!"));
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new JSONException(ErrorMessage.errorMsg(line, i, "Unexpected EOF!"));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new JSONException(ErrorMessage.errorMsg(line, i, "Unexpected EOF!"));
                    }
                }
                else if (isIn)
                {
                    if (lastChar != '\\')
                    {
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    }
                    else
                    {
                        throw new JSONException(ErrorMessage.errorMsg(line, i, "Got unexpected char '\\'"));
                    }
                }

                lastChar = c;
            }
            lines.Add(currentLine); //dodanie ostatniej linii
            TokensList.Add(new Token(Token.END, i, line));
            return false;
        }
    }
}



//    {
//        public static string jsonText = string.Empty;
//        public static List<string> allLines = new List<string>();
//        private static Regex numberRegex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?$"); // double, int i postaci wykladnicze
//        private static char[] numberChars = { 'e', 'E', '-', '.', '+' };//tablica z akceptowanymi znakami dla liczby nie liczac cyfr

//        //public lex(string json,List<Token> listTokens )
//        //{
//        //    jsonText = json;

//        //}

//        private bool lexer(string json, List<Token> listTokens)
//        {
//            allLines.Clear();
//            jsonText = json;
//            char lastchar = ' ';

//            int counterLine = 1;
//            int counterChar = 0;

//            bool inWord = false;

//            if (jsonText == "")
//            {
//                throw new JSONException("JSON file is empty.\n");

//            }

//            foreach (var c in jsonText)
//            {

//                if (c == '{')
//                {
//                    if (inWord)
//                        listTokens.Add(new Token(Token.CHAR, i, line));
//                    else
//                        listTokens.Add(new Token(Token.OBJECTSTART, i, line));
//                }
//                else if (c == '}')
//                {
//                    if (inWord)
//                        listTokens.Add(new Token(Token.CHAR, i, line));
//                    else
//                        listTokens.Add(new Token(Token.OBJECTEND, i, line));
//                }
//                else if (c == '[')
//                {
//                    if (inWord)
//                        listTokens.Add(new Token(Token.CHAR, i, line));
//                    else
//                        listTokens.Add(new Token(Token.ARRAYSTART, i, line));
//                }
//                else if (c == ']')
//                {
//                    if (inWord)
//                        listTokens.Add(new Token(Token.CHAR, i, line));
//                    else
//                        listTokens.Add(new Token(Token.ARRAYEND, i, line));
//                }

//                //////////////////////////////////
//                ///


//                else if (c == '"')
//                {
//                    if (inWord && lastChar != '\\')
//                    {
//                        isIn = false;
//                        TokensList.Add(new Token(Token.QUOTE, i, line));
//                    }
//                    else if (isIn && lastChar == '\\')
//                    {
//                        TokensList.Add(new Token(Token.CHAR, i, line));
//                    }
//                    else
//                    {
//                        TokensList.Add(new Token(Token.QUOTE, i, line));
//                        isIn = true;
//                    }
//                }
//                else if (c == ':')
//                {
//                    if (isIn)
//                        TokensList.Add(new Token(Token.CHAR, i, line));
//                    else
//                        TokensList.Add(new Token(Token.COLON, i, line));
//                }
//                else if (c == ',')
//                {
//                    if (isIn)
//                        TokensList.Add(new Token(Token.CHAR, i, line));
//                    else
//                        TokensList.Add(new Token(Token.COMMA, i, line));
//                }
//                else if (c == '.')
//                {
//                    if (isIn)
//                        TokensList.Add(new Token(Token.CHAR, i, line));
//                    else
//                        TokensList.Add(new Token(Token.DOT, i, line));
//                }
//                else if (c == '\n')
//                {
//                    lines.Add(currentLine);
//                    currentLine = "";
//                    line++;
//                }
//                else if (Char.IsWhiteSpace(c))
//                {
//                    continue;
//                }



//            }


//            return true;
//        }




//    }
//}