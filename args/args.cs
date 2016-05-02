using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace args
{

    public class Schema : ISchema
    {
        int schemaFieldCount = 4;
        int schemaNamePosition = 0;
        int schemaTypePosition = 1;
        //int schemaValueTypePosition = 2;
        int schemaDefaultValuePosition = 3;

        private string[,] mySchema;
        public string[,] MySchema
        {
            get { return mySchema; }
        }



        public string[,] GetArguments(int argNamePosition, int argTypePosition, int argValuePosition)
        {
            string[,] myArguments = new string[schemaArgumentCount, schemaFieldCount];
            for (int i = 0; i < schemaArgumentCount; i++)
            {
                myArguments[i, argNamePosition] = mySchema[i, schemaNamePosition];
                myArguments[i, argTypePosition] = mySchema[i, schemaTypePosition];
                myArguments[i, argValuePosition] = mySchema[i, schemaDefaultValuePosition];
            }
            return myArguments;
        }

        private int schemaArgumentCount = 0;
        public int SchemaArgumentCount
        {
            get { return schemaArgumentCount; }
        }


        public Schema(string schemaStringToLoad)
        {
            schemaArgumentCount = 0;
            if (schemaStringToLoad.Length == 0)
                return;
            else
            {
                string[] tokenizedSchema = schemaStringToLoad.Split(',');

                int elementCount = tokenizedSchema.Count();
                if (elementCount % schemaFieldCount == 0)
                    schemaArgumentCount = elementCount / schemaFieldCount;
                else
                    throw new System.ArgumentException("Schema requires a name, type and default value for all parameters.");

                mySchema = new string[schemaArgumentCount, schemaFieldCount];
                for (int i = 0; i < schemaArgumentCount; i++)
                {
                    int offset = i * schemaFieldCount;
                    mySchema[i, schemaNamePosition] = tokenizedSchema[offset + schemaNamePosition].Trim();
                    mySchema[i, schemaTypePosition] = tokenizedSchema[offset + schemaTypePosition].Trim();
                    mySchema[i, schemaDefaultValuePosition] = tokenizedSchema[offset + schemaDefaultValuePosition].Trim();
                }
            }
        }

        public string GetArgumentType(string argumentName)
        {
            for (int i = 0; i < schemaArgumentCount; i++)
            {
                if (mySchema[i, schemaNamePosition] == argumentName)
                {
                    return mySchema[i, schemaTypePosition];
                }

            }
            throw new System.ArgumentException("\"" + argumentName + "\" Not Found In Schema");
        }

    }



    public class Parser
    {
        ISchema schema;
        int argNamePosition = 0;
        int argValuePosition = 1;
        int argTypePosition = 2;
        //string argName;
        //string argType;
        //string argValue;



        private string[,] myArguments;
        public string[,] MyProperty
        {
            get { return myArguments; }
        }

        private int argumentCount = 0;
        public int ArgumentCount
        {
            get { return argumentCount; }
        }


        private void setDefaultArgumentValues(ISchema schema)
        {
            myArguments = schema.GetArguments(argNamePosition, argTypePosition, argValuePosition);
        }

        public Parser(ISchema loadThisSchema)
        {
            schema = loadThisSchema;
            setDefaultArgumentValues(schema);
        }

        public Parser(string schemaString)
        {
            schema = new Schema(schemaString);
            setDefaultArgumentValues(schema);
        }


        public void ParseArguments(string arguments)
        {

            string[] tokenizedArguments = arguments.Split(' ');
            int argumentCount = tokenizedArguments.Count();
            for(int i = 0; i < argumentCount; i++)
            {
                string curElement = tokenizedArguments[i];
                if (curElement.Length==2 && curElement.Substring(0,1)=="-")
                {
                    myArguments[0, argValuePosition] = "true";
                }
            }
        }

        public string GetArgumentValue(string argumentName)
        {
            for (int i = 0; i < schema.SchemaArgumentCount; i++)
            {
                if (myArguments[i, argNamePosition] == argumentName)
                {
                    return myArguments[i, argValuePosition];
                }

            }
            throw new System.ArgumentException("\"" + argumentName + "\" Not Found In Schema");
        }

        public string GetArgumentType(string argumentName)
        {
            return schema.GetArgumentType(argumentName);
        }

    }
}
