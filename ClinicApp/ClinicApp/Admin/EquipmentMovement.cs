using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    public class EquipmentMovement
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int NewRoomId { get; set; }
        public int Amount { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
