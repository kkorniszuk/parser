
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



        public string Start()
        {
            errorListDisplay.AppendFormat("+Praser: \n", errorCounter.ToString());
            string retVal = "";

            if (ObjectStart())
            {
                JsonMainSchemaStructures();

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
        /// Check JsonMainSchemaStructures::= '{' keywords (',' keywords)*'}'
        /// </summary>
        public bool JsonMainSchemaStructures()
        {
            bool retVal = false;
            int iterationJsonMSS = 0;

            bool tmpMain = true;
            while (tmpMain)
            {
                int keywords = 0;
                if (iterationJsonMSS > 0)
                {
                    if (Coma())
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
                                errorListDisplay.Append("JSON schema not valid - $schema field is Required.");
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
                                errorListDisplay.Append("JSON schema not valid - $schema field is Required.");
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
                                errorListDisplay.Append("JSON schema not valid - $schema field is Required.");
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
                                errorListDisplay.Append("JSON schema not valid - $schema field is Required.");
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
                        errorListDisplay.Append("JSON schema not valid - $schema field is Required.");
                    }

                }
                iterationJsonMSS++;
            }
            return retVal;

        }

        /// <summary>
        /// index++
        /// </summary>
        public void GetNextToken()
        {
            if (index < tokens.Count)
            {
                index++;
            }
        }

        /// <summary>
        /// loop checking keywords
        /// </summary>
        /// <returns>return 1 if decect keywords, return 2 if detect EOF, return 3 if detect '}'</returns>
        public int Keywords()
        {
            int retVal = 1;//1-continue, 2-EOF, 3 ObjectEnd
            bool tmp=true;
            while (tmp)
            {
                tmp = false;
                if (tokens[index].GetToken() == Token.Id)
                {
                    GetNextToken();
                    if (IdToken())
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
                else if (tokens[index].GetToken() == Token.Schema)
                {
                    GetNextToken();
                    if (SchemaToken())
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
                else if (tokens[index].GetToken() == Token.Title)
                {
                    GetNextToken();
                    if (TitleToken())
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
                else if (tokens[index].GetToken() == Token.Type)
                {
                    GetNextToken();
                    if (TypeToken())
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
                else if (tokens[index].GetToken() == Token.Properties)
                {
                    GetNextToken();
                    if (PropertiesToken())
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
                else if (tokens[index].GetToken() == Token.Required)
                {
                    GetNextToken();
                    if (Required())
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
                else if (tokens[index].GetToken() == Token.Definitions)
                {
                    GetNextToken();
                    if (DefinitionsToken())
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
                else if ((tokens[index].GetToken() == Token.END))
                {
                    retVal = 2;
                }
                else if ((tokens[index].GetToken() == Token.ObjectEnd))
                {
                    retVal = 3;
                }
                else
                {
                    //errorListDisplay.AppendFormat("  - line{0}:Symbol not valid( error:{1})\n", tokens[index].GetLine(), Error.ErrorObjectSstop);
                    tmp = true;
                    index++;
                }

            }
                return retVal;

        }

        /// <summary>
        /// Check string
        /// </summary>
        /// <returns></returns>
        public bool StringToken()
        {
            bool retVal = false;


            if (tokens[index].GetToken() == Token.StringToken)
            {
                GetNextToken();
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
        public bool Coma()
        {
            bool retVal = false;


            if (tokens[index].GetToken() == Token.Coma)
            {
                GetNextToken();
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
        public bool IsUrlString()
        {
            index--;
            bool retVal = Uri.IsWellFormedUriString(tokens[index].GetValString(), UriKind.RelativeOrAbsolute);
            if (!retVal)
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorUrl));
            }
            else
            {
                GetNextToken();
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
            int a = tokens[index].GetToken();
            if (tokens[index].GetToken() == Token.ObjectStart)
            {
                retVal = true;
                GetNextToken();
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
        public bool ObjectEnd()
        {
            bool retVal = false;

            if (tokens[index].GetToken() == Token.ObjectEnd)
            {
                GetNextToken();
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
        public bool ArrayStart()
        {
            bool retVal = false;

            if (tokens[index].GetToken() == Token.ArrayStart)
            {
                retVal = true;
                GetNextToken();

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
        public bool ArrayEnd()
        {
            bool retVal = false;



            if (tokens[index].GetToken() == Token.ArrayEnd)
            {
                retVal = true;
                GetNextToken();
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
        public bool WhiteSpace()
        {
            bool retVal = false;

            if (tokens[index].GetToken() == Token.WhiteSpace)
            {
                GetNextToken();
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


            if (tokens[index].GetToken() == Token.Number)
            {
                GetNextToken();
                retVal = true;
            }

            return retVal;
        }

        /// <summary>
        /// Chheck ': '
        /// </summary>
        /// <returns></returns>
        public bool Colon()
        {
            bool retVal = false;


            if (tokens[index].GetToken() == Token.Colon)
            {
                GetNextToken();
                if (tokens[index].GetToken() == Token.WhiteSpace)
                {
                    GetNextToken();
                    bool Tmp = true;
                    while (Tmp)
                    {
                        Tmp = WhiteSpace();
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
        public bool IdToken() // : string
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorID));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (Colon() && StringToken())
            {
                retVal = true;
            }
            return retVal;
        }

        public bool SchemaToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorSchema));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (Colon() && StringToken() && IsUrlString())
            {

                hasJsonSchema = true;
                retVal = true;

            }
            return retVal;
        }

        /// <summary>
        /// Check title::= string 
        /// </summary>
        /// <returns></returns>
        public bool TitleToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorTitle));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (Colon() && StringToken())
            {
                retVal = true;
            }
            return retVal;
        }


        /// <summary>
        ///  Compare StringToken with Item Array StringType
        ///  "object" || "string" || "number" || "array" || "boolean" ||"null"
        /// </summary>
        /// <param name="parametr"></param>
        /// <returns></returns>
        public bool StringType(string parametr)
        {

            bool retVal = false;
            string[] tmpArray = { "object", "string", "number", "array", "boolean", "null", "integer" };
            for (int k = 0; k < tmpArray.Length; k++)
            {
                if (parametr == tmpArray[k])
                {
                    GetNextToken();
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
        public bool TypeToken() //  ::= (StringType ',')*
        {
            int iterationType = 0;
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorType));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (Colon())
            {
                bool tmp = true;
                while (tmp)
                {
                    string tmpS = tokens[index].GetValString();

                    if ((iterationType < 1) && StringType(tmpS))
                    {
                        tmp = true;
                        retVal = true;
                    }
                    else if (Coma())
                    {
                        tmpS = tokens[index].GetValString();
                        if (StringType(tmpS))
                        {
                            retVal = true;
                            tmp = true;
                        }
                        else
                        {
                            if (StringToken())
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
        public bool PropertiesToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorProperties));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;


            if (Colon() && ObjectStart())
            {
                bool tmp = true;
                int iterrationProperties = 0;

                while (tmp)
                {
                    string tmpS = tokens[index].GetValString();

                    if ((iterrationProperties < 1) && ObjectProperties())
                    {
                        tmp = true;
                        retVal = true;
                    }
                    else
                    {
                        if (Coma())
                        {
                            if (ObjectProperties())
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
                            if (ObjectProperties())
                            {
                                retVal = false;
                                tmp = false;
                            }
                            else
                            {
                                if (ObjectEnd())
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
        public bool ObjectProperties()
        {
            bool retVal = false;

            if (StringToken() && Colon() && ObjectStart())
            {

                if (Fields())
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
        public bool Fields()
        {
            bool retVal = false;

            bool tmp = true;
            int iterrationFields = 0;

            while (tmp)
            {
                string tmpS = tokens[index].GetValString();

                if ((iterrationFields < 1) && FieldsItem())
                {
                    tmp = true;
                    retVal = true;
                }
                else
                {
                    if (Coma())
                    {
                        if (FieldsItem())
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
                        if (ObjectEnd())
                        {
                            tmp = false;
                            retVal = true;
                        }
                        else if (FieldsItem())
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
        /// Check fieldsItems::= (Description|type|Minimum|Maximum|enum|MinLength|MaxLength)
        /// </summary>
        /// <returns></returns>
        public bool FieldsItem()
        {
            bool retVal = false;
            int tmpI = tokens[index].GetToken();

            switch (tmpI)
            {
                case Token.Properties:
                    GetNextToken();
                    retVal = PropertiesToken();
                    break;
                case Token.Type:
                    GetNextToken();
                    retVal = TypeToken();
                    break;
                case Token.Description:
                    GetNextToken();
                    retVal = Description();
                    break;
                case Token.MaxLength:
                    GetNextToken();
                    retVal = MaxLength();
                    break;
                case Token.MinLength:
                    GetNextToken();
                    retVal = MinLength();
                    break;
                case Token.Minimum:
                    GetNextToken();
                    retVal = Minimum();
                    break;
                case Token.Maximum:
                    GetNextToken();
                    retVal = Maximum();
                    break;
                case Token.EnumToken:
                    GetNextToken();
                    retVal = EnumToken();
                    break;
                case Token.RefToken:
                    GetNextToken();
                    retVal = RefToken();
                    break;
                default:
                    retVal = false;
                    break;
            }

            return retVal;
        }


        public bool Description()
        {

            bool retVal = false;

            if (Colon() && StringToken())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check Required::= '[' string (',' string)* ']'
        /// </summary>
        /// <returns></returns>
        public bool Required()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorRequired));
            int iterationRequired = 0;
            bool retVal = false;
            if (Colon() && ArrayStart()) //::= string (',' string )*
            {
                bool tmp = true;
                while (tmp)
                {
                    if ((iterationRequired < 1) && StringToken())
                    {
                        tmp = true;
                    }
                    else
                    {
                        if (Coma())
                        {
                            if (StringToken())
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
                            if (StringToken())
                            {
                                retVal = false;
                                tmp = false;
                            }
                            else
                            {
                                if (ArrayEnd())
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
        /// Check Minimum::= Number
        /// </summary>
        /// <returns></returns>
        public bool Minimum()
        {
            bool retVal = false;
            if (Colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check Maximum::= Number
        /// </summary>
        /// <returns></returns>
        public bool Maximum()
        {
            bool retVal = false;
            if (Colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check MinLength::= Number
        /// </summary>
        /// <returns></returns>
        public bool MinLength()
        {
            bool retVal = false;
            if (Colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check MaxLength::= Number
        /// </summary>
        /// <returns></returns>
        public bool MaxLength()
        {
            bool retVal = false;
            if (Colon() && Number())
            {
                retVal = true;
            }
            return retVal;
        }

        /// <summary>
        /// Check enum::= '[' string (',' string)* ']'
        /// </summary>
        /// <returns></returns>
        public bool EnumToken()
        {
            int iterationEnum = 0;
            bool retVal = false;
            if (Colon() && ArrayStart()) //::= string (',' string )*
            {
                bool tmp = true;
                while (tmp)
                {
                    if ((iterationEnum < 1) && StringToken())
                    {
                        tmp = true;
                        retVal = true;
                    }
                    else
                    {
                        if (Coma())
                        {
                            if (StringToken())
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
                            if (ArrayEnd())
                            {
                                retVal = true;
                                tmp = false;
                            }
                            else//(StringToken())
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
        public bool DefinitionsToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorDefinitions));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;

            if (Colon() && ObjectStart())
            {
                if (ObjectProperties()&&ObjectEnd())
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
        public bool RefToken()
        {
            // current token is ref element
            bool retVal = false;
            if (Colon() && StringToken())
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

