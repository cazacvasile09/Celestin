﻿using AutoMapper;
using Celestin.API.Common;
using Celestin.API.Interfaces;
using Celestin.API.Models.CelestinModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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

        [Route("GetCelestinsByCountry")]
        [HttpGet]
        public IActionResult GetCelestinsByCountry(string country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var celestins = celestinRepository.GetCelestinsByCountry(country);
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

        [Route("CreateCelestin")]
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

        [Route("UpdateCelestin")]
        [HttpPut]
        public IActionResult UpdateCelestin([FromBody] CelestinForUpdationDto celestin)
        {
            
            if( celestin == null )
            {
                return BadRequest();
            }

            if( celestin.Id == 0 )
            {
                return BadRequest();
            }

            var recordEntry = celestinRepository.GetCelestin( celestin.Id , false );

            if ( recordEntry == null )
            {
                return NotFound();
            }
            
            if( celestin.Name != null && celestin.Name != recordEntry.Name )
            {
                recordEntry.Name = celestin.Name;
            }
            
            if (celestin.Mass != 0 && celestin.Mass != recordEntry.Mass)
            {
                recordEntry.Mass = celestin.Mass;
            } 
            
            if(celestin.EquatorialDiameter != 0 && celestin.EquatorialDiameter != recordEntry.EquatorialDiameter )
            {
                recordEntry.EquatorialDiameter = celestin.EquatorialDiameter;
            }

            if(celestin.SurfaceTemperature != 0 && celestin.SurfaceTemperature != recordEntry.SurfaceTemperature)
            {
                recordEntry.SurfaceTemperature = celestin.SurfaceTemperature; 
            }
            
            if(celestin.DiscoverySourceId != 0 && celestin.DiscoverySourceId != recordEntry.DiscoverySourceId)
            {
                recordEntry.DiscoverySourceId = celestin.DiscoverySourceId;
            }

            if(celestin.DiscoveryDate != DateTime.MinValue && celestin.DiscoveryDate != recordEntry.DiscoveryDate)
            {
                recordEntry.DiscoveryDate = celestin.DiscoveryDate;
            }
            
            celestinRepository.UpdateCelestin(recordEntry);
            celestinRepository.Save();

            return Ok(
                $"Id : {recordEntry.Id}\n" + $"Name : {recordEntry.Name}\n" + $"Mass : {recordEntry.Mass}\n" + $"EquatorialDiameter : {recordEntry.EquatorialDiameter}\n" + $"SurfaceTemperature : {recordEntry.SurfaceTemperature}\n" + $"DiscoverySourceId : {recordEntry.DiscoverySourceId}\n" + $"DiscoveryDate : {recordEntry.DiscoveryDate}" 
                );
        }
    }
}