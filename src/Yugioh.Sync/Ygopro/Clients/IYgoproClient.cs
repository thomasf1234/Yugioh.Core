using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Yugioh.Sync.Ygopro.Entities;
using Yugioh.Sync.Ygopro.Responses;

namespace Yugioh.Sync.Ygopro.Clients
{
    public interface IYgoproClient
    {
        Task<GetCardsResponse> GetCardsAsync();
        Task<Image> GetImageAsync(int imageId);
        Task<List<CardSetEntity>> GetCardSetsAsync();
        Task<CardSetInfoEntity> GetCardSetInfoAsync(string cardNumber);
    }
}
