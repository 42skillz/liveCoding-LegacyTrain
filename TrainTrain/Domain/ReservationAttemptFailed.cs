using System.Collections.Generic;

namespace TrainTrain.Domain
{
    public class ReservationAttemptFailed : ReservationAttempt
    {
        public ReservationAttemptFailed(string trainId, int seatsRequestedCount) :base(trainId, seatsRequestedCount, new List<Seat>())
        {
        }
    }
}