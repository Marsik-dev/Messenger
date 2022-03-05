using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Messager
{
    public class Bd
    {

        static int id_first = -1;
        static int id_second = -1;

        public static int IdFirst => id_first;
        public static int IdSecond => id_second;

        public static void resetFirst() => id_first = -1;
        public static void resetSecond() => id_second = -1;

        public static bool SaveData = true;

        public static regResult Reg(messager messager, string login, string pass, string name)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            if (login.Length < 6 || pass.Length < 8 || name.Length < 6) return regResult.lenght;
            if (!new Regex("[0-9]").IsMatch(pass)) return regResult.symbols;

            switch (messager)
            {

                case messager.mess1:

                    if ((from user in mess1.User where user.login.Equals(login) select user).Count() != 0) return regResult.exist;

                    break;

                case messager.mess2:
                    
                    if ((from user in mess2.User where user.login.Equals(login) select user).Count() != 0) return regResult.exist;

                    break;

            }

            switch (messager)
            {

                case messager.mess1:

                    mess1.User.Add(new User()
                    {

                        name = name,
                        login = login,
                        pass = pass

                    });
                    if(SaveData) mess1.SaveChanges();

                    return regResult.done;

                case messager.mess2:

                    mess2.User.Add(new User()
                    {

                        name = name,
                        login = login,
                        pass = pass

                    });
                    if (SaveData) mess2.SaveChanges();

                    return regResult.done;

                default: return regResult.error;

            }

        }

        public static bool Auth(messager messager, string login, string pass)
        {

            if (login == null || pass == null) return false;

            switch (messager)
            {

                case messager.mess1:

                    {

                        Mess1 mess1 = new Mess1();

                        var result = from user in mess1.User where user.login.Equals(login) select user;
                        if (result.Count() == 0) return false;
                        id_first = result.First().id;
                        return result.First().pass.Equals(pass);

                    }

                case messager.mess2:

                    {

                        Mess2 mess2 = new Mess2();

                        var result = from user in mess2.User where user.login.Equals(login) select user;
                        if (result.Count() == 0) return false;
                        id_second = result.First().id;
                        return result.First().pass.Equals(pass);

                    }

                default: return false;

            }

        }

        public static User getUserMess1(int id) => (from user in new Mess1().User where user.id == id select user).FirstOrDefault();
        public static User getUserMess2(int id) => (from user in new Mess2().User where user.id == id select user).FirstOrDefault();

        public static List<User> getUserMess1(string name) => (from user in new Mess1().User where user.name.Contains(name) && user.id != id_first select user).ToList();
        public static List<User> getUserMess2(string name) => (from user in new Mess2().User where user.name.Contains(name) && user.id != id_second select user).ToList();

        public static List<User> getUserMess1() => (from user in new Mess1().User where user.id != id_first select user).ToList();
        public static List<User> getUserMess2() => (from user in new Mess2().User where user.id != id_second select user).ToList();

        public static int getDialogId1(int second)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var dialogsF = from d in mess1.Dialog where d.idUser == id_first select d;
            var dialogsS = (from d in mess1.Dialog where d.idUser == second select d).ToList();

            foreach(var dialog in dialogsF)
            {

                var res = dialogsS.Find(d => d.id == dialog.id);
                if (res != null) return res.id;

            }

            return -1;

        }

        public static int getDialogId2(int first)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var dialogsF = from d in mess2.Dialog where d.idUser == first select d;
            var dialogsS = (from d in mess2.Dialog where d.idUser == id_second select d).ToList();

            foreach (var dialog in dialogsF)
            {

                var res = dialogsS.Find(d => d.id == dialog.id);
                if (res != null) return res.id;

            }

            return -1;

        }

        public static List<MyDialog> getDialogs()
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var result = new List<MyDialog>();

            {

                var dialogs = from d in mess1.Dialog where d.idUser == id_first select d;

                foreach (var d in dialogs)
                {

                    var id_user = (from d1 in mess1.Dialog where d1.id == d.id && d1.idUser != id_first select d1).First().idUser;

                    result.Add(new MyDialog()
                    {

                        id_first = d.id,
                        name = (from u in mess1.User where u.id == id_user select u).First().name 

                    });

                }

            }

            {

                List<JsonSave.SaveObject> currentDialogs;

                try { currentDialogs = JsonSave.GetDialogs(); } catch (Exception e) { currentDialogs = new List<JsonSave.SaveObject>(); }

                var dialogs = from d in mess2.Dialog where d.idUser == id_second select d;

                foreach (var d in dialogs)
                {

                    var id_user = (from d1 in mess2.Dialog where d1.id == d.id && d1.idUser != id_second select d1).First().idUser;

                    var dialog = currentDialogs.Find(d1 => d1.id_second == d.id);
                    if (dialog != null)
                    {

                        var item = result.FindIndex(d1 => d1.id_first == dialog.id_first);
                        if (item != -1)
                        {

                            result[item].name += $" {(from u in mess2.User where u.id == id_user select u).First().name}";
                            result[item].id_second = d.id;

                        }

                    }
                    else
                    {

                        result.Add(new MyDialog()
                        {

                            id_second = d.id,
                            name = (from u in mess2.User where u.id == id_user select u).First().name

                        });

                    }

                }

            }

            return result;

        }

        public static List<MyMessage> getMessages(int id_first, int id_second)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var result = new List<MyMessage>();

            {

                var messages = from d in mess1.Message where d.idDialog == id_first select d.Object;

                foreach (var m in messages)
                {

                    result.Add(new MyMessage()
                    {

                        isUser = m.idUser == IdFirst,
                        text = m.Text.Count() != 0 ? m.Text.First().text1 : "",
                        image = m.Image.Count() != 0 ? m.Image.First().image1 : null,
                        time = m.time,
                        SenderName = m.User.name

                    });

                }

            }

            {

                var messages = from d in mess2.Message where d.idDialog == id_second select d.Object;

                foreach (var m in messages)
                {

                    result.Add(new MyMessage()
                    {

                        isUser = m.idUser == IdSecond,
                        text = m.Text.Count() != 0 ? m.Text.First().text1 : "",
                        image = m.Image.Count() != 0 ? m.Image.First().image1 : null,
                        time = m.time,
                        SenderName = m.User.name

                    });

                }

            }

            result.Sort();
            return result;

        }

        public static bool addSub1(int id)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            if ((from sub in mess1.Sub where sub.idUser == id_first && sub.idSub == id select sub).Count() != 0) return false;

            mess1.Sub.Add(new Sub()
            {

                idUser = id_first,
                idSub = id

            });

            if (SaveData) mess1.SaveChanges();

            return true;

        }

        public static bool addSub2(int id)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            if ((from sub in mess2.Sub where sub.idUser == id_first && sub.idSub == id select sub).Count() != 0) return false;

            mess2.Sub.Add(new Sub()
            {

                idUser = id_first,
                idSub = id

            });

            if (SaveData) mess2.SaveChanges();

            return true;

        }

        public static bool addFriend1(int id)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var dialogs1 = from d in mess1.Dialog where d.idUser == id_first select d;
            var dialogs2 = (from d in mess1.Dialog where d.idUser == id select d).ToList();
            foreach (var d in dialogs1) if (dialogs2.Find(dialog => dialog.id == d.id) != null) return false;

            var dialogId = mess1.Dialog.Count() != 0 ? (from d in mess1.Dialog select d.id).Max() + 1 : 0;

            mess1.Dialog.Add(new Dialog()
            {

                id = dialogId,
                idUser = id_first

            });

            mess1.Dialog.Add(new Dialog()
            {

                id = dialogId,
                idUser = id

            });

            try
            {

                if (SaveData) mess1.SaveChanges();

            }
            catch (Exception e) { return false; }

            return true;

        }

        public static bool addFriend2(int id)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var dialogs1 = from d in mess2.Dialog where d.idUser == id_first select d;
            var dialogs2 = (from d in mess2.Dialog where d.idUser == id select d).ToList();
            foreach (var d in dialogs1) if (dialogs2.Find(dialog => dialog.id == d.id) != null) return false;

            var dialogId = mess2.Dialog.Count() != 0 ? (from d in mess2.Dialog select d.id).Max() + 1 : 0;

            mess2.Dialog.Add(new Dialog()
            {

                id = dialogId,
                idUser = id_second

            });

            mess2.Dialog.Add(new Dialog()
            {

                id = dialogId,
                idUser = id

            });

            try
            {

                if (SaveData) mess2.SaveChanges();

            }
            catch (Exception e) { return false; }

            return true;

        }

        public static void Send(MyDialog dialog, String text)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var obj = new Object()
            {

                time = DateTime.UtcNow

            };

            var txt = new Text()
            {

                id = obj.id,
                text1 = text

            };

            var mess = new Message()
            {

                id = obj.id

            };

            if (dialog.id_first != 0 && dialog.id_second == 0)
            {

                obj.idUser = id_first;
                mess.idDialog = dialog.id_first;

                mess1.Object.Add(obj);
                mess1.Text.Add(txt);
                mess1.Message.Add(mess);

                if (SaveData) mess1.SaveChanges();

                return;

            }

            if (dialog.id_second != 0 && dialog.id_first == 0)
            {

                obj.idUser = id_second;
                mess.idDialog = dialog.id_second;

                mess2.Object.Add(obj);
                mess2.Text.Add(txt);
                mess2.Message.Add(mess);

                if (SaveData) mess2.SaveChanges();

                return;

            }

            var res = MessageBox.Show("Отправить это сообщение через первый мессенджер?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if(res == MessageBoxResult.Yes)
            {

                obj.idUser = id_first;
                mess.idDialog = dialog.id_first;

                mess1.Object.Add(obj);
                mess1.Text.Add(txt);
                mess1.Message.Add(mess);

                if (SaveData) mess1.SaveChanges();

            }
            else
            {

                obj.idUser = id_second;
                mess.idDialog = dialog.id_second;

                mess2.Object.Add(obj);
                mess2.Text.Add(txt);
                mess2.Message.Add(mess);

                if (SaveData) mess2.SaveChanges();

            }

        }

        public static void Send(String text)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var obj = new Object()
            {

                time = DateTime.UtcNow

            };

            var txt = new Text()
            {

                id = obj.id,
                text1 = text

            };

            var res = MessageBox.Show("Отправить эту новость через первый мессенджер?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (res == MessageBoxResult.Yes)
            {

                obj.idUser = id_first;

                mess1.Object.Add(obj);
                mess1.Text.Add(txt);

                if (SaveData) mess1.SaveChanges();

            }
            else
            {

                obj.idUser = id_second;

                mess2.Object.Add(obj);
                mess2.Text.Add(txt);

                if (SaveData) mess2.SaveChanges();

            }

        }

        public static List<MyMessage> getNews()
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var result = new List<MyMessage>();

            {
                
                var subs = from s in mess1.Sub where s.idUser == id_first select s;

                foreach (var s in subs)
                {

                    var messages = from d in mess1.Object where d.idUser == s.idSub select d;

                    foreach (var m in messages)
                    {

                        result.Add(new MyMessage()
                        {

                            isUser = m.idUser == IdFirst,
                            text = m.Text.Count() != 0 ? m.Text.First().text1 : "",
                            image = m.Image.Count() != 0 ? m.Image.First().image1 : null,
                            time = m.time,
                            SenderName = m.User.name

                        });

                    }

                }

            }

            {

                var subs = from s in mess2.Sub where s.idUser == id_second select s;

                foreach (var s in subs)
                {

                    var messages = from d in mess2.Object where d.idUser == s.idSub select d;

                    foreach (var m in messages)
                    {

                        result.Add(new MyMessage()
                        {

                            isUser = m.idUser == IdSecond,
                            text = m.Text.Count() != 0 ? m.Text.First().text1 : "",
                            image = m.Image.Count() != 0 ? m.Image.First().image1 : null,
                            time = m.time,
                            SenderName = m.User.name

                        });

                    }

                }

            }

            result.Sort();
            return result;

        }

        public static void Send(MyDialog dialog, byte[] image)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var obj = new Object()
            {

                time = DateTime.UtcNow

            };

            var img = new Image()
            {

                id = obj.id,
                image1 = image

            };

            var mess = new Message()
            {

                id = obj.id

            };

            if (dialog.id_first != 0 && dialog.id_second == 0)
            {

                obj.idUser = id_first;
                mess.idDialog = dialog.id_first;

                mess1.Object.Add(obj);
                mess1.Image.Add(img);
                mess1.Message.Add(mess);

                if (SaveData) mess1.SaveChanges();

                return;

            }

            if (dialog.id_second != 0 && dialog.id_first == 0)
            {

                obj.idUser = id_second;
                mess.idDialog = dialog.id_second;

                mess2.Object.Add(obj);
                mess2.Image.Add(img);
                mess2.Message.Add(mess);

                if (SaveData) mess2.SaveChanges();

                return;

            }

            var res = MessageBox.Show("Отправить это сообщение через первый мессенджер?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (res == MessageBoxResult.Yes)
            {

                obj.idUser = id_first;
                mess.idDialog = dialog.id_first;

                mess1.Object.Add(obj);
                mess1.Image.Add(img);
                mess1.Message.Add(mess);

                if (SaveData) mess1.SaveChanges();

            }
            else
            {

                obj.idUser = id_second;
                mess.idDialog = dialog.id_second;

                mess2.Object.Add(obj);
                mess2.Image.Add(img);
                mess2.Message.Add(mess);

                if (SaveData) mess2.SaveChanges();

            }

        }

        public static void Send(byte[] image)
        {

            Mess1 mess1 = new Mess1();
            Mess2 mess2 = new Mess2();

            var obj = new Object()
            {

                time = DateTime.UtcNow

            };

            var img = new Image()
            {

                id = obj.id,
                image1 = image

            };

            var res = MessageBox.Show("Отправить эту новость через первый мессенджер?", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (res == MessageBoxResult.Yes)
            {

                obj.idUser = id_first;

                mess1.Object.Add(obj);
                mess1.Image.Add(img);

                if (SaveData) mess1.SaveChanges();

            }
            else
            {

                obj.idUser = id_second;

                mess2.Object.Add(obj);
                mess2.Image.Add(img);

                if (SaveData) mess2.SaveChanges();

            }

        }

        public enum messager
        {

            mess1,
            mess2

        }

        public enum regResult
        {

            error,
            exist,
            lenght,
            symbols,
            done

        }

    }
}
