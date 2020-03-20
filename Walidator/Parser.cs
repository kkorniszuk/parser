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
        }

        public int start()
        {
            int retVal = 0;
            if (this.tokens[index].GetToken() == Token.objectStart)
            {
                this.jsonMainSchemaStructures();
                if (hasJsonSchema)
                {
                    // success - file is valid
                }
                else
                {
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "JSON schema not valid - $schema field is required."));
                }
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Starting symbol } required."));
            }
            return retVal;
        }

        public int jsonMainSchemaStructures()
        {
            this.getNextToken();
            int retVal = 0;

            if (this.tokens[index].GetToken() == Token.id)
            {
                this.idToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.schema)
            {
                this.schemaToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.title)
            {
                this.titleToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.type)
            {
                this.typeToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.properties)
            {
                this.propertiesToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.required)
            {


            }
            else if (this.tokens[index].GetToken() == Token.definitions)
            {
                this.definitionsToken();
                this.jsonMainSchemaStructures();
            }
            else if (this.tokens[index].GetToken() == Token.objectEnd && index == this.tokens.Count - 1)
            {
                // not sure what type should it be 
                retVal = true;
            }
            else
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Symbol not valid."));
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
            if (CheckStartOfObject())
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
        public bool definitionsToken()
        {
            bool retVal = false;
            CheckStartOfObject();
            definitionToken();
            if(ObjectEnd()) 
            {
                if(Comma()) 
                {
                    return true;
                }
                else 
                {
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected coma"));
                }
            }
            else 
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected end of object"));
            }
            return retVal;
        }

        public bool definitionToken() {
            bool retVal = false;
            if(checkNextTokenValue().GetToken() == token.objectEnd) 
            {
                // it should return true - next token will be end of object
                retVal = true;
            }
            else if(checkNextTokenValue().GetToken() == token.stringToken) 
            {
                if(String())
                {
                    CheckStartOfObject();
                    getNextToken();
                    if(tokens[index].GetToken() == token.type) 
                    {
                        typeToken();
                    }
                    else if (tokens[index].GetToken() == token.properties)
                    {
                        propertiesToken();
                    }
                    else 
                    {
                        throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected keywords: type or properties"));
                    }
                    ObjectEnd();
                }
                else 
                {
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected string token"));
                }
                definitionToken();
            }
            else 
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected property name or end of object. "));
            }
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

    // only take value of next token without changing current index
        public Token checkNextTokenValue()
        {
            try
            {
                return tokens[index + 1];                
            }
            catch (System.Exception)
            {
                throw SystemException;
            }
        }

        public bool CheckStartOfObject() 
        {
            bool retVal = false;

            if(Colon()) 
            {
                if(ObjectStart()) 
                {
                    retVal = true;
                }
                else 
                {
                    throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected start of object"));    
                }
            }
            else 
            {
                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected colon"));    
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
