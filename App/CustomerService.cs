using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public class CustomerService
    {
        private const string CUSTOMERTYPE_VERYIMPORTANT = "VeryImportantClient";
        private const string CUSTOMERTYPE_IMPORTANT = "ImportantClient";

        ICompanyRepository _companyRepository = null;
        ICustomerRepository _customerRepository = null;
        ICustomerCreditService _customerCreditService = null;

        public CustomerService() : this(new CompanyRepository(), new CustomerRepository(), new CustomerCreditServiceClient())
        { }
        public CustomerService(ICompanyRepository companyRepository, ICustomerRepository customerRepository, ICustomerCreditService customerCreditService)
        {
            _companyRepository = companyRepository;
            _customerRepository = customerRepository;
            _customerCreditService = customerCreditService;
        }

        public bool AddCustomer(string firname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            //Could use a NuGet Library to check for valid email addresses.
            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            if (GetAge(dateOfBirth) < 21)
                return false;

            var companyRepository = new CompanyRepository();
            var company = _companyRepository.GetById(companyId);

            var customer = new Customer
            {
                Company = company,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            if (company.Name == CUSTOMERTYPE_VERYIMPORTANT)
            {
                // Skip credit check
                SetCreditLimit(CreditLimit.None, customer);
            }
            else if (company.Name == CUSTOMERTYPE_IMPORTANT)
            {
                // Skip credit check
                SetCreditLimit(CreditLimit.Double, customer);
            }
            else
            {
                // Skip credit check
                SetCreditLimit(CreditLimit.Normal, customer);
            }

            if (customer.HasCreditLimit && customer.CreditLimit < 500)
            {
                return false;
            }

            _customerRepository.AddCustomer(customer);

            return true;
        }

        private int GetAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }

        private void SetCreditLimit(CreditLimit creditLimitType, Customer customer)
        {
            if (creditLimitType == CreditLimit.None)
                customer.HasCreditLimit = false;
            else
            {
                customer.HasCreditLimit = true;

                var creditLimit = _customerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
                creditLimit = creditLimitType == CreditLimit.Normal ? creditLimit : creditLimit * 2;
                customer.CreditLimit = creditLimit;
            }
        }
    }
}