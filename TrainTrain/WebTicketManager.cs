using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Dal;
using TrainTrain.Dal.Entities;

namespace TrainTrain
{
    public class WebTicketManager
    {
        private const string uriBookingReferenceService = "http://localhost:51691/";
        private const string urITrainDataService = "http://localhost:50680";
        private ITrainCaching _trainCaching;
        public WebTicketManager()
        {
            _trainCaching = new TrainCaching();
            _trainCaching.Clear();
        }
        public async Task<string> Reserve(string train, int seats)
        {
            List<Seat> availableSeats = new List<Seat>();
            int count = 0;
            var result = string.Empty;
            string bookingRef;

            // get the train
            var JsonTrain = await GetTrain(train);

            result = JsonTrain;

            var trainInst = new Train(JsonTrain);
            if ((trainInst.ReservedSeats + seats) <= Math.Floor(ThreasholdManager.GetMaxRes() * trainInst.GetMaxSeat()))
            {
                var numberOfReserv = 0;
                // find seats to reserve
                for (int index = 0, i = 0; index < trainInst.Seats.Count; index++)
                {
                    var each = trainInst.Seats[index];
                    if (each.BookingRef == "")
                    {
                        i++;
                        if (i <= seats)
                        {
                            availableSeats.Add(each);
                        }
                    }
                }

                foreach (var a in availableSeats)
                {
                    count++;
                }

                var reservedSets = 0;


                if (count != seats)
                {
                    return string.Format("{{\"train_id\": \"{0}\", \"booking_reference\": \"\", \"seats\": []}}",
                        train);
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        bookingRef = await GetBookRef(client);
                    }

                    foreach (var availableSeat in availableSeats)
                    {
                        availableSeat.BookingRef = bookingRef;
                        numberOfReserv++;
                        reservedSets++;
                    }
                }

                if (numberOfReserv == seats)
                {
                    await _trainCaching.Save(train, trainInst, bookingRef);

                    using (var client = new HttpClient())
                    {
                        var value = new MediaTypeWithQualityHeaderValue("application/json");
                        client.BaseAddress = new Uri(urITrainDataService);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(value);

                        if (reservedSets == 0)
                        {
                            Console.WriteLine("Reserved seat(s): ", reservedSets);
                        }

                        // HTTP POST
                        HttpContent resJSON = new StringContent(buildPostContent(train, bookingRef, availableSeats), Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("reserve", resJSON);

                        response.EnsureSuccessStatusCode();

                        var todod = "[TODOD]";


                        return string.Format(
                            "{{\"train_id\": \"{0}\", \"booking_reference\": \"{1}\", \"seats\": {2}}}", 
                            train,
                            bookingRef, 
                            dumpSeats(availableSeats));
                    }
                }
            }

            return string.Format("{{\"train_id\": \"{0}\", \"booking_reference\": \"\", \"seats\": []}}", train);
        }

        private string dumpSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");

            var firstTime = true;
            foreach (var seat in seats)
            {
                if (!firstTime)
                {
                    sb.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                sb.Append(string.Format("\"{0}{1}\"", seat.SeatNumber, seat.CoachName));
            }

            sb.Append("]");

            return sb.ToString();
        }

        private static string buildPostContent(string trainId, string bookingRef, IEnumerable<Seat> availableSeats)
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

                seats.Append(string.Format("\"{0}{1}\"", s.SeatNumber, s.CoachName));
            }
            seats.Append("]");

            var result = string.Format(
                "{{\r\n\t\"train_id\": \"{0}\",\r\n\t\"seats\": {1},\r\n\t\"booking_reference\": \"{2}\"\r\n}}",
                trainId, seats.ToString(), bookingRef);

            return result;
        }

        protected async Task<string> GetTrain(string train)
        {
            string JsonTrainTopology;
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(urITrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);

                // HTTP GET
                var response = await client.GetAsync(string.Format("api/data_for_train/{0}", train));
                response.EnsureSuccessStatusCode();
                JsonTrainTopology = await response.Content.ReadAsStringAsync();
            }
            return JsonTrainTopology;
        }

        protected async Task<string> GetBookRef(HttpClient client)
        {
            var value = new MediaTypeWithQualityHeaderValue("application/json");
            client.BaseAddress = new Uri(uriBookingReferenceService);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(value);

            // HTTP GET
            var response = await client.GetAsync("/booking_reference");
            response.EnsureSuccessStatusCode();

            var bookingRef = await response.Content.ReadAsStringAsync();
            return bookingRef;
        }
    }
}