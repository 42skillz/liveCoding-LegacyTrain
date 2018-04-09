using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace TrainTrain.Test.Acceptance
{
    public class TrainTrainShould
    {
        private const string TrainId = "9043-2018-04-18";
        private const string BookingReference = "75bcd15";

        [Test]
        public void Reserve_seats_when_the_train_is_empty()
        {
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(TrainId).Returns(Task.FromResult(
                "{\"seats\": {" +
                "\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, " +
                "\"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}," +
                "\"3A\": {\"booking_reference\": \"\", \"seat_number\": \"3\", \"coach\": \"A\"}," +
                "\"4A\": {\"booking_reference\": \"\", \"seat_number\": \"4\", \"coach\": \"A\"}," +
                "\"5A\": {\"booking_reference\": \"\", \"seat_number\": \"5\", \"coach\": \"A\"}," +
                "\"6A\": {\"booking_reference\": \"\", \"seat_number\": \"6\", \"coach\": \"A\"}," +
                "\"7A\": {\"booking_reference\": \"\", \"seat_number\": \"7\", \"coach\": \"A\"}," +
                "\"8A\": {\"booking_reference\": \"\", \"seat_number\": \"8\", \"coach\": \"A\"}," +
                "\"9A\": {\"booking_reference\": \"\", \"seat_number\": \"9\", \"coach\": \"A\"}," +
                "\"10A\": {\"booking_reference\": \"\", \"seat_number\": \"10\", \"coach\": \"A\"}" +
                "}}"));

            var webTicketManager = new WebTicketManager(trainDataService);

            var reservation = webTicketManager.Reserve(TrainId, 3).Result;

            Check.That(reservation).IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1A\", \"2A\", \"3A\"]}}");
        }
    }
}
