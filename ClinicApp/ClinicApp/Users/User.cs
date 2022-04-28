using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Users
{
    public abstract class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public Role Role { get; set; }

        public abstract int MenuWrite();
        public abstract void MenuDo(int option);
    }
}
