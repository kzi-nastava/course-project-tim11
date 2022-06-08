using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ClinicApp.Users
{
    public enum Blocked
    {
        Unblocked, Secretary, System
    };

    public class Patient : User
    {
        public Blocked Blocked { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Referral> Referrals { get; set; }
        public static Dictionary<DateTime, string> ActivityHistory { get; set; } = new Dictionary<DateTime, string>();

        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        public Patient(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender, Blocked blocked)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Patient;
            MessageBox = new MessageBox(this);
            Blocked = blocked;
            Appointments = new List<Appointment>();
            Referrals = new List<Referral>();
            //ActivityHistory = new Dictionary<DateTime, string>();
            //LoadActivityHistory();
            Prescriptions = new List<Prescription>();
        }

        public Patient(string text)
        {
            string[] data = text.Split("|");

            UserName = data[0];
            Password = data[1];
            Name = data[2];
            LastName = data[3];
            DateOfBirth = DateTime.Parse(data[4]);
            Gender = data[5][0];
            Role = Roles.Patient;
            MessageBox = new MessageBox(this);
            Blocked temp;
            try
            {
                if (Blocked.TryParse(data[7], out temp))
                    this.Blocked = temp;
            }
            catch
            {
                this.Blocked = Blocked.Unblocked;
            }


            Appointments = new List<Appointment>();
            Referrals = new List<Referral>();
            //ActivityHistory = new Dictionary<DateTime, string>();
            Prescriptions = new List<Prescription>();
            //LoadActivityHistory();

        }

//============================================= KRAJ PACIJENTA , MAIN KLASA ZA PACIJENTA =========================================
//========================== MENU KLASA ZA PACIJENTA????? ==============================================
        public override int MenuWrite()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: Log out");

            Console.WriteLine("2: Display new messages (" + MessageBox.NumberOfMessages + ")");
            Console.WriteLine("3: Make appointment");
            Console.WriteLine("4: Edit appointment");
            Console.WriteLine("5: Cancel appointment");
            Console.WriteLine("6: View appointments");
            Console.WriteLine("7: Appointment suggestion");
            Console.WriteLine("8: View health record history");
            Console.WriteLine("9: Search doctors");
            Console.WriteLine("0: Exit");

            return 9;

        }

        public override void MenuDo(int option)
        {
            switch (option)
            {
                case 2:
                    MessageBox.DisplayMessages();
                    break;
                case 3:
                    CreateExamination();
                    break;
                case 4:
                    EditExamination();
                    break;
                case 5:
                    DeleteExamination();
                    break;
                case 6:
                    ViewExaminations();
                    break;
                case 7:
                    SuggestAppointment();
                    break;
                case 8:
                    ViewAnamnesis();
                    break;
            }

        }
//==================================================================================================



    }
}