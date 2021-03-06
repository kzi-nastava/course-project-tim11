namespace ClinicApp.AdminFunctions
{
    public class Equipment
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Amount { get; set; }
        public int RoomId { get; set; }
        public EquipmentType Type { get; set; }
        public bool Dynamic { get; set; }

    }
    public enum EquipmentType { Operations, RoomFurniture, Hallway, Examinations, Gauzes, Stiches, Vaccines, Bandages }
}