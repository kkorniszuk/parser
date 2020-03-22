
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
        private int index;              //index obecnie przetwarzanego tokeny
        List<Error> ErrorList = new List<Error>();
        StringBuilder errorListDisplay = new StringBuilder();
        int errorCounter=0;
      
        public Parser(List<Token> tokenList)
        {
            this.tokens = tokenList;
            this.index = 0;
        }



        public string start()
        {
            errorListDisplay.AppendFormat("+Praser: \n", errorCounter.ToString());
            string retVal = "";

            if (ObjectStart())
            {
                jsonMainSchemaStructures();

            }

            
            if(errorCounter==0)
            {
                errorListDisplay.AppendFormat("\n---- JSON Valid ----", errorCounter.ToString());
            }

            return retVal = errorListDisplay.ToString();
        }

    
        private string ErrorListToString(List<Error> errorList)
        {
            string retVal = "";
            StringBuilder ListError = new StringBuilder();
            int i = 0;

            foreach (var error in errorList)
            {
                if (i < 1)
                {
                    ListError.AppendFormat("  Line:{0} Error:{1} \n", error.GetLine(), error.GetDescription());
                }
                else
                {
                    ListError.AppendFormat("   - line:{0} error:{1} \n", error.GetLine(), error.GetDescription());
                }
                i++;
            }

            retVal = ListError.ToString();
            return retVal;
        }

        /// <summary>
        /// Check jsonMainSchemaStructures::= '{' keywords (',' keywords)*'}'
        /// </summary>
        public bool jsonMainSchemaStructures()
        {
            bool retVal = false;
            int iterationJsonMSS = 0;

            bool tmpMain = true;
            while (tmpMain)
            {
                int keywords = 0;
                if (iterationJsonMSS > 0)
                {
                    if (Comma())
                    {
                        keywords = Keywords();
                        if (keywords == 1)
                        {
                            tmpMain = true;
                        }
                        else if (keywords == 2)
                        {
                            tmpMain = false;
                            if (hasJsonSchema)
                            {
                                tmpMain = false;
                                errorListDisplay.Append("JSON schema not valid - Found not '}'");
                            }
                            else
                            {
                                tmpMain = false;
                                errorListDisplay.Append("JSON schema not valid - $schema field is required.");
                                errorListDisplay.Append("                      - Found not '}'");

                            }
                        }
                        else if (keywords == 3)
                        {
                            keywords = Keywords();
                            if (hasJsonSchema)
                            {
                                tmpMain = false;
                            }
                            else
                            {
                                tmpMain = false;
                                errorListDisplay.Append("JSON schema not valid - $schema field is required.");
                            }
                        }
                        else if (keywords == 2)
                        {
                            tmpMain = false;
                            if (hasJsonSchema)
                            {
                                tmpMain = false;
                                errorListDisplay.Append("JSON schema not valid - Found not '}'");
                            }
                            else
                            {
                                tmpMain = false;
                                errorListDisplay.Append("JSON schema not valid - $schema field is required.");
                                errorListDisplay.Append("                      - Found not '}'");

                            }
                        }
                    }
                    else
                    {
                        keywords=Keywords();
                        if (keywords == 3)
                        {
                            keywords = Keywords();
                            if (hasJsonSchema)
                            {
                                tmpMain = false;
                            }
                            else
                            {
                                tmpMain = false;
                                errorListDisplay.Append("JSON schema not valid - $schema field is required.");
                            }
                        }
                    }
                }
                else
                {
                    keywords = Keywords();
                    if (keywords == 1)
                    {
                        tmpMain = true;
                    }
                    else
                    {
                        errorListDisplay.Append("JSON schema not valid - $schema field is required.");
                    }

                }
                iterationJsonMSS++;
            }
            return retVal;

        }

        /// <summary>
        /// index++
        /// </summary>
        public void getNextToken()
        {
            if (this.index < this.tokens.Count)
            {
                this.index++;
            }
        }

        /// <summary>
        /// loop checking keywords
        /// </summary>
        /// <returns>return 1 if decect keywords, return 2 if detect EOF, return 3 if detect '}'</returns>
        public int Keywords()
        {
            int retVal = 1;//1-continue, 2-EOF, 3 objectEnd


            if (this.tokens[index].GetToken() == Token.id)
            {
                this.getNextToken();
                if (idToken())
                {
                    ErrorList.Clear();
                }
                else
                {
                    errorCounter = errorCounter + ErrorList.Count;
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
            }
            else if (this.tokens[index].GetToken() == Token.schema)
            {
                this.getNextToken();
                if (schemaToken())
                {
                    hasJsonSchema = true;
                    ErrorList.Clear();
                }
                else
                {
                    errorCounter = errorCounter + ErrorList.Count;
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
            }
            else if (this.tokens[index].GetToken() == Token.title)
            {
                this.getNextToken();
                if (titleToken())
                {
                    ErrorList.Clear();
                }
                else
                {
                    errorCounter = errorCounter + ErrorList.Count;
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
            }
            else if (this.tokens[index].GetToken() == Token.type)
            {
                this.getNextToken();
                if (typeToken())
                {
                    ErrorList.Clear();
                }
                else
                {
                    errorCounter = errorCounter + ErrorList.Count;
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
            }
            else if (this.tokens[index].GetToken() == Token.properties)
            {
                this.getNextToken();
                if (propertiesToken())
                {
                    ErrorList.Clear();
                }
                else
                {
                    errorCounter = errorCounter + ErrorList.Count;
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
            }
            else if (this.tokens[index].GetToken() == Token.required)
            {
                this.getNextToken();
                if (required())
                {
                    ErrorList.Clear();
                }
                else
                {
                    errorCounter = errorCounter + ErrorList.Count;
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
            }
            else if (this.tokens[index].GetToken() == Token.definitions)
            {
                this.getNextToken();
                if (definitionsToken())
                {
                    ErrorList.Clear();
                }
                else
                {
                    errorCounter = errorCounter + ErrorList.Count;
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
            }
            else if ((this.tokens[index].GetToken() == Token.END))
            {
                retVal = 2;
            }
            else
            {
                if ((this.tokens[index].GetToken() == Token.objectEnd))
                { retVal = 3; }
                else
                {
                    errorListDisplay.AppendFormat("  - line{0}:Symbol not valid( error:{1})\n", tokens[index].GetLine(), Error.ErrorObjectSstop);
                }
            }

            return retVal;

        }

        /// <summary>
        /// Check string
        /// </summary>
        /// <returns></returns>
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
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorString));
            }

            return retVal;
        }

        /// <summary>
        /// Check ','
        /// </summary>
        /// <returns></returns>
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
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorComma));
            }

            return retVal;
        }


        /// <summary>
        /// Check Url
        /// </summary>
        /// <returns></returns>
        public bool isUrlString()
        {
            index--;
            bool retVal = Uri.IsWellFormedUriString(tokens[index].GetValString(), UriKind.RelativeOrAbsolute);
            if (!retVal)
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorUrl));
            }
            else
            {
                getNextToken();
            }
            return retVal;
        }

        /// <summary>
        /// Check '{'
        /// </summary>
        /// <returns></returns>
        public bool ObjectStart()
        {
            bool retVal = false;
            int a = this.tokens[index].GetToken();
            if (this.tokens[index].GetToken() == Token.objectStart)
            {
                retVal = true;
                this.getNextToken();
            }
            else
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorObjectStart));
            }
            return retVal;
        }

        /// <summary>
        /// Check '}'
        /// </summary>
        /// <returns></returns>
        public bool objectEnd()
        {
            bool retVal = false;

            if (this.tokens[index].GetToken() == Token.objectEnd)
            {
                this.getNextToken();
                retVal = true;
            }
            else
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorObjectSstop));
            }
            return retVal;
        }

        /// <summary>
        /// Check '['
        /// </summary>
        /// <returns></returns>
        public bool arrayStart()
        {
            bool retVal = false;

            if (this.tokens[index].GetToken() == Token.arrayStart)
            {
                retVal = true;
                this.getNextToken();

            }
            else
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorArraytStart));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found '[' "));
            }
            return retVal;
        }

        /// <summary>
        /// Check ']'
        /// </summary>
        /// <returns></returns>
        public bool arrayEnd()
        {
            bool retVal = false;



            if (this.tokens[index].GetToken() == Token.arrayEnd)
            {
                retVal = true;
                this.getNextToken();
            }
            else
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorArrayEnd));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found ']' "));
            }
            return retVal;
        }

        /// <summary>
        /// Check WhiteSpace
        /// </summary>
        /// <returns></returns>
        public bool whiteSpace()
        {
            bool retVal = false;

            if (this.tokens[index].GetToken() == Token.WhiteSpace)
            {
                this.getNextToken();
                retVal = true;
            }

            return retVal;
        }

        
        /// <summary>
        /// Check number
        /// </summary>
        /// <returns></returns>
        public bool Number()
        {
            bool retVal = false;


            if (this.tokens[index].GetToken() == Token.number)
            {
                this.getNextToken();
                retVal = true;
            }

            return retVal;
        }

        /// <summary>
        /// Chheck ': '
        /// </summary>
        /// <returns></returns>
        public bool colon()
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
                        Tmp = whiteSpace();
                    }
                    retVal = true;
                }
                else
                {
                    ErrorList.Add(new Error(tokens[index].GetLine(), Error.WhiteSpace));
                }
            }
            else
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorColon));
            }
            return retVal;
        }

        /// <summary>
        /// Check $id: string
        /// </summary>
        /// <returns>True if correct </returns>
        public bool idToken() // : string
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorID));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (colon() && String())
            {
                retVal = true;
            }
            return retVal;
        }

        public bool schemaToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorSchema));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (colon() && String() && isUrlString())
            {

                this.hasJsonSchema = true;
                retVal = true;

            }
            return retVal;
        }

        /// <summary>
        /// Check title::= string 
        /// </summary>
        /// <returns></returns>
        public bool titleToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorTitle));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (colon() && String())
            {
                retVal = true;
            }
            return retVal;
        }


        /// <summary>
        ///  Compare String with Item Array StringType
        ///  "object" || "string" || "number" || "array" || "boolean" ||"null"
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool stringType(string parametr)
        {

            bool retVal = false;
            string[] tmpArray = { "object", "string", "number", "array", "boolean", "null", "integer" };
            for (int k = 0; k < tmpArray.Length; k++)
            {
                if (parametr == tmpArray[k])
                {
                    getNextToken();
                    retVal = true;
                }

            }

            if (!retVal)
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorStringType));
            }

            return retVal;
        }


        /// <summary>
        /// Check type : StringType (',' StringType)*
        /// </summary>
        /// <returns></returns>
        public bool typeToken() //  ::= (StringType ',')*
        {
            int iterationType = 0;
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorType));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (colon())
            {
                bool tmp = true;
                while (tmp)
                {
                    string tmpS = this.tokens[index].GetValString();

                    if ((iterationType < 1) && stringType(tmpS))
                    {
                        tmp = true;
                        retVal = true;
                    }
                    else if (Comma())
                    {
                        tmpS = this.tokens[index].GetValString();
                        if (stringType(tmpS))
                        {
                            retVal = true;
                            tmp = true;
                        }
                        else
                        {
                            if (String())
                            {
                                tmp = true;
                                retVal = false;
                                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorStringType));
                            }
                            else
                            {
                                ErrorList.Remove(ErrorList.Last());
                                ErrorList.Remove(ErrorList.Last());
                                index--;
                                tmp = false;
                                
                            }
                        }

                    }
                    else 
                    {
                        
                        tmp = false;
                    }

                    iterationType++;
                }
            }
            return retVal;
        }


        /// <summary>
        /// Check properties= '{' ObjectProperties (',' ObjectProperties)* '}'
        /// </summary>
        /// <returns></returns>
        public bool propertiesToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorProperties));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;


            if (colon() && ObjectStart())
            {
                bool tmp = true;
                int iterrationProperties = 0;

                while (tmp)
                {
                    string tmpS = this.tokens[index].GetValString();

                    if ((iterrationProperties < 1) && objectProperties())
                    {
                        tmp = true;
                        retVal = true;
                    }
                    else
                    {
                        if (Comma())
                        {
                            if (objectProperties())
                            {
                                retVal = true;
                                tmp = true;
                            }
                            else
                            {
                                tmp = false;
                                retVal = false;
                            }
                        }
                        else
                        {
                            if (objectProperties())
                            {
                                retVal = false;
                                tmp = false;
                            }
                            else
                            {
                                if (objectEnd())
                                {
                                    retVal = true;
                                    tmp = false;
                                }
                                else
                                {
                                    retVal = false;
                                    tmp = false;
                                }
                            }

                        }

                    }
                    iterrationProperties++;
                }

            }
            return retVal;
        }

        /// <summary>
        /// Check ObjectProperties::= '{' fields '}'
        /// </summary>
        /// <returns></returns>
        public bool objectProperties()
        {
            bool retVal = false;

            if (String() && colon() && ObjectStart())
            {

                if (fields())
                {
                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            else
            {
                retVal = false;
            }
            return retVal;
        }

        /// <summary>
        /// fields::= fieldsItems (','fieldsItems)*
        /// </summary>
        /// <returns></returns>
        public bool fields()
        {
            bool retVal = false;

            bool tmp = true;
            int iterrationFields = 0;

            while (tmp)
            {
                string tmpS = this.tokens[index].GetValString();

                if ((iterrationFields < 1) && fieldsItem())
                {
                    tmp = true;
                    retVal = true;
                }
                else
                {
                    if (Comma())
                    {
                        if (fieldsItem())
                        {
                            retVal = true;
                            tmp = true;
                        }
                        else
                        {
                            tmp = false;
                            retVal = false;
                        }
                    }
                    else
                    {
                        if (objectEnd())
                        {
                            tmp = false;
                            retVal = true;
                        }
                        else if (fieldsItem())
                        {
                            retVal = false;
                            tmp = false;
                        }
                        else
                        {
                            retVal = true;
                            tmp = false;
                        }

                    }
                }
                iterrationFields++;
            }

            return retVal;
        }

        /// <summary>
        /// Check fieldsItems::= (description|type|minimum|maximum|enum|minLength|maxLength)
        /// </summary>
        /// <returns></returns>
        public bool fieldsItem()
        {
            bool retVal = false;
            int tmpI = tokens[index].GetToken();

            switch (tmpI)
            {
                case Token.properties:
                    getNextToken();
                    retVal = propertiesToken();
                    break;
                case Token.type:
                    getNextToken();
                    retVal = typeToken();
                    break;
                case Token.description:
                    getNextToken();
                    retVal = description();
                    break;
                case Token.maxLength:
                    getNextToken();
                    retVal = maxLength();
                    break;
                case Token.minLength:
                    getNextToken();
                    retVal = minLength();
                    break;
                case Token.minimum:
                    getNextToken();
                    retVal = minimum();
                    break;
                case Token.maximum:
                    getNextToken();
                    retVal = maximum();
                    break;
                case Token.enumToken:
                    getNextToken();
                    retVal = enumToken();
                    break;
                case Token.refToken:
                    getNextToken();
                    retVal = refToken();
                    break;
                default:
                    retVal = false;
                    break;
            }

            return retVal;
        }


        public bool description()
        {

            bool retVal = false;

            if (colon() && String())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check required::= '[' string (',' string)* ']'
        /// </summary>
        /// <returns></returns>
        public bool required()
        {
            int iterationRequired = 0;
            bool retVal = false;
            if (colon() && arrayStart()) //::= string (',' string )*
            {
                bool tmp = true;
                while (tmp)
                {
                    if ((iterationRequired < 1) && String())
                    {
                        tmp = true;
                    }
                    else
                    {
                        if (Comma())
                        {
                            if (String())
                            {
                                retVal = true;
                                tmp = true;
                            }
                            else
                            {
                                retVal = false;
                                tmp = false;
                            }
                        }
                        else
                        {
                            if (String())
                            {
                                retVal = false;
                                tmp = false;
                            }
                            else
                            {
                                if (arrayEnd())
                                {
                                    retVal = true;
                                    tmp = false;
                                }
                                else
                                {
                                    retVal = false;
                                    tmp = false;
                                }

                            }
                        }
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Check minimum::= Number
        /// </summary>
        /// <returns></returns>
        public bool minimum()
        {
            bool retVal = false;
            if (colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check maximum::= Number
        /// </summary>
        /// <returns></returns>
        public bool maximum()
        {
            bool retVal = false;
            if (colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check minLength::= Number
        /// </summary>
        /// <returns></returns>
        public bool minLength()
        {
            bool retVal = false;
            if (colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check maxLength::= Number
        /// </summary>
        /// <returns></returns>
        public bool maxLength()
        {
            bool retVal = false;
            if (colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check enum::= '[' string (',' string)* ']'
        /// </summary>
        /// <returns></returns>
        public bool enumToken()
        {
            int iterationEnum = 0;
            bool retVal = false;
            if (colon() && arrayStart()) //::= string (',' string )*
            {
                bool tmp = true;
                while (tmp)
                {
                    if ((iterationEnum < 1) && String())
                    {
                        tmp = true;
                        retVal = true;
                    }
                    else
                    {
                        if (Comma())
                        {
                            if (String())
                            {
                                retVal = true;
                                tmp = true;
                            }
                            else
                            {
                                retVal = false;
                                tmp = false;
                            }
                        }
                        else
                        {
                            if (arrayEnd())
                            {
                                retVal = true;
                                tmp = false;
                            }
                            else//(String())
                            {
                                retVal = false;
                                tmp = false;
                            }
                        }
                    }
                    iterationEnum++;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Check definitions='{' objectDefinitions (',' objectDefinitions)* '}'
        /// </summary>
        /// <returns></returns>
        public bool definitionsToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorDefinitions));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;

            if (colon() && ObjectStart())
            {
                if (objectProperties()&&objectEnd())
                {
                    retVal = true;
                }
            }

            return retVal;
        }



        /// <summary>
        /// Check $ref
        /// </summary>
        /// <returns></returns>
        public bool refToken()
        {
            // current token is ref element
            bool retVal = false;
            if (colon() && String())
            {
                index--;
                string refString = tokens[index].GetValString();
                if ((refString[0] == '#'))
                {
                    retVal = true;
                }
                else
                {
                    ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorRef));
                }
                index++;
            }

            return retVal;
        }

       
    }
}

