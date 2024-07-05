using AutoMapper;
using Celestin.API.Common;
using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using Celestin.API.Models;
using Celestin.API.Models.CelestinModels;
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

        [HttpPut("{id}")]
        public IActionResult UpdateCelestin(int id, [FromBody] CelestinForUpdateDto celestinDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (celestinDto == null)
            {
                return BadRequest();
            }

            var existingCelestin = celestinRepository.GetCelestin(id, false);
            
            if (existingCelestin == null)
            {
                return NotFound();
            }

            mapper.Map(celestinDto, existingCelestin);
            // Update the existing Celestin entity with values from the DTO
            celestinRepository.UpdateCelestin(existingCelestin);

            // Save changes to the database
            //celestinRepository.Save();

            return Ok(mapper.Map<CelestinWithDiscoveryDto>(existingCelestin));
        }

        [Route("by-country")]
        [HttpGet]
        public IActionResult GetCelestinsByCountry(string country)
        {
            var celestins = celestinRepository.GetCelestinsByCountry(country);
            if (celestins == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<CelestinWithDiscoveryDto>>(celestins));
        }

        [Route("GetCountryWithMostBlackHoleDiscoveries")]
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

        [Route("Delete")]
        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            var celestin = celestinRepository.GetCelestin(id, false);
            if (celestin == null)
            {
                return NotFound();
            }

            celestinRepository.DeleteById(celestin);
            if (!celestinRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return Ok("Deleted!");
        }
    }

    }
