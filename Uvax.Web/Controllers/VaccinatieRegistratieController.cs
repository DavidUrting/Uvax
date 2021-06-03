using Microsoft.AspNetCore.Mvc;
using Uvax.Web.Models;

namespace Uvax.Web.Controllers
{
    /// <summary>
    /// Via deze API kan een medewerker een opgebelde gebruiker al dan niet registreren voor een vaccinatie.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinatieRegistratieController : ControllerBase
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
        public IActionResult Post([FromBody] VaccinatieRegistratie registratie)
        {
            // Hier zou je de score (en opmerkingen) kunnen loggen in de database.
            // Dat is uiteraard out of scope voor dit examen!
            // ...

            // Onderstaande code controleert of het INSZ nummer gekend is.
            // Zoniet wordt een NotFound teruggestuurd (HTTP status code 404).
            // Zoja wordt een boodschap teruggestuurd naar de front end waarin het toegediende vaccin zit (HTTP status code 200).
            if (!ReservelijstController._personenOpLijst.Exists(pol => pol.Insz == registratie.Insz))
            {
                return BadRequest($"De persoon met INSZ {registratie.Insz} komt niet op de reservelijst voor.");
            }
            else
            {
                if (registratie.IsBeschikbaar)
                {
                    PersoonOpReseverlijst vanLijstTeHalenPersoon = ReservelijstController._personenOpLijst.Find(pol => pol.Insz == registratie.Insz);
                    ReservelijstController._personenOpLijst.Remove(vanLijstTeHalenPersoon);
                    return Ok("Pfizer");
                }
                else return Ok();
            }
            
            // Ter info voor de backend-geïnteresseerden: bemerk dat deze action een IActionResult teruggeeft. 
            // Dat komt omdat we ofwel een JSON willen teruggeven met een string ofwel een BadRequest.
        }
    }
}
