using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KTN5.Models;
namespace KTN5.Controllers
{
    public class FundApiController : ApiController
    {
        dbktnEntities db = new dbktnEntities();
        // GET: api/FundApi
        public IEnumerable<FundsView> Get()
        {
            List<FundsView> funds = new List<FundsView>();
            var query = (from f in db.Fund
                         join c in db.Charity_Member on f.cId equals c.cId
                         select new
                         {
                             fId = f.fId,
                             fName = f.fName,
                             cName = c.c_name,
                             targetMoney = f.targetMoney,
                             accMoney = f.accMoney,
                             startTime = f.startTime,
                             endTime = f.endTime,
                             fPhoto = f.fPhoto,

                         }).ToList();

            foreach (var item in query)
            {
                funds.Add(new FundsView()
                {
                    fId = item.fId,
                    fName = item.fName,
                    cName = item.cName,
                    accMoney = Convert.ToInt32(item.accMoney),
                    startTime = item.startTime,
                    endTime = item.endTime,
                    countDown = (item.endTime - item.startTime),
                    progress = (item.accMoney / item.targetMoney),
                    fPhoto = item.fPhoto,
                });
            }

            return funds;
        }

        // GET: api/FundApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/FundApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/FundApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FundApi/5
        public void Delete(int id)
        {
        }
    }
}
