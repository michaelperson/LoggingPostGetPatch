using LoggingAndErrors.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingAndErrors.Infra.Formatters
{
    public class CsvFormatter : TextOutputFormatter
    {

        public CsvFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8); SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(Employee).IsAssignableFrom(type) 
                || 
                typeof(IEnumerable<Employee>).IsAssignableFrom(type)) 
            { 
                return base.CanWriteType(type); 
            }
            return false; 
        }
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            HttpResponse response = context.HttpContext.Response;
            StringBuilder buffer = new StringBuilder(); 
            if (context.Object is IEnumerable<Employee>) 
            { 
                foreach (Employee emp in (IEnumerable<Employee>)context.Object) 
                {
                    EmpToCsv(buffer, emp); 
                }
            }
            else 
            {
                EmpToCsv(buffer, (Employee)context.Object); 
            }
            await response.WriteAsync(buffer.ToString());
        }

        private void EmpToCsv(StringBuilder buffer, Employee emp)
        {
            buffer.AppendLine($"{emp.Id},\"{emp.FirstName},\"{emp.LastName}\"");
        }
    }
}

