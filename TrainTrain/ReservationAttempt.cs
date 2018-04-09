using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationAttempt
    {
        public string TrainId { get; }
        public string BookingReference { get; private set; }
        public List<Seat> Seats { get; private set; }
        private readonly int _seatsRequestedCount;
        public bool IsFulFilled => Seats.Count == _seatsRequestedCount;

        public ReservationAttempt(string trainId, List<Seat> seats, int seatsRequestedCount)
        {
            _seatsRequestedCount = seatsRequestedCount;
            TrainId = trainId;
            Seats = seats;
        }

        public void AssignBookingReference(string bookingReference)
        {
            BookingReference = bookingReference;
            var seats = new List<Seat>();

            foreach (var seat in Seats)
            {
                seats.Add(new Seat(seat.CoachName, seat.SeatNumber, seat.BookingRef));
            }
            Seats = seats;
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }
    }
}