using Microsoft.AspNetCore.Mvc;
using System;
using Uvax.Web.Models;

namespace Uvax.Web.Controllers
{
    /// <summary>
    /// Via deze API kan een medewerker een opgebelde gebruiker al dan niet registreren voor een vaccinatie.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BeschikbaarheidController : ControllerBase
    {
        /// <summary>
        /// Deze action kan je oproepen met een HTTP POST request naar de url /api/vaccinatieregistratie.
        /// Aan deze action kan je een JSON object meegeven van volgende structuur: 
        /// { 
        ///     "inze": "...", 
        ///     "isBeschikbaar: true|false 
        /// }.
        /// Opgelet: je moet dus een object doorgeven, geen array!
        /// </summary>
        /// <param name="insz">INSZ nummer van de persoon die werd opgebeld.</param>
        /// <param name="isBeschikbaar">true indien de persoon beschikbaar is en dus kan komen.</param>
        /// <returns>
        /// Indien het INSZ nummer gekend is en de persoon is beschikbaar krijg je een JSON antwoord terug met een string in waarin de naam van het vaccin zit dat toegediend zal worden.
        /// Indien het INSZ nummer gekend is en de persoon is niet beschikbaar krijg je een leeg antwoord terug.
        /// Indien het INSZ nummer niet gekend zit krijg je een NotFound terug. Dat duidt op een bug in jouw code: probeer ervoor te zorgen dat de gebruiker enkel gekende INSZ nummers kan doorsturen.
        /// </returns>
        [HttpPost]
        [Produces("application/json")]
        public IActionResult Post([FromBody] PersoonBeschikbaarheid registratie)
        {
            // Onderstaande code controleert of het INSZ nummer gekend is.
            if (!ReservelijstController._personenOpLijst.Exists(pol => pol.Insz == registratie.Insz))
            {
                // Zoniet wordt een NotFound teruggestuurd (HTTP status code 404).
                return BadRequest($"De persoon met INSZ {registratie.Insz} komt niet op de reservelijst voor.");
            }
            else
            {
                // Zoja, wordt nagegaan of de persoon beschikbaar is om in te vallen...
                PersoonOpReservelijst persoonOpDeLijst = ReservelijstController._personenOpLijst.Find(pol => pol.Insz == registratie.Insz);
                if (registratie.IsBeschikbaar)
                {
                    if (string.IsNullOrWhiteSpace(registratie.ValtInVoorInsz))
                    {
                        return BadRequest($"Persoon {registratie.Insz} is beschikbaar. In dat geval moet {nameof(registratie.ValtInVoorInsz)} ingevuld zijn.");
                    }
                    else
                    {
                        // Hier zou je originele afspraak kunnen ophalen en koppelen aan het nieuwe INSZ.
                        // Dat is uiteraard out of scope voor dit examen!
                        // ...

                        // Zoja wordt een boodschap teruggestuurd naar de front end waarin het toegediende vaccin zit (HTTP status code 200).
                        ReservelijstController._personenOpLijst.Remove(persoonOpDeLijst);
                        return Ok("Pfizer");
                    }
                }
                else
                {
                    // Zoniet onthoudt het systeem dat de klant (vandaag) niet meer moet aangeboden worden als potentiële invaller.
                    persoonOpDeLijst.LaastGecontacteerdOp = DateTime.Now;
                    return Ok();
                }
            }
            
            // Ter info voor de backend-geïnteresseerden: bemerk dat deze action een IActionResult teruggeeft. 
            // Dat komt omdat we ofwel een JSON willen teruggeven met een string ofwel een BadRequest.
        }
    }
}
