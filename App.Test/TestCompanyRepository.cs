using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App;

namespace App.Test
{
    class TestCompanyRepository : ICompanyRepository
    {
        public Company GetById(int id)
        {
            switch (id)
            {
                case 1:
                    return new Company()
                    {
                        Id = 1,
                        Name = "VeryImportantClient",
                        Classification = Classification.Gold
                    };
                    break;
                case 2:
                    return new Company()
                    {
                        Id = 2,
                        Name = "ImportantClient",
                        Classification = Classification.Silver
                    };
                    break;
                default:
                    return new Company()
                    {
                        Id = 3,
                        Name = "OrdinaryClient",
                        Classification = Classification.Bronze
                    };
            }
        }
    }
}
