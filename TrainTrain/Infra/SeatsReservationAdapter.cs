using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;

namespace TrainTrain.Infra
{
    public class SeatsReservationAdapter
    {
        private readonly IReserveSeats _hexagon;

        public SeatsReservationAdapter(IReserveSeats hexagon)
        {
            _hexagon = hexagon;
        }

        public async Task<string> Post(ReservationRequestDto reservationRequestDto)
        {
            // Adapt from infra to domain
            var trainId = reservationRequestDto.train_id;
            var numberOfSeats = reservationRequestDto.number_of_seats;
            // Call business logic
            var reservation = await _hexagon.Reserve(trainId, numberOfSeats);
            // Adapt from domain to infra
            return AdaptReservation(reservation);
        }

        public static string AdaptReservation(Reservation reservation)
        {
            return $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingReference}\", \"seats\": {DumpSeats(reservation.Seats)}}}";
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
    }
}