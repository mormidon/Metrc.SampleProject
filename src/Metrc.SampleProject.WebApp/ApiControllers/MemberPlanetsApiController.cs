using Microsoft.AspNetCore.Mvc;
using System;
using Metrc.SampleProject.Contracts.MemberPlanets;
using Metrc.SampleProject.Services.Infrastructure;
using Metrc.SampleProject.Services.MemberPlanets;
using Metrc.SampleProject.WebApp.Models;

namespace Metrc.SampleProject.WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberPlanetsApiController : ControllerBase
    {
        private static MemberPlanetsRepository _MemberPlanetsRepository;

        static MemberPlanetsApiController()
        {
            _MemberPlanetsRepository = Repositories.MemberPlanetsRepository;
        }

        [HttpGet]
        public Paged<MemberPlanetsDocument> Get()
        {
            var result = _MemberPlanetsRepository.GetMemberPlanets();
            return result;
        }

        [HttpGet("{id}", Name = "GetMemberPlanet")]
        public MemberPlanetsDocument Get(Int64 id)
        {
            var result = _MemberPlanetsRepository.GetMemberPlanetsById(id);
            return result;
        }

        [HttpPost("create", Name = "Create Member Planet")]
        public ActionResult CreateMemberPlanet([FromBody] MemberPlanetsModel planetsModel)
        {
            _MemberPlanetsRepository.Create(
                planetsModel.Name, planetsModel.xcoordinates, planetsModel.ycoordinates, planetsModel.zcoordinates);

            return Ok(true);
        }

        [HttpPost("update", Name = "Update Member Planet")]
        public ActionResult UpdateMemberPlanet([FromBody] MemberPlanetsModel planetsModel)
        {
            if (planetsModel.Id <= 0)
            {
                return BadRequest();
            }

            _MemberPlanetsRepository.Update(
                planetsModel.Id, planetsModel.Name, planetsModel.xcoordinates, planetsModel.ycoordinates, planetsModel.zcoordinates);

            return Ok(true);
        }

        [HttpPost("archive", Name = "Archive Member Planet")]
        public ActionResult ArchiveMemberPlanet([FromBody] MemberPlanetsModel planetsModel)
        {
            if (planetsModel.Id <= 0)
            {
                return BadRequest();
            }

            _MemberPlanetsRepository.Archive(planetsModel.Id);

            return Ok(true);
        }
    }
}
