using ShoppingApi.Domain;
using ShoppingApi.Models.Curbside;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public interface IDoCurbsideCommands
    {
        Task<CurbsideOrder> AddOrder(PostCurbsideOrderRequest orderToPlace, bool doAsync = true);
        Task<CurbsideOrder> AddOrderWs(PostCurbsideOrderRequest orderToBePlaced, string connectionId);
    }
}