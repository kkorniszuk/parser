using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walidator
{
    /* Analizator składniowy */
    public class Parser
    {
        public List<Token> tokens;
        private int currentToken;        //numer obecnie przetwarzanego tokenu
        private int index;              //index obecnie przetwarzanego tokeny

        /*
         * Całość wg gramatyki w docx 
         */
        public Parser()
        {
            tokens = new List<Token>();
            currentToken = 0;
            index = 0;
        }

        public void parse(List<Token> TokensList)
        {
            tokens = TokensList;
            jsonfile();
        }

        private void jsonfile()
        {
            currentToken = tokens[++index].GetToken();
            switch (currentToken)
            {
                case Token.objectStart:
                    objectFun();
                    break;
                case Token.arrayStart:
                    array();
                    break;
                default:
                    throw new JSONException("Failed on start of JSON file! Line:1 Char:1");
            }
        }

        private void array()
        {
            checkLex(Token.arrayStart);
            elements();
            checkLex(Token.arrayEnd);
        }


        private void objectFun()
        {
            checkLex(Token.objectStart);
            properties();
            checkLex(Token.objectEnd);
        }

        private void properties()
        {
            if (currentToken == Token.qute)
            {
                pair();
                rest();
            }
        }

        private void rest()
        {
            if (currentToken == Token.comma)
            {
                checkLex(Token.comma);
                pair();
                rest();
            }
        }

        private void pair()
        {
            sstring();
            checkLex(Token.colon);//:
            value();
        }

        private void elements()
        {
            switch (currentToken)
            {
                case Token.qute:
                case Token.TRUE:
                case Token.FALSE:
                case Token.number:
                case Token.NULL:
                case Token.arrayStart:
                case Token.objectStart:
                    value();
                    restvalue();
                    break;
                default:
                    break;
            }
        }

        private void value()
        {
            switch (currentToken)
            {
                case Token.qute:
                    sstring();
                    break;
                case Token.TRUE:
                    logicVal();
                    break;
                case Token.FALSE:
                    logicVal();
                    break;
                case Token.number:
                    checkLex(Token.number);
                    break;
                case Token.NULL:
                    checkLex(Token.NULL);
                    break;
                case Token.arrayStart:
                    array();
                    break;
                case Token.objectStart:
                    objectFun();
                    break;
                default:
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index], "TRUE, FALSE, Number, Null, array, object", tokens[index].GetLine()));
            }
        }

        private void sstring()
        {
            switch (currentToken)
            {
                case Token.qute:
                    checkLex(Token.qute);
                    stringrow();
                    checkLex(Token.qute);
                    break;
                default:
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index], "QUOTE", tokens[index].GetLine()));
            }
        }

        private void stringrow()
        {
            switch (currentToken)
            {
                case Token.CHAR:
                    checkLex(Token.CHAR);
                    stringrow();
                    break;
                default:
                    break;
            }
        }

        private void logicVal()
        {
            switch (currentToken)
            {
                case Token.TRUE:
                    checkLex(Token.TRUE);
                    break;
                case Token.FALSE:
                    checkLex(Token.TRUE);
                    break;
                default:
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index], "true or false", tokens[index].GetLine()));
            }
        }

        private void restvalue()
        {
            switch (currentToken)
            {
                case Token.comma:
                    checkLex(Token.comma);
                    elements();
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// We check if lex is as intended
        /// </summary>
        /// <param name="p"></param>
        private void checkLex(int p)
        {
            if (p == currentToken)
            {
                index++;
                currentToken = tokens[index].GetToken();
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index], new Token(p, 0, 0).ToString(), tokens[index].GetLine()));
            }
        }


    }
}
