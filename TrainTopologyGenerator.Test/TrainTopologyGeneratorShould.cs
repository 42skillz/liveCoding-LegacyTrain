using NFluent;
using NUnit.Framework;

namespace TrainTrain.Test
{
    public class TrainTopologyGeneratorShould
    {
        [Test]
        public void Generate_10_available_seats()
        {
            var generateParams = new GenerateParams
            {
                CoachesCount = 1,
                SeatsCountPerCoach = 10
            };

            Check.That(TrainTopologyGenerator.Generate(generateParams))
                .IsEqualTo(TrainTopologyStaticGenerator.With_10_available_seats());
        }
        [Test]
        public void Generate_10_seats_and_6_already_reserved()
        {
            var generateParams = new GenerateParams
            {
                CoachesCount = 1,
                SeatsCountPerCoach = 10,
                SeatsAlreadyReservedCount = 6,
                BookingReference = "75bcd16"
            };

            Check.That(TrainTopologyGenerator.Generate(generateParams))
                .IsEqualTo(TrainTopologyStaticGenerator.With_10_seats_and_6_already_reserved());
        }

        [Test]
        public void Generate_2_coaches_and_9_seats_already_reserved_in_the_first_coach()
        {
            var generateParams = new GenerateParams
            {
                CoachesCount = 2,
                SeatsCountPerCoach = 10,
                SeatsAlreadyReservedCount = 9,
                BookingReference = "75bcd16",
                CoachNumberWhereReserved = 1
            };

            Check.That(TrainTopologyGenerator.Generate(generateParams))
                .IsEqualTo(TrainTopologyStaticGenerator.With_2_coaches_and_9_seats_already_reserved_in_the_first_coach());
        }
    }
}
