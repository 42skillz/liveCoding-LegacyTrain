namespace TrainTrain.Test
{
    public struct GenerateParams
    {
        public GenerateParams(int coachesCount, int seatsCountPerCoach, string bookingReference = "", int seatsAlreadyReservedCount = 0, int coachNumberWhereReserved = 1)
        {
            CoachesCount = coachesCount;
            SeatsCountPerCoach = seatsCountPerCoach;
            BookingReference = bookingReference;
            SeatsAlreadyReservedCount = seatsAlreadyReservedCount;
            CoachNumberWhereReserved = coachNumberWhereReserved;
        }

        public int CoachesCount { get; set; }
        public int SeatsCountPerCoach { get; set; }
        public string BookingReference { get; set; }
        public int SeatsAlreadyReservedCount { get; set; }
        public int CoachNumberWhereReserved { get; set; }
    }
}