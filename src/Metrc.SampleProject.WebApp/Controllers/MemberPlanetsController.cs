using Microsoft.AspNetCore.Mvc;
using System;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.MemberPlanets;
using Metrc.SampleProject.WebApp.Models;

namespace Metrc.SampleProject.WebApp.Controllers
{
    public class MemberPlanetsController : Controller
    {
        private MemberPlanetsRepository _MemberPlanetsRepository;

        public MemberPlanetsController()
        {
            _MemberPlanetsRepository = Repositories.MemberPlanetsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEdit(Int64? id = null)
        {
            var model = new MemberPlanetsModel
            {
                UpdateModal = id.HasValue
            };

            if (id.HasValue)
            {
                var memberPlanet = _MemberPlanetsRepository.GetMemberPlanetsById(id.Value);

                model.Id = memberPlanet.Id;
                model.Name = memberPlanet.Name;
                model.xcoordinates = memberPlanet.Xcoordinates;
                model.ycoordinates = memberPlanet.Ycoordinates;
                model.zcoordinates = memberPlanet.Zcoordinates;
            }

            return View(model);
        }
    }
}