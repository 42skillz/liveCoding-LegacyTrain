using System.Collections.Generic;

namespace TrainTrain
{
    public class Reservation
    {
        public string TrainId { get; }
        public string BookingReference { get; }
        public List<Seat> Seats { get; }

        public Reservation(string trainId, string bookingReference, List<Seat> seats)
        {
            TrainId = trainId;
            BookingReference = bookingReference;
            Seats = seats;
        }
    }
}