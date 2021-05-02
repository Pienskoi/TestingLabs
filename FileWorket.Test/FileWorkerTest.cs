using System;
using System.IO;
using IIG.FileWorker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileWorker.Test
{
    [TestClass]
    public class FileWorkerTest
    {
        private const string Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n"
                                    + "Donec tempor in augue ac dapibus.\n"
                                    + "Sed sed rutrum ligula.";
        private const string OverwriteText = "This text should overwrite contents of existing file";
        private const string RootDirectory = @"\FileWorker_TempDir";
        private const string RelativePathFileName = "TestFile_RelativePath.txt";
        private const string RelativePath = RootDirectory + "\\" + RelativePathFileName;
        private const string RelativePathDirName = "TestDir_RelativePath";
        private const string DirRelativePath = RootDirectory + "\\" + RelativePathDirName;
        private static string RootDirectoryFullPath;
        private const string FullPathFileName = "TestFile_FullPath.txt";
        private static string FullPath;
        private const string FullPathDirName = "TestDir_FullPath";
        private static string DirFullPath;
        private const string CopyToPath = RootDirectory + @"\TestFile_RelativePath_TryCopy.txt";
        private const string NotExistingFile = RootDirectory + "\\" + "NotExistingFile.txt";

        public FileWorkerTest()
        {
            DirectoryInfo di = Directory.CreateDirectory(RootDirectory);
            RootDirectoryFullPath = di.FullName;
            FullPath = RootDirectoryFullPath + "\\" + FullPathFileName;
            DirFullPath = RootDirectoryFullPath + "\\" + FullPathDirName;
        }

        [TestInitialize]
        public void Test_Write_CreateNewFile_True()
        {
            var fullPathWriteResult = BaseFileWorker.Write(Text, FullPath);
            var relativePathWriteResult = BaseFileWorker.Write(Text, RelativePath);

            Assert.IsTrue(File.Exists(FullPath),
                "New File by Full Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(FullPath),
                "Contents of File by Full Path are Not Equal with Text Passed to Parameters!");
            Assert.IsTrue(fullPathWriteResult,
                "Write to New File by Full Path Wrong Result!");

            Assert.IsTrue(File.Exists(RelativePath),
                "New File by Relative Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(RelativePath),
                "Contents of File by Relative Path are Not Equal with Text Passed to Parameters!");
            Assert.IsTrue(relativePathWriteResult,
                "Write to New File by Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_Write_OverwriteFile_True()
        {
            var fullPathWriteResult = BaseFileWorker.Write(OverwriteText, FullPath);
            var relativePathWriteResult = BaseFileWorker.Write(OverwriteText, RelativePath);

            Assert.AreEqual(OverwriteText, File.ReadAllText(FullPath),
                "Contents of File by Full Path are Not Overwritten!");
            Assert.IsTrue(fullPathWriteResult,
                "Write to Existing File by Full Path Wrong Result!");

            Assert.AreEqual(OverwriteText, File.ReadAllText(RelativePath),
                "Contents of File by Relative Path are Not Overwritten!");
            Assert.IsTrue(relativePathWriteResult,
                "Write to Existing File by Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_Write_False()
        {
            Assert.IsFalse(BaseFileWorker.Write(Text, RootDirectory),
                "Write to Directory Wrong Result!");
            Assert.IsFalse(BaseFileWorker.Write(Text, string.Empty),
                "Write to Empty Path Wrong Result!");
            Assert.IsFalse(BaseFileWorker.Write(Text, " "),
                "Write to White Space Path Wrong Result!");
            Assert.IsFalse(BaseFileWorker.Write(Text, null),
                "Write to Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_ReadAll_Valid()
        {
            var fullPathContent = BaseFileWorker.ReadAll(FullPath);
            var relativePathContent = BaseFileWorker.ReadAll(RelativePath);

            Assert.AreEqual(Text, fullPathContent,
                "ReadAll Full Path Result and Actual File Content are Not Equal!");
            Assert.AreEqual(Text, relativePathContent,
                "ReadAll Relative Path Resul and Actual File Content are Not Equal!");
        }

        [TestMethod]
        public void Test_ReadAll_Null()
        {
            Assert.IsNull(BaseFileWorker.ReadAll(NotExistingFile),
                "ReadAll Not Existing File Wrong Result!");
            Assert.IsNull(BaseFileWorker.ReadAll(RootDirectory),
                "ReadAll Directory Wrong Result!");
            Assert.IsNull(BaseFileWorker.ReadAll(string.Empty),
                "ReadAll Empty Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.ReadAll(" "),
                "ReadAll White Space Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.ReadAll(null),
                "ReadAll Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_ReadLines_Valid()
        {
            var expectedLines = Text.Split("\n");

            var fullPathLines = BaseFileWorker.ReadLines(FullPath);
            var relativePathLines = BaseFileWorker.ReadLines(RelativePath);

            CollectionAssert.AreEqual(expectedLines, fullPathLines,
                "ReadLines Full Path Result and Actual File Content are Not Equal!");
            CollectionAssert.AreEqual(expectedLines, relativePathLines,
                "ReadLines Relative Path Result and Actual File Content are Not Equal!");
        }

        [TestMethod]
        public void Test_ReadLines_Null()
        {
            Assert.IsNull(BaseFileWorker.ReadLines(NotExistingFile),
                "ReadLines Not Existing File Wrong Result!");
            Assert.IsNull(BaseFileWorker.ReadLines(RootDirectory),
                "ReadLines Directory Wrong Result!");
            Assert.IsNull(BaseFileWorker.ReadLines(string.Empty),
                "ReadLines Empty Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.ReadLines(" "),
                "ReadLines White Space Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.ReadLines(null),
                "ReadLines Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_CreateNewFile_True()
        {
            var fullPath = RootDirectoryFullPath + @"\TestFile_FullPath_TryWrite.txt";
            var relativePath = RootDirectory + @"\TestFile_RelativePath_TryWrite.txt";

            var fullPathTryWriteResult = BaseFileWorker.TryWrite(Text, fullPath);
            var relativePathTryWriteResult = BaseFileWorker.TryWrite(Text, relativePath);

            Assert.IsTrue(File.Exists(fullPath),
                "New File by Full Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(fullPath),
                "Contents of File by Full Path are Not Equal with Text Passed to Parameters!");
            Assert.IsTrue(fullPathTryWriteResult, "TryWrite to New File by Full Path Wrong Result!");

            Assert.IsTrue(File.Exists(relativePath),
                "New File by Relative Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(relativePath),
                "Contents of File by Relative Path are Not Equal with Text Passed to Parameters!");
            Assert.IsTrue(relativePathTryWriteResult, "TryWrite to New File by Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_OverwriteFile_True()
        {
            var fullPathTryWriteResult = BaseFileWorker.TryWrite(OverwriteText, FullPath);
            var relativePathTryWriteResult = BaseFileWorker.TryWrite(OverwriteText, RelativePath);

            Assert.AreEqual(OverwriteText, File.ReadAllText(FullPath),
                "Contents of File by Full Path are Not Overwritten!");
            Assert.IsTrue(fullPathTryWriteResult,
                "TryWrite to Existing File by Full Path Wrong Result!");

            Assert.AreEqual(OverwriteText, File.ReadAllText(RelativePath),
                "Contents of File by Relative Path are Not Overwritten!");
            Assert.IsTrue(relativePathTryWriteResult,
                "TryWrite to Existing File by Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_False()
        {
            Assert.IsFalse(BaseFileWorker.TryWrite(Text, RootDirectory),
                "TryWrite to Directory Wrong Result!");
            Assert.IsFalse(BaseFileWorker.TryWrite(Text, string.Empty),
                "TryWrite to Empty Path Wrong Result!");
            Assert.IsFalse(BaseFileWorker.TryWrite(Text, " "),
                "TryWrite to White Space Path Wrong Result!");
            Assert.IsFalse(BaseFileWorker.TryWrite(Text, null),
                "TryWrite to Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_InRangeTries_Two_True()
        {
            var tryWriteResult = BaseFileWorker.TryWrite(OverwriteText, FullPath, 2);

            Assert.AreEqual(OverwriteText, File.ReadAllText(FullPath),
                "Contents of File by Full Path are Not Overwritten!");
            Assert.IsTrue(tryWriteResult, "TryWrite 2 Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_InRangeTries_Random_True()
        {
            var tryWriteResult = BaseFileWorker.TryWrite(OverwriteText, FullPath, new Random().Next(1, int.MaxValue));

            Assert.AreEqual(OverwriteText, File.ReadAllText(FullPath),
                "Contents of File by Full Path are Not Overwritten!");
            Assert.IsTrue(tryWriteResult, "TryWrite Random In Range Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_InRangeTries_MaxMinusOne_True()
        {
            var tryWriteResult = BaseFileWorker.TryWrite(OverwriteText, FullPath, int.MaxValue - 1);

            Assert.AreEqual(OverwriteText, File.ReadAllText(FullPath),
                "Contents of File by Full Path are Not Overwritten!");
            Assert.IsTrue(tryWriteResult, "TryWrite Max Int - 1 Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_OnBorderTries_Max_True()
        {
            var tryWriteResult = BaseFileWorker.TryWrite(OverwriteText, FullPath, int.MaxValue);

            Assert.AreEqual(OverwriteText, File.ReadAllText(FullPath),
                "Contents of File by Full Path are Not Overwritten!");
            Assert.IsTrue(tryWriteResult, "TryWrite Max Int Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_OutOfRangeTries_Zero_False()
        {
            var tryWriteResult = BaseFileWorker.TryWrite(OverwriteText, FullPath, 0);

            Assert.IsFalse(tryWriteResult, "TryWrite 0 Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_OutOfRangeTries_Min_False()
        {
            var tryWriteResult = BaseFileWorker.TryWrite(OverwriteText, FullPath, int.MinValue);

            Assert.IsFalse(tryWriteResult, "TryWrite Min Int Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryWrite_OutOfRangeTries_Random_False()
        {
            var tryWriteResult = BaseFileWorker.TryWrite(OverwriteText, FullPath, new Random().Next(int.MinValue, 1));

            Assert.IsFalse(tryWriteResult, "TryWrite Random Out of Range Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_CreateNewFile_RewriteTrue_True()
        {
            var copyToFullPath = RootDirectoryFullPath + @"\TestFile_FullPath_TryCopy.txt";
            var copyToRelativePath = CopyToPath;

            var fullPathTryCopyResult = BaseFileWorker.TryCopy(FullPath, copyToFullPath, true);
            var relativePathTryCopyResult = BaseFileWorker.TryCopy(RelativePath, copyToRelativePath, true);

            Assert.IsTrue(File.Exists(copyToFullPath),
                "New File by Full Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(copyToFullPath),
                "Contents of File by Full Path are Not Equal with Contents of Copied File!");
            Assert.IsTrue(fullPathTryCopyResult,
                "TryCopy to New File by Full Path Wrong Result!");

            Assert.IsTrue(File.Exists(RelativePath),
                "New File by Relative Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(RelativePath),
                "Contents of File by Relative Path are Not Equal with Contents of Copied File!");
            Assert.IsTrue(relativePathTryCopyResult,
                "TryCopy to New File by Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_CreateNewFile_RewriteFalse_True()
        {
            var copyToFullPath = RootDirectoryFullPath + @"\TestFile_FullPath_TryCopy.txt";
            var copyToRelativePath = CopyToPath;

            var fullPathTryCopyResult = BaseFileWorker.TryCopy(FullPath, copyToFullPath, false);
            var relativePathTryCopyResult = BaseFileWorker.TryCopy(RelativePath, copyToRelativePath, false);

            Assert.IsTrue(File.Exists(copyToFullPath),
                "New File by Full Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(copyToFullPath),
                "Contents of File by Full Path are Not Equal with Contents of Copied File!");
            Assert.IsTrue(fullPathTryCopyResult,
                "TryCopy to New File by Full Path Wrong Result!");

            Assert.IsTrue(File.Exists(RelativePath),
                "New File by Relative Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(RelativePath),
                "Contents of File by Relative Path are Not Equal with Contents of Copied File!");
            Assert.IsTrue(relativePathTryCopyResult,
                "TryCopy to New File by Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_OverwriteFile_RewriteTrue_True()
        {
            var copyFrom = RootDirectory + @"\TestFile_CopyFrom.txt";
            BaseFileWorker.Write(OverwriteText, copyFrom);

            var fullPathTryCopyResult = BaseFileWorker.TryCopy(copyFrom, FullPath, true);
            var relativePathTryCopyResult = BaseFileWorker.TryCopy(copyFrom, RelativePath, true);

            Assert.AreEqual(OverwriteText, File.ReadAllText(FullPath),
                "Contents of File by Full Path are Not Overwritten!");
            Assert.IsTrue(fullPathTryCopyResult,
                "TryCopy to Existing File by Full Path Wrong Result!");

            Assert.AreEqual(OverwriteText, File.ReadAllText(RelativePath),
                "Contents of File by Relative Path are Not Overwritten!");
            Assert.IsTrue(relativePathTryCopyResult,
                "TryCopy to Existing File by Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_OverwriteFile_RewriteFalse_Exception()
        {
            var copyFrom = RootDirectory + @"\TestFile_CopyFrom.txt";
            BaseFileWorker.Write(OverwriteText, copyFrom);

            Assert.ThrowsException<IOException>(
                () => BaseFileWorker.TryCopy(copyFrom, FullPath, false),
                "TryCopy Overwrite by Full Path with False Rewrite Flag Throw None or Wrong Exception!");
            Assert.ThrowsException<IOException>(
                () => BaseFileWorker.TryCopy(copyFrom, RelativePath, false),
                "TryCopy Overwrite by Relative Path with False Rewrite Flag Throw None or Wrong Exception!");
        }

        [TestMethod]
        public void Test_TryCopy_InRangeTries_Two_True()
        {
            var tryCopyResult = BaseFileWorker.TryCopy(FullPath, CopyToPath, false, 2);

            Assert.IsTrue(File.Exists(CopyToPath),
                "New File by Full Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(CopyToPath),
                "Contents of File by Full Path are Not Equal with Contents of Copied File!");
            Assert.IsTrue(tryCopyResult, "TryCopy 2 Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_InRangeTries_Random_True()
        {
            var tryCopyResult = BaseFileWorker.TryCopy(FullPath, CopyToPath, false, new Random().Next(1, int.MaxValue));

            Assert.IsTrue(File.Exists(CopyToPath),
                "New File by Full Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(CopyToPath),
                "Contents of File by Full Path are Not Equal with Contents of Copied File!");
            Assert.IsTrue(tryCopyResult, "TryCopy Random In Range Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_InRangeTries_MaxMinusOne_True()
        {
            var tryCopyResult = BaseFileWorker.TryCopy(FullPath, CopyToPath, false, int.MaxValue - 1);

            Assert.IsTrue(File.Exists(CopyToPath),
                "New File by Full Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(CopyToPath),
                "Contents of File by Full Path are Not Equal with Contents of Copied File!");
            Assert.IsTrue(tryCopyResult, "TryCopy Max Int - 1 Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_OnBorderTries_Max_True()
        {
            var tryCopyResult = BaseFileWorker.TryCopy(FullPath, CopyToPath, false, int.MaxValue);

            Assert.IsTrue(File.Exists(CopyToPath),
                "New File by Full Path is Not Created!");
            Assert.AreEqual(Text, File.ReadAllText(CopyToPath),
                "Contents of File by Full Path are Not Equal with Contents of Copied File!");
            Assert.IsTrue(tryCopyResult, "TryCopy Max Int Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_OutOfRangeTries_Zero_False()
        {
            var tryCopyResult = BaseFileWorker.TryCopy(FullPath, CopyToPath, false, 0);

            Assert.IsFalse(tryCopyResult, "TryCopy 0 Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_OutOfRangeTries_Min_False()
        {
            var tryCopyResult = BaseFileWorker.TryCopy(FullPath, CopyToPath, false, int.MinValue);

            Assert.IsFalse(tryCopyResult, "TryCopy Min Int Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_OutOfRangeTries_Random_False()
        {
            var tryCopyResult = BaseFileWorker.TryCopy(FullPath, CopyToPath, false, new Random().Next(int.MinValue, 1));

            Assert.IsFalse(tryCopyResult, "TryCopy Random Out of Range Tries Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_InvalidSourceFile_False()
        {
            Assert.IsFalse(BaseFileWorker.TryCopy(string.Empty, CopyToPath, false),
                "TryCopy from Empty Path Wrong Result!");
            Assert.IsFalse(BaseFileWorker.TryCopy(null, CopyToPath, false),
                "TryCopy from Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_InvalidSourceFile_Exception()
        {
            Assert.ThrowsException<FileNotFoundException>(
                () => BaseFileWorker.TryCopy(NotExistingFile, CopyToPath, false),
                "TryCopy from Not Existing File Throw None or Wrong Exception!");
            Assert.ThrowsException<UnauthorizedAccessException>(
                () => BaseFileWorker.TryCopy(RootDirectory, CopyToPath, false),
                "TryCopy from Directory Throw None or Wrong Exception!");
            Assert.ThrowsException<ArgumentException>(
                () => BaseFileWorker.TryCopy(" ", CopyToPath, false),
                "TryCopy from White Space Throw None or Wrong Exception");
        }

        [TestMethod]
        public void Test_TryCopy_InvalidDestinationFile_False()
        {
            Assert.IsFalse(BaseFileWorker.TryCopy(FullPath, string.Empty, false),
                "TryCopy to Empty Path Wrong Result!");
            Assert.IsFalse(BaseFileWorker.TryCopy(FullPath, null, false),
                "TryCopy to Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_TryCopy_InvalidDestinationFile_Exception()
        {
            Assert.ThrowsException<UnauthorizedAccessException>(
                () => BaseFileWorker.TryCopy(RootDirectory, CopyToPath, false),
                "TryCopy to Directory Throw None or Wrong Exception!");
            Assert.ThrowsException<ArgumentException>(
                () => BaseFileWorker.TryCopy(" ", CopyToPath, false),
                "TryCopy to White Space Path Throw None or Wrong Exception");
        }

        [TestMethod]
        public void Test_GetFullPath_Valid()
        {
            var fullPathOfFull = BaseFileWorker.GetFullPath(FullPath);
            var fullPathOfRelative = BaseFileWorker.GetFullPath(RelativePath);

            Assert.AreEqual(FullPath, fullPathOfFull,
                "GetFullPath of Full Path Wrong Result!");
            Assert.AreEqual(RootDirectoryFullPath + "\\" + RelativePathFileName, fullPathOfRelative,
                "GetFullPath of Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_GetFullPath_Null()
        {
            Assert.IsNull(BaseFileWorker.GetFullPath(NotExistingFile),
                "GetFullPath Not Existing File Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetFullPath(RootDirectory),
                "GetFullPath Directory Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetFullPath(string.Empty),
                "GetFullPath Empty Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetFullPath(" "),
                "GetFullPath White Space Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetFullPath(null),
                "GetFullPath Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_GetFileName_Valid()
        {
            var fileNameOfFull = BaseFileWorker.GetFileName(FullPath);
            var fileNameOfRelative = BaseFileWorker.GetFileName(RelativePath);

            Assert.AreEqual(FullPathFileName, fileNameOfFull,
                "GetFileName of FullPath Wrong Result!");
            Assert.AreEqual(RelativePathFileName, fileNameOfRelative,
                "GetFileName of Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_GetFileName_Null()
        {
            Assert.IsNull(BaseFileWorker.GetFileName(NotExistingFile),
                "GetFileName Not Existing File Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetFileName(RootDirectory),
                "GetFileName Directory Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetFileName(string.Empty),
                "GetFileName Empty Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetFileName(" "),
                "GetFileName White Space Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetFileName(null),
                "GetFileName Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_GetPath_Valid()
        {
            var pathOfFull = BaseFileWorker.GetPath(FullPath);
            var pathOfRelative = BaseFileWorker.GetPath(RelativePath);

            Assert.AreEqual(RootDirectoryFullPath, pathOfFull,
                "GetPath of Full Path Wrong Result!");
            Assert.AreEqual(RootDirectoryFullPath, pathOfRelative,
                "GetPath of Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_GetPath_Null()
        {
            Assert.IsNull(BaseFileWorker.GetPath(NotExistingFile),
                "GetPath Not Existing File Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetPath(RootDirectory),
                "GetPath Directory Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetPath(string.Empty),
                "GetPath Empty Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetPath(" "),
                "GetPath White Space Path Wrong Result!");
            Assert.IsNull(BaseFileWorker.GetPath(null),
                "GetPath Null Path Wrong Result!");
        }

        [TestMethod]
        public void Test_MkDir_Valid()
        {
            var fullPathResult = BaseFileWorker.MkDir(DirFullPath);
            var relativePathResult = BaseFileWorker.MkDir(DirRelativePath);

            Assert.IsTrue(Directory.Exists(DirFullPath),
                "New Directory by Full Path is Not Created!");
            Assert.AreEqual(DirFullPath, fullPathResult,
                "MkDir Full Path Wrong Result!");

            Assert.IsTrue(Directory.Exists(DirRelativePath),
                "New Directory by Relative Path is Not Created!");
            Assert.AreEqual(RootDirectoryFullPath + "\\" + RelativePathDirName, relativePathResult,
                "MkDir Relative Path Wrong Result!");
        }

        [TestMethod]
        public void Test_MkDir_Exception()
        {
            Assert.ThrowsException<ArgumentException>(
                () => BaseFileWorker.MkDir(string.Empty),
                "MkDir Empty Path Throw None or Wrong Exception!");
            Assert.ThrowsException<ArgumentNullException>(
                () => BaseFileWorker.MkDir(null),
                "MkDir Null Path Throw None or Wrong Exception!");
            Assert.ThrowsException<ArgumentException>(
                () => BaseFileWorker.MkDir(" "),
                "MkDir White Space Path Throw None or Wrong Exception!");
            Assert.ThrowsException<IOException>(
                () => BaseFileWorker.MkDir(FullPath),
                "MkDir Existing File Throw None or Wrong Exception");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Directory.Delete(RootDirectory, true);
        }
    }
}
