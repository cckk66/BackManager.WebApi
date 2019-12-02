using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitOfWork;

namespace BackManager.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("/api/get1")]
        public IActionResult Get1()
        {
            return Ok(new User { ID = 12, Name = "sssssssssssss" });
        }

        //[HttpGet]
        //[Route("/api/get2")]
        //public IActionResult Get2()
        //{
        //    string connection = @"Data Source=localhost;port=4406;Initial Catalog=magicadmin;uid=root;password=123456;AllowLoadLocalInfile=true";
        //    DbContextOptions<UnitOfWorkDbContext> dbContextOption = new DbContextOptions<UnitOfWorkDbContext>();
        //    DbContextOptionsBuilder<UnitOfWorkDbContext> dbContextOptionBuilder = new DbContextOptionsBuilder<UnitOfWorkDbContext>(dbContextOption);
        //    using (UnitOfWorkDbContext _dbContext = new UnitOfWorkDbContext(dbContextOptionBuilder.UseMySql(connection).Options))
        //    {

        //        return Ok(_dbContext.SysUsers.ToList());
        //    }

        //    //return Ok(new User { ID = 12, Name = "sssssssssssss" });
        //}
    }
    public class User
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
}
