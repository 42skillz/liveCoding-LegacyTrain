using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TrainTrain
{
    public class BookingReferenceService : IBookingReferenceService
    {
        private readonly string _uriBookingReferenceService;

        public BookingReferenceService(string uriBookingReferenceService)
        {
            _uriBookingReferenceService = uriBookingReferenceService;
        }

        public async Task<string> GetBookingReference()
        {
            string bookingRef;
            using (var client = new HttpClient())
            {
                bookingRef = await GetBookRef(client);
            }

            return bookingRef;
        }

        public async Task<string> GetBookRef(HttpClient client)
        {
            var value = new MediaTypeWithQualityHeaderValue("application/json");
            client.BaseAddress = new Uri(_uriBookingReferenceService);
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