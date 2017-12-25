using System.Threading.Tasks;

namespace TrainTrain.Domain
{
    public interface IReserveSeats
    {
        Task<Reservation> Reserve(string trainId, int seatsRequestedCount);
    }
}