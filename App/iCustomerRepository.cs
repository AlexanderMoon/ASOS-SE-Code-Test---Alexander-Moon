using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
    }
}
