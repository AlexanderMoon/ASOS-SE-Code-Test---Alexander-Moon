using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Test
{
    [TestClass]
    public class CustomerServiceTest
    {
        [TestMethod]
        public void AddCustomer_Fail_TooYoung()
        {
            //I should really change these to use a mocking framework like Moq or Rhino, but tbh these are so simple this is easier and quicker.
            TestCustomerRepository customerRepository = new TestCustomerRepository();
            TestCompanyRepository companyRepository = new TestCompanyRepository();
            TestCustomerCreditService customerCreditService = new TestCustomerCreditService();

            Customer customer = new Customer()
            {
                Id = 1,
                Firstname = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-20),
                EmailAddress = "JohnDoe@notarealemailaddress.co.uk",
            };

            CustomerService customerService = new CustomerService(companyRepository, customerRepository, customerCreditService);

            Assert.IsFalse(customerService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, customer.DateOfBirth, 0));
        }

        [TestMethod]
        public void AddCustomer_Fail_InvalidEmail()
        {
            TestCustomerRepository customerRepository = new TestCustomerRepository();
            TestCompanyRepository companyRepository = new TestCompanyRepository();
            TestCustomerCreditService customerCreditService = new TestCustomerCreditService();

            Customer customer = new Customer()
            {
                Id = 1,
                Firstname = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-22),
                EmailAddress = "JohnDoe_notarealemailaddress_co_uk",
            };

            CustomerService customerService = new CustomerService(companyRepository, customerRepository, customerCreditService);

            Assert.IsFalse(customerService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, customer.DateOfBirth, 0));
        }

        [TestMethod]
        public void AddCustomer_fail_CreditTooLow()
        {
            TestCustomerRepository customerRepository = new TestCustomerRepository();
            TestCompanyRepository companyRepository = new TestCompanyRepository();
            TestCustomerCreditService customerCreditService = new TestCustomerCreditService();

            Customer customer = new Customer()
            {
                Id = 1,
                Firstname = "John",
                Surname = "Poor",
                DateOfBirth = DateTime.Now.AddYears(-22),
                EmailAddress = "JohnDoe@notarealemailaddress.co.uk",
            };

            CustomerService customerService = new CustomerService(companyRepository, customerRepository, customerCreditService);

            Assert.IsFalse(customerService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, customer.DateOfBirth, 0));
        }

        [TestMethod]
        public void AddCustomer_Success_NoCreditLimit()
        {
            TestCustomerRepository customerRepository = new TestCustomerRepository();
            TestCompanyRepository companyRepository = new TestCompanyRepository();
            TestCustomerCreditService customerCreditService = new TestCustomerCreditService();

            Customer customer = new Customer()
            {
                Firstname = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-22),
                EmailAddress = "JohnDoe@notarealemailaddress.co.uk",
            };

            CustomerService customerService = new CustomerService(companyRepository, customerRepository, customerCreditService);

            customerService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, customer.DateOfBirth, 1);

            var addedCustomer = customerRepository.GetLastAddedCustomer();

            Assert.AreEqual(customer.Firstname, addedCustomer.Firstname);
            Assert.AreEqual(customer.Surname, addedCustomer.Surname);
            Assert.AreEqual(customer.EmailAddress, addedCustomer.EmailAddress);
            Assert.AreEqual(customer.DateOfBirth, addedCustomer.DateOfBirth);
            Assert.IsFalse(addedCustomer.HasCreditLimit);           
        }
    

        [TestMethod]
        public void AddCustomer_Success_NormalCreditLimit()
        {
            TestCustomerRepository customerRepository = new TestCustomerRepository();
            TestCompanyRepository companyRepository = new TestCompanyRepository();
            TestCustomerCreditService customerCreditService = new TestCustomerCreditService();

            Customer customer = new Customer()
            {
                Firstname = "John",
                Surname = "Normal",
                DateOfBirth = DateTime.Now.AddYears(-22),
                EmailAddress = "JohnDoe@notarealemailaddress.co.uk",
            };

            CustomerService customerService = new CustomerService(companyRepository, customerRepository, customerCreditService);

            customerService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, customer.DateOfBirth, 3);

            var addedCustomer = customerRepository.GetLastAddedCustomer();

            Assert.AreEqual(customer.Firstname, addedCustomer.Firstname);
            Assert.AreEqual(customer.Surname, addedCustomer.Surname);
            Assert.AreEqual(customer.EmailAddress, addedCustomer.EmailAddress);
            Assert.AreEqual(customer.DateOfBirth, addedCustomer.DateOfBirth);
            Assert.IsTrue(addedCustomer.HasCreditLimit);
            Assert.AreEqual(customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth), addedCustomer.CreditLimit);
        }

        [TestMethod]
        public void AddCustomer_Success_DoubleCreditLimit()
        {
            TestCustomerRepository customerRepository = new TestCustomerRepository();
            TestCompanyRepository companyRepository = new TestCompanyRepository();
            TestCustomerCreditService customerCreditService = new TestCustomerCreditService();

            Customer customer = new Customer()
            {
                Firstname = "John",
                Surname = "Normal",
                DateOfBirth = DateTime.Now.AddYears(-22),
                EmailAddress = "JohnDoe@notarealemailaddress.co.uk",
            };

            CustomerService customerService = new CustomerService(companyRepository, customerRepository, customerCreditService);

            customerService.AddCustomer(customer.Firstname, customer.Surname, customer.EmailAddress, customer.DateOfBirth, 2);

            var addedCustomer = customerRepository.GetLastAddedCustomer();

            Assert.AreEqual(customer.Firstname, addedCustomer.Firstname);
            Assert.AreEqual(customer.Surname, addedCustomer.Surname);
            Assert.AreEqual(customer.EmailAddress, addedCustomer.EmailAddress);
            Assert.AreEqual(customer.DateOfBirth, addedCustomer.DateOfBirth);
            Assert.IsTrue(addedCustomer.HasCreditLimit);
            Assert.AreEqual(customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth) * 2, addedCustomer.CreditLimit);
        }
    }
}
