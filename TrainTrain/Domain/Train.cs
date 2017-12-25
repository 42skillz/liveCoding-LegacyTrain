using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainTrain.Domain
{
    public class Train
    {
        public Dictionary<string, Coach> Coaches { get; } = new Dictionary<string, Coach>();
        public int MaxSeat => Seats.Count;
        public int ReservedSeats => Seats.Count(s => !string.IsNullOrEmpty(s.BookingRef));
        public List<Seat> Seats => Coaches.Values.SelectMany(c => c.Seats).ToList();

        public Train(IEnumerable<Seat> seats)
        {
            foreach (var seat in seats)
            {
                if (!Coaches.ContainsKey(seat.CoachName))
                {
                    Coaches[seat.CoachName] = new Coach(seat.CoachName);
                }
                Coaches[seat.CoachName].Add(seat);
            }
        }

        public bool DoesNotExceedOveralTrainCapacity(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= Math.Floor(ThreasholdCapacity.MaxRes * MaxSeat);
        }

        public ReservationAttempt BuildReservationAttempt(string trainId, int seatsRequestedCount)
        {
            foreach (var coach in Coaches.Values)
            {
                var reservationAttempt = coach.BuildReservationAttempt(trainId, seatsRequestedCount);
                if (reservationAttempt.IsFulfilled)
                    return reservationAttempt;
            }
            return new ReservationAttemptFailed(trainId, seatsRequestedCount);
        }
    }
}