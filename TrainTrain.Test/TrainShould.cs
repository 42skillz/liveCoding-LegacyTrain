using NFluent;
using NUnit.Framework;
using TrainTrain.Test.Acceptance;

namespace TrainTrain.Test
{
    class TrainShould
    {
        private const string TrainId = "9043-2018-04-18";

        [Test]
        public void Expose_Coaches()
        {
            var train = new Train(TrainId, TrainDataService.AdaptTrainTopology(TrainTopologyGenerator
                .Get_train_with_2_coaches_and_9_seats_already_reserved_in_the_first_coach()));
            Check.That(train.Coaches).HasSize(2);
            Check.That(train.Coaches["A"].Seats).HasSize(10);
            Check.That(train.Coaches["B"].Seats).HasSize(10);
        }

        [Test]
        public void Should_be_an_immutable_value_type()
        {
            var train = new Train(TrainId, TrainDataService.AdaptTrainTopology(TrainTopologyGenerator
                .Get_train_with_2_coaches_and_9_seats_already_reserved_in_the_first_coach()));

            var otherTrain = new Train(TrainId, TrainDataService.AdaptTrainTopology(TrainTopologyGenerator
                .Get_train_with_2_coaches_and_9_seats_already_reserved_in_the_first_coach()));

            Check.That(train).IsEqualTo(otherTrain);
        }
    }
}
