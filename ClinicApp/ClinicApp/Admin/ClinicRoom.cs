namespace ClinicApp.Admin { 
    public class ClinicRoom
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public RoomType Type {get;set;}
    }
    public enum RoomType {Operations, Waiting, STORAGE, Examinations}
}