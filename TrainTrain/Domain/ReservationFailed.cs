using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class ReservationFailed : Reservation
    {
        public ReservationFailed(string trainId):base(trainId, string.Empty, new List<Seat>())
        {
        }
    }
}