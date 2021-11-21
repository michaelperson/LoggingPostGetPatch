using LoggingAndErrors.Models;
using System.Collections.Generic;

namespace LoggingAndErrors.Interfaces
{
    public interface IContext
    {
        List<Employee> Employees { get; set; }
    }
}