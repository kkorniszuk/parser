
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
        List<Error> ErrorList = new List<Error>();
        StringBuilder errorListDisplay = new StringBuilder();

        private Token token;
        public Parser(List<Token> tokenList)
        {
            this.tokens = tokenList;
            this.currentToken = 0;
            this.index = 0;
        }



        public string start()
        {
            string retVal = "";



            if (ObjectStart())
            {
                jsonMainSchemaStructures();

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
                if (i < 0)
                {
                    ListError.AppendFormat("Line:{0} Error:{1} \n", error.GetLine(), error.GetDescription());
                }
                else
                {
                    ListError.AppendFormat("  - line:{} error:{1} \n", error.GetLine(), error.GetDescription());
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
                if (iterationJsonMSS > 0)
                {
                    if (Comma())
                    {
                        if (Keywords() == 1)
                        {
                            tmpMain = true;
                        }
                        else if (Keywords() == 2)
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
                        else if (Keywords() == 3)
                        {
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
                    if (Keywords() == 1)
                    {
                        tmpMain = true;
                    }
                    else
                    {
                        errorListDisplay.Append("JSON schema not valid - $schema field is required.");
                    }

                }
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
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
                this.jsonMainSchemaStructures();
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
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
                this.jsonMainSchemaStructures();
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
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
                this.jsonMainSchemaStructures();
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
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
                this.jsonMainSchemaStructures();
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
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
                this.jsonMainSchemaStructures();
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
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
                jsonMainSchemaStructures();

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
                    errorListDisplay.Append(ErrorListToString(ErrorList));
                    ErrorList.Clear();
                }
                jsonMainSchemaStructures();


            }
            else if ((this.tokens[index].GetToken() == Token.END))
            {
                retVal = 2;
            }
            else
            {
                if ((this.tokens[index].GetToken() == Token.objectEnd))
                { retVal = 2; }
                else
                {
                    errorListDisplay.AppendFormat("  - line:Symbol not valid( error:{0})\n", tokens[index].GetLine());
                }
            }

            return retVal;

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
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorString));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found string!"));
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
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorComma));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found comma!"));
            }

            return retVal;
        }



        public bool isUrlString()
        {
            index--;
            bool retVal = Uri.IsWellFormedUriString(tokens[index].GetValString(), UriKind.RelativeOrAbsolute);
            if (!retVal)
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorUrl));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found Url!"));
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
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorObjectSstop));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found '}' "));
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
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorArraytStart));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found '[' "));
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
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorArrayEnd));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found ']' "));
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

        public bool NewLine()
        {
            bool retVal = false;

            if (this.tokens[index].GetToken() == Token.NewLine)
            {
                this.getNextToken();
                retVal = true;
            }

            return retVal;
        }




        public bool skipSpace()
        {
            bool retVal = false;
            bool tmp = true;
            while (tmp)
            {
                tmp = WhiteSpace() || NewLine();
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
                    ErrorList.Add(new Error(tokens[index].GetLine(), Error.WhiteSpace));
                    //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found WhiteSpace!"));
                }
            }
            else
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorColon));
                //throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Not found Colon!"));
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
            if (Colon() && String())
            {
                retVal = true;
            }
            return retVal;
        }

        public bool schemaToken()
        {
            ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorSchema));//Zostanie usunięte jeśli retVal==true
            bool retVal = false;
            if (Colon() && String() && isUrlString())
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
            if (Colon() && String())
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
        public bool StringType(string parametr)
        {

            bool retVal = false;
            string[] tmpArray = { "object", "string", "number", "array", "boolean", "null" };
            for (int k = 0; k < tmpArray.Length; k++)
            {
                if (parametr == tmpArray[k])
                {
                    retVal = true;
                }
                else
                {
                    ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorStringType));
                }
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
            if (Colon())
            {
                bool tmp = true;
                while (tmp)
                {
                    string tmpS = this.tokens[index].GetValString();

                    if ((iterationType < 1) && StringType(tmpS))
                    {
                        tmp = true;
                        retVal = true;
                        skipSpace();
                    }
                    else if (Comma())
                    {
                        skipSpace();
                        retVal = false;
                    }
                    else if (StringType(tmpS))
                    {
                        skipSpace();
                        retVal = true;
                    }
                    else if (String())
                    {
                        tmp = false;
                        retVal = false;
                    }
                    else
                    {
                        tmp = false;
                        retVal = true;
                        ErrorList.Remove(ErrorList.Last());
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

            if (Colon() && ObjectStart())
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
                        //skipSpace();
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
                        iterrationProperties++;
                    }
                }

            }
            return retVal;
        }

        /// <summary>
        /// Check ObjectProperties::= '{' fields (',' fields)* '}'
        /// </summary>
        /// <returns></returns>
        public bool objectProperties()
        {
            bool retVal = false;

            if (String() && Colon() && ObjectStart())
            {
                bool tmp = true;
                int iterrationObjectProperties = 0;

                while (tmp)
                {
                    string tmpS = this.tokens[index].GetValString();

                    if ((iterrationObjectProperties < 1) && fields())
                    {
                        tmp = true;
                        retVal = true;
                        //skipSpace();
                    }
                    else
                    {
                        if (Comma())
                        {
                            if (fields())
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
                            if (fields())
                            {
                                retVal = false;
                                tmp = false;
                            }
                            else
                            {
                                tmp = false;
                                retVal = true;
                            }
                        }
                    }
                    iterrationObjectProperties++;
                }
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
            if (String() && Colon() && String())
            {
                bool tmp = true;
                int iterrationFields = 0;

                while (tmp)
                {
                    string tmpS = this.tokens[index].GetValString();

                    if ((iterrationFields < 1) && fieldsItem())
                    {
                        tmp = true;
                        retVal = true;
                        //skipSpace();
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
                            if (fieldsItem())
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
                //for Definitions
                case Token.properties:
                    propertiesToken();
                    break;
                case Token.type:
                    typeToken();
                    break;
                //for Properties
                case Token.description:
                    description();
                    break;
                case Token.maxLength:
                    maxLength();
                    break;
                case Token.minLength:
                    minLength();
                    break;
                case Token.minimum:
                    minimum();
                    break;
                case Token.maximum:
                    maximum();
                    break;
                case Token.enumToken:
                    enumToken();
                    break;
                case Token.refToken:
                    refToken();
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

            if (Colon() && String() && Comma())
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
            if (Colon() && ArrayStart()) //::= string (',' string )*
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
                                retVal = true;
                                tmp = false;
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
            if (Colon() && Number())
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
            if (Colon() && Number())
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
            if (Colon() && Number())
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
        public bool enumToken()
        {

            int iterationEnum = 0;
            bool retVal = false;
            if (Colon() && ArrayStart()) //::= string (',' string )*
            {
                bool tmp = true;
                while (tmp)
                {
                    if ((iterationEnum < 1) && String())
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
                                retVal = true;
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
            bool retVal = false;

            retVal = objectProperties();
            //if (Colon() && ObjectStart(false))
            //{
            //    bool tmp = true;
            //    int iterrationDefinitions = 0;

            //    while (tmp)
            //    {
            //        string tmpS = this.tokens[index].GetValString();

            //        if ((iterrationDefinitions < 1) && ObjectDefinitions())
            //        {
            //            tmp = true;
            //            retVal = true;
            //            //skipSpace();
            //        }
            //        else
            //        {
            //            if (Comma())
            //            {
            //                if (ObjectDefinitions())
            //                {
            //                    retVal = true;
            //                    tmp = true;
            //                }
            //                else
            //                {
            //                    tmp = false;
            //                    retVal = false;
            //                }
            //            }
            //            else
            //            {
            //                if (ObjectDefinitions())
            //                {
            //                    retVal = false;
            //                    tmp = false;
            //                }
            //                else
            //                {
            //                    if (ObjectEnd())
            //                    {
            //                        retVal = true;
            //                        tmp = false;
            //                    }
            //                    else
            //                    {
            //                        retVal = false;
            //                        tmp = false;
            //                    }
            //                }

            //            }
            //            iterrationDefinitions++;
            //        }
            //    }
            //}
            return retVal;
        }

        /// <summary>
        /// ObjectDefinitions::= '{' fields, (',' fields)* '}'
        /// </summary>
        /// <returns></returns>
        public bool ObjectDefinitions()
        {
            bool retVal = false;



            return retVal;
        }

        //public bool definitionToken()
        //{
        //    bool retVal = false;
        //    if (checkNextTokenValue().GetToken() == Token.objectEnd)
        //    {
        //        // it should return true - next token will be end of object
        //        retVal = true;
        //    }
        //    else if (checkNextTokenValue().GetToken() == Token.stringToken)
        //    {
        //        if (String())
        //        {
        //            CheckStartOfObject();
        //            getNextToken();
        //            if (tokens[index].GetToken() == Token.type)
        //            {
        //                typeToken();
        //            }
        //            else if (tokens[index].GetToken() == Token.properties)
        //            {
        //                propertiesToken();
        //            }
        //            else
        //            {
        //                throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected keywords: type or properties"));
        //            }
        //            ObjectEnd();
        //        }
        //        else
        //        {
        //            throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected string token"));
        //        }
        //        definitionToken();
        //    }
        //    else
        //    {
        //        throw new JSONException(ErrorMessage.errorMsg(tokens[index].GetLine(), "Expected property name or end of object. "));
        //    }

        //    return retVal;
        //}

        /// <summary>
        /// Check $ref
        /// </summary>
        /// <returns></returns>
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
                    ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorRef));
                }
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
            catch (System.Exception ex)
            {
                //throw SystemException;
                return null;
            }
        }

        public bool CheckStartOfObject()
        {
            bool retVal = false;

            if (Colon())
            {
                if (ObjectStart())
                {
                    retVal = true;
                }
                else
                {
                    ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorObjectStart));
                }
            }
            else
            {
                ErrorList.Add(new Error(tokens[index].GetLine(), Error.ErrorObjectSstop));
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

