using Microsoft.AspNetCore.Mvc;
using System;
using Metrc.SampleProject.Contracts.ShipType;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.ShipType;
using Metrc.SampleProject.WebApp.Models;

namespace Metrc.SampleProject.WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipTypeApiController : ControllerBase
    {
        private static ShipTypeRepository _ShipTypeRepository;

        static ShipTypeApiController()
        {
            _ShipTypeRepository = Repositories.ShipTypeRepository;
        }

        [HttpGet]
        public Paged<ShipTypeDocument> Get()
        {
            var result = _ShipTypeRepository.GetShipType();
            return result;
        }

        [HttpGet("{id}", Name = "GetShipType")]
        public ShipTypeDocument Get(Int64 id)
        {
            var result = _ShipTypeRepository.GetShipTypeById(id);
            return result;
        }

        [HttpPost("create", Name = "Create Ship Type")]
        public ActionResult CreateShipType([FromBody] ShipTypeModel shipTypeModel)
        {
            _ShipTypeRepository.Create(shipTypeModel.Name, shipTypeModel.TopSpeed);

            return Ok(true);
        }

        [HttpPost("update", Name = "Update Ship Type")]
        public ActionResult UpdateShipType([FromBody] ShipTypeModel shipTypeModel)
        {
            if (shipTypeModel.Id <= 0)
            {
                return BadRequest();
            }

            _ShipTypeRepository.Update(shipTypeModel.Id, shipTypeModel.Name, shipTypeModel.TopSpeed);

            return Ok(true);
        }

        [HttpPost("archive", Name = "Archive Ship Type")]
        public ActionResult ArchiveShipType([FromBody] ShipTypeModel shipTypeModel)
        {
            if (shipTypeModel.Id <= 0)
            {
                return BadRequest();
            }

            _ShipTypeRepository.Archive(shipTypeModel.Id);

            return Ok(true);
        }
    }
}
