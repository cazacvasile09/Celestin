﻿using AutoMapper;
using Celestin.API.Common;
using Celestin.API.Interfaces;
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

        //[Route("name")]
        [HttpGet("name/{name}")]
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
    }
}