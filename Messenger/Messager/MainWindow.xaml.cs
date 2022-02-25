using Messager.Pages;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
namespace Messager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static Frame MainPage;
        static Reg reg1;
        static Auth auth1;
        static Reg reg2;
        static Auth auth2;
        static Choose choose;
        static Main main;

        public MainWindow()
        {

            InitializeComponent();
            MainPage = mainPage;

            reg1 = new Reg(Bd.messager.mess1);
            auth1 = new Auth(Bd.messager.mess1);
            reg2 = new Reg(Bd.messager.mess2);
            auth2 = new Auth(Bd.messager.mess2);
            choose = new Choose();
            main = new Main();

            ChangePage(pagesType.choose);

        }

        public static void ChangePage(pagesType type)
        {

            switch (type)
            {

                case pagesType.choose:

                    MainPage.Navigate(choose);
                    choose.update();

                    break;

                case pagesType.auth1:

                    MainPage.Navigate(auth1);

                    break;

                case pagesType.reg1:

                    MainPage.Navigate(reg1);

                    break;

                case pagesType.auth2:

                    MainPage.Navigate(auth2);

                    break;

                case pagesType.reg2:

                    MainPage.Navigate(reg2);

                    break;

                case pagesType.main:

                    MainPage.Navigate(main);

                    break;

            }

        }

        public enum pagesType
        {

            choose,
            reg1,
            auth1,
            reg2,
            auth2,
            main

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            main.thread.Abort();

        }

    }
}
