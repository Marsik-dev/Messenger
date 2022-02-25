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
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Page
    {
        public Reg(Bd.messager messager)
        {

            InitializeComponent();

            regButton.Click += (s, e) =>
            {

                switch (Bd.Reg(messager, login.Text, pass.Password, name.Text))
                {

                    case Bd.regResult.done:

                        MainWindow.ChangePage(MainWindow.pagesType.choose);

                        break;

                    case Bd.regResult.exist:

                        MessageBox.Show("Аккаунт с данным логином уже существует");

                        break;

                    case Bd.regResult.symbols:

                        MessageBox.Show("Пароль должен содержать символы и цифры");

                        break;

                    case Bd.regResult.lenght:

                        MessageBox.Show("Логин и имя должены состоять более чем из 6 символов\nПароль должен состоять более чем из 8 символов");

                        break;

                    case Bd.regResult.error:

                        MessageBox.Show("Произошла неизвестная ошибка. Обратитесь к разработчику приложения");

                        break;

                }

            };

            toAuthButton.Click += (s, e) =>
            {

                switch (messager)
                {

                    case Bd.messager.mess1:

                        MainWindow.ChangePage(MainWindow.pagesType.auth1);

                        break;

                    case Bd.messager.mess2:

                        MainWindow.ChangePage(MainWindow.pagesType.auth2);

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
