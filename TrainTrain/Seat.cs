using System;
using System.Collections.Generic;
using Value;

namespace TrainTrain
{
    public class Seat: ValueType<Seat>
    {
        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get; }

        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            CoachName = coachName;
            SeatNumber = seatNumber;
            BookingRef = bookingRef;
        }

        public bool IsAvailable()
        {
            return BookingRef == "";
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new List<object> {CoachName, SeatNumber, BookingRef};
        }
    }
}