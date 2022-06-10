using ClinicApp.Clinic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class MedicineRequestService
    {
        public static void Approve(int id)
        {
            MedicineRequest toApprove = MedicineRequestRepo.Get(id);
            if (toApprove is null)
                return;
            MedicineRepo.Medicine.Add(toApprove.Medicine.Name, toApprove.Medicine);
            MedicineRequestRepo.Delete(id);
            MedicineRequestRepo.PersistChanges();
        }
        public static void Reject(int id, string comment)
        {
            MedicineRequest toReject = MedicineRequestRepo.Get(id);
            if (toReject is null)
                return;
            toReject.Comment = comment;
            MedicineRequestRepo.PersistChanges();
        }
        public static void CreateMedicineRequest()
        {
            CLI.CLIWriteLine("Enter medicine name");
            string name = CLI.CLIEnterStringWithoutDelimiter("|");
            while (MedicineRepo.Medicine.ContainsKey(name))
            {
                CLI.CLIWriteLine("Name already taken, enter another name");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            List<string> chosenIngrediants = IngrediantService.ChooseIngrediants();
            Medicine medicine = new Clinic.Medicine(name, chosenIngrediants);
            MedicineRequest mr = new MedicineRequest { Medicine = medicine, Comment = "" };
            MedicineRequestRepo.Add(mr);
        }
        public static void ReviewedMedsMenu()
        {
            ListMedicineRequests();
            int id = CLI.CLIEnterNumber();
            string name;
            CLI.CLIWriteLine("Enter medicine name");
            name = CLI.CLIEnterStringWithoutDelimiter("|");
            while (MedicineRepo.Medicine.ContainsKey(name))
            {
                CLI.CLIWriteLine("Name already taken, enter another name");
                name = CLI.CLIEnterStringWithoutDelimiter("|");
            }
            List<string> chosenIngrediants = IngrediantService.ChooseIngrediants();
            Clinic.Medicine medicine = new Clinic.Medicine(name, chosenIngrediants);
            MedicineRequest newRequest = new MedicineRequest { Medicine = medicine, Comment = "" };
            MedicineRequestRepo.Update(id, newRequest);

        }

        
        public static void ListMedicineRequests(string parameter = "rbd") //rbd = reviewed by doctor (default), sba = sent by admin, all = all requests 
        {
            if (parameter == "rbd")
            {
                CLI.CLIWriteLine("These requests have been reviewed by a doctor and should be fixed up");
                foreach (var request in MedicineRequestRepo.GetAll())
                {
                    CLI.CLIWriteLine("----------------------------------------------------------");
                    if (request.Comment != "")
                    {
                        CLI.CLIWriteLine("Request ID: " + request.Id +
                            "\nMedicine name: " + request.Medicine.Name +
                            "\nMedicine ingrediants: " + request.Medicine.Ingredients +
                            "\nDoctor's comment: " + request.Comment);
                        CLI.CLIWriteLine("----------------------------------------------------------");
                    }
                }
            }
            else if (parameter == "sba")
            {
                CLI.CLIWriteLine("These requests have been sent by an admin and should be reviewed");
                foreach (var request in MedicineRequestRepo.GetAll())
                {
                    CLI.CLIWriteLine("----------------------------------------------------------");
                    if (request.Comment == "")
                    {
                        CLI.CLIWriteLine("Request ID: " + request.Id +
                            "\nMedicine name: " + request.Medicine.Name +
                            "\nMedicine ingrediants: " + request.Medicine.Ingredients + "\n");
                        CLI.CLIWriteLine("----------------------------------------------------------");
                    }
                }
            }
            else if (parameter == "all")
            {
                CLI.CLIWriteLine("All medicine requests");
                foreach (var request in MedicineRequestRepo.GetAll())
                {
                    CLI.CLIWriteLine("----------------------------------------------------------");
                    CLI.CLIWriteLine("Request ID: " + request.Id +
                            "\nMedicine name: " + request.Medicine.Name +
                            "\nMedicine ingrediants: " + request.Medicine.Ingredients +
                            "\nDoctor's comment: " + request.Comment);
                    CLI.CLIWriteLine("----------------------------------------------------------");

                }
            }
        }

        public static void ReviewMedicineRequests()
        {
            string choice;
            CLI.CLIWriteLine("Medicine requests: ");
            foreach (var request in MedicineRequestRepo.GetAll())
            {
                CLI.CLIWriteLine("----------------------------------------------------------");
                if (request.Comment == "")
                {
                    CLI.CLIWriteLine("Request ID: " + request.Id +
                        "\nMedicine name: " + request.Medicine.Name +
                        "\nMedicine ingrediants: " + request.Medicine.Ingredients + "\n");
                    CLI.CLIWriteLine("----------------------------------------------------------");
                }

                CLI.CLIWriteLine("Do you want to approve this medicine(y/n)");
                choice = CLI.CLIEnterString();
                if (choice.ToUpper() == "Y")
                {
                    Approve(request.Id);
                }
                else
                {
                    CLI.CLIWriteLine("Do you want to reject this medicine(y/n)");
                    choice = CLI.CLIEnterString();
                    if (choice.ToUpper() == "Y")
                    {
                        CLI.CLIWriteLine("Why do you want to reject this medicine? Write a short comment.");
                        string comment = CLI.CLIEnterString();
                        Reject(request.Id, comment);
                    }
                    
                }

            }

        }
    }
}
