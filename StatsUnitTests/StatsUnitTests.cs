using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LakeStats;
using System.Diagnostics;

//[assembly: InternalsVisibleTo("StatsUnitTests")]
//PrivateObject obj = new PrivateObject(fm);

namespace StatsUnitTests
{

    [TestClass]
     class StatsUnitTests
    {
        [TestMethod]
        public void TestFeedURL()
        {
            var fm = new LakeStats.FeedManager();

            DateTime aDate = new DateTime(2017, 01, 01); //DateTime.Today
            FeedResult aResult = fm.GetData(aDate, FeedManager.WIND_SPEED) ;
            //Microsoft.VisualStudio.TestTools.UnitTesting.Assert
            //
            //System.Diagnostics.Debug Assert Fail Write WriteLine
            //System.Diagnostics.Trace.

            Assert.AreNotEqual(aResult.mean, 0.000, 0.001, "Data Not retrived; Mean not calculated.");

        }
    }

    //{
    //Class target = new Class();
    //PrivateObject obj = new PrivateObject(target);
    //var retVal = obj.Invoke("PrivateMethod");
    //Assert.AreEqual(retVal);
    //}
}




//namespace Bank_UnitTestProject1
//{
//    [TestClass]
//    public class BankAccountTests
//    {


//        [TestMethod]
//        public void TestMethod1()
//        {
//            Console.WriteLine("Unit Test TestMethod1 Completed");
//        }

//        // unit test code  
//        [TestMethod]
//        public void Debit_WithValidAmount_UpdatesBalance()
//        {
//            // arrange  
//            double beginningBalance = 11.99;
//            double debitAmount = 4.55;
//            double expected = 7.44;
//            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

//            // act  
//            account.Debit(debitAmount);

//            // assert  
//            double actual = account.Balance;
//            Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");

//        }

//        //unit test method  
//        [TestMethod]
//        //--[ExpectedException(typeof(ArgumentOutOfRangeException))]
//        public void Debit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange()
//        {
//            // arrange  
//            double beginningBalance = 11.99;
//            double debitAmount = -100.00;
//            BankAccount account = new BankAccount("Mr. Bryan Walton", beginningBalance);

//            //-- act  
//            //account.Debit(debitAmount);
//            //-- assert is handled by ExpectedException  

//            // act  
//            try
//            {
//                account.Debit(debitAmount);
//            }
//            catch (ArgumentOutOfRangeException e)
//            {
//                // assert  
//                StringAssert.Contains(e.Message, BankAccount.DebitAmountLessThanZeroMessage);
//            }
//        }

//        [TestMethod]
//        //-- [ExpectedException(typeof(ArgumentOutOfRangeException))]
//        public void Debit_WhenAmountIsMoreThanBalance_ShouldThrowArgumentOutOfRange()
//        {
//            //arrange
//            double beginningBalance = 12.00;
//            double debitAmount = 120.00;
//            BankAccount account = new BankAccount("Mr. Adam Smith", beginningBalance);

//            //-- act  
//            //account.Debit(debitAmount);
//            //-- assert is handled by ExpectedException  

//            // act  
//            try
//            {
//                account.Debit(debitAmount);
//            }
//            catch (ArgumentOutOfRangeException e)
//            {
//                // assert  
//                StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);
//                return;
//            }
//            Assert.Fail("No exception was thrown.");

//        }
//    }
//}
