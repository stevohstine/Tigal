using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tigal.Server.Data;
using Tigal.Server.Models;
using Tigal.Server.Services;
using Tigal.Server.Utils;
using Tigal.Shared;
using Tigal.Shared.Models;
using Tigal.Shared.Models.Houses;

namespace Tigal.Server.Controllers
{
    [ApiController]
    [Authorize]
    public class PropertiesController : ControllerBase
    {
        private readonly ManagementDBContext _context;
        private readonly IJWTGenerator _jwtGenerator;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private ILogger<PropertiesController> _logger;

        public PropertiesController(IMapper mapper, ILogger<PropertiesController> logger, IJWTGenerator jwtGenerator, ManagementDBContext context, IUserAccessor userAccessor)
        {
            _jwtGenerator = jwtGenerator;
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("/api/v1/properties/get/all")]
        public async Task<List<GetPropertyDTO>> GetHouses()
        {
            List<GetPropertyDTO> allProperties = new List<GetPropertyDTO>();
            var properties = await _context.Houses.ToListAsync();

            foreach(var property in properties)
            {
                GetPropertyDTO propertyDTO = new GetPropertyDTO();
                propertyDTO.PropertyImages = new List<PropertyImages>();
                propertyDTO.propertyComments = new List<PropertyComments>();
                var images  = await _context.PropertyImages.Where(x => x.PropertyID == property.Id).ToListAsync();
                var comments  = await _context.PropertyComments.Where(x => x.PropertyID == property.Id).ToListAsync();
                propertyDTO =  _mapper.Map<Houses, GetPropertyDTO>(property);

                if(images.Count > 0)
                {
                    propertyDTO.PropertyImages = images;
                }

                if(comments.Count > 0)
                {
                    propertyDTO.propertyComments = comments;
                    propertyDTO.Comments = comments.Count;
                }
                
                allProperties.Add(propertyDTO);
            }

            return allProperties;
        }

        [HttpGet("/api/v1/properties/get/{id}")]
        public async Task<GetPropertyDTO> GetUsers(int id)
        {
            GetPropertyDTO Property = new GetPropertyDTO();
            Property.PropertyImages = new List<PropertyImages>();
            Property.propertyComments = new List<PropertyComments>();
            var property = await _context.Houses.Where(x => x.Id == id).FirstOrDefaultAsync();
            var images  = await _context.PropertyImages.Where(x => x.PropertyID == property.Id).ToListAsync();
            var comments  = await _context.PropertyComments.Where(x => x.PropertyID == property.Id).ToListAsync();
            Property =  _mapper.Map<Houses, GetPropertyDTO>(property);
            Property.PropertyImages = images;
            Property.propertyComments = comments;
            return Property;
        }

        [AllowAnonymous]
        [HttpPost("/api/v1/properties/create")]
        public async Task<GenericResponseModel> CreateHouse([FromBody] CreatePropertyDto _createPropertyDto)
        {
            if (_createPropertyDto != null)
            {
                var house = new Houses();
                house.PropertyDescription = _createPropertyDto.PostText;
                house.PropertyNature = _createPropertyDto.PostNature;
                house.PropertyType = _createPropertyDto.PropertyType;
                house.PropertySetup = _createPropertyDto.SelectHouse;
                house.BusinessCategory = _createPropertyDto.TransactionType;
                house.PropertyPrice = _createPropertyDto.Amount;
                house.Latitude = _createPropertyDto.Location.Latitude.ToString();
                house.Longitude = _createPropertyDto.Location.Longitude.ToString();
                // house.State = _createPropertyDto.State;
                house.CreatedAt = DateTime.Now;
                house.UpdatedAt = DateTime.Now;
                _context.Houses.Add(house);
                await _context.SaveChangesAsync();

                foreach(var image in _createPropertyDto.SelectedImages)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.Ticks.ToString();
                    var fileUrl = UtilsHelper.SaveImage(image, uniqueFileName);
                    PropertyImages propertyImages = new PropertyImages();
                    propertyImages.PropertyID = house.Id;
                    propertyImages.ImageUrl = fileUrl;
                    _context.PropertyImages.Add(propertyImages);
                    await _context.SaveChangesAsync();
                }

                return new GenericResponseModel()
                {
                    Message = "House added successfully.",
                    StatusCode = "0",
                    Success = true
                };
            }
            else
            {
                return new GenericResponseModel()
                {
                    Message = "Failed.",
                    StatusCode = "1",
                    Success = false
                };
            }
        }
    }
}