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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messager.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth(Bd.messager messager)
        {

            InitializeComponent();

            authButton.Click += (s, e) =>
            {

                if (Bd.Auth(messager, login.Text, pass.Password)) MainWindow.ChangePage(MainWindow.pagesType.choose);
                else MessageBox.Show("Неверный логин или пароль");

            };

            toRegButton.Click += (s, e) =>
            {

                switch (messager)
                {

                    case Bd.messager.mess1:

                        MainWindow.ChangePage(MainWindow.pagesType.reg1);

                        break;

                    case Bd.messager.mess2:

                        MainWindow.ChangePage(MainWindow.pagesType.reg2);

                        break;

                }

            };

            back.Click += (s, e) =>
            {

                MainWindow.ChangePage(MainWindow.pagesType.choose);

            };

        }
    }
}
