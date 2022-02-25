using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messager
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {

            InitializeComponent();

            news.Checked += (s, e) =>
            {

                mess1.IsChecked = false;
                mess2.IsChecked = false;

            };

            mess1.Checked += (s, e) =>
            {

                search_first.IsEnabled = (bool)mess1.IsChecked;
                first.IsEnabled = (bool)mess1.IsChecked;

                search_second.IsEnabled = (bool)news.IsChecked ? !(bool)mess1.IsChecked : (bool)mess2.IsChecked;
                second.IsEnabled = (bool)news.IsChecked ? !(bool)mess1.IsChecked : (bool)mess2.IsChecked;

            };

            mess2.Checked += (s, e) =>
            {

                search_first.IsEnabled = (bool)news.IsChecked ? !(bool)mess2.IsChecked : (bool)mess1.IsChecked;
                first.IsEnabled = (bool)news.IsChecked ? !(bool)mess2.IsChecked : (bool)mess1.IsChecked;

                search_second.IsEnabled = (bool)mess2.IsChecked;
                second.IsEnabled = (bool)mess2.IsChecked;

            };

            search.Click += (s, e) =>
            {

                first.Items.Clear();
                second.Items.Clear();

                if ((bool)mess1.IsChecked)
                {

                    List<User> list;

                    if (search_first.Text.Equals("")) list = Bd.getUserMess1();
                    else list = Bd.getUserMess1(search_first.Text);

                    foreach(var l in list)
                    {

                        var item = new ListBoxItem();
                        item.Content = l.name;
                        item.DataContext = l;
                        first.Items.Add(item);

                    }

                }

                if ((bool)mess2.IsChecked)
                {

                    List<User> list;

                    if (search_second.Text.Equals("")) list = Bd.getUserMess2();
                    else list = Bd.getUserMess2(search_second.Text);

                    foreach (var l in list)
                    {

                        var item = new ListBoxItem();
                        item.Content = l.name;
                        item.DataContext = l;
                        second.Items.Add(item);

                    }

                }

            };

            add.Click += (s, e) =>
            {

                if ((bool)news.IsChecked)
                {

                    if ((bool)mess1.IsChecked)
                    {

                        if(first.SelectedItem == null) { MessageBox.Show("Выберите пользователя"); return; }

                        var item = (ListBoxItem)first.SelectedItem;
                        var user = (User)item.DataContext;

                        if (Bd.addSub1(user.id)) MessageBox.Show("Все прошло успешно!");
                        else MessageBox.Show("Вы уже подписаны на " + user.name);

                    }
                    else
                    {

                        if ((bool)mess2.IsChecked)
                        {

                            if (second.SelectedItem == null) { MessageBox.Show("Выберите пользователя"); return; }

                            var item = (ListBoxItem)second.SelectedItem;
                            var user = (User)item.DataContext;

                            if (Bd.addSub2(user.id)) MessageBox.Show("Все прошло успешно!");
                            else MessageBox.Show("Вы уже подписаны на " + user.name);

                        }
                        else MessageBox.Show("Выберите один из мессенджеров");

                    }

                }
                else
                {

                    if ((bool)friends.IsChecked)
                    {

                        if ((bool)mess1.IsChecked)
                        {

                            if (first.SelectedItem == null) { MessageBox.Show("Выберите пользователя"); return; }

                            var item = (ListBoxItem)first.SelectedItem;
                            var user = (User)item.DataContext;

                            if (Bd.addFriend1(user.id)) MessageBox.Show($"Пользователь {user.name} успешно добавлен в друзья!");
                            else MessageBox.Show($"Пользователь {user.name} уже находится в друзьях");

                        }

                        if ((bool)mess2.IsChecked)
                        {

                            if (second.SelectedItem == null) { MessageBox.Show("Выберите пользователя"); return; }

                            var item = (ListBoxItem)second.SelectedItem;
                            var user = (User)item.DataContext;

                            if (Bd.addFriend2(user.id)) MessageBox.Show($"Пользователь {user.name} успешно добавлен в друзья!");
                            else MessageBox.Show($"Пользователь {user.name} уже находится в друзьях");

                        }

                        if((bool)mess2.IsChecked && (bool)mess1.IsChecked)
                        {

                            var item1 = (ListBoxItem)first.SelectedItem;
                            var user1 = (User)item1.DataContext;

                            var item2 = (ListBoxItem)second.SelectedItem;
                            var user2 = (User)item2.DataContext;

                            //не id user а id диалога

                            if (JsonSave.SaveDialog(Bd.getDialogId1(user1.id), Bd.getDialogId2(user2.id))) MessageBox.Show("Общий чат успешно создан!");
                            else MessageBox.Show("Пользователь уже находится в общем чате");

                        }

                        if(!(bool)mess2.IsChecked && !(bool)mess1.IsChecked) MessageBox.Show("Выберите хотя бы один из мессенджеров");

                    }
                    else MessageBox.Show("Выберите куда добавлять пользователя");

                }

            };

        }
    }
}
