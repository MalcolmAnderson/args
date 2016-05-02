using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using args;

namespace args  
{
    public static class Constants
    {

        #region SchemaSampleValues
        public static string SampleValue_Argument
        {
            get { return "argument"; }
        }
        public static string SampleValue_Type
        {
            get { return "type"; }
        }
        public static string SampleValue_ValueType
        {
            get { return "valueType"; }
        }
        public static string SampleValue_DefaultValue
        {
            get { return "defaultValue"; }
        }
        public static string SampleArguments
        {
            get
            {
                string value = Constants.SampleValue_Argument
                    + ", " + Constants.SampleValue_Type
                    + ", " + Constants.SampleValue_ValueType
                    + ", " + Constants.SampleValue_DefaultValue;
                return value;
            }
        }
        #endregion SchemaSampleValues

        public static string InitialSchema
        {
            get { return "l, bool, single, false, p, int, single, 8080, d, string, single, /usr/logs"; }
        }
        public static string InitialArguments
        {
            // -l = logging <bool>
            // -p = port <int>
            // -d = directory <string>
            get { return "-l -p 8080 -d /usr/logs"; }
        }

        public static string GroupSchema
        {
            get { return "g, string, group, empty, d, int, group, empty"; }
        }
        public static string GroupArguments
        {
            get { return "-g this, is, a list, -d 1, 2, -3, 5"; }
        }
    }


    [TestClass]
    public class SchemaGroupCreationTests
    {
        [TestMethod]
        public void ShouldDetectTypeGroup()
        {
            Args o = new Args(Constants.GroupSchema);
            Assert.AreEqual(2, o.SchemaArgumentCount);
            //Assert.AreEqual(2, o.SchemaGroupArgumentCount);
        }
    }

    [TestClass]
    public class SchemaInitialCreationTests
    {
        [TestMethod]
        public void ZeroArgOnSchemaShouldFind_Zero_SchemaArgs()
        {
            Args o = new Args("");
            Assert.AreEqual(0, o.SchemaArgumentCount);
        }

        [TestMethod]
        public void OneArgOnSchemaShouldFind_One_SchemaArgs()
        {
            Args o = new Args(Constants.SampleArguments);
            Assert.AreEqual(1, o.SchemaArgumentCount);
        }

        [TestMethod]
        public void ThreeArgOnSchemaShouldFind_Three_SchemaArgs()
        {
            Args o = new Args(Constants.InitialSchema);
            Assert.AreEqual(3, o.SchemaArgumentCount);
        }

        [TestMethod]
        public void ShouldBeAbleToGetArgumentName()
        {
            Args o = new Args(Constants.SampleArguments);
            string varName = o.MySchema[0, 0];
            Assert.AreEqual(Constants.SampleValue_Argument, varName);
        }

        [TestMethod]
        public void ShouldBeAbleToGetArgumentType()
        {
            Args o = new Args(Constants.SampleArguments);
            string argumentType = o.MySchema[0, 1];
            Assert.AreEqual(Constants.SampleValue_Type, argumentType);
        }

        [TestMethod]
        public void ShouldBeAbleToGetArgumentDefaultValue()
        {
            Args o = new Args(Constants.SampleArguments);
            string argumentDefaultValue = o.MySchema[0, 3];
            Assert.AreEqual(Constants.SampleValue_DefaultValue, argumentDefaultValue);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ShouldThrowExceptionIfNotEnoughFieldsInSchema()
        {
            try
            {
                Args o = new Args("argument1, type1, notEnoughArguments");
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
            Args o = new Args(Constants.InitialSchema);
            string thirdDefaultValue = o.MySchema[2, 3];
            Assert.AreEqual("/usr/logs", thirdDefaultValue);
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
            string schema = Constants.InitialSchema;
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
        }

        [TestMethod]
        public void LoggingShouldDefaultToFalse()
        {
            o.ParseArguments("");
            string loggingValue = o.GetArgumentValue("l");
            Assert.AreEqual("false", loggingValue);
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

        [TestMethod]
        public void ShouldBeAbleToGetLoggingType()
        {
            o.ParseArguments("-l");
            string loggingType = o.GetArgumentType("l");
            Assert.AreEqual("bool", loggingType);
        }

        [TestMethod]
        public void InitialTestFullArgumentList()
        {
            o.ParseArguments(Constants.InitialArguments);
            Assert.AreEqual("true", o.GetArgumentValue("l"));
            Assert.AreEqual("bool", o.GetArgumentType("l"));
            Assert.AreEqual("8080", o.GetArgumentValue("p"));
            Assert.AreEqual("int", o.GetArgumentType("p"));
            Assert.AreEqual("/usr/logs", o.GetArgumentValue("d"));
            Assert.AreEqual("string", o.GetArgumentType("d"));
        }
    }
}
