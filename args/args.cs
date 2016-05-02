﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace args
{
    public class Args
    {
        //int schemaFieldCount = 3;
        int schemaNamePosition = 0;
        int schemaTypePosition = 1;
        int schemaDefaultValuePosition = 2;
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


        private string[,] mySchema;
        public string[,] MySchema
        {
            get { return mySchema; }
        }


        private int schemaArgumentCount = 0;
        public int SchemaArgumentCount
        {
            get { return schemaArgumentCount; }
        }


        public Args(string schema)
        {
            schemaArgumentCount = 0;
            if (schema.Length == 0)
                return;
            else
            {
                string[] tokenizedSchema = schema.Split(',');

                int elementCount = tokenizedSchema.Count();
                if (elementCount % 3 == 0)
                    schemaArgumentCount = elementCount / 3;
                else
                    throw new System.ArgumentException("Schema requires a name, type and default value for all parameters.");

                mySchema = new string[schemaArgumentCount, 3];
                for (int i = 0; i < schemaArgumentCount; i++)
                {
                    int offset = i * 3;
                    mySchema[i, schemaNamePosition] = tokenizedSchema[offset + schemaNamePosition].Trim();
                    mySchema[i, schemaTypePosition] = tokenizedSchema[offset + schemaTypePosition].Trim();
                    mySchema[i, schemaDefaultValuePosition] = tokenizedSchema[offset + schemaDefaultValuePosition].Trim();
                }
            }
        }

        public void ParseArguments(string arguments)
        {
            myArguments = new string[schemaArgumentCount, 3];
            for(int i=0; i<schemaArgumentCount; i++)
            {
                myArguments[i, argNamePosition] = mySchema[i, schemaNamePosition];
                myArguments[i, argTypePosition] = mySchema[i, schemaTypePosition];
                myArguments[i, argValuePosition] = mySchema[i, schemaDefaultValuePosition];
            }
           

            string[] tokenizedArguments = arguments.Split(' ');
            int argumentCount = tokenizedArguments.Count();
            for(int i = 0; i < argumentCount; i++)
            {
                string curElement = tokenizedArguments[i];
                if (curElement.Length==2 && curElement.Substring(0,1)=="-")
                {
                    //string argName = curElement.Substring(1, 1);
                    myArguments[0, argValuePosition] = "true";

                    //find argName in Schema
                    //set type to type in schema
                    // if boolean set value to true
                }
                    
            }
        }

        public string GetArgumentValue(string argumentName)
        {
            for (int i = 0; i < schemaArgumentCount; i++)
            {
                if (mySchema[i, schemaNamePosition] == argumentName)
                {
                    return myArguments[i, argValuePosition];
                    //return mySchema[i, GetArgumentValue];
                }

            }
            throw new System.ArgumentException("\"" + argumentName + "\" Not Found In Schema");
        }

        public string GetArgumentType(string argumentName)
        {
            for (int i = 0; i < schemaArgumentCount; i++)
            {
                if (mySchema[i, schemaNamePosition] == argumentName)
                {
                    return myArguments[i, argTypePosition];
                    //return mySchema[i, GetArgumentValue];
                }

            }
            throw new System.ArgumentException("\"" + argumentName + "\" Not Found In Schema");
        }
    }
}