using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Coach
    {
        public List<Seat> Seats { get; } = new List<Seat>();
        public string CoachName { get; }

        public Coach(string coachName)
        {
            CoachName = coachName;
        }

        public void Add(Seat seat)
        {
            Seats.Add(seat);
        }

        public ReservationAttempt BuildReservationAttempt(string trainId, int seatsRequestedCount)
        {
            var availableSeats =
                Seats.Where(s => string.IsNullOrEmpty(s.BookingRef)).Take(seatsRequestedCount).ToList();
            return new ReservationAttempt(trainId, seatsRequestedCount, availableSeats);
        }
    }
}