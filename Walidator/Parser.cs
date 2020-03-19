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
            this.tokens = new List<Token>();
            this.currentToken = 0;
            this.index = 0;
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
