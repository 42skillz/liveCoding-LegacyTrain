﻿using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationAttemptFailure: ReservationAttempt
    {
        public ReservationAttemptFailure(string trainId, int seatsRequestedCount) : base(trainId, seatsRequestedCount, new List<Seat>())
        {
        }
    }
}