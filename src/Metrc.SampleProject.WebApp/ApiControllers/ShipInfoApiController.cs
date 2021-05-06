using Microsoft.AspNetCore.Mvc;
using System;
using Metrc.SampleProject.Contracts.ShipInfo;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.ShipInfo;
using Metrc.SampleProject.WebApp.Models;

namespace Metrc.SampleProject.WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipInfoApiController : ControllerBase
    {
        private static ShipInfoRepository _ShipInfoRepository;

        static ShipInfoApiController()
        {
            _ShipInfoRepository = Repositories.ShipInfoRepository;
        }

        [HttpGet]
        public Paged<ShipInfoDocument> Get()
        {
            var result = _ShipInfoRepository.GetShipInfo();
            return result;
        }

        [HttpGet("{id}", Name = "GetShipInfo")]
        public ShipInfoDocument Get(Int64 id)
        {
            var result = _ShipInfoRepository.GetShipInfoById(id);
            return result;
        }

        [HttpPost("create", Name = "Create Ship Info")]
        public ActionResult CreateShipInfo([FromBody] ShipInfoModel shipInfoModel)
        {
            if (shipInfoModel is null || shipInfoModel.Name is null ||
                shipInfoModel.Name.Length == 0 || shipInfoModel.ShipTypeId <= 0)
            {
                return BadRequest();
            }
            var result = _ShipInfoRepository.QueryShipInfo(builder =>
            {
                builder.Where("Name = @name", new { name = shipInfoModel.Name });
            });
            if (result.Data.Length != 0)
            {
                return BadRequest();
            }
            _ShipInfoRepository.Create(shipInfoModel.Name, shipInfoModel.Occupancy == "on",
                shipInfoModel.Status, shipInfoModel.ShipTypeId);

            return Ok(true);
        }

        [HttpPost("update", Name = "Update Ship Info")]
        public ActionResult UpdateShipInfo([FromBody] ShipInfoModel shipInfoModel)
        {
            if (shipInfoModel is null || shipInfoModel.Id <= 0 || shipInfoModel.Name is null ||
                shipInfoModel.Name.Length == 0 || shipInfoModel.ShipTypeId <= 0)
            {
                return BadRequest();
            }
            var result = _ShipInfoRepository.QueryShipInfo(builder =>
            {
                builder.Where("Name = @name", new { name = shipInfoModel.Name });
                builder.Where("Id != @id", new { id = shipInfoModel.Id });
            });
            if (result.Data.Length != 0)
            {
                return BadRequest();
            }

            _ShipInfoRepository.Update(shipInfoModel.Id, shipInfoModel.Name,
                shipInfoModel.Occupancy == "on", shipInfoModel.Status, shipInfoModel.ShipTypeId);

            return Ok(true);
        }

        [HttpPost("archive", Name = "Archive Ship Info")]
        public ActionResult ArchiveShipInfo([FromBody] ShipInfoModel shipInfoModel)
        {
            if (shipInfoModel.Id <= 0)
            {
                return BadRequest();
            }

            _ShipInfoRepository.Archive(shipInfoModel.Id);

            return Ok(true);
        }
    }
}
