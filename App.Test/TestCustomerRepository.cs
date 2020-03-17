using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App;

namespace App.Test
{
    class TestCustomerRepository : ICustomerRepository
    {
        Customer _customer = null;

        public TestCustomerRepository()
        {
            _customer = null;
        }
                
        public void AddCustomer(Customer customer)
        {
            _customer = customer;
        }

        public Customer GetLastAddedCustomer()
        {
            return _customer;
        }
    }
}
