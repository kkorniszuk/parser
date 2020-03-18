
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

        private static Regex numberRegex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?$"); // double, int charCounter postaci wykladnicze
        private static char[] numberChars = { 'e', 'E', '-', '.', '+' };//tablica z akceptowanymi znakami dla liczby nie liczac cyfr
        public static string jsonf; //kopia tekstu json
        public static List<string> lines = new List<string>(); // zbiór linii

        /*Analiza leksykalna*/
        public bool lexer(string json, ref List<Token> TokensList)
        {
            jsonf = json; // kopia textu json   
            lines.Clear();

            int line = 1;//licznik linii
            int charCounter = 0; //licznik char

            char lastChar = ' '; //przechowuje przedostatni znak
            char iterateSigns; //zmienna do iteracji po znakach

            string currentLine = ""; // akumulowana obecna linia
            string tmp = ""; //tymczasowa pomocnicza zmienna

            bool isInsideText = false;// zmienna do sprawdzania czy jest w srodku czy poza napisem


            // Jeśli nic nie dostalismy => koniec
            if (json.Length == 0)
            {
                throw new JSONException("JSON file is empty.\n");
            }

            TokensList.Add(new Token(Token.jsonFile, charCounter, line)); //dodajemy token startowy

            for (; charCounter < json.Length; charCounter++)
            {
                iterateSigns = json[charCounter];
                currentLine += iterateSigns;
                if (iterateSigns == '{')
                {
                    if (isInsideText)
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    else
                        TokensList.Add(new Token(Token.objectStart, charCounter, line));
                }
                else if (iterateSigns == '}')
                {
                    if (isInsideText)
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    else
                        TokensList.Add(new Token(Token.objectEnd, charCounter, line));
                }
                else if (iterateSigns == '[')
                {
                    if (isInsideText)
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    else
                        TokensList.Add(new Token(Token.arrayStart, charCounter, line));
                }
                else if (iterateSigns == ']')
                {
                    if (isInsideText)
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    else
                        TokensList.Add(new Token(Token.arrayEnd, charCounter, line));
                }

                /*
                 * Sprawdzamy:
                 * 1. Czy jestesmy w napisie i czy NIE ma \ przed znakiem => QUOTE
                 * 2. Czy jestesmy w napisie i czy jest \ przed znakiem => CHAR
                 * 3. Jesli nie jestesmy w napisie => QUOTE                 
                 */

                else if (iterateSigns == '"')
                {
                    if (isInsideText && lastChar != '\\')// (\")==(") 
                    {
                        isInsideText = false;
                        TokensList.Add(new Token(Token.qute, charCounter, line));
                    }
                    else if (isInsideText && lastChar == '\\')
                    {
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    }
                    else
                    {
                        TokensList.Add(new Token(Token.qute, charCounter, line));
                        isInsideText = true;
                    }
                }
                else if (iterateSigns == ':')
                {
                    if (isInsideText)
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    else
                        TokensList.Add(new Token(Token.colon, charCounter, line));
                }
                else if (iterateSigns == ',')
                {
                    if (isInsideText)
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    else
                        TokensList.Add(new Token(Token.comma, charCounter, line));
                }
                else if (iterateSigns == '.')
                {
                    if (isInsideText)
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    else
                        TokensList.Add(new Token(Token.dot, charCounter, line));
                }
                else if (iterateSigns == '\n')
                {
                    lines.Add(currentLine);
                    currentLine = "";
                    line++;
                }
                else if (Char.IsWhiteSpace(iterateSigns))
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
                else if (!isInsideText)
                {
                    try
                    {
                        //NUMBER
                        if (iterateSigns == '-' || iterateSigns == '+' || iterateSigns == 'E' || iterateSigns == 'e' || iterateSigns == '.' || Char.IsDigit(iterateSigns))
                        {
                            //iterujemy charCounter laczymy cyfery/znaki a potem patrzymy czy dobry regex
                            while (numberChars.Contains(json[charCounter]) || Char.IsDigit(json[charCounter]))
                            {
                                tmp += json[charCounter].ToString();
                                charCounter++;
                            }
                            charCounter--;
                            if (numberRegex.IsMatch(tmp))
                                TokensList.Add(new Token(Token.number, charCounter, line));
                            else
                                throw new JSONException(ErrorMessage.errorMsg(line, charCounter, "Number, logic value or string not in quotes!"));
                            tmp = "";
                        }
                        //TRUE
                        else if (iterateSigns == 't' || iterateSigns == 'T')
                        {
                            tmp = (json[charCounter].ToString() + json[charCounter + 1].ToString() + json[charCounter + 2].ToString() + json[charCounter + 3].ToString()).ToLower();
                            if (String.Equals(tmp, "true"))
                            {
                                TokensList.Add(new Token(Token.TRUE, charCounter, line));
                                charCounter += 3;
                                tmp = "";
                                iterateSigns = json[charCounter];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, charCounter, "Number, logic value or string not in quotes!"));
                            }
                        }
                        //FALSE
                        else if (iterateSigns == 'f' || iterateSigns == 'F')
                        {
                            tmp = (json[charCounter].ToString() + json[charCounter + 1].ToString() + json[charCounter + 2].ToString() + json[charCounter + 3].ToString() + json[charCounter + 4]).ToLower();
                            if (String.Equals(tmp, "false"))
                            {
                                TokensList.Add(new Token(Token.FALSE, charCounter, line));
                                charCounter += 4;
                                tmp = "";
                                iterateSigns = json[charCounter];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, charCounter, "Number, logic value or string not in quotes!"));
                            }
                        }
                        //NULL
                        else if (iterateSigns == 'n' || iterateSigns == 'N')
                        {
                            tmp = (json[charCounter].ToString() + json[charCounter + 1].ToString() + json[charCounter + 2].ToString() + json[charCounter + 3].ToString()).ToLower();
                            if (String.Equals(tmp, "null"))
                            {
                                TokensList.Add(new Token(Token.NULL, charCounter, line));
                                charCounter += 3;
                                tmp = "";
                                iterateSigns = json[charCounter];
                            }
                            else
                            {
                                throw new JSONException(ErrorMessage.errorMsg(line, charCounter, "Number, logic value or string not in quotes!"));
                            }
                        }
                        // throw exception - symbol is not correct
                        else
                        {
                            throw new JSONException(ErrorMessage.errorMsg(line, charCounter, "Number, logic value or string not in quotes!"));
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new JSONException(ErrorMessage.errorMsg(line, charCounter, "Unexpected EOF!"));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new JSONException(ErrorMessage.errorMsg(line, charCounter, "Unexpected EOF!"));
                    }
                }
                else if (isInsideText)
                {
                    if (lastChar != '\\')
                    {
                        TokensList.Add(new Token(Token.CHAR, charCounter, line));
                    }
                    else
                    {
                        throw new JSONException(ErrorMessage.errorMsg(line, charCounter, "Got unexpected char '\\'"));
                    }
                }

                lastChar = iterateSigns;
            }
            lines.Add(currentLine); //dodanie ostatniej linii
            TokensList.Add(new Token(Token.END, charCounter, line));
            return false;
        }
    }
}

