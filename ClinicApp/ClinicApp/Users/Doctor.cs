using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClinicApp.AdminFunctions;
using ClinicApp.HelperClasses;

namespace ClinicApp.Users
{
    public class Doctor : User
    {
        public List<Appointment> Appointments;
        public Fields Field;
        public int RoomId { get; set; } // id of the room in which the doctor works

        public Doctor(string userName, string password, string name, string lastName, DateTime dateOfBirth, char gender, Fields field)
        {
            UserName = userName;
            Password = password;
            Name = name;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Role = Roles.Doctor;
            MessageBox = new MessageBox(this);
            Appointments = new List<Appointment>();
            Field = field;
        }

        public Doctor(string text)
        {
            string[] data = text.Split("|");

            UserName = data[0];
            Password = data[1];
            Name = data[2];
            LastName = data[3];
            DateOfBirth = DateTime.Parse(data[4]);
            Gender = data[5][0];
            Role = Roles.Doctor;
            Enum.TryParse(data[7], out Field);
            RoomId = Convert.ToInt32(data[8]);
            MessageBox = new MessageBox(this);
            Appointments = new List<Appointment>();
        }

        public override string Compress()
        {
            return UserName + "|" + Password + "|" + Name + "|" + LastName + "|" + DateOfBirth.ToString("dd/MM/yyyy") + "|" + Gender + "|" + Role + "|" + Field.ToString() + "|" + Convert.ToString(RoomId);
        }


        //=======================================================================================================================================================================
        // MENU

        public override int MenuWrite()
        {
            return Menus.Doctors.Menu.Write(this);
        }

        public override void MenuDo(int option)
        {
            Menus.Doctors.Menu.Do(this, option);
        }
       

        

    }


}