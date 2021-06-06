using Microsoft.AspNetCore.Mvc;
using System;
using Uvax.Web.Models;

namespace Uvax.Web.Controllers
{
    /// <summary>
    /// Via deze API kan een medewerker aangeven of een gecontacteerde persoon al dan niet beschikbaar is.
    /// Indien de persoon niet gecontacteerd kon worden wordt deze API niet gebruikt.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BeschikbaarheidController : ControllerBase
    {
        /// <summary>
        /// Deze action kan je oproepen met een HTTP POST request naar de url /api/beschikbaarheid.
        /// </summary>
        /// <param name="persoonBeschikbaarheid">        
        /// Aan deze action kan je een JSON object meegeven van volgende structuur: 
        /// { 
        ///     "insz": "...", // insz nummer van de persoon die gecontacteerd werd
        ///     "isBeschikbaar: true|false, // true indien de persoon beschikbaar is
        ///     "valtInVoorInsz": "" // insz nummer van de persoon die geannuleerd heeft
        /// }.
        /// Opgelet: je moet dus een object doorgeven, geen array!
        /// </param>
        /// <returns>
        /// Indien het INSZ nummer gekend is en de persoon is beschikbaar krijg je een leeg antwoord terug. De persoon wordt dan van de reservelijst gehaald en aan de afspraak gekoppeld.
        /// (Opgelet: indien de persoon beschikbaar is moet "valtInVoorInsz" ook ingevuld zijn, anders krijg je een BadRequest terug).
        /// Indien het INSZ nummer gekend is en de persoon is niet beschikbaar krijg je een leeg antwoord terug. De persoon zal niet meer geselecteerd worden vandaag.
        /// Indien het INSZ nummer niet gekend zit krijg je een NotFound terug. Dat duidt op een bug in jouw code: probeer ervoor te zorgen dat de gebruiker enkel gekende INSZ nummers kan doorsturen.
        /// </returns>
        [HttpPost]
        [Produces("application/json")]
        public IActionResult Post([FromBody] PersoonBeschikbaarheid persoonBeschikbaarheid)
        {
            // Onderstaande code controleert of het INSZ nummer gekend is.
            if (!ReservelijstController._personenOpLijst.Exists(pol => pol.Insz == persoonBeschikbaarheid.Insz))
            {
                // Zoniet wordt een NotFound teruggestuurd (HTTP status code 404).
                return NotFound($"De persoon met INSZ {persoonBeschikbaarheid.Insz} komt niet op de reservelijst voor.");
            }
            else
            {
                // Zoja, wordt nagegaan of de persoon beschikbaar is om in te vallen...
                PersoonOpReservelijst persoonOpDeLijst = ReservelijstController._personenOpLijst
                    .Find(pol => pol.Insz == persoonBeschikbaarheid.Insz);
                if (persoonBeschikbaarheid.IsBeschikbaar)
                {
                    // ... de persoon is beschikbaar.
                    if (string.IsNullOrWhiteSpace(persoonBeschikbaarheid.ValtInVoorInsz))
                    {
                        // In dat geval moet ook het INSZ nummer doorgegeven worden van de persoon die geannuleerd heeft.
                        return BadRequest($"Persoon {persoonBeschikbaarheid.Insz} is beschikbaar. In dat geval moet {nameof(persoonBeschikbaarheid.ValtInVoorInsz)} ingevuld zijn.");
                    }
                    else
                    {
                        // Hier zou je originele afspraak kunnen ophalen en koppelen aan het nieuwe INSZ.
                        // Dat is uiteraard out of scope voor dit examen!
                        // var afspraak = (from a in _context.Afspraken
                        //                where a.Insz == persoonBeschikbaarheid.ValtInVoorInsz
                        //                select a).FirstOrDefault();
                        // afspraak.Insz = persoonBeschikbaarheid.ValtInVoorInsz;
                        // _context.SaveChanges();
                        // vaccin = afspraak.Vaccing;
                        // ...

                        // Vervolgens wordt een boodschap teruggestuurd naar de front end waarin het toegediende vaccin zit (HTTP status code 200).
                        ReservelijstController._personenOpLijst.Remove(persoonOpDeLijst);
                        return Ok();
                    }
                }
                else
                {
                    // ... de persoon is niet beschikbaar. Het systeem onthoudt dat de klant (vandaag) niet meer moet aangeboden worden als potentiële invaller.
                    persoonOpDeLijst.LaastGecontacteerdOp = DateTime.Now;
                    return Ok();
                }
            }
            
            // Ter info voor de backend-geïnteresseerden: bemerk dat deze action een IActionResult teruggeeft. 
            // Dat komt omdat we ofwel een JSON willen teruggeven met een string ofwel een BadRequest.
        }
    }
}
