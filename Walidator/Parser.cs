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
                    this.idToken();
                    this.getNextToken();
                    this.jsonMainSchemaStructures();
                } else if(this.tokens[index].GetToken() == Token.schema) {
                    this.schemaToken();
                    this.getNextToken();
                    this.jsonMainSchemaStructures();
                } else if(this.tokens[index].GetToken() == Token.title) {
                    this.titleToken();
                    this.getNextToken();
                    this.jsonMainSchemaStructures();
                } else if(this.tokens[index].GetToken() == Token.type) {
                    this.typeToken();
                    this.getNextToken();
                    this.jsonMainSchemaStructures();
                } else if(this.tokens[index].GetToken() == Token.properties) {

                } else if(this.tokens[index].GetToken() == Token.required) {

                } else if(this.tokens[index].GetToken() == Token.definitions) {

                } else if(this.tokens[index].GetToken() == Token.objectEnd && index == this.tokens.Count - 1) {
                    // that's end of file - success 
                    return true;
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
           if(this.tokens[index].GetToken() == Token.colon) {
               this.getNextToken();
               if(this.tokens[index].GetToken() == Token.stringToken) {
                this.getNextToken();
                   if(this.getCommaToken()) {
                       return true;
                   } else {
                       // throw error - comma expected
                   }
               } else {
                   // throw error - string excepted
               }
           } else {
               // throw error - colon expected
           }
        }
        public bool schemaToken()
        {
            this.getNextToken();
            if(this.tokens[index].GetToken() == Token.colon) {
                this.getNextToken();
                if(this.)
            } else {
                // throw error - colon expected
            }
        }
        public bool titleToken()
        {   
            this.getNextToken();
            if(this.tokens[index].GetToken() == Token.colon) {
                this.getNextToken();
                if(this.tokens[index].GetToken() == Token.stringToken) {
                    this.getNextToken();
                    if(this.getCommaToken()) {
                        return true;
                    }
                } else {
                    // throw error = string expexted
                }
            } else {
                // throw error - colon expected
            }
        }
        public bool typeToken()
        {
            this.getNextToken();
            if(this.tokens[index].GetToken() == Token.colon) {
                this.getNextToken();
                if(this.tokens[index].GetToken() == Token.stringToken && 
                   (this.tokens[index].GetValString() == "object" ||
                   this.tokens[index].GetValString() == "string" ||
                   this.tokens[index].GetValString() == "number" ||
                   this.tokens[index].GetValString() == "array" ||
                   this.tokens[index].GetValString() == "boolean" ||
                   this.tokens[index].GetValString() == "null")) {
                   this.getNextToken();
                   if(this.getCommaToken()) {
                       return true;
                   } else {
                       // throw error - comma expected
                   }
                } else {
                    // throw error - string expected
                }
            } else {
                // throw error - colon expected
            }
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
        public bool getCommaToken() {
            // I will add more code later - it should throw an error on last line
            if(this.tokens[index].GetToken() == Token.comma ) {
                return true;
            } else {    
                // throw error - comma expexted
            }
        }
    }
}
