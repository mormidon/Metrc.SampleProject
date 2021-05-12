using Microsoft.AspNetCore.Mvc;
using System;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.ShipInfo;
using Metrc.SampleProject.WebApp.Models;

namespace Metrc.SampleProject.WebApp.Controllers
{
    public class ShipInfoController : Controller
    {
        private ShipInfoRepository _ShipInfoRepository;

        public ShipInfoController()
        {
            _ShipInfoRepository = Repositories.ShipInfoRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEdit(Int64? id = null)
        {
            var model = new ShipInfoModel
            {
                UpdateModal = id.HasValue
            };

            if (id.HasValue)
            {
                var shipInfo = _ShipInfoRepository.GetShipInfoById(id.Value);

                model.Id = shipInfo.Id;
                model.Name = shipInfo.Name;
                model.Occupancy = shipInfo.Occupancy;
                model.Status = shipInfo.Status;
                model.ShipTypeId = shipInfo.ShipTypeId;
            }

            return View(model);
        }
    }
}