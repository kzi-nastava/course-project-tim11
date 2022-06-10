using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    public static class MedicineRequestRepo
    {
        public static List<MedicineRequest> MedicineRequests;
        static MedicineRequestRepo()
        {
            MedicineRequests = LoadMedicineRequests();
        }

        public static List<MedicineRequest> GetAll() => MedicineRequests;

        public static MedicineRequest? Get(int id) => MedicineRequests.FirstOrDefault(p => p.Id == id);
        public static void Add(MedicineRequest item)
        {
            if (MedicineRequests.Count == 0)
            {
                item.Id = 1;
            }
            else
            {
                item.Id = MedicineRequests.Last().Id + 1;
            }
            MedicineRequests.Add(item);
            PersistChanges();
        }
        public static void Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return;
            MedicineRequests.Remove(item);
            PersistChanges();
        }
        public static void Update(int id, MedicineRequest newMedicineRequest)
        {
            MedicineRequest toUpdate = Get(id);
            if (toUpdate is null)
                return;
            toUpdate.Medicine = newMedicineRequest.Medicine;
            toUpdate.Comment = newMedicineRequest.Comment;
            PersistChanges();
        }
        //===============================files stuff====================================

        static MedicineRequest ParseMedicineRequest(string line)
        {
            string[] parameters = line.Split("|");
            MedicineRequest medicineRequest = new MedicineRequest {
                Id = Convert.ToInt32(parameters[0]),
                Medicine = new Clinic.Medicine(parameters[1], parameters[2].Split("/").ToList()),
                Comment = parameters[3]
            };
            return medicineRequest;
        }

        public static List<MedicineRequest> LoadMedicineRequests()
        {
            List<MedicineRequest> lista = new List<MedicineRequest>();
            using (StreamReader reader = new StreamReader("../../../Admin/Data/medicineRequests.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    MedicineRequest mr = ParseMedicineRequest(line);
                    lista.Add(mr);
                }
            }
            return lista;
        }
        public static void PersistChanges()
        {
            File.Delete("../../../Admin/Data/medicineRequests.txt");
            foreach (MedicineRequest medicineRequest in MedicineRequests)
            {
                string newLine = Convert.ToString(medicineRequest.Id) + "|" + medicineRequest.Medicine.Compress() + "|" + medicineRequest.Comment;
                using (StreamWriter sw = File.AppendText("../../../Admin/Data/medicineRequests.txt"))
                {
                    sw.WriteLine(newLine);
                }
            }
        }
    }
}
