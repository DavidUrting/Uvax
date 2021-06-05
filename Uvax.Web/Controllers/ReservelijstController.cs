using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Uvax.Web.Models;

namespace Uvax.Web.Controllers
{
    /// <summary>
    /// Deze API geeft beheert personen die ingeschreven zijn op de Uvax-reservelijst.
    /// Met Get kan je (willekeurig) een ingeschreven persoon ophalen.
    /// Met Post kan je een persoon toevoegen aan de lijst.
    /// Met Delete kan je een persoon weer verwijderen van de lijst.
    /// 
    /// Als sleutel wordt gebruik gemaakt van het INSZ nummer (ook wel het rijksregisternummer genoemd).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReservelijstController : ControllerBase
    {
        internal static List<PersoonOpReservelijst> _personenOpLijst = new List<PersoonOpReservelijst>
        {
            new PersoonOpReservelijst()
            {
                Insz = "34061301234",
                Voornaam = "Leonard",
                Familienaam = "Kleinrock",
                Telefoonnummer = "112345670"
            },
            new PersoonOpReservelijst()
            {
                Insz = "43062301234",
                Voornaam = "Vinton",
                Familienaam = "Cerf",
                Telefoonnummer = "112345671"
            },
            new PersoonOpReservelijst()
            {
                Insz = "38122301234",
                Voornaam = "Bob",
                Familienaam = "Kahn",
                Telefoonnummer = "112345672"
            },
            new PersoonOpReservelijst()
            {
                Insz = "55060801234",
                Voornaam = "Tim",
                Familienaam = "Berners-Lee",
                Telefoonnummer = "4412457812"
            },
            new PersoonOpReservelijst()
            {
                Insz = "37061701234",
                Voornaam = "Cailliau",
                Familienaam = "Robert",
                Telefoonnummer = "012123456"
            }
        };

        // Een 'randomizer'. Dit object geeft willekeurige getallen terug.
        // Dit object heeft dus hetzelfde doel als de Math.random() in JavaScript.
        // Het wordt gebruikt om willekeurig personen te selecteren.
        private Random _random = new Random();

        /// <summary>
        /// Deze action kan je oproepen met een HTTP GET request naar de relatieve url /api/reservelijst.
        /// </summary>
        /// <returns>
        /// Een JSON antwoord met één PersoonOpReservelijst object (bestaande uit 4 properties).
        /// - insz (bevat het rijksregisternummer van de persoon).
        /// - voornaam
        /// - familienaam
        /// - telefoonnumer (= dit nummer zal gebeld worden door de medewerker van het vaccinatiecentrum)
        /// Opgelet: je krijgt dus één object terug, geen array!
        /// Indien er geen kandidaten (meer) zijn wordt null teruggegeven.
        /// </returns>
        [HttpGet]
        public PersoonOpReservelijst Get()
        {
            var personenDieNogNietGecontacteerdWerdenVandaag = _personenOpLijst
                .Where(pol => pol.LaastGecontacteerdOp == null || pol.LaastGecontacteerdOp < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                .ToList();

            if (personenDieNogNietGecontacteerdWerdenVandaag.Count > 0)
            {
                // Onderstaande statement geeft een willekeurige index in het interval [0, #personen - 1].
                int willekeurigePersoonIndex = _random.Next(0, personenDieNogNietGecontacteerdWerdenVandaag.Count);

                // Op basis van deze index wordt de overeenkomstige persoon uit de lijst gehaald.
                PersoonOpReservelijst randomPersoonOpLijst = personenDieNogNietGecontacteerdWerdenVandaag[willekeurigePersoonIndex];

                return randomPersoonOpLijst;
            }
            else return null;
        }

        /// <summary>
        /// Bonus: inschrijven van een persoon.
        /// Deze action kan je oproepen met een HTTP POST request naar de relatieve url /api/reservelijst.
        /// </summary>
        /// <param name="inTeSchrijvenPersoon">Een JSON request met één PersoonOpReservelijst object.</param>
        [HttpPost]
        public IActionResult Post([FromBody] PersoonOpReservelijst inTeSchrijvenPersoon)
        {
            if (!_personenOpLijst.Exists(pol => pol.Insz == inTeSchrijvenPersoon.Insz))
            {
                _personenOpLijst.Add(inTeSchrijvenPersoon);
                return NoContent();
            }
            else
            {
                return BadRequest($"Er is al een persoon met INSZ {inTeSchrijvenPersoon.Insz} ingeschreven op de lijst.");
            }
        }

        /// <summary>
        /// Bonus: uitschrijven van een persoon.
        /// Deze action kan je oproepen met een HTTP DELETE request naar de relatieve url /api/reservelijst.
        /// </summary>
        /// <param name="insz">Een JSON request met één string (dus geen object doorgeven! Enkel een string doorgeven)</param>
        [HttpDelete]
        public void Delete([FromBody] string insz)
        {
            PersoonOpReservelijst uitTeSchrijvenPersoon = _personenOpLijst.Find(pol => pol.Insz == insz);
            if (uitTeSchrijvenPersoon != null)
            {
                _personenOpLijst.Remove(uitTeSchrijvenPersoon);
            }
        }
    }
}
