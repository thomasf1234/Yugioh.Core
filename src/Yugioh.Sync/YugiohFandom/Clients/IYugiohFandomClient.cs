using System.Threading.Tasks;
using Yugioh.Sync.YugiohFandom.Entities;

namespace Yugioh.Sync.YugiohFandom.Clients
{
    public interface IYugiohFandomClient
    {
        Task<CardEntity> GetCardAsync(string passcode);
    } 
}
