﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain
{
    public class WebTicketManager
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainCaching _trainCaching;
        private readonly ITrainDataService _trainDataService;

        public WebTicketManager():this(new TrainDataService(UriTrainDataService))
        {

        }

        public WebTicketManager(ITrainDataService trainDataService)
        {
            _trainDataService = trainDataService;
            _trainCaching = new TrainCaching();
            _trainCaching.Clear();
        }

        public async Task<string> Reserve(string trainId, int seatsRequestedCount)
        {
            // get the train
            var jsonTrain = await _trainDataService.GetTrain(trainId);
            var train = new Train(jsonTrain);

            if ((train.ReservedSeats + seatsRequestedCount) <= Math.Floor(ThreasholdManager.GetMaxRes() * train.GetMaxSeat()))
            {
                var availableSeats = new List<Seat>();
                var numberOfReserv = 0;
                // find seats to reserve
                for (int index = 0, i = 0; index < train.Seats.Count; index++)
                {
                    var seat = train.Seats[index];
                    if (seat.BookingRef == "")
                    {
                        i++;
                        if (i <= seatsRequestedCount)
                        {
                            availableSeats.Add(seat);
                        }
                    }
                }

                int count = 0;
                foreach (var unused in availableSeats)
                {
                    count++;
                }
                string bookingRef;
                if (count != seatsRequestedCount)
                {
                    return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
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
                    }
                }

                if (numberOfReserv == seatsRequestedCount)
                {
                    await _trainCaching.Save(trainId, train, bookingRef);

                    using (var client = new HttpClient())
                    {
                        var value = new MediaTypeWithQualityHeaderValue("application/json");
                        client.BaseAddress = new Uri(UriTrainDataService);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(value);

                        // HTTP POST
                        HttpContent resJson = new StringContent(BuildPostContent(trainId, bookingRef, availableSeats), Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("reserve", resJson);
                        response.EnsureSuccessStatusCode();


                        return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bookingRef}\", \"seats\": {DumpSeats(availableSeats)}}}";
                    }
                }
            }

            return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
        }

        private static string DumpSeats(IEnumerable<Seat> seats)
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

                sb.Append($"\"{seat.SeatNumber}{seat.CoachName}\"");
            }

            sb.Append("]");

            return sb.ToString();
        }

        private static string BuildPostContent(string trainId, string bookingRef, IEnumerable<Seat> availableSeats)
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

            var result = $"{{\r\n\t\"train_id\": \"{trainId}\",\r\n\t\"seats\": {seats},\r\n\t\"booking_reference\": \"{bookingRef}\"\r\n}}";

            return result;
        }

        protected async Task<string> GetBookRef(HttpClient client)
        {
            var value = new MediaTypeWithQualityHeaderValue("application/json");
            client.BaseAddress = new Uri(UriBookingReferenceService);
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