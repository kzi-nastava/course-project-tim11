namespace ClinicApp.AdminFunctions { 
    public class ClinicEquipment
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Amount { get; set; }
        public int RoomId { get; set; }
        public EquipmentType Type {get;set;}
    }
    public enum EquipmentType {Operations, RoomFurniture, Hallway, Examinations}
}