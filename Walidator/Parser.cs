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
            if (this.tokens[index].GetToken() == Token.objectStart)
            {
                this.getNextToken();
                this.jsonMainSchemaStructures();
                if (hasJsonSchema)
                {
                    // success
                }
                else
                {
                    // throw error > not $schema field found 
                }
            }
            else
            {
                // throw error > not starting { found 
            }
            return retVal;
        }

        public int jsonMainSchemaStructures()
        {
            int retVal = 0;

            if (this.tokens[index].GetToken() == Token.id)
            {
                this.idToken();
                this.getNextToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.schema)
            {
                this.schemaToken();
                this.getNextToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.title)
            {
                this.titleToken();

                this.getNextToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.type)
            {
                this.typeToken();
                this.getNextToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.properties)
            {
                this.propertiesToken();
                this.getNextToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.required)
            {


            }
            else if (this.tokens[index].GetToken() == Token.definitions)
            {

            }
            else if (this.tokens[index].GetToken() == Token.objectEnd && index == this.tokens.Count - 1)
            {
                // that's end of file - success 

            }
            else
            {

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

        public bool String()
        {
            bool retVal = false;

            this.getNextToken();
            if (this.tokens[index].GetToken() == Token.stringToken)
            {
                retVal = true;
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found string!"));
            }

            return retVal;
        }


        public bool Comma()
        {
            bool retVal = false;

            this.getNextToken();
            if (this.tokens[index].GetToken() == Token.comma)
            {
                retVal = true;
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found comma!"));
            }

            return retVal;
        }


        public bool isUrlString()
        {
            this.getNextToken();
            bool retVal = Uri.IsWellFormedUriString(tokens[index].GetValString(), UriKind.RelativeOrAbsolute);
            if (!retVal)
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found Url!"));
            }
            return retVal;
        }

        public bool ObjectStart()
        {
            bool retVal = false;
            this.getNextToken();


            if (this.tokens[index].GetToken() == Token.objectStart)
            {
                retVal = true;
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found '{' "));
            }
            return retVal;
        }

        public bool ObjectEnd()
        {
            bool retVal = false;
            this.getNextToken();


            if (this.tokens[index].GetToken() == Token.objectStart)
            {
                retVal = true;
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found '}' "));
            }
            return retVal;
        }

        public bool ArrayStart()
        {
            bool retVal = false;
            this.getNextToken();


            if (this.tokens[index].GetToken() == Token.arrayStart)
            {
                retVal = true;
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found '[' "));
            }
            return retVal;
        }

        public bool ArrayEnd()
        {
            bool retVal = false;
            this.getNextToken();


            if (this.tokens[index].GetToken() == Token.arrayEnd)
            {
                retVal = true;
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found ']' "));
            }
            return retVal;
        }

        public bool WhiteSpace()
        {
            bool retVal = true;

            this.getNextToken();
            if (this.tokens[index].GetToken() == Token.WhiteSpace)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool Number()
        {
            bool retVal = true;

            this.getNextToken();
            if (this.tokens[index].GetToken() == Token.number)
            {
                retVal = false;
            }

            return retVal;
        }

        public bool Colon()
        {
            bool retVal = false;

            this.getNextToken();
            if (this.tokens[index].GetToken() == Token.colon)
            {
                this.getNextToken();
                if (this.tokens[index].GetToken() == Token.WhiteSpace)
                {
                    bool Tmp = true;
                    while (Tmp)
                    {
                        Tmp = WhiteSpace();
                    }
                    retVal = true;
                }
                else
                {
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found WhiteSpace!"));
                }
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found Colon!"));
            }
            return retVal;
        }

        public bool idToken()
        {
            bool retVal = false;
            if (Colon() && String() && Comma())
            {
                retVal = true;
            }
            return retVal;
        }

        public bool schemaToken()
        {
            bool retVal = false;
            if (Colon() && String() && isUrlString() && Comma())
            {

                this.hasJsonSchema = true;
                retVal = true;

            }
            return retVal;
        }

        public bool titleToken()
        {
            bool retVal = false;
            if (Colon() && String() && Comma())
            {
                retVal = true;
            }
            return retVal;
        }

        public bool typeToken()
        {
            bool retVal = false;
            if (Colon())
            {
                this.getNextToken();
                if (this.tokens[index].GetToken() == Token.stringToken &&
                   (this.tokens[index].GetValString() == "object" ||
                   this.tokens[index].GetValString() == "string" ||
                   this.tokens[index].GetValString() == "number" ||
                   this.tokens[index].GetValString() == "array" ||
                   this.tokens[index].GetValString() == "boolean" ||
                   this.tokens[index].GetValString() == "null"))
                {
                    if (Comma())
                    {
                        retVal = true;
                    }

                }
                else
                {
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found string!"));
                }
            }
            return retVal;
        }
        public bool propertiesToken()
        {
            bool retVal = false;
            if (Colon() && ObjectStart())
            {
                if (String() && Colon())
                {
                    bool tmp = this.propertyToken();
                    while (tmp)
                    {
                        tmp = this.propertyToken();
                        retVal = true;
                    }
                }
                else
                {
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found any object in properties!"));
                }

            }
            return retVal;
        }

        public bool propertyToken()
        {
            bool retVal = false;

            this.getNextToken();
            if (typeToken() || minimum() || description() || enumToken())
            {
                retVal = true;

            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found any value in object"));
            }

            return retVal;
        }
        public bool description()
        {
            bool retVal = false;

            if (Colon() && String() && Comma())
            {
                retVal = true;
            }
            return retVal;
        }
        public bool required()
        {
            bool retVal = false;
            if (Colon() && ArrayStart())
            {
                bool tmp = true;
                while (tmp)
                {
                    if (String())
                    {
                        if (Comma())
                        {
                            tmp = true;
                        }
                        else
                        {
                            tmp = false;
                            if (String())
                            {
                                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found comma between value in array "));
                            }

                        }
                    }
                    else
                    {
                        tmp = false;
                        throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found any value in array "));

                    }
                }

            }

            return retVal;
        }
        public bool minimum()
        {
            bool retVal = false;
            if (Colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }
        public bool maximum()
        {
            bool retVal = false;
            if (Colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }
        public bool minLength()
        {
            bool retVal = false;
            if (Colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }
        public bool maxLength()
        {
            bool retVal = false;
            if (Colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }
        public bool enumToken()
        {
            bool retVal = false;
            if (Colon() && ArrayStart())
            {
                bool tmp = true;
                while (tmp)
                {
                    if (String())
                    {
                        if (Comma())
                        {
                            tmp = true;
                        }
                        else
                        {
                            tmp = false;
                            if (String())
                            {
                                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found comma between value in array "));
                            }

                        }
                    }
                    else
                    {
                        tmp = false;
                        throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found comma between value in array "));

                    }
                }

            }
            return retVal;
        }
        public bool definitions()
        {
            bool retVal = false;

            return retVal;
        }
        public bool refToken()
        {
            // current token is ref element
            bool retVal = false;
            if (String())
            {
                string refString = tokens[index].GetValString();
                if (refString[0] == '#')
                {
                    retVal = true;
                }
                else
                {
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Invalid path to ref"));
                }
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "String expected!"));
            }
            return retVal;
        }
        //public bool getCommaToken() {
        //    // I will add more code later - it should throw an error on last line
        //    if(this.tokens[index].GetToken() == Token.comma ) {
        //        return true;
        //    } else {    
        //        // throw error - comma expexted
        //    }
        //}
        //public bool getCommaToken() {
        //    // I will add more code later - it should throw an error on last line
        //    if(this.tokens[index].GetToken() == Token.comma ) {
        //        return true;
        //    } else {    
        //        // throw error - comma expexted
        //    }
        //}



    }
}
