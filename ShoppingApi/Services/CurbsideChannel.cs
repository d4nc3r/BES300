using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class CurbsideChannel
    {
        private const int MaxMessagesInChannel = 100;
        private readonly Channel<CurbsideChannelRequest> _theChannel;
        private readonly ILogger<CurbsideChannel> _logger;

        public CurbsideChannel(ILogger<CurbsideChannel> logger)
        {
            _logger = logger;
            var options = new BoundedChannelOptions(MaxMessagesInChannel)
            {
                SingleReader = true,
                SingleWriter = false
            };
            _theChannel = Channel.CreateBounded<CurbsideChannelRequest>(options);
        }

        public async Task<bool> AddCurbside(CurbsideChannelRequest order, CancellationToken ct = default)
        {
            while(await _theChannel.Writer.WaitToWriteAsync(ct) && !ct.IsCancellationRequested)
            {
                if(_theChannel.Writer.TryWrite(order))
                {
                    return true;
                }
            }
            return false;
        }

        public IAsyncEnumerable<CurbsideChannelRequest> ReadAllAsync(CancellationToken ct = default) => _theChannel.Reader.ReadAllAsync(ct);
    }
}
