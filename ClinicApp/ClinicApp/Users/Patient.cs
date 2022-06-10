using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ClinicApp.HelperClasses;

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

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role + "|" + Blocked.ToString();
        }

        //============================================= KRAJ PACIJENTA , MAIN KLASA ZA PACIJENTA =========================================
        //========================== MENU KLASA ZA PACIJENTA????? ==============================================
        public override int MenuWrite()
        {
            return Menu.PatientMenuWrite(this);
        }

        public override void MenuDo(int option)
        {
            Menu.PatientMenuDo(this, option);
        }
//==================================================================================================



    }
}