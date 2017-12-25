using System.Threading.Tasks;

namespace TrainTrain.Domain
{
    public interface ITrainDataService
    {
        Task<Train> GetTrain(string train);
        Task BookSeats(ReservationAttempt reservationAttempt);
    }
}