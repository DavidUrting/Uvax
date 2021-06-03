using Microsoft.AspNetCore.Mvc;
using Uvax.Web.Models;
using System;
using System.Collections.Generic;

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
        internal static List<PersoonOpReseverlijst> _personenOpLijst = new List<PersoonOpReseverlijst>
        {
            new PersoonOpReseverlijst()
            {
                Insz = "55060801234",
                Voornaam = "Tim",
                Familienaam = "Berners-Lee",
                Telefoonnummer = "4412457812"
            },
            new PersoonOpReseverlijst()
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
        /// Een JSON antwoord met één PersoonOpLijst object (bestaande uit 4 properties).
        /// - insz bevat het rijksregisternummer van de persoon.
        /// - voornaam
        /// - familienaam
        /// - telefoonnumer (= dit nummer zal gebeld worden door de medewerker van het vaccinatiecentrum)
        /// Opgelet: je krijgt dus één object terug, geen array!
        /// </returns>
        [HttpGet]
        public PersoonOpReseverlijst Get()
        {
            // Onderstaande statement geeft een willekeurige index in het interval [0, #quotes - 1].
            int randomJokeIndex = _random.Next(0, _personenOpLijst.Count - 1);

            // Op basis van deze index wordt de overeenkomstige quote uit de lijst gehaald.
            PersoonOpReseverlijst randomPersoonOpLijst = _personenOpLijst[randomJokeIndex];

            return randomPersoonOpLijst;
        }

        /// <summary>
        /// Bonus: inschrijven van een persoon.
        /// Deze action kan je oproepen met een HTTP POST request naar de relatieve url /api/reservelijst.
        /// </summary>
        /// <param name="inTeSchrijvenPersoon"></param>
        [HttpPost]
        public void Post([FromBody] PersoonOpReseverlijst inTeSchrijvenPersoon)
        {
            if (!_personenOpLijst.Exists(pol => pol.Insz == inTeSchrijvenPersoon.Insz))
            {
                _personenOpLijst.Add(inTeSchrijvenPersoon);
            }
        }

        /// <summary>
        /// Bonus: uitschrijven van een persoon.
        /// Deze action kan je oproepen met een HTTP DELETE request naar de relatieve url /api/reservelijst.
        /// </summary>
        /// <param name="insz"></param>
        [HttpDelete]
        public void Delete([FromBody] string insz)
        {
            PersoonOpReseverlijst uitTeSchrijvenPersoon = _personenOpLijst.Find(pol => pol.Insz == insz);
            if (uitTeSchrijvenPersoon != null)
            {
                _personenOpLijst.Remove(uitTeSchrijvenPersoon);
            }
        }
    }
}
