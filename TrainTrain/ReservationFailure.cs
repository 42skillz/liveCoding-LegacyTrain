using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationFailure : Reservation
    {
        public ReservationFailure(string trainId) : base(trainId, string.Empty, new List<Seat>())
        {
        }
    }
}