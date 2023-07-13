using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace My_Project
{
    public static class GeneralCodes
    {
        public static MainWindow main;

        //In this method, we do renting book and it will save on table which is for private info table on sql server.
        public static void GetSelectedBook(MainWindow main)
        {
            using (LibraryContext context = new LibraryContext())
            {
                TblSelectedBook SelectedBook = new TblSelectedBook();
                TblBooks book = new TblBooks();
               
                //Here, when someone is loginned the system and when clicked the button for to view the books that the user have but user can see only her/him book not someone else's.
                context.TblBooks.OrderBy(pr => pr.BookId).Take(1010).Load();
                var SelectedBookUser = context.TblUsers.FirstOrDefault(pr => pr.Email == LoginOperation.logininfo); 
                var selectedItems = main.datagridBooks.SelectedItems.Cast<TblBooks>().ToList();
                var BookCount = context.TblSelectedBook.Where(pr => pr.Email == LoginOperation.srEmail).ToList();
                
                //If there is a book the user wants, user can give that book and if the user is student, user won't get more than 5 books

                if (main.datagridBooks.SelectedIndex>0)
                {
                    
                   //If the user decide to giving a book from library, it will save on the table that is for who rented the book and if this user is student, he or she has to give it back up to 7 days later
                    foreach(var Items in selectedItems)
                    {
                        var selectedbook = context.TblBooks.FirstOrDefault(pr => pr.Name == Items.Name);

                        if (Items.Number != 0)
                        {
                            SelectedBook.Email = SelectedBookUser.Email;
                            SelectedBook.UserType = SelectedBookUser.UserType;
                            SelectedBook.WhichBook = Items.Name;
                            SelectedBook.Author = Items.Author;
                            SelectedBook.LeasedDate = DateTime.Now;

                            if (SelectedBookUser.UserType == 1)
                            {
                                
                              SelectedBook.RestitutionDate = DateTime.Now.AddDays(7);
                                if (BookCount.Count() >= 5)
                                {
                                    MessageBox.Show("This user cannot have more than 5 books");
                                    return;
                                }
                            }
                            //But if the user is teacher, there is no problem about giving it back, teacher can give it back 999 days later or lets say after more than 2 years
                            else
                            {
                                SelectedBook.RestitutionDate = DateTime.Now.AddDays(999);
                            }
                            //And when someone gives a book, the number of available books in the library will decrease!
                            selectedbook.Number = selectedbook.Number - 1;
                            main.datagridBooks.Items.Refresh();
                        }       
                        
                        else
                        {
                            MessageBox.Show("This book not available");
                        }

                        try
                        {
                            context.TblSelectedBook.Add(SelectedBook);
                            main.DataAccountInfos.Items.Refresh();
                            context.SaveChanges();

                        }
                        catch (Exception E)
                        {
                            MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                            return;
                        }

                    }
                    
                }

            }
        }

        //This method is for showing table which includes books on sql server
        public static void GetBooksOnMe(MainWindow main)
        {
            using (LibraryContext context = new LibraryContext())
            {

                context.TblSelectedBook.Where(pr => pr.Email == LoginOperation.srEmail).OrderBy(pr => pr.SelectedBookId).Take(1000).Load();
                main.DataAccountInfos.ItemsSource = context.TblSelectedBook.Local.ToBindingList();
                main.DataAccountInfos.Items.Refresh();

            }

        }

        
        public static void Searching(MainWindow main)
        {
            using (LibraryContext context = new LibraryContext())
            {
                //For searching, it will be similar with quicksearching, but the different thing quick search is an instant action
                string Search = main.SearchText.Text;

                switch (main.SearchCombo.SelectedIndex)
                {
                    case 0:
                        context.TblBooks.Where(pr => pr.Name.Contains(Search)).OrderByDescending(pr => pr.Name).Take(1010).Load();
                        break;
                    case 1:
                        context.TblBooks.Where(pr => pr.Genre.Contains(Search)).OrderByDescending(pr => pr.Genre).Take(1010).Load();
                        break;
                    case 2:
                        context.TblBooks.Where(pr => pr.Author.Contains(Search)).OrderByDescending(pr => pr.Author).Take(1010).Load();
                        break;
                    default:
                        break;
                }

                main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                main.datagridBooks.Items.Refresh();


            }
        }
   
        public static void Filters(MainWindow main)
        {
            using (LibraryContext context = new LibraryContext())
            {
                //For filtering : If book number is 0, it will hide from library panel so people can eaisly see that which book is available
                if (main.Filtering.IsChecked == true)
                {
                    context.TblBooks.Where(pr => !pr.Number.ToString().Contains("0")).OrderByDescending(pr => pr.BookId).Take(1010).Load();
                    main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList().Reverse();
                    main.datagridBooks.Items.Refresh();
                }

                if (main.Filtering.IsChecked == false)
                {
                    context.TblBooks.OrderBy(pr => pr.BookId).Take(1010).Load();
                    main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                    main.datagridBooks.Items.Refresh();

                }
            }
        }

        public static void Sorting(MainWindow main)
        {
            //For Sorting: It is one of the useful thing while searching. User can sort books by book name or author or genre
            using (LibraryContext context = new LibraryContext())
            {

                switch (main.SortCombo.SelectedIndex)
                {
                    case 0:
                        context.TblBooks.OrderBy(pr => pr.BookId).Take(1010).Load();
                        main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                        main.datagridBooks.Items.Refresh();
                        break;
                    case 1:
                        context.TblBooks.OrderBy(pr => pr.Name).Take(1010).Load();
                        main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                        main.datagridBooks.Items.Refresh();
                        break;
                    case 2:
                        context.TblBooks.OrderBy(pr => pr.Genre).Take(1010).Load();
                        main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                        main.datagridBooks.Items.Refresh();
                        break;
                    case 3:
                        context.TblBooks.OrderBy(pr => pr.Author).Take(1010).Load();
                        main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                        main.datagridBooks.Items.Refresh();
                        break;
                    default:
                        break;
                }

                main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList();
                main.datagridBooks.Items.Refresh();


            }
        }

        public static void GetFastSearching(MainWindow main)
        {
            //For fastsearhing, program is checking that when something is written on textbox, it will show us all the things whick has same letter or word in items and also it will sort
            using (LibraryContext context = new LibraryContext())
            {
                string QuickSearch = main.SearchText.Text;
                context.TblBooks.Where(pr => pr.Name.Contains(QuickSearch)).OrderByDescending(pr => pr.BookId).Take(1010).Load();
                main.datagridBooks.ItemsSource = context.TblBooks.Local.ToBindingList().Reverse();
                main.datagridBooks.Items.Refresh();
            }

        }

        public static void Notfics(MainWindow main)
        {
            using (LibraryContext context = new LibraryContext())
            {

                TblUsers usernotf = new TblUsers();
                TblSelectedBook bookdate = new TblSelectedBook();
                BkNotifications notf = new BkNotifications();

                if (usernotf.UserType == 1)
                {
                    if (bookdate.LeasedDate >= bookdate.LeasedDate.AddDays(8))
                    {
                        MessageBox.Show("There are books that you have to resitutate!");
                        notf.BkName = bookdate.WhichBook;
                        notf.BkAuthor = bookdate.Author;
                        notf.BkLeasedDate = bookdate.LeasedDate;

                        context.BkNotifications.Add(notf);
                        context.SaveChanges();

                    }

                }

            }
        }
        public static void getnotification(Notification noti)
        {
            using (LibraryContext context = new LibraryContext())
            {
                context.BkNotifications.Where(pr => pr.Email == LoginOperation.logininfo).FirstOrDefault();
                noti.DatagridNotification.ItemsSource = context.BkNotifications.Local.ToBindingList();
                noti.DatagridNotification.Items.Refresh();
            }
        }


        //When I saw this computing, I searched on google and I got more infos from here
        //https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        public static string ComputeSha256Hash(this string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
