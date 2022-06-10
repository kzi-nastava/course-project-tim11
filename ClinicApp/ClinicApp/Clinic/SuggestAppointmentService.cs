using System;
using ClinicApp.Users;

namespace ClinicApp.Clinic
{
    public class SuggestAppointmentService
    {
        public SuggestAppointmentService()
        {
        }

        private void SuggestAppointment()
        {
            int duration = 15;
            //todo take user input for doctor and time for examination, also time period for appoinment to be done and priority(doctor or time of examination)
            Console.WriteLine("You are currently using the appointment suggestion system.");
            Console.WriteLine("System will suggest your appointment by priority, your priority can be doctor or time of appointment.");
            Console.WriteLine("Please enter the date and time by which the appointment must be made at the latest.");
            DateTime lastAppointment = DateTime.Now;

            Console.Write("\nPlease enter the date for the last possible Examination (e.g. 22/10/1987)\n>> ");

            DateTime date = OtherFunctions.GetGoodDate();

            Console.Write("\nenter the time for the last possible Examination (e.g. 14:30)\n>> ");


            //TODO ask for nonpasttime
            DateTime time;
            do
            {
                time = CLI.CLIEnterTime();
                time = date.Date + time.TimeOfDay;
                if (time < DateTime.Now)
                {
                    Console.WriteLine("You can't enter that time, its in the past");
                }
            } while (time < DateTime.Now);

            lastAppointment = time;

            //todo take time of appointment
            Console.WriteLine("Please enter the preferred time of your Examination in format [HH:mm]:");
            DateTime preferredTime = CLI.CLIEnterTime();


            Console.WriteLine("Enter the username of your preferred doctor. Do you want to view the list of doctors? (y/n)");
            Console.Write(">>");
            string choice = Console.ReadLine();
            Console.WriteLine();
            if (choice.ToUpper() == "Y")
            {
                ViewAllDoctors();
            }
            Console.WriteLine("\nEnter the username:");
            string userName = Console.ReadLine();
            Doctor doctor = null;
            if (!UserRepository.Doctors.TryGetValue(userName, out doctor))
            {
                Console.WriteLine("Doctor with that username does not exist");
                return;
            }
            doctor = UserRepository.Doctors[userName];

            Console.WriteLine("Please enter the priority for your search. Enter 'd' if doctor is your priority, enter 'a' if appointment is your priority.");
            string priority = Console.ReadLine();
            //first check preferred doctor and preferred time
            DateTime initial_appointment = DateTime.Today + preferredTime.TimeOfDay;
            bool available = doctor.CheckAppointment(initial_appointment, duration);
            if (available)
            {
                Console.WriteLine("Your doctor is available, congrats you made appointment.");
                int id;
                try
                {
                    id = SystemFunctions.AllAppointments.Values.Last().ID + 1;
                }
                catch
                {
                    id = 1;
                }
                Examination examination = new Examination(id, initial_appointment, doctor, this, false, 0, 0);
                InsertAppointment(examination);
                doctor.InsertAppointment(examination);
                SystemFunctions.AllAppointments.Add(id, examination);
                SystemFunctions.CurrentAppointments.Add(id, examination);
                //Console.WriteLine("\nNew examination successfully created\n");
                ActivityHistory.Add(DateTime.Now, "CREATE");
                return;
            }
            if (priority.ToUpper() == "D")
            {
                //todo doctor priority
                bool availableDoctor = SuggestDoctorPriority(ref doctor, lastAppointment, preferredTime);
                if (!availableDoctor)
                {
                    Console.WriteLine("Sorry your doctor is not available in this period of time.");
                    //todo give three appointments for patient
                }
            }
            else if (priority.ToUpper() == "A")
            {
                //todo appointment priority
                bool availableAppointment = SuggestAppointmentPriority(preferredTime, lastAppointment);
                if (!availableAppointment)
                {
                    Console.WriteLine("Sorry your prefered appointment is not available in requested time span.");
                    //todo give three appointments for patient
                }
            }
            else
            {
                Console.WriteLine("Invalid input");
                return;
            }
        }


        //suggest appointment doctor priority
        private bool SuggestDoctorPriority(ref Doctor doctor, DateTime lastAppointment, DateTime preferredTime)
        {
            int duration = 15;
            bool appointmentFound = false;
            DateTime today = DateTime.Today + preferredTime.TimeOfDay;
            while (today < lastAppointment)
            {
                bool available = doctor.CheckAppointment(today, duration);
                today = today + TimeSpan.FromMinutes(15);
                if (available)
                {
                    Console.WriteLine("Your doctor is available. You just made appointment.");
                    Console.WriteLine("Date of your appointment is:" + today.ToString() + ".");
                    int id;
                    try
                    {
                        id = SystemFunctions.AllAppointments.Values.Last().ID + 1;
                    }
                    catch
                    {
                        id = 1;
                    }
                    Examination examination = new Examination(id, today, doctor, this, false, 0, 0);
                    InsertAppointment(examination);
                    doctor.InsertAppointment(examination);
                    SystemFunctions.AllAppointments.Add(id, examination);
                    SystemFunctions.CurrentAppointments.Add(id, examination);
                    //Console.WriteLine("\nNew examination successfully created\n");
                    ActivityHistory.Add(DateTime.Now, "CREATE");
                    return true;
                }
            }
            return appointmentFound;
        }

        //suggest appointment appointment priority
        private bool SuggestAppointmentPriority(DateTime preferredTime, DateTime lastAppointment)
        {
            int duration = 15;
            bool appoinmentFound = false;
            DateTime preferredAppointment = DateTime.Today + preferredTime.TimeOfDay;
            while (preferredAppointment < lastAppointment)
            {
                foreach (Doctor doctor in UserRepository.Doctors.Values)
                {
                    bool check = doctor.CheckAppointment(preferredAppointment, duration);
                    if (check)
                    {
                        Console.WriteLine("Your preferred appointment is available in your requested timespan.");
                        int id;
                        try
                        {
                            id = SystemFunctions.AllAppointments.Values.Last().ID + 1;
                        }
                        catch
                        {
                            id = 1;
                        }
                        Examination examination = new Examination(id, preferredAppointment, doctor, this, false, 0, 0);
                        InsertAppointment(examination);
                        doctor.InsertAppointment(examination);
                        SystemFunctions.AllAppointments.Add(id, examination);
                        SystemFunctions.CurrentAppointments.Add(id, examination);
                        //Console.WriteLine("\nNew examination successfully created\n");
                        ActivityHistory.Add(DateTime.Now, "CREATE");
                        return true;
                    }
                }
            }
            return appoinmentFound;
        }

    }
}
