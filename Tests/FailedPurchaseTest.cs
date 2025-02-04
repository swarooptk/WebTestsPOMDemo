﻿
using WebTestsPOMDemo.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace WebTestsPOMDemo.Tests
{
    [TestFixture]
    public class FailedPurchaseTest : TestBase
    {
        IWebDriver _driver;

        [OneTimeSetUp]
        public void TestClassStart()
        {
            _driver = Init();
            NavigateToHome();
        }
        [OneTimeTearDown]
        public void TestClassEnd()
        {
            QuitBrowser();
        }

        [Test]
        public void VerifyFailedPurchaseFlow()
        {
            //Getting the instance of the landing Home Page
            var homepage = new HomePage(_driver);
            var prodname = homepage.GetProductTitle();
            //Assert the product title is same
            Assert.AreEqual("Midtrans Pillow", prodname.Trim());
            //clicking on Buy button to navigate to shopping cart page
            homepage.ClickBuyNow();

            //getting the instance of the shopping cart page
            var cartpage = new ShoppingCartPage(_driver);
            //Asserting the quantity of the item
            Assert.AreEqual("1", cartpage.GetQuantityValue());
            //clicking on Checkout to navigate to order summary page
            cartpage.ClickCheckout();

            //getting the instance of the order summary iframe
            var osummary = new OrderSummaryPage(_driver);
            //Assert the product name shown in the order summary
            Assert.AreEqual("Midtrans Pillow", osummary.GetProductName().Trim());
            osummary.ClickContinueButton();

            //getting the instance of the payment list iframe
            var paymentlist = new PaymentListPage(_driver);
            paymentlist.ClickCreditCard();

            //getting the instance of the credit card details page
            var ccpage = new CardDetailsPage(_driver);
            // pass the values read from the config file
            ccpage.EnterCardDetails(failccnumber, ccexpiry, cccvv);
            ccpage.ClickContinueButton();

            //getting the instance of the Bank OTP page
            var otppage = new OTPPage(_driver);
            otppage.EnterOTP(bankotp);
            otppage.ClickOkButton();

            //getting the instance of the transaction status page
            var txpage = new TxStatusPage(_driver);
            var statusmsg = txpage.GetFailedTxStatusMessage();

            //Assert the failed payment status message in transaction page
            Assert.IsTrue(statusmsg.ToLower().Trim().Contains("failed"));
        }
    }
}
