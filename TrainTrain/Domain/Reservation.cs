using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class Reservation
    {
        public string TrainId { get; }
        public string BookingReference { get; }
        public List<Seat> Seats { get; }

        public Reservation(string trainId, string bookingReference, List<Seat> seats)
        {
            Seats = seats;
            TrainId = trainId;
            BookingReference = bookingReference;
        }
    }
}