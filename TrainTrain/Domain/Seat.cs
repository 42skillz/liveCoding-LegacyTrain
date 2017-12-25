namespace TrainTrain.Domain
{
    public class Seat
    {
        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get; set;  }

        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            CoachName = coachName;
            SeatNumber = seatNumber;
            BookingRef = bookingRef;
        }
    }
}