using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Domain;
using ShoppingApi.Models.Curbside;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class EntityFrameworkCurbsideData : IDoCurbsideQueries, IDoCurbsideCommands
    {
        private readonly ShoppingDataContext _context;
        private readonly IMapper _mapper;
        private readonly CurbsideChannel _channel;

        public EntityFrameworkCurbsideData(ShoppingDataContext context, IMapper mapper, CurbsideChannel channel)
        {
            _context = context;
            _mapper = mapper;
            _channel = channel;
        }

        public async Task<CurbsideOrder> AddOrder(PostCurbsideOrderRequest orderToPlace)
        {

            var order = _mapper.Map<CurbsideOrder>(orderToPlace);
            _context.CurbsideOrders.Add(order);
            await _context.SaveChangesAsync();
            try
            {
                await _channel.AddCurbside(new CurbsideChannelRequest { OrderId = order.Id });
            } catch(OperationCanceledException ex)
            {
                // do something?
                throw;
            }
            return order;
        }

        public async Task<GetCurbsideOrdersResponse> GetAll()
        {
            var response = new GetCurbsideOrdersResponse();
            var data = await _context.CurbsideOrders.ToListAsync();
            response.Data = data;
            response.NumberOfApprovedOrders = response.Data.Count(o => o.Status == CurbsideOrderStatus.Approved);
            response.NumberOfDeniedOrders = response.Data.Count(o => o.Status == CurbsideOrderStatus.Denied);
            response.NumberOfFulfilledOrders = response.Data.Count(o => o.Status == CurbsideOrderStatus.Fulfilled);
            response.NumberOfPendingOrders = response.Data.Count(o => o.Status == CurbsideOrderStatus.Pending);

            return response;
        }

        public async Task<CurbsideOrder> GetById(int orderId)
        {
            return await _context.CurbsideOrders.FindAsync(orderId);
        }
    }
}
