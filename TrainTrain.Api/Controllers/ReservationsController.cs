using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrainTrain.Infra;

namespace TrainTrain.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReservationsController : Controller
    {
        private readonly SeatsReservationAdapter _seatsReservationAdapter;

        public ReservationsController(SeatsReservationAdapter seatsReservationAdapter)
        {
            _seatsReservationAdapter = seatsReservationAdapter;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/reservations
        [HttpPost]
        public async Task<string> Post([FromBody]ReservationRequestDto reservationRequest)
        {
            return await _seatsReservationAdapter.Post(reservationRequest);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
