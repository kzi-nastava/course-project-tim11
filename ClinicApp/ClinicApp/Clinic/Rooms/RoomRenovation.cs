using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class RoomRenovation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public HelperClasses.DateRange Duration {get; set;}
        public bool Done { get; set; }
        public RenovationType Type { get; set; }
        public Room NewRoom { get; set; }
        public int JoinedRoomId { get; set; }
    }
    public enum RenovationType{
        Simple, ComplexJoin, ComplexSplit
    }
}
