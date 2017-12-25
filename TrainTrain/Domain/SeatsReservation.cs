using System.Threading.Tasks;

namespace TrainTrain.Domain
{
    public class SeatsReservation : IReserveSeats
    {
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;

        public SeatsReservation(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }

        public async Task<Reservation> Reserve(string trainId, int seatsRequestedCount)
        {
            var train = await _trainDataService.GetTrain(trainId);

            if (train.DoesNotExceedOveralTrainCapacity(seatsRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(trainId, seatsRequestedCount);

                if (reservationAttempt.IsFulfilled)
                {
                    var bookingReference = await _bookingReferenceService.GetBookingReference();

                    reservationAttempt.AssignBookingReference(bookingReference);

                    await _trainDataService.BookSeats(reservationAttempt);

                    return reservationAttempt.Confirm();
                }
            }
            return new ReservationFailed(trainId);
        }
    }
}