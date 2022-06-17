using System;
using System.Collections.Generic;

namespace ClinicApp.Clinic.Medicine
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

        
        public static void ListMedicineRequests(string parameter = "rbd") //rbd = reviewed by doctor (default), sba = sent by admin, all = all requests 
        {
            if (parameter == "rbd")
            {
                Console.WriteLine("These requests have been reviewed by a doctor and should be fixed up");
                foreach (var request in MedicineRequestRepository.GetAll())
                {
                    if (request.Comment != "")
                    {
                        Console.WriteLine("----------------------------------------------------------");
                        Console.WriteLine("Request ID: " + request.Id +
                            "\nMedicine name: " + request.Medicine.Name +
                            "\nMedicine ingrediants: " + WriteMedicineIngrediants(request.Medicine.Ingredients) +
                            "\nDoctor's comment: " + request.Comment);
                        Console.WriteLine("----------------------------------------------------------");
                    }
                }
            }
            else if (parameter == "sba")
            {
                Console.WriteLine("These requests have been sent by an admin and should be reviewed");
                foreach (var request in MedicineRequestRepository.GetAll())
                {
                    if (request.Comment == "")
                    {
                        Console.WriteLine("----------------------------------------------------------");
                        Console.WriteLine("Request ID: " + request.Id +
                            "\nMedicine name: " + request.Medicine.Name +
                            "\nMedicine ingrediants: " + WriteMedicineIngrediants(request.Medicine.Ingredients) + "\n");
                        Console.WriteLine("----------------------------------------------------------");
                    }
                }
            }
            else if (parameter == "all")
            {
                Console.WriteLine("All medicine requests");
                foreach (var request in MedicineRequestRepository.GetAll())
                {
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine("Request ID: " + request.Id +
                            "\nMedicine name: " + request.Medicine.Name +
                            "\nMedicine ingrediants: " + WriteMedicineIngrediants(request.Medicine.Ingredients) +
                            "\nDoctor's comment: " + request.Comment);
                    Console.WriteLine("----------------------------------------------------------");

                }
            }
        }
        public static string WriteMedicineIngrediants(List<string> ingrediants)
        {
            string output = "";
            foreach (var ingr in ingrediants)
            {
                output += ingr + ", ";
            }
            return output;
        }

        
    }
}
