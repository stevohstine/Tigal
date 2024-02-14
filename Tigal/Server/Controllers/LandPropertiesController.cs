using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tigal.Server.Data;
using Tigal.Server.Services;
using Tigal.Shared;

namespace Tigal.Server.Controllers
{
    [ApiController]
    [Authorize]
    public class LandPropertiesController : ControllerBase
    {
        private readonly ManagementDBContext _context;
        private readonly IJWTGenerator _jwtGenerator;
        private readonly IUserAccessor _userAccessor;
        private ILogger<LandPropertiesController> _logger;

        public LandPropertiesController(ILogger<LandPropertiesController> logger, IJWTGenerator jwtGenerator, ManagementDBContext context, IUserAccessor userAccessor)
        {
            _jwtGenerator = jwtGenerator;
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
        }
    }
}