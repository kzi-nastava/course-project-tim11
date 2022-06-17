﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Clinic
{
    public class OperationService
    {
        public static void View(Operation operation)
        {
            Console.WriteLine($"OPERATION ID: {operation.ID}\nDate and time:{operation.DateTime}\nDuration: {operation.Duration}min\nPatient name: {operation.Patient.Name}; ");
            Console.WriteLine($"Patient last name: {operation.Patient.LastName};");
            Console.WriteLine($"Date of birth {operation.Patient.DateOfBirth.ToShortDateString()}");
        }
    }
}
