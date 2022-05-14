﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    public class RoomRenovation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateRange Duration {get; set;}
        public bool Done { get; set; }
        public RenovationType Type { get; set; }
        public ClinicRoom NewRoom { get; set; }
        public int JoinedRoomId { get; set; }
    }
    public enum RenovationType{
        Simple, ComplexJoin, ComplexSplit
    }
}
