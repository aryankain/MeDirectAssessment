using MeDirectAssessmentAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MeDirectAssessmentAPI.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class LightOffGame : ControllerBase
    {
        private AppSettingsDBContext _context;

        public LightOffGame(AppSettingsDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IDictionary<string, string> Get()
        {
            var list = _context.AppSettings.ToList();
            return list.ToDictionary(x => x.AppSettingKey, y => y.AppSettingValue);
        }
    }
}
