using System.Collections.Generic;
using Yugioh.Sync.Ygopro.Entities;

namespace Yugioh.Sync.Ygopro.Responses
{
    public class GetCardsResponse
    {
        public List<CardEntity> Data { get; set; }
    }
}
