using LoggingAndErrors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingAndErrors.Models
{
    public class FakeContext : IContext
    {
        private List<Employee> _employees;

        public FakeContext()
        {
            _employees = new List<Employee>()
            {
                new Employee(){ Id=1, FirstName="Jhon", LastName="Son" },
                new Employee(){ Id=2, FirstName="Fitz", LastName="Smith" },
                new Employee(){ Id=3, FirstName="Gerald", LastName="Dîne" },
                new Employee(){ Id=4, FirstName="Ken", LastName="Wood" },
                new Employee(){ Id=5, FirstName="Eddy", LastName="Merkx" }
            };

        }

        public List<Employee> Employees
        {
            get
            {
                return _employees;
            }

            set
            {
                _employees = value;
            }
        }
    }
}
