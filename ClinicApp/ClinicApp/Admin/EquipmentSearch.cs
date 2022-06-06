﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.AdminFunctions
{
    public class EquipmentSearch  //small helper class to ease searching equipment
    {
        public string SearchTerm { get; set; }
        public bool FilterByEqTypeBool { get; set; }
        public EquipmentType FilterByEq { get; set; }
        public bool FilterByAmountBool { get; set; }
        public int STAmount { get; set; }
        public bool FilterByRoomTypeBool { get; set; }
        public RoomType FilterByRoom { get; set; }

    }
}
