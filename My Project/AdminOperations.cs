using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace My_Project
{
    public static class AdminOperations
    {
        public static MainWindow main;

        public static void TeacherConfirms(MainWindow main)
        {
            //In here : After teacher registered the system, his or her infos will be send on admin's teacher confirm panel and admin can give acess to them
            using (LibraryContext context = new LibraryContext())
            {
                if (main.TeacherConfirmData.SelectedIndex >= 0)
                {
                    TeacherApproving teacher = new TeacherApproving();
                    var selectedteacher = main.TeacherConfirmData.SelectedItems.Cast<TeacherApproving>().ToList();

                    foreach (var vrper in selectedteacher)
                    {
                        //If admin accept being a teacher request, teacher will shown as a teacher on system
                        var selectedteacherinformation = context.TeacherApproving.Where(pr => pr.TeacherMail == vrper.TeacherMail).FirstOrDefault();
                        var selectedteacheruser = context.TblUsers.Where(pr => pr.Email == vrper.TeacherMail).FirstOrDefault();
                        selectedteacheruser.UserType = 2;

                        try
                        {
                            context.TblUsers.Update(selectedteacheruser);
                            context.TeacherApproving.Update(selectedteacherinformation);
                            MessageBox.Show("Teacher status was successfully approved!");
                            main.TeacherConfirmData.ItemsSource = context.TeacherApproving.Local.ToBindingList();
                            context.TeacherApproving.Remove(selectedteacherinformation);
                            main.TeacherConfirmData.Items.Refresh();
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

        public static void GivingBackBook(MainWindow main)
        {
            using (LibraryContext context = new LibraryContext())
            {             
                //If someone wants to return a book, this user's informations will send to admin for confirmation
                TblGivingBack approveoperation = new TblGivingBack();
                var selectedItems = main.DataAccountInfos.SelectedItems.Cast<TblSelectedBook>().ToList();
              
                foreach (var item in selectedItems)
                {

                    var seelectedbookreturn = context.TblSelectedBook.Where(pr => pr.WhichBook == item.WhichBook).FirstOrDefault();

                    approveoperation.WhichMail = item.Email;
                    approveoperation.BookToGiveBack = item.WhichBook;
                    approveoperation.WhenLeased= item.LeasedDate;
                   
                    try
                    {
                        MessageBox.Show("Your Request sent successfully to admin!");
                        context.TblSelectedBook.Remove(seelectedbookreturn);
                        context.TblGivingBack.Add(approveoperation);
                        main.DataAccountInfos.Items.Refresh();
                        main.GivingBackData.Items.Refresh();
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

        public static void BookConfirms(MainWindow main)
        {

            using (LibraryContext context = new LibraryContext())
            {
                //If the student wanted to his or her book back, this request will send to admin and admin must accept that request for giving back the book
                var selectediadeonayı = main.GivingBackData.SelectedItems.Cast<TblGivingBack>().ToList();

                foreach (var vrper in selectediadeonayı)
                {
                    var selectedbook = context.TblBooks.Where(pr => pr.Name == vrper.BookToGiveBack).FirstOrDefault();
                    var bookforgivingback = context.TblGivingBack.Where(pr => pr.BookToGiveBack == vrper.BookToGiveBack).FirstOrDefault();
                    selectedbook.Number = selectedbook.Number + 1;

                    try
                    {

                        context.TblBooks.Update(selectedbook);
                        context.TblGivingBack.Local.Remove(bookforgivingback);
                        main.GivingBackData.Items.Refresh();
                        main.DataAccountInfos.Items.Refresh();
                        MessageBox.Show("Book return has been accepted!");
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
        public static void AddingABook(MainWindow main)
        {
            using (LibraryContext context = new LibraryContext())
            {
                //In these codes admin will add book on the system
                TblBooks AddBk = new TblBooks();
                
                AddBk.Name = main.AddBookName.Text;

                AddBk.Genre = main.AddBookGenre.Text;

                AddBk.ReleaseDate = Int32.Parse(main.AddReleaseDate.Text);

                AddBk.Pages = Convert.ToInt32(main.AddBookPages.Text);

                AddBk.Author = main.AddBookAuthor.Text;

                AddBk.Number = Convert.ToInt32(main.AddBookNumber.Text);

                context.TblBooks.Add(AddBk);
                main.datagridBooks.Items.Refresh();
                main.ViewBooksDataGrid.Items.Refresh();
                context.SaveChanges();
                MessageBox.Show("This book is added successfully!");

            }
        }

    }
}
    


