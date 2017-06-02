using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using Selenium;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Threading;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private string SITEURL = "https://localhost:44313/";
        [TestMethod]
        public void SignUpTest()
        {
            var chrome = new ChromeDriver("C:\\seleniumdrivers\\bin");
        

            chrome.Navigate().GoToUrl(SITEURL+"/users/signup");
            var name = chrome.FindElementById("FirstName");
            name.SendKeys("Fernando");
            var lname = chrome.FindElementById("LastName");
            lname.SendKeys("Moj");
            var dob = chrome.FindElementById("DOB");
            dob.SendKeys("04/02/1990");
            var username = chrome.FindElementById("username");
            username.SendKeys("fer");
            var password = chrome.FindElementById("password");
            password.SendKeys("fer");
            var passwordconfirmation = chrome.FindElementById("passwordconfirmation");
            passwordconfirmation.SendKeys("fer");

            var savebutton = chrome.FindElementByCssSelector("input[type=submit]");
            savebutton.Click();

            Thread.Sleep(3000);

            var url = chrome.Url;

            if (url.Contains("Questionaire"))
            {
                Console.WriteLine("Signup test passed");
            }

            else
            {
                Console.WriteLine("Signup test failed");
            }

            chrome.Quit();
        }
    }
}
