using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using args;

namespace args  
{
    public static class Constants
    {
        public static string FullSchema
        {
            get { return "l,bool,false,p,int,8080,d,string,'/usr/logs'"; }
        }

        public static string FullArguments
        {
            // -l = logging <bool>
            // -p = port <int>
            // -d = directory <string>
            get { return "-l -p 8080 -d /usr/logs"; }
        }
    }

    [TestClass]
    public class SchemaCreationTests
    {
        string schema_finalexam = Constants.FullSchema;

        [TestMethod]
        public void ZeroArgOnSchemaShouldFind_Zero_SchemaArgs()
        {
            Args o = new Args("");
            Assert.AreEqual(0, o.SchemaArgumentCount);
        }

        [TestMethod]
        public void OneArgOnSchemaShouldFind_One_SchemaArgs()
        {
            Args o = new Args("argument, type, value");
            Assert.AreEqual(1, o.SchemaArgumentCount);
        }

        [TestMethod]
        public void ThreeArgOnSchemaShouldFind_Three_SchemaArgs()
        {
            Args o = new Args(schema_finalexam);
            Assert.AreEqual(3, o.SchemaArgumentCount);
        }

        [TestMethod]
        public void ShouldBeAbleToGetArgumentName()
        {
            Args o = new Args("argument, type, defaultValue");
            string varName = o.MySchema[0, 0];
            Assert.AreEqual("argument", varName);
        }

        [TestMethod]
        public void ShouldBeAbleToGetArgumentType()
        {
            Args o = new Args("argument, type, defaultValue");
            string argumentType = o.MySchema[0, 1];
            Assert.AreEqual("type", argumentType);
        }

        [TestMethod]
        public void ShouldBeAbleToGetArgumentDefaultValue()
        {
            Args o = new Args("argument, type, defaultValue");
            string argumentDefaultValue = o.MySchema[0, 2];
            Assert.AreEqual("defaultValue", argumentDefaultValue);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ShouldThrowExceptionIfNotEnoughFieldsInSchema()
        {
            try
            {
                Args o = new Args("argument1, type1, defaultValue1, oops");
                Assert.Fail("Expected exception was not thrown");
            }
            catch(ArgumentException ex)
            {
                string expectedExceptionString
                    = "Schema requires a name, type and default value for all parameters.";
                Assert.AreEqual(expectedExceptionString, ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void ShouldBeAbleToCheckAllNineElements()
        {
            Args o = new Args(schema_finalexam);
            string thirdDefaultValue = o.MySchema[2, 2];
            Assert.AreEqual("'/usr/logs'", thirdDefaultValue);
        }

    }

    [TestClass]
    public class ArgumentParserTests
    {
        Args o;
        string arguments;

        [TestInitialize]
        public void Setup()
        {
            arguments = "";
            string schema = Constants.FullSchema;
            o = new Args(schema);
        }

        [TestMethod]
        public void InitialStateShouldBeZeroArguments()
        {
            o = new Args("");
            o.ParseArguments(arguments);
            Assert.AreEqual(0, o.ArgumentCount);
        }

        [TestMethod]
        public void ShouldSetLoggingToTrue()
        {
            o.ParseArguments("-l");
            string loggingValue = o.GetArgumentValue("l");
            Assert.AreEqual("true", loggingValue);
            //string loggingType = o.GetArgumentType("l");
            //Assert.AreEqual("bool", loggingValue);
        }

        [TestMethod]
        public void LoggingShouldDefaultToFalse()
        {
            o.ParseArguments("");
            string loggingValue = o.GetArgumentValue("l");
            Assert.AreEqual("false", loggingValue);
            //string loggingType = o.GetArgumentType("l");
            //Assert.AreEqual("bool", loggingValue);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ShouldThrowExceptionIfArgumentNotInSchema()
        {
            try
            {
                o.GetArgumentValue("notAValidName");
                Assert.Fail("Expected exception was not thrown");
            }
            catch (ArgumentException ex)
            {
                string expectedExceptionString
                    = "\"notAValidName\"" + " Not Found In Schema";
                Assert.AreEqual(expectedExceptionString, ex.Message);
                throw;
            }
        }
    }
}
