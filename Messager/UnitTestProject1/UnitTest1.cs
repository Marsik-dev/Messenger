using Messager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        static UnitTest1()
        {

            Bd.SaveData = false;

        }

        [TestMethod]
        public void Auth_1_true()
        {

            string login = "123456789", pass = "123456789";
            var messager = Bd.messager.mess1;
            bool exepted = true;

            Assert.AreEqual(exepted, Bd.Auth(messager, login, pass));

        }

        [TestMethod]
        public void Auth_1_false()
        {

            string login = "qwerty123", pass = "123123123";
            var messager = Bd.messager.mess1;
            bool exepted = false;

            Assert.AreEqual(exepted, Bd.Auth(messager, login, pass));

        }

        [TestMethod]
        public void Auth_2_true()
        {

            string login = "12345678", pass = "12345678";
            var messager = Bd.messager.mess2;
            bool exepted = true;

            Assert.AreEqual(exepted, Bd.Auth(messager, login, pass));

        }

        [TestMethod]
        public void Auth_2_false()
        {

            string login = "qwerty", pass = "qwerty123";
            var messager = Bd.messager.mess2;
            bool exepted = false;

            Assert.AreEqual(exepted, Bd.Auth(messager, login, pass));

        }

        [TestMethod]
        public void Reg_1_done()
        {

            string login = "qwerty123qwe", pass = "qwerty123";
            var messager = Bd.messager.mess1;
            var exepted = (int)Bd.regResult.done;

            Assert.AreEqual(exepted, (int)Bd.Reg(messager, login, pass, "123456"));

        }

        [TestMethod]
        public void Reg_1_lenght()
        {

            string login = "123", pass = "456";
            var messager = Bd.messager.mess1;
            var exepted = (int)Bd.regResult.lenght;

            Assert.AreEqual(exepted, (int)Bd.Reg(messager, login, pass, "123456"));

        }

        [TestMethod]
        public void Reg_2_done()
        {

            string login = "123qwerty", pass = "12345678";
            var messager = Bd.messager.mess2;
            var exepted = (int)Bd.regResult.done;

            Assert.AreEqual(exepted, (int)Bd.Reg(messager, login, pass, "123456"));

        }

        [TestMethod]
        public void Reg_2_exist()
        {

            string login = "123456789", pass = "123456789";
            var messager = Bd.messager.mess2;
            var exepted = (int)Bd.regResult.exist;

            Assert.AreEqual(exepted, (int)Bd.Reg(messager, login, pass, "123456"));

        }

    }
}
