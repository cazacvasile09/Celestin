using AutoMapper;
using Celestin.API.Common;
using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using Celestin.API.Models.CelestinModels;
using Celestin.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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

        [HttpPost]
        [Route("CelestinForCreationDto")]
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

        [HttpPut("{id}")]
        public IActionResult UpdateCelestin(int id, [FromBody] CelestinForUpdateDto celestinUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

        

            var existingCelestin = celestinRepository.GetCelestin(id, false);

            if (existingCelestin == null)
            {
                return NotFound();
            }

            // Mapăm din CelestinForUpdateDto în DbModels.Celestin utilizând AutoMapper
            mapper.Map(celestinUpdateDto, existingCelestin);

            // Update the Celestin entity in repository
            celestinRepository.UpdateCelestin(existingCelestin);

            // Save changes in repository
            celestinRepository.Save();

            // Return 204 No Content response
            return Ok(mapper.Map<CelestinWithoutDiscoveryDto>(existingCelestin));

        }


        [HttpGet("by-country/{country}")]
        public IActionResult GetCelestinsByCountry(string country)
        {
            var celestins = celestinRepository.GetCelestinsByCountry(country);

            if (celestins == null )
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<CelestinWithDiscoveryDto>>(celestins));
        }


        [Route("BlackHole")]
        [HttpGet]
        public IActionResult GetCountryWithMostBlackHoleDiscoveries()
        {
            var country = celestinRepository.GetCountryWithMostBlackHoleDiscoveries();

            if (string.IsNullOrEmpty(country))
            {
                return NotFound();
            }

            return Ok(country);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCelestin(int id)
        {
            var celestinToDelete = celestinRepository.GetCelestin(id, false);

            if (celestinToDelete == null)
            {
                return NotFound();
            }

            celestinRepository.DeleteCelestin(celestinToDelete);
            celestinRepository.Save();

            return NoContent();
        }


    }
}