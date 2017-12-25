using System.Threading.Tasks;

namespace TrainTrain.Domain
{
    public interface IBookingReferenceService
    {
        Task<string> GetBookingReference();
    }
}