using System;
using System.Text;
using IIG.BinaryFlag;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryFlag.Test
{
    [TestClass]
    public class BinaryFlagTest
    {
        private Random _random;
        private int _uintFlagLength;
        private MultipleBinaryFlag _trueUIntFlag;
        private MultipleBinaryFlag _falseUIntFlag;
        private string _trueUIntFlagString;
        private string _falseUIntFlagString;
        private int _ulongFlagLength;
        private MultipleBinaryFlag _trueULongFlag;
        private MultipleBinaryFlag _falseULongFlag;
        private string _trueULongFlagString;
        private string _falseULongFlagString;
        private int _uintArrayFlagIntLength;
        private MultipleBinaryFlag _trueUIntArrayIntLengthFlag;
        private MultipleBinaryFlag _falseUIntArrayIntLengthFlag;
        private string _trueUIntArrayIntLengthFlagString;
        private string _falseUIntArrayIntLengthFlagString;
        private ulong _uintArrayFlagLength;
        private MultipleBinaryFlag _trueUIntArrayFlag;
        private MultipleBinaryFlag _falseUIntArrayFlag;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _random = new Random();
            var sb = new StringBuilder();

            _uintFlagLength = 10;
            _trueUIntFlag = new MultipleBinaryFlag((ulong)_uintFlagLength);
            _falseUIntFlag = new MultipleBinaryFlag((ulong)_uintFlagLength, false);
            sb.Append('T', _uintFlagLength);
            _trueUIntFlagString = sb.ToString();
            _falseUIntFlagString = _trueUIntFlagString.Replace('T', 'F');

            _ulongFlagLength = 50;
            _trueULongFlag = new MultipleBinaryFlag((ulong)_ulongFlagLength);
            _falseULongFlag = new MultipleBinaryFlag((ulong)_ulongFlagLength, false);
            sb.Append('T', _ulongFlagLength - _uintFlagLength);
            _trueULongFlagString = sb.ToString();
            _falseULongFlagString = _trueULongFlagString.Replace('T', 'F');

            _uintArrayFlagIntLength = 100;
            _trueUIntArrayIntLengthFlag = new MultipleBinaryFlag((ulong)_uintArrayFlagIntLength);
            _falseUIntArrayIntLengthFlag = new MultipleBinaryFlag((ulong)_uintArrayFlagIntLength, false);
            sb.Append('T', _uintArrayFlagIntLength - _ulongFlagLength);
            _trueUIntArrayIntLengthFlagString = sb.ToString();
            _falseUIntArrayIntLengthFlagString = _trueUIntArrayIntLengthFlagString.Replace('T', 'F');

            _uintArrayFlagLength = (ulong)int.MaxValue + 10;
            _trueUIntArrayFlag = new MultipleBinaryFlag(_uintArrayFlagLength);
            _falseUIntArrayFlag = new MultipleBinaryFlag(_uintArrayFlagLength, false);
        }

        [TestMethod]
        public void Test_Constructor_InvalidLength_Exception()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new MultipleBinaryFlag(1),
                "Constructor with Length Less Than 2 Throws None or Wrong Exception!");
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new MultipleBinaryFlag(17179868705),
                "Constructor with Length Greater Than 17179868704 Throws None or Wrong Exception!");
        }

        [TestMethod]
        public void Test_GetFlag_TrueInit_True()
        {
            Assert.AreEqual(true, _trueUIntFlag.GetFlag(),
                "GetFlag on True UInt Binary Flag Wrong Result!");
            Assert.AreEqual(true, _trueULongFlag.GetFlag(),
                "GetFlag on True ULong Binary Flag Wrong Result!");
            Assert.AreEqual(true, _trueUIntArrayIntLengthFlag.GetFlag(),
                "GetFlag on True UIntArray Binary Flag with Int Range Length Wrong Result!");
            Assert.AreEqual(true, _trueUIntArrayFlag.GetFlag(),
                "GetFlag on True UIntArray Binary Flag Wrong Result!");
        }

        [TestMethod]
        public void Test_GetFlag_FalseInit_False()
        {
            Assert.AreEqual(false, _falseUIntFlag.GetFlag(),
                "GetFlag on False UInt Binary Flag Wrong Result!");
            Assert.AreEqual(false, _falseULongFlag.GetFlag(),
                "GetFlag on False ULong Binary Flag Wrong Result!");
            Assert.AreEqual(false, _falseUIntArrayIntLengthFlag.GetFlag(),
                "GetFlag on False UIntArray Binary Flag with Int Range Length Wrong Result!");
            Assert.AreEqual(false, _falseUIntArrayFlag.GetFlag(),
                "GetFlag on False UIntArray Binary Flag Wrong Result!");
        }

        [TestMethod]
        public void Test_ToString_TrueInit_T()
        {
            Assert.AreEqual(_trueUIntFlagString, _trueUIntFlag.ToString(),
                "ToString on True UInt Binary Flag Wrong Result!");
            Assert.AreEqual(_trueULongFlagString, _trueULongFlag.ToString(),
                "ToString on True ULong Binary Flag Wrong Result!");
            Assert.AreEqual(_trueUIntArrayIntLengthFlagString, _trueUIntArrayIntLengthFlag.ToString(),
                "ToString on True UIntArray Binary Flag Wrong Result!");
        }
        
        [TestMethod]
        public void Test_ToString_FalseInit_F()
        {
            Assert.AreEqual(_falseUIntFlagString, _falseUIntFlag.ToString(),
                "ToString on False UInt Binary Flag Wrong Result!");
            Assert.AreEqual(_falseULongFlagString, _falseULongFlag.ToString(),
                "ToString on False ULong Binary Flag Wrong Result!");
            Assert.AreEqual(_falseUIntArrayIntLengthFlagString, _falseUIntArrayIntLengthFlag.ToString(),
                "ToString on False UIntArray Binary Flag Wrong Result!");
        }

        [TestMethod]
        public void Test_SetFlag_InvalidPosition_Exception()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _trueUIntFlag.SetFlag((ulong)_uintFlagLength),
                "SetFlag with OutOfRange Position on UInt Flag Throws None or Wrong Exception!");
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _trueULongFlag.SetFlag((ulong)_ulongFlagLength),
                "SetFlag with OutOfRange Position on ULong Flag Throws None or Wrong Exception!");
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _trueUIntArrayFlag.SetFlag(_uintArrayFlagLength),
                "SetFlag with OutOfRange Position on UIntArray Flag Throws None or Wrong Exception!");
        }

        [TestMethod]
        public void Test_SetFlag_GetFlag_TrueInit_True()
        {
            _trueUIntFlag.SetFlag((ulong)_random.Next(_uintFlagLength));
            _trueULongFlag.SetFlag((ulong)_random.Next(_ulongFlagLength));
            _trueUIntArrayIntLengthFlag.SetFlag((ulong)_random.Next(_uintArrayFlagIntLength));
            _trueUIntArrayFlag.SetFlag((ulong)_random.Next());

            Assert.AreEqual(true, _trueUIntFlag.GetFlag(),
                "GetFlag on True UInt Flag after SetFlag Wrong Result!");
            Assert.AreEqual(true, _trueULongFlag.GetFlag(),
                "GetFlag on True ULong Flag after SetFlag Wrong Result!");
            Assert.AreEqual(true, _trueUIntArrayIntLengthFlag.GetFlag(),
                "GetFlag on True UIntArray with Int Range Length Flag after SetFlag Wrong Result!");
            Assert.AreEqual(true, _trueUIntArrayFlag.GetFlag(),
                "GetFlag on True UIntArray Flag after SetFlag Wrong Result!");
        }

        [TestMethod]
        public void Test_SetFlag_GetFlag_FalseInit_False()
        {
            _falseUIntFlag.SetFlag((ulong)_random.Next(_uintFlagLength));
            _falseULongFlag.SetFlag((ulong)_random.Next(_ulongFlagLength));
            _falseUIntArrayIntLengthFlag.SetFlag((ulong)_random.Next(_uintArrayFlagIntLength));
            _falseUIntArrayFlag.SetFlag((ulong)_random.Next());

            Assert.AreEqual(false, _falseUIntFlag.GetFlag(),
                "GetFlag on False UInt Flag after SetFlag Wrong Result!");
            Assert.AreEqual(false, _falseULongFlag.GetFlag(),
                "GetFlag on False ULong Flag after SetFlag Wrong Result!");
            Assert.AreEqual(false, _falseUIntArrayIntLengthFlag.GetFlag(),
                "GetFlag on False UIntArray with Int Range Length Flag after SetFlag Wrong Result!");
            Assert.AreEqual(false, _falseUIntArrayFlag.GetFlag(),
                "GetFlag on False UIntArray Flag after SetFlag Wrong Result!");
        }

        [TestMethod]
        public void Test_SetFlag_ToString_TrueInit_Valid()
        {
            var uintPosition = _random.Next(_uintFlagLength);
            var ulongPosition = _random.Next(_ulongFlagLength);
            var uintArrayPosition = _random.Next(_uintArrayFlagIntLength);

            _trueUIntFlag.SetFlag((ulong)uintPosition);
            _trueULongFlag.SetFlag((ulong)ulongPosition);
            _trueUIntArrayIntLengthFlag.SetFlag((ulong)uintArrayPosition);

            Assert.AreEqual(_trueUIntFlagString, _trueUIntFlag.ToString(),
                "ToString on True UInt Flag after SetFlag Wrong Result!");
            Assert.AreEqual(_trueULongFlagString, _trueULongFlag.ToString(),
                "ToString on True ULong Flag after SetFlag Wrong Result!");
            Assert.AreEqual(_trueUIntArrayIntLengthFlagString, _trueUIntArrayIntLengthFlag.ToString(),
                "ToString on True UIntArray Flag after SetFlag Wrong Result!");
        }

        [TestMethod]
        public void Test_SetFlag_ToString_FalseInit_Valid()
        {
            var uintPosition = _random.Next(_uintFlagLength);
            var ulongPosition = _random.Next(_ulongFlagLength);
            var uintArrayPosition = _random.Next(_uintArrayFlagIntLength);
            var uintResult = _falseUIntFlagString
                .Remove(uintPosition, 1)
                .Insert(uintPosition, "T");
            var ulongResult = _falseULongFlagString
                .Remove(ulongPosition, 1)
                .Insert(ulongPosition, "T");
            var uintArrayResult = _falseUIntArrayIntLengthFlagString
                .Remove(uintArrayPosition, 1)
                .Insert(uintArrayPosition, "T");

            _falseUIntFlag.SetFlag((ulong)uintPosition);
            _falseULongFlag.SetFlag((ulong)ulongPosition);
            _falseUIntArrayIntLengthFlag.SetFlag((ulong)uintArrayPosition);

            Assert.AreEqual(uintResult, _falseUIntFlag.ToString(),
                "ToString on False UInt Flag after SetFlag Wrong Result!");
            Assert.AreEqual(ulongResult, _falseULongFlag.ToString(),
                "ToString on False ULong Flag after SetFlag Wrong Result!");
            Assert.AreEqual(uintArrayResult, _falseUIntArrayIntLengthFlag.ToString(),
                "ToString on False UIntArray Flag after SetFlag Wrong Result!");
        }

        [TestMethod]
        public void Test_ResetFlag_InvalidPosition_Exception()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _trueUIntFlag.ResetFlag((ulong)_uintFlagLength),
                "ResetFlag with OutOfRange Position on UInt Flag Throws None or Wrong Exception!");
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _trueULongFlag.ResetFlag((ulong)_ulongFlagLength),
                "ResetFlag with OutOfRange Position on ULong Flag Throws None or Wrong Exception!");
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => _trueUIntArrayFlag.ResetFlag(_uintArrayFlagLength),
                "ResetFlag with OutOfRange Position on UIntArray Flag Throws None or Wrong Exception!");
        }

        [TestMethod]
        public void Test_ResetFlag_GetFlag_TrueInit_False()
        {
            _trueUIntFlag.ResetFlag((ulong)_random.Next(_uintFlagLength));
            _trueULongFlag.ResetFlag((ulong)_random.Next(_ulongFlagLength));
            _trueUIntArrayIntLengthFlag.ResetFlag((ulong)_random.Next(_uintArrayFlagIntLength));
            _trueUIntArrayFlag.ResetFlag((ulong)_random.Next());

            Assert.AreEqual(false, _trueUIntFlag.GetFlag(),
                "GetFlag on True UInt Flag after ResetFlag Wrong Result!");
            Assert.AreEqual(false, _trueULongFlag.GetFlag(),
                "GetFlag on True ULong Flag after ResetFlag Wrong Result!");
            Assert.AreEqual(false, _trueUIntArrayIntLengthFlag.GetFlag(),
                "GetFlag on True UIntArray Flag with Int Range Length after ResetFlag Wrong Result!");
            Assert.AreEqual(false, _trueUIntArrayFlag.GetFlag(),
                "GetFlag on True UIntArray Flag after ResetFlag Wrong Result!");
        }

        [TestMethod]
        public void Test_ResetFlag_GetFlag_FalseInit_False()
        {
            _falseUIntFlag.ResetFlag((ulong)_random.Next(_uintFlagLength));
            _falseULongFlag.ResetFlag((ulong)_random.Next(_ulongFlagLength));
            _falseUIntArrayIntLengthFlag.ResetFlag((ulong)_random.Next(_uintArrayFlagIntLength));
            _falseUIntArrayFlag.ResetFlag((ulong)_random.Next());

            Assert.AreEqual(false, _falseUIntFlag.GetFlag(),
                "GetFlag on False UInt Flag after ResetFlag Wrong Result!");
            Assert.AreEqual(false, _falseULongFlag.GetFlag(),
                "GetFlag on False ULong Flag after ResetFlag Wrong Result!");
            Assert.AreEqual(false, _falseUIntArrayIntLengthFlag.GetFlag(),
                "GetFlag on False UIntArray Flag with Int Range Length after ResetFlag Wrong Result!");
            Assert.AreEqual(false, _falseUIntArrayFlag.GetFlag(),
                "GetFlag on False UIntArray Flag after ResetFlag Wrong Result!");
        }

        [TestMethod]
        public void Test_ResetFlag_ToString_TrueInit_Valid()
        {
            var uintPosition = _random.Next(_uintFlagLength);
            var ulongPosition = _random.Next(_ulongFlagLength);
            var uintArrayPosition = _random.Next(_uintArrayFlagIntLength);
            var uintResult = _trueUIntFlagString
                .Remove(uintPosition, 1)
                .Insert(uintPosition, "F");
            var ulongResult = _trueULongFlagString
                .Remove(ulongPosition, 1)
                .Insert(ulongPosition, "F");
            var uintArrayResult = _trueUIntArrayIntLengthFlagString
                .Remove(uintArrayPosition, 1)
                .Insert(uintArrayPosition, "F");

            _trueUIntFlag.ResetFlag((ulong)uintPosition);
            _trueULongFlag.ResetFlag((ulong)ulongPosition);
            _trueUIntArrayIntLengthFlag.ResetFlag((ulong)uintArrayPosition);

            Assert.AreEqual(uintResult, _trueUIntFlag.ToString(),
                "ToString on True UInt Flag after ResetFlag Wrong Result!");
            Assert.AreEqual(ulongResult, _trueULongFlag.ToString(),
                "ToString on True ULong Flag after ResetFlag Wrong Result!");
            Assert.AreEqual(uintArrayResult, _trueUIntArrayIntLengthFlag.ToString(),
                "ToString on True UIntArray Flag after ResetFlag Wrong Result!");
        }

        [TestMethod]
        public void Test_ResetFlag_ToString_FalseInit_Valid()
        {
            var uintPosition = _random.Next(_uintFlagLength);
            var ulongPosition = _random.Next(_ulongFlagLength);
            var uintArrayPosition = _random.Next(_uintArrayFlagIntLength);

            _falseUIntFlag.ResetFlag((ulong)uintPosition);
            _falseULongFlag.ResetFlag((ulong)ulongPosition);
            _falseUIntArrayIntLengthFlag.ResetFlag((ulong)uintArrayPosition);

            Assert.AreEqual(_falseUIntFlagString, _falseUIntFlag.ToString(),
                "ToString on False UInt Flag after ResetFlag Wrong Result!");
            Assert.AreEqual(_falseULongFlagString, _falseULongFlag.ToString(),
                "ToString on False ULong Flag after ResetFlag Wrong Result!");
            Assert.AreEqual(_falseUIntArrayIntLengthFlagString, _falseUIntArrayIntLengthFlag.ToString(),
                "ToString on False UIntArray Flag after ResetFlag Wrong Result!");
        }

        [TestMethod]
        public void Test_Dispose_GetFlag_Null()
        {
            _trueUIntFlag.Dispose();
            _trueULongFlag.Dispose();
            _trueUIntArrayFlag.Dispose();

            Assert.IsNull(_trueUIntFlag.GetFlag(),
                "GetFlag on UInt Flag after Dispose Wrong Result!");
            Assert.IsNull(_trueULongFlag.GetFlag(),
                "GetFlag on ULong Flag after Dispose Wrong Result!");
            Assert.IsNull(_trueUIntArrayFlag.GetFlag(),
                "GetFlag on UIntArray Flag after Dispose Wrong Result!");
        }

        [TestMethod]
        public void Test_Dispose_ToString_Null()
        {
            _trueUIntFlag.Dispose();
            _trueULongFlag.Dispose();
            _trueUIntArrayFlag.Dispose();

            Assert.IsNull(_trueUIntFlag.ToString(),
                "ToString on UInt Flag after Dispose Wrong Result!");
            Assert.IsNull(_trueULongFlag.ToString(),
                "ToString on ULong Flag after Dispose Wrong Result!");
            Assert.IsNull(_trueUIntArrayFlag.ToString(),
                "ToString on UIntArray Flag after Dispose Wrong Result!");
        }
    }
}
