using IIG.CoSFE.DatabaseUtils;
using IIG.BinaryFlag;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integration.Test
{
    [TestClass]
    public class FlagpoleTest
    {
        private const string Server = @"DESKTOP-RPD8Q03\SQLEXPRESS";
        private const string Database = @"IIG.CoSWE.FlagpoleDB";
        private const bool IsTrusted = false;
        private const string Login = "sa";
        private const string Password = @"L}EjpfCgru9X@GLj";
        private const int ConnectionTimeout = 75;

        private static FlagpoleDatabaseUtils DbUtils;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            DbUtils = new FlagpoleDatabaseUtils(
                Server, Database, IsTrusted, Login, Password, ConnectionTimeout);
        }

        [TestMethod]
        public void Test_AddFlag_GetFlag_TrueInit_True()
        {
            var flag = new MultipleBinaryFlag(50);
            var flagView = flag.ToString();
            var flagValue = (bool)flag.GetFlag();

            Assert.IsTrue(DbUtils.AddFlag(flagView, flagValue), 
                "AddFlag True Binary Flag Wrong Result!");

            var flagId = (int)DbUtils.GetIntBySql(
                "SELECT TOP(1) MultipleBinaryFlagID " +
                "FROM [dbo].[MultipleBinaryFlags] " +
                "WHERE MultipleBinaryFlagView = " +
                $"'{flagView}'");

            Assert.IsTrue(DbUtils.GetFlag(flagId, out string outFlagView, out bool? outFlagValue),
                "GetFlag True Binary Flag Wrong Result!");
            Assert.AreEqual(flagView, outFlagView,
                "True Flag Input and Output FlagViews Are Not Equal!");
            Assert.AreEqual(flagValue, outFlagValue,
                "True Flag Input and Output FlagValues Are Not Equal!");
        }

        [TestMethod]
        public void Test_AddFlag_GetFlag_FalseInit_True()
        {
            var flag = new MultipleBinaryFlag(50, false);
            var flagView = flag.ToString();
            var flagValue = (bool)flag.GetFlag();

            Assert.IsTrue(DbUtils.AddFlag(flagView, flagValue),
                "AddFlag False Binary Flag Wrong Result!");

            var flagId = (int)DbUtils.GetIntBySql(
                "SELECT TOP(1) MultipleBinaryFlagID " +
                "FROM [dbo].[MultipleBinaryFlags] " +
                "WHERE MultipleBinaryFlagView = " +
                $"'{flagView}'");

            Assert.IsTrue(DbUtils.GetFlag(flagId, out string outFlagView, out bool? outFlagValue),
                "GetFlag False Binary Flag Wrong Result!");
            Assert.AreEqual(flagView, outFlagView,
                "False Flag Input and Output FlagViews Are Not Equal!");
            Assert.AreEqual(flagValue, outFlagValue,
                "False Flag Input and Output FlagValues Are Not Equal!");
        }

        [TestMethod]
        public void Test_AddFlag_GetFlag_TrueInit_Changed_True()
        {
            var flag = new MultipleBinaryFlag(50);
            flag.ResetFlag(10);
            var flagView = flag.ToString();
            var flagValue = (bool)flag.GetFlag();

            Assert.IsTrue(DbUtils.AddFlag(flagView, flagValue),
                "AddFlag Changed Binary Flag Wrong Result!");

            var flagId = (int)DbUtils.GetIntBySql(
                "SELECT TOP(1) MultipleBinaryFlagID " +
                "FROM [dbo].[MultipleBinaryFlags] " +
                "WHERE MultipleBinaryFlagView = " +
                $"'{flagView}'");

            Assert.IsTrue(DbUtils.GetFlag(flagId, out string outFlagView, out bool? outFlagValue),
                "GetFlag Changed Binary Flag Wrong Result!");
            Assert.AreEqual(flagView, outFlagView,
                "Changed Flag Input and Output FlagViews Are Not Equal!");
            Assert.AreEqual(flagValue, outFlagValue,
                "Changed Flag Input and Output FlagValues Are Not Equal!");
        }

        [TestMethod]
        public void Test_AddFlag_LongFlagView_True()
        {
            var flag = new MultipleBinaryFlag(300_000_000);
            var flagView = flag.ToString();
            var flagValue = (bool)flag.GetFlag();

            Assert.IsTrue(DbUtils.AddFlag(flagView, flagValue),
                "AddFlag True Binary Flag with Big Length Wrong Result!");

            var flagId = (int)DbUtils.GetIntBySql(
                "SELECT TOP(1) MultipleBinaryFlagID " +
                "FROM [dbo].[MultipleBinaryFlags] " +
                "WHERE MultipleBinaryFlagView = " +
                $"'{flagView}'");

            Assert.IsTrue(DbUtils.GetFlag(flagId, out string outFlagView, out bool? outFlagValue),
                "GetFlag True Binary Flag with Big Length Wrong Result!");
            Assert.AreEqual(flagView, outFlagView,
                "True Flag with Big Length Input and Output FlagViews Are Not Equal!");
            Assert.AreEqual(flagValue, outFlagValue,
                "True Flag with Big Length Input and Output FlagValues Are Not Equal!");
        }
    }
}
