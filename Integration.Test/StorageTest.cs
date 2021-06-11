using System;
using System.Text;
using System.IO;
using IIG.CoSFE.DatabaseUtils;
using IIG.FileWorker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Integration.Test
{
    [TestClass]
    public class StorageTest
    {
        private const string Server = @"DESKTOP-RPD8Q03\SQLEXPRESS";
        private const string Database = @"IIG.CoSWE.StorageDB";
        private const bool IsTrusted = false;
        private const string Login = "sa";
        private const string Password = @"L}EjpfCgru9X@GLj";
        private const int ConnectionTimeout = 75;

        private const string RootDirectory = @"\IntegrationTest_TempDir";
        private static StorageDatabaseUtils DbUtils;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            DbUtils = new StorageDatabaseUtils(
                Server, Database, IsTrusted, Login, Password, ConnectionTimeout);
            BaseFileWorker.MkDir(RootDirectory);
        }

        [TestMethod]
        public void Test_AddFile_GetFile_True()
        {
            var path = RootDirectory + @"\TestAddFile";
            BaseFileWorker.Write("TestAddFile_Content", path);
            var fileName = BaseFileWorker.GetFileName(path);
            var fileContent = Encoding.Unicode.GetBytes(BaseFileWorker.ReadAll(path));

            Assert.IsTrue(DbUtils.AddFile(fileName, fileContent), 
                "AddFile Wrong Result!");

            var flagId = (int)DbUtils.GetIntBySql(
                "SELECT TOP(1) FileID " +
                "FROM [dbo].[Files] " +
                "WHERE FileName = " +
                $"'{fileName}'");

            Assert.IsTrue(DbUtils.GetFile(flagId, out string outFileName, out byte[] outFileContent), 
                "GetFile Wrong Result!");
            Assert.AreEqual(fileName, outFileName,
                "Input and Output FileNames Are Not Equal!");
            CollectionAssert.AreEqual(fileContent, outFileContent,
                "Input and Output FileContents Are Not Equal!");
        }

        [TestMethod]
        public void Test_AddFile_GetFile_LongFileName_True()
        {
            var sb = new StringBuilder();
            var path = RootDirectory + @"\" + sb.Append('x', 255).ToString();
            BaseFileWorker.Write("TestLongFileName_Content", path);
            var fileName = BaseFileWorker.GetFileName(path);
            var fileContent = Encoding.Unicode.GetBytes(BaseFileWorker.ReadAll(path));
            var fileContentString = "0x" + $"{BitConverter.ToString(fileContent).Replace("-", "")}";

            Assert.IsTrue(DbUtils.AddFile(fileName, fileContent), 
                "AddFile with Long FileName Wrong Result!");

            var flagId = (int)DbUtils.GetIntBySql(
                "SELECT TOP(1) FileID " +
                "FROM [dbo].[Files] " +
                "WHERE FileContent = " +
                $"{fileContentString}");

            Assert.IsTrue(DbUtils.GetFile(flagId, out string outFileName, out byte[] outFileContent),
                "GetFile with Long FileName Wrong Result!");
            Assert.AreEqual(fileName, outFileName,
                "Input and Output Long FileNames Are Not Equal!");
            CollectionAssert.AreEqual(fileContent, outFileContent,
                "Input and Output FileContents of File with Long FileName Are Not Equal!");
        }

        [TestMethod]
        public void Test_AddFile_GetFile_LongFileContent_True()
        {
            var sb = new StringBuilder();
            var path = RootDirectory + @"\TestLongFileContent";
            BaseFileWorker.Write(sb.Append('x', 513).ToString(), path);
            var fileName = BaseFileWorker.GetFileName(path);
            var fileContent = Encoding.Unicode.GetBytes(BaseFileWorker.ReadAll(path));

            Assert.IsTrue(DbUtils.AddFile(fileName, fileContent), 
                "AddFile with Long FileContent Wrong Result!");

            var flagId = (int)DbUtils.GetIntBySql(
                "SELECT TOP(1) FileID " +
                "FROM [dbo].[Files] " +
                "WHERE FileName = " +
                $"'{fileName}'");

            Assert.IsTrue(DbUtils.GetFile(flagId, out string outFileName, out byte[] outFileContent),
                "GetFile with Long FileContent Wrong Result!");
            Assert.AreEqual(fileName, outFileName,
                "Input and Output FileNames of File with Long FileContent Are Not Equal!");
            CollectionAssert.AreEqual(fileContent, outFileContent,
                "Input and Output Long FileContents Are Not Equal!");
        }

        [TestMethod]
        public void Test_AddFile_DeleteFile_True()
        {
            var path = RootDirectory + @"\TestDeleteFile";
            BaseFileWorker.Write("TestDeleteFil_Content", path);
            var fileName = BaseFileWorker.GetFileName(path);
            var fileContent = Encoding.Unicode.GetBytes(BaseFileWorker.ReadAll(path));
            
            DbUtils.AddFile(fileName, fileContent);
            var fileId = (int)DbUtils.GetIntBySql(
                "SELECT TOP(1) FileID " +
                "FROM [dbo].[Files] " +
                "WHERE FileName = " +
                $"'{fileName}'");

            Assert.IsTrue(DbUtils.DeleteFile(fileId), 
                "DeleteFile Wrong Result!");
            Assert.IsFalse(DbUtils.GetFile(fileId, out _, out _), 
                "File Was Not Deleted from Database!");
        }

        [TestMethod]
        public void Test_AddFile_GetFiles_OneFile_Valid()
        {
            var path = RootDirectory + @"\TestGetFiles_OneFile";
            BaseFileWorker.Write("TestGetFiles_Content", path);
            var fileName = BaseFileWorker.GetFileName(path);
            var fileContent = Encoding.Unicode.GetBytes(BaseFileWorker.ReadAll(path));

            DbUtils.AddFile(fileName, fileContent);

            var dt = DbUtils.GetFiles(fileName);

            Assert.AreEqual(1, dt.Rows.Count, 
                "GetFiles on One Unique File Returns Wrong Amount of Files!");
            var itemArray = dt.Rows[0].ItemArray;
            Assert.AreEqual(fileName, itemArray[1] as string,
                "Input and Output FileNames Are Not Equal!");
            CollectionAssert.AreEqual(fileContent, itemArray[2] as byte[],
                "Input and Output FileContents Are Not Equal!");
        }

        [TestMethod]
        public void Test_AddFile_GetFiles_MultipleFiles_Valid()
        {
            var path1 = RootDirectory + @"\TestGetFiles_MultipleFiles";
            BaseFileWorker.Write("First_TestGetFiles_Content", path1);
            var fileName1 = BaseFileWorker.GetFileName(path1);
            var fileContent1 = Encoding.Unicode.GetBytes(BaseFileWorker.ReadAll(path1));
            BaseFileWorker.MkDir(RootDirectory + @"\SubDirectory");
            var path2 = RootDirectory + @"\SubDirectory\TestGetFiles_MultipleFiles";
            BaseFileWorker.Write("Second_TestGetFiles_Content", path2);
            var fileName2 = BaseFileWorker.GetFileName(path2);
            var fileContent2 = Encoding.Unicode.GetBytes(BaseFileWorker.ReadAll(path2));

            DbUtils.AddFile(fileName1, fileContent1);
            Assert.IsTrue(DbUtils.AddFile(fileName2, fileContent2), 
                "AddFile with Same FileNames Wrong Result!");

            var dt = DbUtils.GetFiles(fileName1);

            Assert.AreEqual(2, dt.Rows.Count,
                "GetFiles on Two Files with Same FileName Returns Wrong Amount of Files!");
            var itemArray1 = dt.Rows[0].ItemArray;
            Assert.AreEqual(fileName1, itemArray1[1] as string,
                "Input and Output FileNames of First File with Same FileName Are Not Equal!");
            CollectionAssert.AreEqual(fileContent1, itemArray1[2] as byte[],
                "Input and Output FileContents of First File with Same FileName Are Not Equal!");
            var itemArray2 = dt.Rows[1].ItemArray;
            Assert.AreEqual(fileName2, itemArray2[1] as string,
                "Input and Output FileNames of Second File with Same FileName Are Not Equal!");
            CollectionAssert.AreEqual(fileContent2, itemArray2[2] as byte[],
                "Input and Output FileContents of Second File with Same FileName Are Not Equal!");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Directory.Delete(RootDirectory, true);
        }
    }
}
