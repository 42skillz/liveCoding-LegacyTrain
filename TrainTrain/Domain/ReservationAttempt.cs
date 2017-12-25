using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class ReservationAttempt
    {
        public string TrainId { get; }
        public string BookingReference { get; private set; }
        public List<Seat> Seats { get; }
        public bool IsFulfilled => Seats.Count == _seatsRequestedCount;

        private readonly int _seatsRequestedCount;

        public ReservationAttempt(string trainId, int seatsRequestedCount, List<Seat> seats)
        {
            _seatsRequestedCount = seatsRequestedCount;
            TrainId = trainId;
            Seats = seats;
        }

        public void AssignBookingReference(string bookingReference)
        {
            BookingReference = bookingReference;
            foreach (var availableSeat in Seats)
            {
                availableSeat.BookingRef = bookingReference;
            }
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }
    }
}