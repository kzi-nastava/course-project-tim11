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
            MedicineRequest toApprove = MedicineRequestRepository.Get(id);
            if (toApprove is null)
                return;
            MedicineRepo.Add(toApprove.Medicine);
           
            MedicineRequestRepository.Delete(id);
            MedicineRequestRepository.PersistChanges();
        }
        public static void Reject(int id, string comment)
        {
            MedicineRequest toReject = MedicineRequestRepository.Get(id);
            if (toReject is null)
                return;
            toReject.Comment = comment;
            MedicineRequestRepository.PersistChanges();
        }
        public static void CreateNew(string name, List<string> chosenIngrediants)
        {
            Medicine medicine = new Medicine(name, chosenIngrediants);
            MedicineRequest mr = new MedicineRequest { Medicine = medicine, Comment = "" };
            MedicineRequestRepository.Add(mr);
        }
        public static void UpdateReviewed(int id, string name, List<string> chosenIngrediants)
        {
            Medicine medicine = new Medicine(name, chosenIngrediants);
            MedicineRequest newRequest = new MedicineRequest { Medicine = medicine, Comment = "" };
            MedicineRequestRepository.Update(id, newRequest);
        }

        public static void ReviewMedicineRequests()
        {
            string choice;
            CLI.CLIWriteLine("Medicine requests: ");
            List<MedicineRequest> allRequests = MedicineRequestRepository.LoadMedicineRequests();
            foreach (var request in allRequests)
            {
                
                if (request.Comment == "")
                {
                    CLI.CLIWriteLine("----------------------------------------------------------");
                    CLI.CLIWriteLine("Request ID: " + request.Id +
                        "\nMedicine name: " + request.Medicine.Name +
                        "\nMedicine ingrediants: " + OtherFunctions.WriteMedicineIngrediants(request.Medicine.Ingredients) + "\n");
                    CLI.CLIWriteLine("----------------------------------------------------------");
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
}
