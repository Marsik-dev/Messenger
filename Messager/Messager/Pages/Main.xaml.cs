using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Messager.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {

        public Thread thread;
        static bool isUpdate = true;

        public Main()
        {

            InitializeComponent();

            StandartUserRow = new GridLength(50);

            thread = new Thread(() =>
            {

                while (true)
                {

                    if (isUpdate)
                    {

                        isUpdate = false;

                        UpdateUsers();
                        UpdateMessages();

                    }

                    Thread.Sleep(500);

                }

            });
            thread.IsBackground = true;
            thread.Start();

            Add.MouseUp += (s, e) =>
            {

                new AddUser().Show();

            };

            News.MouseUp += (s, e) =>
            {

                current = null;
                UpdateNews();

            };

            Send.Click += (s, e) =>
            {

                if (Text.Text.Equals("")) { MessageBox.Show("Введите текст"); return; }

                if (current != null) Bd.Send(current, Text.Text);
                else Bd.Send(Text.Text);

                Text.Text = "";

            };

            Attach.Click += (s, e) =>
            {

                var fileDialog = new OpenFileDialog();
                fileDialog.Filter = "PNG images (*.png)|*.png|JPEG images (*.jpeg)|*.jpeg";

                if (fileDialog.ShowDialog() == true)
                {

                    using (var stream = new FileStream(fileDialog.FileName, FileMode.Open))
                    {

                        var mem = new MemoryStream();

                        var img = new byte[stream.Length];

                        stream.Read(img, 0, (int)stream.Length);

                        if (current != null) Bd.Send(current, img);
                        else Bd.Send(img);

                    }

                }

            };

        }

        GridLength StandartUserRow;

        MyDialog current;

        public void UpdateUsers()
        {

            var list = Bd.getDialogs();

            Dispatcher.Invoke(() =>
            {

                if (UserList.Children.Count > 2)
                {

                    UserList.Children.RemoveRange(2, UserList.Children.Count - 2);
                    UserList.RowDefinitions.RemoveRange(2, UserList.RowDefinitions.Count - 2);

                }

                int i = 2;

                foreach (var user in list)
                {

                    var text = new TextBlock();
                    text.Text = user.name;
                    text.Padding = new Thickness(15);
                    text.FontSize = 18;
                    text.TextAlignment = TextAlignment.Center;

                    text.MouseUp += (s, e) =>
                    {

                        current = user;
                        UpdateMessages(user);

                    };

                    Grid.SetRow(text, i);
                    i++;

                    var row = new RowDefinition();
                    row.Height = StandartUserRow;

                    UserList.Children.Add(text);
                    UserList.RowDefinitions.Add(row);

                }

            });

        }

        void UpdateMessages()
        {

            if (current != null) UpdateMessages(current);
            else UpdateNews();

        }

        void UpdateMessages(MyDialog dialog)
        {

            var list = Bd.getMessages(dialog.id_first, dialog.id_second);

            Dispatcher.Invoke(() =>
            {

                if (current == null)
                {

                    isUpdate = true;
                    return;

                }

                MessageList.Children.Clear();
                MessageList.RowDefinitions.Clear();

                title.Text = current == null ? "" : current.name;

                int i = 0;

                foreach (var mess in list)
                {

                    var name = new TextBlock();
                    name.Text = $"{mess.SenderName} ({mess.time.ToLocalTime().ToShortDateString()}, {mess.time.ToLocalTime().ToShortTimeString()})";
                    name.Padding = new Thickness(5);
                    name.FontSize = 14;
                    name.TextAlignment = TextAlignment.Center;

                    name.HorizontalAlignment = mess.isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;

                    Grid.SetRow(name, i);

                    MessageList.Children.Add(name);
                    MessageList.RowDefinitions.Add(new RowDefinition());

                    i++;

                    if (!mess.text.Equals(""))
                    {

                        var text = new TextBlock();
                        text.Text = mess.text;
                        text.Padding = new Thickness(5);
                        text.FontSize = 18;
                        text.TextAlignment = TextAlignment.Center;

                        text.HorizontalAlignment = mess.isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;

                        Grid.SetRow(text, i);


                        MessageList.Children.Add(text);
                        MessageList.RowDefinitions.Add(new RowDefinition());

                        i++;

                    }

                    if (mess.image != null)
                    {

                        var image = new System.Windows.Controls.Image();
                        var img = new BitmapImage();
                        img.BeginInit();
                        img.StreamSource = new MemoryStream(mess.image);
                        img.EndInit();
                        image.Source = img;
                        image.Margin = new Thickness(15, 5, 15, 5);

                        image.HorizontalAlignment = mess.isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;

                        Grid.SetRow(image, i);


                        MessageList.Children.Add(image);
                        MessageList.RowDefinitions.Add(new RowDefinition());

                        i++;

                    }

                    MessageList.RowDefinitions.Add(new RowDefinition());
                    i++;

                }

                isUpdate = true;

            });

        }

        void UpdateNews()
        {

            var list = Bd.getNews();

            Dispatcher.Invoke(() =>
            {

                if (current != null)
                {

                    isUpdate = true;
                    return;

                }

                MessageList.Children.Clear();
                MessageList.RowDefinitions.Clear();
                title.Text = "Новости";


                int i = 0;

                foreach (var mess in list)
                {

                    var name = new TextBlock();
                    name.Text = $"{mess.SenderName} ({mess.time.ToLocalTime().ToShortTimeString()})";
                    name.Padding = new Thickness(5);
                    name.FontSize = 14;
                    name.TextAlignment = TextAlignment.Center;

                    name.HorizontalAlignment = mess.isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;

                    Grid.SetRow(name, i);

                    MessageList.Children.Add(name);
                    MessageList.RowDefinitions.Add(new RowDefinition());

                    i++;

                    if (!mess.text.Equals(""))
                    {

                        var text = new TextBlock();
                        text.Text = mess.text;
                        text.Padding = new Thickness(5);
                        text.FontSize = 18;
                        text.TextAlignment = TextAlignment.Center;

                        text.HorizontalAlignment = mess.isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;

                        Grid.SetRow(text, i);

                        MessageList.Children.Add(text);
                        MessageList.RowDefinitions.Add(new RowDefinition());

                        i++;

                    }

                    if (mess.image != null)
                    {

                        var image = new System.Windows.Controls.Image();
                        var img = new BitmapImage();
                        img.BeginInit();
                        img.StreamSource = new MemoryStream(mess.image);
                        img.EndInit();
                        image.Source = img;

                        image.HorizontalAlignment = mess.isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left;

                        Grid.SetRow(image, i);
                        Dispatcher.Invoke(() =>
                        {
                            MessageList.Children.Add(image);
                            MessageList.RowDefinitions.Add(new RowDefinition());
                        });

                        i++;

                    }

                    MessageList.RowDefinitions.Add(new RowDefinition());
                    i++;

                }

                isUpdate = true;

            });

        }

    }
}
