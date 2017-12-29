using System;
using System.Text;

namespace TrainTrain.Test
{
    public class TrainTopologyGenerator
    {
        public static string Generate(GenerateParams generateParams)
        {
            var jsonTrainTopology = new StringBuilder("{\"seats\": {");
            generateParams.CoachNumberWhereReserved =
                generateParams.CoachesCount == 1 ? 1 : generateParams.CoachNumberWhereReserved;

            for (var coachNumber = 1; coachNumber <= generateParams.CoachesCount; coachNumber++)
            {
                var coachName = Convert.ToChar('A' + coachNumber - 1).ToString();

                if (!coachNumber.Equals(generateParams.CoachNumberWhereReserved))
                {
                    generateParams.BookingReference = string.Empty;
                }

                jsonTrainTopology.Append(GenerateCoach(coachName, generateParams.SeatsCountPerCoach, generateParams.SeatsAlreadyReservedCount, generateParams.BookingReference));

                jsonTrainTopology.Append(coachNumber + 1 <= generateParams.CoachesCount ? "," : "");
            }

            jsonTrainTopology.Append("}}");

            return jsonTrainTopology.ToString();
        }

        private static string GenerateCoach(string coachName, int seatsCountPerCoach, int seatsReservedCount, string bookingReference)
        {
            var jsonCoachTopology = new StringBuilder();

            var reservedSeats = 1;

            for (var seatNumber = 1; seatNumber <= seatsCountPerCoach; seatNumber++)
            {
                if (!string.IsNullOrEmpty(bookingReference))
                {
                    reservedSeats++;
                    bookingReference = reservedSeats <= seatsReservedCount + 1 ? bookingReference : string.Empty;
                }
                else
                {
                    bookingReference = string.Empty;
                }

                jsonCoachTopology.Append(
                    $"\"{seatNumber}{coachName}\": {{\"booking_reference\": \"{bookingReference}\", \"seat_number\": \"{seatNumber}\", \"coach\": \"{coachName}\"}}");

                jsonCoachTopology.Append(seatNumber + 1 <= seatsCountPerCoach ? ", " : "");
            }
            return jsonCoachTopology.ToString();
        }
    }
}
