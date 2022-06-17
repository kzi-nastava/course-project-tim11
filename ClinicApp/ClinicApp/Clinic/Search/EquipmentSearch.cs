using ClinicApp.Clinic.Equipments;
using ClinicApp.Clinic.Rooms;

namespace ClinicApp.Clinic.Search
{
    public class EquipmentSearch  //small helper class to ease searching equipment
    {
        public string SearchTerm { get; set; }
        public bool FilterByEqTypeBool { get; set; }
        public EquipmentType FilterByEq { get; set; }
        public bool FilterByAmountBool { get; set; }
        public int Amount { get; set; }
        public bool FilterByRoomTypeBool { get; set; }
        public RoomType FilterByRoom { get; set; }

    }
}
