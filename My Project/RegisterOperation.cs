    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Security.Cryptography;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;

namespace My_Project
{
    public static class RegisterOperation
    {
        public static MainWindow main;


        public static void RegisterStatus()
        {
            string RegisterName = main.RegisterNameText.Text;
            string Email = main.RegisterMail.Text.ToLower();
            string Password = main.Pw1.Password.ToString();
            string AgainPassword = main.Pw2.Password.ToString();
            Random scrandom = new Random();
            TblUsers User = new TblUsers();

            //List for prohibited chracters in registering
            List<char> notallowedchars = new List<char> { '%', '-', '$', '/', '\\', '(', ')', '[', ']', '’', '“', '”', ',', '!', '?', '{', '}', '=', '&' };
            int MinPassLength = 8;
            
            
            //if there is a prohibited chracter, what program will show that
            foreach (char SrPer in RegisterName)
            {
                if (notallowedchars.Contains(SrPer) == true)
                {
                    MessageBox.Show($"Name & Surname cannot contain {SrPer} character!");
                    return;
                }
            }

            foreach (char SrPer in Email.ToCharArray())
            {
                if (notallowedchars.Contains(SrPer) == true)
                {
                    MessageBox.Show($"Email cannot contain {SrPer} character!");
                    return;
                }
            }

            using(LibraryContext context = new LibraryContext())
            {
                var vruser = context.TblUsers.Where(pr => pr.Email == main.RegisterMail.Text).FirstOrDefault();
                
                if (vruser!=null)
                {
                    MessageBox.Show("This user already registered!");
                    return; 
                }
            }
           

            //In here, codes will check password and if there is a problem, it will give an error
            if (Password == "" && AgainPassword == "")
            {
                MessageBox.Show("Password field cannot be blank!");
                return;
            }

            if (Password.Length < MinPassLength)
            {
                MessageBox.Show("Your password cannot be less than 8 characters");
                return;
            }

            //This part is controlling that user's password will contain at least 1 Big letter, 1 Small letter and password must be min 8 chracter
            bool resultdigit = false, resultletter = false, resultuperrletter = false, resultlowerletter = false;

            foreach (char Per in Password.ToCharArray())
            {
                if (char.IsDigit(Per))
                {
                    resultdigit = true;
                }
                if (char.IsLetter(Per))
                {
                    resultletter = true;
                }
                if (char.IsUpper(Per))
                {
                    resultuperrletter = true;
                }
                if (char.IsLower(Per))
                {
                    resultlowerletter = true;
                }
            }

            if (resultdigit == false)
            {
                MessageBox.Show("Your password must have a number!");
                return;
            }
            if (resultletter == false)
            {
                MessageBox.Show("Your password must have a letter!");
                return;
            }
            if (resultuperrletter == false)
            {
                MessageBox.Show("Your password must have an upper letter!");
                return;
            }
            if (resultlowerletter == false)
            {
                MessageBox.Show("Your password must have a lower letter!");
                return;
            }

            if (main.Combo.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select a UserType");
                return;
            }

            //In here, information of people who are registered will save on the table that I created on my sql server
            using (LibraryContext context = new LibraryContext())
            { 
                TeacherApproving Tch = new TeacherApproving();

                User.Name = RegisterName;
                User.Email = Email;
               
                if (Password == AgainPassword)
                {
                    User.Password = GeneralCodes.ComputeSha256Hash(Password);
                }

                else
                {
                    MessageBox.Show("Passwords are not same");
                    return;
                }

                User.UserType = 1;

                //Teacher will save like a student at first without admin's confirming
                if (main.Combo.SelectedIndex == 2)
                {
                    string message = "Waiting for approving!";
                    Tch.TeacherName = User.Name;
                    Tch.TeacherMail = User.Email;
                    Tch.Approval = message;
                    context.TeacherApproving.Add(Tch);
                    main.TeacherConfirmData.Items.Refresh();
                    context.SaveChanges();


                }
                User.SchoolNumber = scrandom.Next(100000, 999999);
               
                //In here, all the infos will be save on table that in sql server
                try
                {
                    context.TblUsers.Add(User);
                    context.SaveChanges();

                }
                catch (Exception E)
                {
                    MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                    return;
                }

              
            }

            //if there is no problem, we can access to library
            
                MessageBox.Show($"Welcome {RegisterName.ToLower()}");
                main.Acname.Content = main.Acname.Content + RegisterName.ToLower();
                main.LibraryTab.IsEnabled = true;
                main.LibraryTab.IsSelected = true;
                main.RegisterTab.IsEnabled = false;
                main.LoginBut.IsEnabled = false;
            

            

        }

    }
}
