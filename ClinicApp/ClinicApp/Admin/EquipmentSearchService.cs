using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    class EquipmentSearchService
    {
        public static List<Equipment> Search(EquipmentSearch searchTerm)
        {
            searchTerm.SearchTerm = searchTerm.SearchTerm.ToLower();
            var results = new List<Equipment>();
            foreach (var item in EquipmentRepository.EquipmentList)
            {
                if (item.Name.ToLower().Contains(searchTerm.SearchTerm) || item.Type.ToString().ToLower().Contains(searchTerm.SearchTerm) || RoomRepository.Get(item.RoomId).Name.ToLower().Contains(searchTerm.SearchTerm))
                {
                    results.Add(item);
                }
            }
            if (searchTerm.FilterByEqTypeBool == true)
            {
                results = FilterByEqType(results, searchTerm.FilterByEq);
            }
            if (searchTerm.FilterByRoomTypeBool == true)
            {
                results = FilterByRoomType(results, searchTerm.FilterByRoom);
            }
            if (searchTerm.FilterByAmountBool == true)
            {
                switch (searchTerm.Amount)
                {
                    case 1:
                        results = FilterByNumbers(results, 0, 0);
                        break;
                    case 2:
                        results = FilterByNumbers(results, 1, 10);
                        break;
                    case 3:
                        results = FilterByNumbers(results, 11, 10000000);
                        break;
                }
            }
            return results;
        }
        public static List<Equipment> FilterByEqType(List<Equipment> inputList, EquipmentType type)
        {
            var results = new List<Equipment>();
            foreach (var item in inputList)
            {
                if (item.Type == type)
                {
                    results.Add(item);
                }
            }
            return results;
        }
        public static List<Equipment> FilterByRoomType(List<Equipment> inputList, RoomType type)
        {
            var results = new List<Equipment>();
            foreach (var item in inputList)
            {
                if (RoomRepository.Get(item.RoomId).Type == type)
                {
                    results.Add(item);
                }
            }
            return results;
        }
        public static List<Equipment> FilterByNumbers(List<Equipment> inputList, int lowerBound, int upperBound)
        {
            var results = new List<Equipment>();
            foreach (var item in inputList)
            {
                if (item.Amount >= lowerBound && item.Amount <= upperBound)
                {
                    results.Add(item);
                }
            }
            return results;
        }
        
    }
}
