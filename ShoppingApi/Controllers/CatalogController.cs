using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoppingApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingApi.Models.Catalog;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;

namespace ShoppingApi.Controllers
{
    public class CatalogController : ControllerBase
    {
        private readonly ShoppingDataContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ShoppingDataContext context, IConfiguration config, IMapper mapper, MapperConfiguration mapperConfiguration, ILogger<CatalogController> logger)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _logger = logger;
        }

        [HttpGet("catalog")]
        public async Task<ActionResult> GetFullCatalog()
        {
            var data = await _context
                .ShoppingItems
                .AsNoTracking()
                .TagWith("catalog#getfullcatalog")
                .Where(item => item.InInventory)
                //.Select(item => new GetCatalogResponseSummaryItem
                //{
                //    Id = item.Id,
                //    Description = item.Description,
                //    Price = item.Cost * _config.GetValue<decimal>("markUp")
                //})
                .ProjectTo<GetCatalogResponseSummaryItem>(_mapperConfiguration)
                .ToListAsync();

            var response = new GetCatalogResponse
            {
                Data = data
            };
            return Ok(response);
        }

        [HttpPost("catalog")]
        public async Task<ActionResult> AddItem([FromBody] PostCatalogRequest newItem)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogInformation("Got a bad request. Looked like this {@newItem}", newItem);
                return BadRequest(ModelState);
            } else
            {
                // what am i missing?
                var item = _mapper.Map<ShoppingItem>(newItem);
                _context.ShoppingItems.Add(item);
                await _context.SaveChangesAsync();
                var response = _mapper.Map<GetCatalogResponseSummaryItem>(item);
                return StatusCode(201, item);
            }
        }
    }
}
