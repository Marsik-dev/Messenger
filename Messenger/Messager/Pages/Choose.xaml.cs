using System.Windows.Controls;

namespace Messager.Pages
{
    /// <summary>
    /// Логика взаимодействия для Choose.xaml
    /// </summary>
    public partial class Choose : Page
    {

        public void update()
        {

            if (Bd.IdFirst != -1)
            {
             
                mess1.Text = Bd.getUserMess1(Bd.IdFirst).name;
                mess1Auth.Content = "Выйти";

            }
            else
            {

                mess1.Text = "Мессенджер1";
                mess1Auth.Content = "Войти";

            }

            if (Bd.IdSecond != -1)
            {
             
                mess2.Text = Bd.getUserMess2(Bd.IdSecond).name;
                mess2Auth.Content = "Выйти";

            }
            else
            {

                mess2.Text = "Мессенджер2";
                mess2Auth.Content = "Войти";

            }

            auth.IsEnabled = Bd.IdFirst != -1 && Bd.IdSecond != -1;

        }

        public Choose()
        {

            InitializeComponent();

            mess1Auth.Click += (s, e) =>
            {

                if (Bd.IdFirst == -1)
                    MainWindow.ChangePage(MainWindow.pagesType.auth1);
                else
                {

                    Bd.resetFirst();
                    update();

                }

            };

            mess2Auth.Click += (s, e) =>
            {

                if (Bd.IdSecond == -1)
                    MainWindow.ChangePage(MainWindow.pagesType.auth2);
                else
                {

                    Bd.resetSecond();
                    update();

                }

            };

            auth.Click += (s, e) =>
            {

                JsonSave.Auth(Bd.IdFirst, Bd.IdSecond);
                MainWindow.ChangePage(MainWindow.pagesType.main);

            };

        }
    }
}
