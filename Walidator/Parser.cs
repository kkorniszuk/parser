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
        private int iterationJsonMSS;  // iteration jsonMainSchemaStructures()

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
          
            //pomiń spacje
            if (ObjectStart())
            {
                //pomiń spacje
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
            iterationJsonMSS++;

            //this.getNextToken();
            if(iterationJsonMSS>1)
            {
                Comma();
            }
            

            if (this.tokens[index].GetToken() == Token.id)
            {
                this.getNextToken();
                this.idToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.schema)
            {
                this.getNextToken();
                this.schemaToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.title)
            {
                this.getNextToken();
                this.titleToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.type)
            {
                this.getNextToken();
                this.typeToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.properties)
            {
                this.getNextToken();
                this.propertiesToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.required)
            {
                this.getNextToken();

            }
            else if (this.tokens[index].GetToken() == Token.definitions)
            {
                this.getNextToken();
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

            if (this.tokens[index].GetToken() == Token.stringToken)
            {
                this.getNextToken();
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
            
            if (this.tokens[index].GetToken() == Token.comma)
            {
                this.getNextToken();
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
            
            bool retVal = Uri.IsWellFormedUriString(tokens[index].GetValString(), UriKind.RelativeOrAbsolute);
            if (!retVal)
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found Url!"));
            }
            else
            {
                getNextToken();
            }
            return retVal;
        }

        public bool ObjectStart()
        {
            bool retVal = false;
            
            if (this.tokens[index].GetToken() == Token.objectStart)
            {
                retVal = true;
                this.getNextToken();
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

            if (this.tokens[index].GetToken() == Token.objectStart)
            {
                this.getNextToken();
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
            

            if (this.tokens[index].GetToken() == Token.arrayStart)
            {
                retVal = true;
                this.getNextToken();

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
            
            if (this.tokens[index].GetToken() == Token.arrayEnd)
            {
                retVal = true;
                this.getNextToken();
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found ']' "));
            }
            return retVal;
        }

        public bool WhiteSpace()
        {
            bool retVal = false;

            if (this.tokens[index].GetToken() == Token.WhiteSpace)
            {
                this.getNextToken();
                retVal = true;
            }

            return retVal;
        }

        public bool Number()
        {
            bool retVal = true;
          
            if (this.tokens[index].GetToken() == Token.number)
            {
                this.getNextToken();
                retVal = false;
            }

            return retVal;
        }

        public bool Colon()
        {
            bool retVal = false;

            if (this.tokens[index].GetToken() == Token.colon)
            {
                this.getNextToken();
                if (this.tokens[index].GetToken() == Token.WhiteSpace)
                {
                    this.getNextToken();
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
            if (Colon() && String())
            {
                retVal = true;
            }
            return retVal;
        }

        public bool schemaToken()
        {
            bool retVal = false;
            if (Colon() && String() && isUrlString())
            {
                this.hasJsonSchema = true;
                retVal = true;
            }
            return retVal;
        }

        public bool titleToken()
        {
            bool retVal = false;
            if (Colon() && String())
            {
                retVal = true;
            }
            return retVal;
        }

        //"object" || "string" || "number" || "array" || "boolean" ||"null"
        public bool CompareStringwithItemArrayStringType(string parametr)
        {
            bool retVal = false;
            string[] tmpArray = { "object" , "string" , "number" , "array" , "boolean" , "null" };
            for (int k = 0; k < tmpArray.Length; k++)
            {
                if (parametr == tmpArray[k])
                {
                    retVal = true;
                }
            }


            return retVal;
        }

        public bool typeToken()
        {
            int itterationTypeToken = 0;
            bool retVal = false;
            if (Colon())
            {
                bool tmp=true;
                while (tmp)
                {
                    itterationTypeToken++;
                    if (itterationTypeToken > 1)
                    {
                        Comma();
                    }
                    if (String())
                    {
                        string tmpS = this.tokens[index].GetValString();
                        if (String() && (CompareStringwithItemArrayStringType(tmpS)))
                        {
                            retVal = true;
                        }
                        else
                        {
                            throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Incorrect type!"));
                        }
                    }
                    else
                    {
                        tmp = false;
                    }

                }
                
               
            }
            return retVal;
        }
        public bool propertiesToken()
        {
            bool retVal = false;
            if (Colon() && ObjectStart())
            {
                bool tmp = true;
                int iterrationObjectProperties = 0;
                while (tmp)
                {
                    iterrationObjectProperties++;

                    if (iterrationObjectProperties > 1)
                    {
                        Comma();
                    }
                    if (String() && Colon())
                    {
                        if (propertyToken())
                        {
                            retVal = true;
                            if (ObjectEnd)
                            {

                            }
                        }
                    }

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
