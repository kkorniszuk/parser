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

        private Token token;
        public Parser(List<Token> tokenList)
        {
            this.tokens = new List<Token>();
            this.currentToken = 0;
            this.index = 0;
            //this.token = new Token();
        }

        public int start()
        {
            int retVal = 0;
            if (this.tokens[index].GetToken() == Token.objectStart)
            {
                this.getNextToken();
                this.jsonMainSchemaStructures();
                if(this.tokens[index].GetToken() == Token.objectEnd) {
                    // success
                }
                else
                {
                    // throw error
                }
            }
            else
            {
                // throw error 
            }
            return retVal;
        }

        public int jsonMainSchemaStructures() {
            int retVal = 0;
            if(this.tokens[index].GetToken() == Token.stringToken) {
                if(this.tokens[index].GetToken() == Token.id) {

                } else if(true) {
                    
                }
            } else {
                // throw error 
            }
            return retVal;
        }

        public void getNextToken()
        {
            if (this.index < this.tokens.Count)
            {
                this.index++;
            }
            else
            {
                // end of tokens 
            }
        }

        public bool id()
        {
            return true;
        }
        public bool schema()
        {
            return true;
        }
        public bool title()
        {
            return true;
        }
        public bool type()
        {
            return true;
        }
        public bool properties()
        {
            return true;
        }
        public bool description()
        {
            return true;
        }
        public bool required()
        {
            return true;
        }
        public bool minimum()
        {
            return true;
        }
        public bool maximum()
        {
            return true;
        }
        public bool minLength()
        {
            return true;
        }
        public bool maxLength()
        {
            return true;
        }
        public bool enumToken()
        {
            return true;
        }
        public bool definitions()
        {
            return true;
        }
        public bool refToken()
        {
            return true;
        }
    }
}
