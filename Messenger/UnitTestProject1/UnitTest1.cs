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

        [TestCategory("Auth")] 
        [TestMethod]
        public void Auth_1_true()
        {

            string login = "123456789", pass = "123456789";
            var messager = Bd.messager.mess1;
            bool exepted = true;

            Assert.AreEqual(exepted, Bd.Auth(messager, login, pass));

        }

        [TestCategory("Auth")]
        [TestMethod]
        public void Auth_1_false()
        {

            string login = "qwerty123", pass = "123123123";
            var messager = Bd.messager.mess1;
            bool exepted = false;

            Assert.AreEqual(exepted, Bd.Auth(messager, login, pass));

        }

        [TestCategory("Auth")]
        [TestMethod]
        public void Auth_2_true()
        {

            string login = "12345678", pass = "12345678";
            var messager = Bd.messager.mess2;
            bool exepted = true;

            Assert.AreEqual(exepted, Bd.Auth(messager, login, pass));

        }

        [TestCategory("Auth")]
        [TestMethod]
        public void Auth_2_false()
        {

            string login = "qwerty", pass = "qwerty123";
            var messager = Bd.messager.mess2;
            bool exepted = false;

            Assert.AreEqual(exepted, Bd.Auth(messager, login, pass));

        }

        [TestCategory("Reg")]
        [TestMethod]
        public void Reg_1_done()
        {

            string login = "qwerty123qwe", pass = "qwerty123";
            var messager = Bd.messager.mess1;
            var exepted = (int)Bd.regResult.done;

            Assert.AreEqual(exepted, (int)Bd.Reg(messager, login, pass, "123456"));

        }

        [TestCategory("Reg")]
        [TestMethod]
        public void Reg_1_lenght()
        {

            string login = "123", pass = "456";
            var messager = Bd.messager.mess1;
            var exepted = (int)Bd.regResult.lenght;

            Assert.AreEqual(exepted, (int)Bd.Reg(messager, login, pass, "123456"));

        }

        [TestCategory("Reg")]
        [TestMethod]
        public void Reg_2_done()
        {

            string login = "123qwerty", pass = "12345678";
            var messager = Bd.messager.mess2;
            var exepted = (int)Bd.regResult.done;

            Assert.AreEqual(exepted, (int)Bd.Reg(messager, login, pass, "123456"));

        }

        [TestCategory("Reg")]
        [TestMethod]
        public void Reg_2_exist()
        {

            string login = "123456789", pass = "123456789";
            var messager = Bd.messager.mess2;
            var exepted = (int)Bd.regResult.exist;

            Assert.AreEqual(exepted, (int)Bd.Reg(messager, login, pass, "123456"));

        }

        [TestCategory("Get user")]
        [TestMethod]
        public void getUser_1_123456789()
        {

            var id = 6;
            var result = "123456789";

            var user = Bd.getUserMess1(id);

            Assert.AreEqual(result, user.name);

        }

        [TestCategory("Get user")]
        [TestMethod]
        public void getUser_1_null()
        {

            var id = -1;

            var user = Bd.getUserMess1(id);

            Assert.AreEqual(null, user);

        }

        [TestCategory("Get user")]
        [TestMethod]
        public void getUser_2_123456789()
        {

            var id = 6;
            var result = "123456789";

            var user = Bd.getUserMess2(id);

            Assert.AreEqual(result, user.name);

        }

        [TestCategory("Get user")]
        [TestMethod]
        public void getUser_2_null()
        {

            var id = -1;

            var user = Bd.getUserMess2(id);

            Assert.AreEqual(null, user);

        }

        [TestCategory("Get all users")]
        [TestMethod]
        public void getUser_1_all()
        {

            var users = Bd.getUserMess1();

            Assert.IsTrue(users.Count > 0);

        }

        [TestCategory("Get all users")]
        [TestMethod]
        public void getUser_2_all()
        {

            var users = Bd.getUserMess2();

            Assert.IsTrue(users.Count > 0);

        }

        [TestCategory("Get all dialogs")]
        [TestMethod]
        public void GetDialogs_123456789_all()
        {

            {

                string login = "123456789", pass = "123456789";
                var messager = Bd.messager.mess1;

                if (!Bd.Auth(messager, login, pass)) Assert.Fail();

            }

            {

                string login = "123456789", pass = "123456789";
                var messager = Bd.messager.mess2;

                if (!Bd.Auth(messager, login, pass)) Assert.Fail();

            }

            var dialogs = Bd.getDialogs();

            Assert.IsTrue(dialogs.Count > 0);

        }

        [TestCategory("Get all dialogs")]
        [TestMethod]
        public void GetDialogs_12345678_all()
        {

            {

                string login = "12345678", pass = "12345678";
                var messager = Bd.messager.mess1;

                if (!Bd.Auth(messager, login, pass)) Assert.Fail();

            }

            {

                string login = "12345678", pass = "12345678";
                var messager = Bd.messager.mess2;

                if (!Bd.Auth(messager, login, pass)) Assert.Fail();

            }

            var dialogs = Bd.getDialogs();

            Assert.IsTrue(dialogs.Count > 0);

        }

    }
}
