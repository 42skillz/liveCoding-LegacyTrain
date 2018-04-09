using System.Threading.Tasks;

namespace TrainTrain
{
    public interface IBookingReferenceService
    {
        Task<string> GetBookingReference();
    }
}