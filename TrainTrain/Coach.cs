using System;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain
{
    public class Coach: ValueType<Coach>
    {
        public string TrainId { get; }
        public int MaxSeat => Seats.Count;

        public int ReservedSeats
        {
            get { return Seats.Count(s => s.BookingRef != string.Empty); }
        }

        public List<Seat> Seats { get; }
        public string CoachName { get; }


        public Coach(string trainId, string coachName) : this(trainId, coachName, new List<Seat>())
        {
        }

        public Coach(string trainId, string coachName, List<Seat> seats)
        {
            TrainId = trainId;
            CoachName = coachName;
            Seats = seats;
        }

        public Coach AddSeat(Seat seat)
        {
            return new Coach(TrainId, seat.CoachName, new List<Seat>(Seats) {seat});
        }

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount)
        {
            var availableSeats = Seats.Where(s => s.IsAvailable()).Take(seatsRequestedCount).ToList();
            return new ReservationAttempt(TrainId, availableSeats, seatsRequestedCount);
        }

        public bool DoesNotExceedCoachOvervallCapityLimit(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= Math.Floor(ThreasholdManager.GetMaxRes() * MaxSeat);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new List<object> {TrainId, MaxSeat, ReservedSeats, new ListByValue<Seat>(Seats), CoachName};
        }
    }
}