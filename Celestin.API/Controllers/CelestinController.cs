using AutoMapper;
using Celestin.API.Common;
using Celestin.API.DbModels;
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

        [Route("CreateCelestin")] //trebuie sa punem o ruta noua
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

            celestinRepository.AddNewCelestin(newCelestin); //apelam metoda de creare de nou obiect

            celestinRepository.Save();

            var createdCelestin = mapper.Map<CelestinWithoutDiscoveryDto>(newCelestin);

            return CreatedAtRoute(
                "GetCelestin",
                new { createdCelestin.Id },
                createdCelestin);
        }

        [Route("UpdateCelestin")]
        [HttpPut]
        /*public IActionResult UpdateCelestin([FromBody] CelestinForCreationDto c) //prima incercare(fail)
        {
            var newCelestin = mapper.Map<DbModels.Celestin>(c);



            celestinRepository.UpdateCelestin(newCelestin);

            celestinRepository.Save();

            var updatedCelestin = mapper.Map<CelestinWithoutDiscoveryDto>(newCelestin);

            //return TryUpdateModelAsync<>();
            return Ok(mapper.Map<IEnumerable<CelestinWithoutDiscoveryDto>>(updatedCelestin));
        }*/
        public IActionResult UpdateCelestin([FromBody] CelestinForUpdateDto updatedCelestin) //in c este obiectul in care sunt noile valori ce trb updatate
        {

            //pasul 1: noile informatii le mapam intr-un alt obiect, dar pastram id-ul obiectului initial/vechi
            var initialCelestin = celestinRepository.GetCelestin(updatedCelestin.Id, false);
            mapper.Map(updatedCelestin, initialCelestin);

            //pasul 2: dam update la acest obiect in baza de date
            celestinRepository.updateCelestin(initialCelestin);

            //pasul 3: dam save
            celestinRepository.Save();

            //pasul 4:afisam un mesaj de confirmare
            return Ok("Update cu succes!");
        }
    }
}