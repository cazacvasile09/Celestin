using AutoMapper;
using Celestin.API.Common;
using Celestin.API.Interfaces;
using Celestin.API.Models.CelestinModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Celestin.API.Controllers
{
    [ApiController]
    [Route("api/celestin")]
    public class CelestinController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICelestinRepository celestinRepository;
        private readonly IDiscoveryRepository discoveryRepository;
        private readonly IAbstractFactory factory;

        public CelestinController(IMapper _mapper, ICelestinRepository _iCelestinRepository, IDiscoveryRepository _discoveryRepository, IAbstractFactory _factory)
        {
            mapper = _mapper ?? throw new ArgumentNullException(nameof(_mapper));
            celestinRepository = _iCelestinRepository ?? throw new ArgumentNullException(nameof(_iCelestinRepository));
            discoveryRepository = _discoveryRepository ?? throw new ArgumentNullException(nameof(_discoveryRepository));
            factory = _factory ?? throw new ArgumentNullException(nameof(_factory));
        }

        [HttpGet]
        public IActionResult GetCelestins()
        {
            var celestins = celestinRepository.GetCelestins();
            return Ok(mapper.Map<IEnumerable<CelestinWithDiscoveryDto>>(celestins));
        }

        [HttpGet("{id}", Name = "GetCelestin")]
        public IActionResult GetCelestin(int id, bool includeDiscovery = false)
        {
            var celestin = celestinRepository.GetCelestin(id, includeDiscovery);

            if (celestin == null)
            {
                return NotFound();
            }

            if (includeDiscovery)
            {
                return Ok(mapper.Map<CelestinWithDiscoveryObjectDto>(celestin));
            }

            return Ok(mapper.Map<CelestinWithoutDiscoveryDto>(celestin));
        }

        [Route("GetCelestinsByName")]
        public IActionResult GetCelestinsByName(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var celestins = celestinRepository.GetCelestinsByName(name);

            return Ok(mapper.Map<IEnumerable<CelestinWithDiscoveryDto>>(celestins));
        }

        [Route("GetCelestinsByType")]
        [HttpGet]
        public IActionResult GetCelestinsByType(string type)
        {
            if (String.IsNullOrEmpty(type))
            {
                ModelState.AddModelError(
                   "Errors",
                   "Provide a valid type name!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var celestins = factory.GetCelestins(type);

            return Ok(mapper.Map<IEnumerable<CelestinWithDiscoveryDto>>(celestins));
        }

        [Route("CelestinForCreationDto")]
        [HttpPost]
        public IActionResult CreateCelestin([FromBody] CelestinForCreationDto celestin)
        {
            if (!discoveryRepository.ExistDiscovery(celestin.DiscoverySourceId))
            {
                ModelState.AddModelError(
                    "DiscoverySource",
                    "The provided DiscoverySourceId does not exist!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCelestin = mapper.Map<DbModels.Celestin>(celestin);

            celestinRepository.AddNewCelestin(newCelestin);

            celestinRepository.Save();

            var createdCelestin = mapper.Map<CelestinWithoutDiscoveryDto>(newCelestin);

            return CreatedAtRoute(
                "GetCelestin",
                new { createdCelestin.Id },
                createdCelestin);
        }

        [Route("CelestinForUpdateDto")]
        [HttpPut]
        public IActionResult UpdateCelestin(int id, [FromBody] CelestinForUpdateDto celestinUpdate) 
        {
            var existingCelestin = celestinRepository.GetCelestin(id, false);
            if (existingCelestin == null) 
            {
                return NotFound();
            }

            mapper.Map(celestinUpdate, existingCelestin);
            

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            celestinRepository.UpdateCelestin(existingCelestin);

            celestinRepository.Save();

            return Ok(mapper.Map<CelestinWithoutDiscoveryDto>(existingCelestin));
        }

        [Route("GetCelestinsByCountry")]
        [HttpGet]

        public IActionResult GetCelestinsByCountry(string name) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var celestins = celestinRepository.GetCelestinsByCountry(name);

            return Ok(mapper.Map<IEnumerable<CelestinWithDiscoveryDto>>(celestins));
        }

        [Route("CountryByMostBlackHoles")]
        [HttpGet]

        public IActionResult GetCountryByMostBlackHoles() 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var celestins = factory.GetCelestins(Commons.BlackHole);

            var countryBlackHoleCounts = celestins.GroupBy(bh => bh.DiscoverySource.StateOwner)
                                                  .Select(grp => new{Country = grp.Key, Count = grp.Count()})
                                                  .OrderByDescending(x => x.Count)
                                                  .ToList();

            var topCountry = countryBlackHoleCounts.FirstOrDefault();

            if (topCountry != null)
            {
                return Ok($"The country that has discovered the most black holes id {topCountry.Country} with {topCountry.Count} discoveries. ");
            }
            else 
            {
                return Ok("No black hole discoveries found.");
            }
        }

        [Route("DeleteById")]
        [HttpDelete]

        public IActionResult DeleteCelestin(int id) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCelestin = celestinRepository.GetCelestin(id, false);

            celestinRepository.DeleteCelestin(existingCelestin);

            celestinRepository.Save();

            return Ok("Celestin was deleted");
        }
    }
}