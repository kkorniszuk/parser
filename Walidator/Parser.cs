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

        private bool hasJsonSchema = false;
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
            if (this.tokens[index].GetToken() == Token.objectStart) {
                this.getNextToken();
                this.jsonMainSchemaStructures();
                if(hasJsonSchema) {
                  // success
                } else {
                    // throw error > not $schema field found 
                }
            } else {
                // throw error > not starting { found 
            }
            return retVal;
        }

        public int jsonMainSchemaStructures() {
            int retVal = 0;
                if(this.tokens[index].GetToken() == Token.id) {
                    if(true) {

                    } else {

                    }
                } else if(this.tokens[index].GetToken() == Token.schema) {
                    
                } else if(this.tokens[index].GetToken() == Token.title) {

                } else if(this.tokens[index].GetToken() == Token.type) {

                } else if(this.tokens[index].GetToken() == Token.properties) {

                } else if(this.tokens[index].GetToken() == Token.required) {

                } else if(this.tokens[index].GetToken() == Token.definitions) {

                } else if (  true ) {

                } else {
                    // throw error > symbol not found 
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

        public bool idToken()
        {
           this.getNextToken();
           if(this.tokens[index].GetToken == Token.colon) {
               this.getNextToken();
               if(this.tokens[index].GetToken == Token.stringToken) {
                   return true;
               } else {
                   // throw error - string excepted
               }
           } else {
               // throw error - colon expected
           }
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
