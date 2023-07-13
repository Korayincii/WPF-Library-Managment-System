using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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


 namespace My_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
            GetTable(this);
            RegisterOperation.main = this;
            LoginOperation.main = this;
            Combo.SelectedIndex = 0;
            adminlabel.Visibility = Visibility.Hidden;
            loginadminpass.Visibility = Visibility.Hidden;
            AdminLoginning.Visibility = Visibility.Hidden;
            AccountOptions.Visibility = Visibility.Hidden;
            LibraryTab.IsEnabled = false;
            AdminTab.IsEnabled = false;

            using (LibraryContext context = new LibraryContext())
            {

                context.TblBooks.OrderBy(pr => pr.BookId).Take(1010).Load();
                ViewBooksDataGrid.ItemsSource = context.TblBooks.Local.ToBindingList();
                context.TblSelectedBook.OrderBy(pr => pr.Email).Take(100).Load();
                ViewBooksInUse.ItemsSource = context.TblSelectedBook.Local.ToBindingList();
                context.TeacherApproving.OrderBy(pr => pr.TeacherName).Take(1000).Load();
                TeacherConfirmData.ItemsSource = context.TeacherApproving.Local.ToBindingList();
                context.TblGivingBack.OrderBy(pr => pr.Id).Take(200).Load();
                GivingBackData.ItemsSource = context.TblGivingBack.Local.ToBindingList();

            }

        }
        private void Registering_Click(object sender, RoutedEventArgs e)
        {
            RegisterOperation.RegisterStatus();
        }

        private void Loginning_Click(object sender, RoutedEventArgs e)
        {
            LoginOperation.LoginStatus();
        }

        public static void GetTable(MainWindow main)
        {
            using ( LibraryContext context = new LibraryContext())
            {

                context.TblBooks.OrderBy(pr => pr.BookId).Take(1010).Load();
                main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
            }
        }


        private void AdminLoginning_Click(object sender, RoutedEventArgs e)
        {
            LoginOperation.AdminLoginStatus();
        }

        private void AdminPanel_Click(object sender, RoutedEventArgs e)
        {
            adminlabel.Visibility = Visibility.Visible;
            loginadminpass.Visibility = Visibility.Visible;
            AdminLoginning.Visibility = Visibility.Visible;
        }

        private void SearchingButton_Click(object sender, RoutedEventArgs e)
        {
            GeneralCodes.Searching(this);
        }

        private void Filtering_Click(object sender, RoutedEventArgs e)
        {
            GeneralCodes.Filters(this);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            LibraryTab.IsEnabled = false;
            LoginBut.IsSelected = true;
            RegisterTab.IsEnabled = true;
            LoginBut.IsEnabled = true;
            logintext.Clear();
            loginpass.Clear();
            loginadminpass.Clear();
            RegisterNameText.Clear();
            RegisterMail.Clear();
            Pw1.Clear();
            Pw2.Clear();
            Combo.SelectedIndex = 0;
            Acname.Content = "Name & Surname : ";
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchCombo.SelectedIndex==3)
            {
                GeneralCodes.GetFastSearching(this);
            }
        }

        private void GettingSelectedBut_Click(object sender, RoutedEventArgs e)
        {
            GeneralCodes.GetSelectedBook(this);
            datagridBooks.Items.Refresh();

        }

        private void AccountInfos_Click(object sender, RoutedEventArgs e)
        {
            AccountOptions.IsSelected = true;
            GeneralCodes.GetBooksOnMe(this);
            
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GeneralCodes.Sorting(this);
        }

        private void GiveBackBook_Click(object sender, RoutedEventArgs e)
        {
            AdminOperations.GivingBackBook(this);
            GivingBackData.Items.Refresh();
        }

        private void ApproveBut_Click(object sender, RoutedEventArgs e)
        {
            AdminOperations.TeacherConfirms(this);
            TeacherConfirmData.Items.Refresh();
        }

        private void AddBookBut_Click(object sender, RoutedEventArgs e)
        {
            AdminOperations.AddingABook(this);
        }

        private void ApproveGivingBackButt_Click(object sender, RoutedEventArgs e)
        {
            AdminOperations.BookConfirms(this);
        }

        private void NotificationBut_Click(object sender, RoutedEventArgs e)
        {
            Notification NT = new Notification();
            NT.Show();
        }
    }
}

