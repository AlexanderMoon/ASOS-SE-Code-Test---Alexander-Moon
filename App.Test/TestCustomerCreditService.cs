using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Test
{
    class TestCustomerCreditService : ICustomerCreditService
    {
        public int GetCreditLimit(string firstname, string surname, DateTime dateOfBirth)
        {
            switch (surname)
            {
                case "Poor":
                    return 200;
                case "Rich":
                    return 9999;
                case "Normal":
                    return 1000;
                case "None":
                    return int.MaxValue;
                default:
                    return 0;
            }
        }
    }
}
