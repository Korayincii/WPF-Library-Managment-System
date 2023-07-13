using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace My_Project
{
    public static class LoginOperation
    {
        public static MainWindow main;

        public static string logininfo = "NA";
        public static bool accuary;
        public static string srEmail = "NA";
        public static int SchoolNumber = 0;

        public static void LoginStatus()
        {
            string LoginPassword = main.loginpass.Password.ToString();
            var vrstudentid = main.logintext.Text;

            //In here, program is checking that is loggining with e-mail or school number
            if (main.LoginComboBox.SelectedIndex==0)
            {
                srEmail = main.logintext.Text;
                accuary = true;
            }
            else
            {
                SchoolNumber = Int32.Parse(main.logintext.Text);
                accuary = false;
            }


            if (LoginPassword == "")
            {
                MessageBox.Show("Password field can't be blank");
                return;
            }

            using (LibraryContext context = new LibraryContext())
            {

                if (accuary == true)
                {
                    var vrMail = context.TblUsers.FirstOrDefault(pr => pr.Email == main.logintext.Text);

                    if (vrMail == null)
                    {
                        MessageBox.Show("Email not found in databases");
                        return;
                    }
                    var vrPass = context.TblUsers.FirstOrDefault(pr => pr.Email == srEmail);
                    srEmail = vrMail.Email;


                    if (vrPass.Password != GeneralCodes.ComputeSha256Hash(main.loginpass.Password.ToString()))
                    {
                        MessageBox.Show("Passwords are not matching!");
                        return;
                    }
                    MessageBox.Show("Welcome");
                    logininfo = srEmail;
                    main.Acname.Content = main.Acname.Content + vrMail.Name;
                    
                }
                else
                {
                    var vrSchool = context.TblUsers.FirstOrDefault(pr => pr.SchoolNumber == SchoolNumber);
                    if (vrSchool == null)
                    {
                        MessageBox.Show("School number not found in databases");
                        return;
                    }
                    if (vrSchool.Password != GeneralCodes.ComputeSha256Hash(main.loginpass.Password.ToString()))
                    {
                        MessageBox.Show("Passwords are not matching!");
                        return;
                    }
                    logininfo = vrSchool.Email;
                    MessageBox.Show("Welcome");
                    main.Acname.Content = main.Acname.Content + vrSchool.Name;
                }

                main.LibraryTab.IsEnabled = true;
                main.LibraryTab.IsSelected = true;
                main.LoginBut.IsEnabled = false;
                main.RegisterTab.IsEnabled = false;
            
            }

        }

        public static void AdminLoginStatus()
        {
            //In login panel, there will be a secret part for admin loggining and if admin's password will equal with passwordbox, admin can access to admin panel
            string adpass = GeneralCodes.ComputeSha256Hash("admin195050016");
            var adminloginning = GeneralCodes.ComputeSha256Hash(main.loginadminpass.Password);

            if (adminloginning == adpass)
            {
                MessageBox.Show("Welcome admin");
                main.AdminTab.IsEnabled = true;
                main.AdminTab.IsSelected = true;
                main.Acname.Content = main.Acname.Content + "admin";
                main.AdminTab.IsSelected = true;
                return;
            }
            else
            {
                MessageBox.Show("Wrong Password!");
                return;
            }
        }

    }
}

