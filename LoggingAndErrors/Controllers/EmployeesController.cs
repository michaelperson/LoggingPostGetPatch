using LoggerService.Interfaces;
using LoggingAndErrors.Infra;
using LoggingAndErrors.Interfaces;
using LoggingAndErrors.Models;
using LoggingAndErrors.Models.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IContext CTX;
        private readonly ILoggerManager _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        public EmployeesController(IContext ctx, ILoggerManager logger, IHostingEnvironment hostingEnvironment)
        {
            CTX = ctx;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet()]
        /// <summary>
        /// GET api/Employees
        /// </summary>
        /// <returns>return a <see cref="IEnumerable{Employee}<"/></returns>
        public IActionResult Get()
        {
            _logger.LogDebug($"[GET] Employees");
            return new OkObjectResult(CTX.Employees.ToAsyncEnumerable<Employee>());
        }


        [HttpGet("ActionGet/{id:int:min(1)}")] 
        public ActionResult<Employee> Get(int id)
        {            
            Employee e = CTX.Employees.SingleOrDefault(e => e.Id.Equals(id));
            if (e != null)
                return e;
            else
                return NotFound(id); 
        }
                 

        [HttpPost]
        /// <summary>
        /// POST api/Employees
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/> to add</param>
        public IActionResult Post(  Employee employee)
        {
            try
            {
                int maxId = CTX.Employees.Max(m => m.Id);
                employee.Id = maxId;
                CTX.Employees.Add(employee);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        /// <summary>
        /// PUT api/Employees
        /// </summary>
        /// <param name="employee">The <see cref="Employee"/> to update</param>
        public async Task<IActionResult> Put([FromQuery]int id,[FromForm]EmployeeDTO employee)
        {
            if(employee.Photo?.Length>0)
            { 
                string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, employee.Photo.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await employee.Photo.CopyToAsync(fileStream);
                }
            }


            int idxEmployee =  CTX.Employees.FindIndex(m => m.Id.Equals(id));
            if (idxEmployee >= 0)
            {
                CTX.Employees[idxEmployee] = new Employee() { FirstName = employee.FirstName, LastName = employee.LastName, Id = employee.Id };
                return NoContent();
            }
            else
            {
                return BadRequest(new { id = id, Employee = employee });
            }
        }


        [HttpPatch]        
        public async Task<IActionResult> Patch([FromQuery] int id, [FromBody] JsonPatchDocument<Employee> employee)
        {
             if(employee == null)
            {
                _logger.LogError("employee object sent from client is null."); 
                return BadRequest("employee object is null");
            }
            int idxEmployee = CTX.Employees.FindIndex(m => m.Id.Equals(id));
            if (idxEmployee >= 0)
            {
                employee.ApplyTo(CTX.Employees[idxEmployee]);
                return NoContent();
            }
            else
            {
                return BadRequest(new { id = id, Employee = employee });
            }

        }




        [HttpDelete]
        /// <summary>
        /// DELETE api/Employees/{id}
        /// </summary>
        /// <param name="id">The Employee id</param> 
        public IActionResult Delete(int id)
        {
            if(CTX.Employees.Remove(CTX.Employees.SingleOrDefault(e => e.Id.Equals(id))))
            {
                return NoContent();
            }
            else
            {
                return BadRequest(id);
            }
        }


    }
}
