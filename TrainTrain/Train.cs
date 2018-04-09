using System;
using System.Collections.Generic;
using System.Linq;
using Value;
using Value.Shared;

namespace TrainTrain
{
    public class Train: ValueType<Train>
    {
        public string TrainId { get; }
        public Dictionary<string, Coach> Coaches { get; } = new Dictionary<string, Coach>();
        public int MaxSeat => Seats.Count;

        public int ReservedSeats
        {
            get { return Seats.Count(s => s.BookingRef != string.Empty); }
        }

        public List<Seat> Seats
        {
            get { return Coaches.SelectMany(c => c.Value.Seats).ToList(); }
        }

        public Train(string trainId, IEnumerable<Seat> seats)
        {
            TrainId = trainId;
            foreach (var seat in seats)
            {
                if (!Coaches.ContainsKey(seat.CoachName))
                {
                   Coaches[seat.CoachName] = new Coach(trainId, seat.CoachName);
                }

                var coach = Coaches[seat.CoachName].AddSeat(seat);
                Coaches[seat.CoachName] = coach;
            }
        }

        public bool DoesNotExceedTrainOvervallCapityLimit(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= Math.Floor(ThreasholdManager.GetMaxRes() * MaxSeat);
        }

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount)
        {
            ReservationAttempt reservationAttempt = new ReservationAttemptFailure(TrainId, seatsRequestedCount);

            foreach (var coach in Coaches.Values)
            {
                if (coach.DoesNotExceedCoachOvervallCapityLimit(seatsRequestedCount))
                {
                    reservationAttempt = coach.BuildReservationAttempt(seatsRequestedCount);

                    if (reservationAttempt.IsFulFilled)
                    {
                        return reservationAttempt;
                    }
                }
            }

            if (!reservationAttempt.IsFulFilled)
            {
                foreach (var coach in Coaches.Values)
                {
                    reservationAttempt = coach.BuildReservationAttempt(seatsRequestedCount);
                    if (reservationAttempt.IsFulFilled)
                    {
                        return reservationAttempt;
                    }
                }
            }
            return reservationAttempt;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new List<object> {TrainId, new DictionaryByValue<string, Coach>(Coaches), MaxSeat};
        }
    }
}