using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;
using Client;
using Gui;
using System.Net.Sockets;

namespace Tests
{

    [TestClass]
    public class GuiTests
    {

        [TestMethod]
        public void TestgetUserInfoReg() 
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            string userName = "Test";
            string passWord = "1234";
            bool isTrue= dataBaseManager.getUserInfoReg(userName, passWord);
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void TestgetUserInfoLogIn()
        {
            DataBaseManager dataBaseManager = new DataBaseManager();
            string userName = "Test";
            string passWord = "1234";
            bool isFalse = dataBaseManager.getUserInfoLogIn(userName, passWord);
            Assert.IsFalse(isFalse);
        }
    }

    [TestClass]
    public class ServerTests
    {

        [TestMethod]
        public void TestWriteData()
        {
        }
    }
}
