using System;
using System.Text.Json.Serialization;

namespace Uvax.Web.Models
{
    public class PersoonOpReservelijst
    {
        public string Insz { get; set; }
        public string Familienaam { get; set; }
        public string Voornaam { get; set; }
        public string Telefoonnummer { get; set; }

        [JsonIgnore]
        public DateTime? LaastGecontacteerdOp { get; set; }
    }
}
