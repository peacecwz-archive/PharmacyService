using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PharmacyService.Controllers
{
    [RoutePrefix("api/pharmacies")]
    public class PharmaciesController : ApiController
    {
        [Route("{city}")]
        public async Task<IHttpActionResult> Get(string city)
        {
            var result = await Services.PharmacyService.Instance.GetList(city.ClearText());
            if (result.Count == 0) return NotFound();
            else return Ok(result);
        }
    }
}
