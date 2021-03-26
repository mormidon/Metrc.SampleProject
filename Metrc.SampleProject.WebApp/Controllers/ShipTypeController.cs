using Microsoft.AspNetCore.Mvc;
using System;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.ShipType;
using Metrc.SampleProject.WebApp.Models;

namespace Metrc.SampleProject.WebApp.Controllers
{
    public class ShipTypeController : Controller
    {
        private ShipTypeRepository _ShipTypeRepository;

        public ShipTypeController()
        {
            _ShipTypeRepository = Repositories.ShipTypeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEdit(Int64? id = null)
        {
            var model = new ShipTypeModel
            {
                UpdateModal = id.HasValue
            };

            if (id.HasValue)
            {
                var shipType = _ShipTypeRepository.GetShipTypeById(id.Value);

                model.Id = shipType.Id;
                model.Name = shipType.Name;
                model.TopSpeed = shipType.TopSpeed;
            }

            return View(model);
        }
    }
}