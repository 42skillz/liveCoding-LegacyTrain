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
            var trainDataService = BuildTrainDataService(TrainTopologyGenerator.Get_train_with_10_available_seats());
            var bookingReferenceService = BuilderBookingReferenceService();

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);

            var reservation = webTicketManager.Reserve(TrainId, 3).Result;

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation))
                .IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1A\", \"2A\", \"3A\"]}}");
        }

        [Test]
        public void Not_reserve_seats_when_it_exceed_max_capacity_threshold()
        {
            var trainDataService = BuildTrainDataService(TrainTopologyGenerator.Get_train_with_10_seats_with_6_already_reserved());
            var bookingReferenceService = BuilderBookingReferenceService();

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);

            var reservation = webTicketManager.Reserve(TrainId, 3).Result;

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation))
                .IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }

        [Test]
        public void Reserve_all_seats_in_the_same_coach()
        {
            var trainDataService = BuildTrainDataService(TrainTopologyGenerator.Get_train_with_2_coaches_and_9_seats_already_reserved_in_the_first_coach());
            var bookingReferenceService = BuilderBookingReferenceService();

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);

            var reservation = webTicketManager.Reserve(TrainId, 3).Result;

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation))
                .IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1B\", \"2B\", \"3B\"]}}");
        }

        [Test]
        public void Ideally_not_reserve_seats_in_a_coach_if_it_exceed_70_percent_of_its_capacity()
        {
            var trainDataService = BuildTrainDataService(TrainTopologyGenerator.Get_train_with_3_coaches_and_5_seats_already_reserved_in_the_first_coach());
            var bookingReferenceService = BuilderBookingReferenceService();

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);

            var reservation = webTicketManager.Reserve(TrainId, 3).Result;

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation))
                .IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1B\", \"2B\", \"3B\"]}}");
        }

        [Test]
        public void Reserve_seats_in_a_coach_even_if_it_exceed_70_percent_of_its_ideal_capacity_when_there_is_no_other_option_left()
        {
            var trainDataService = BuildTrainDataService(TrainTopologyGenerator.Get_train_with_3_coaches_and_6_seats_and_4_already_reserved());
            var bookingReferenceService = BuilderBookingReferenceService();

            var webTicketManager = new WebTicketManager(trainDataService, bookingReferenceService);

            var reservation = webTicketManager.Reserve(TrainId, 6).Result;

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation))
                .IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"5B\", \"6B\", \"7B\", \"8B\", \"9B\", \"10B\"]}}");
        }

        private static IBookingReferenceService BuilderBookingReferenceService()
        {
            var bookingReferenceService = Substitute.For<IBookingReferenceService>();
            bookingReferenceService.GetBookingReference().Returns(Task.FromResult(BookingReference));
            return bookingReferenceService;
        }

        private static ITrainDataService BuildTrainDataService(string trainTopology)
        {
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(TrainId)
                .Returns(Task.FromResult(new Train(TrainId, TrainDataService.AdaptTrainTopology(trainTopology))));
            return trainDataService;
        }
    }
}
