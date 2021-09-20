using System.Drawing;
using System.Threading.Tasks;
using Yugioh.Sync.Ygopro.Responses;

namespace Yugioh.Sync.Ygopro.Clients
{
    public interface IYgoproClient
    {
        Task<GetCardsResponse> GetCardsAsync();
        Task<Image> GetImageAsync(int imageId);
    }
}
