using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ShoppingApi.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class AdminController : ControllerBase
    {
        private readonly IOptions<ConfigurationForMapper> _options;
        IConfigurationRoot _configRoot;

        public AdminController(IConfiguration config, IOptions<ConfigurationForMapper> options)
        {
            _configRoot = (IConfigurationRoot)config;
            _options = options;
        }

        [HttpGet("admin/config")]
        public ActionResult GetConfig()
        {
            return Ok(_configRoot.Providers.Select(p => p.ToString()).ToList());
        }

        [HttpGet("admin/markup")]
        public ActionResult GetMarkup()
        {
            return Ok($"The Markup is {_options.Value.markUp:P}");
        }
    }
}
