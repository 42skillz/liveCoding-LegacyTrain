using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainTrain.Domain;

namespace TrainTrain.Infra
{
    public class TrainDataService : ITrainDataService
    {
        private readonly string _uriTrainDataService;

        public TrainDataService(string uriTrainDataService)
        {
            _uriTrainDataService = uriTrainDataService;
        }

        public async Task<Train> GetTrain(string train)
        {
            string jsonTrainTopology;
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_uriTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);
                // HTTP GET
                var response = await client.GetAsync($"api/data_for_train/{train}");
                response.EnsureSuccessStatusCode();
                jsonTrainTopology = await response.Content.ReadAsStringAsync();
            }
            return new Train(AdaptTrainTopology(jsonTrainTopology));
        }

        public async Task BookSeats(ReservationAttempt reservationAttempt)
        {
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_uriTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);
                // HTTP POST
                HttpContent resJson = new StringContent(BuildPostContent(reservationAttempt.TrainId, reservationAttempt.BookingReference, reservationAttempt.Seats),
                    Encoding.UTF8, "application/json");
                var response = await client.PostAsync("reserve", resJson);

                response.EnsureSuccessStatusCode();
            }
        }

        public static string BuildPostContent(string trainId, string bookingRef, IEnumerable<Seat> availableSeats)
        {
            var seats = new StringBuilder("[");
            bool firstTime = true;

            foreach (var s in availableSeats)
            {
                if (!firstTime)
                {
                    seats.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                seats.Append($"\"{s.SeatNumber}{s.CoachName}\"");
            }
            seats.Append("]");

            var result = $"{{\r\n\t\"train_id\": \"{trainId}\",\r\n\t\"seats\": {seats.ToString()},\r\n\t\"booking_reference\": \"{bookingRef}\"\r\n}}";

            return result;
        }

        public static List<Seat> AdaptTrainTopology(string trainTopology)
        {
            var seats = new List<Seat>();
            //var sample =
            //"{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";

            // Forced to workaround with dynamic parsing since the received JSON is invalid format ;-(
            dynamic parsed = JsonConvert.DeserializeObject(trainTopology);

            foreach (var token in ((Newtonsoft.Json.Linq.JContainer) parsed))
            {
                var allStuffs = ((Newtonsoft.Json.Linq.JObject) ((Newtonsoft.Json.Linq.JContainer) token).First);

                foreach (var stuff in allStuffs)
                {
                    var seat = stuff.Value.ToObject<SeatJsonPoco>();
                    seats.Add(new Seat(seat.coach, Int32.Parse(seat.seat_number), seat.booking_reference));
                }
            }
            return seats;
        }
    }
}