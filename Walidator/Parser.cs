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
        public Parser()
        {
            this.tokens = new List<Token>();
            this.currentToken = 0;
            this.index = 0;
            this.token = new Token();
        }

        public int start() {
            if (this.tokens[index] == this.token.objectStart) {
                this.getNextToken();
                this.jsonSchemaStructure();
                if(this.tokens[index] == this.token.objectEnd) {
                    // success
                } else {
                    // throw error
                }
            } else {
                // throw error 
            }
        }

        public int jsonSchemaStructure() {
            if(this.tokens[index] == this.token.stringToken) {
            
            } else {
                // throw error 
            }
        }

        public void getNextToken() {
            if (this.index < this.tokens.Count) {
                this.index++;
            } else {
                // end of tokens 
            }
        }

        public bool id () {

        }
        public bool schema () {

        }
        public bool  title  () {

        }
        public bool type  () {

        }
        public bool properties  () {

        }
        public bool description  () {

        }
        public bool required () {

        }
        public bool  minimum  () {

        }
       public bool  maximum  () {

       }
        public bool minLength  () {

        }
        public bool maxLength  () {

        }
       public bool  enumToken  () {

       }
       public bool  definitions () {

       }
       public bool  refToken () {

       }
    }
}
