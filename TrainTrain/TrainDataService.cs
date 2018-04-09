using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TrainTrain
{
    public interface ITrainDataService
    {
        Task<string> GetTrain(string trainId);
    }

    public class TrainDataService : ITrainDataService
    {
        private readonly string _uriTrainDataService;

        public TrainDataService(string uriTrainDataService)
        {
            _uriTrainDataService = uriTrainDataService;
        }

        public async Task<string> GetTrain(string trainId)
        {
            string jsonTrainTopology;
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_uriTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);

                // HTTP GET
                var response = await client.GetAsync($"api/data_for_train/{trainId}");
                response.EnsureSuccessStatusCode();
                jsonTrainTopology = await response.Content.ReadAsStringAsync();
            }
            return jsonTrainTopology;
        }
    }
}