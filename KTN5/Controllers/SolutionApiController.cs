using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KTN5.Models;
namespace KTN5.Controllers
{
    public class SolutionApiController : ApiController
    {
        dbktnEntities db = new dbktnEntities();
        // GET: api/SolutionApi/2
        public IEnumerable<Solution> Get(int id)
        {
            return db.Solution.Where(m => m.fId == id);
        }

        // GET: api/SolutionApi/5
        //public Solution Get()
        //{
        //    return db.Solution.Where;
        //}

        //// POST: api/SolutionApi
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/SolutionApi/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/SolutionApi/5
        //public void Delete(int id)
        //{
        //}
    }
}
