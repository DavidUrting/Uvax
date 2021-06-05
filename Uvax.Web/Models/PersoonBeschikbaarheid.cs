namespace Uvax.Web.Models
{
    public class PersoonBeschikbaarheid
    {
        /// <summary>
        /// INSZ nummer van de persoon die gecontacteerd werd.
        /// </summary>
        public string Insz { get; set; }

        /// <summary>
        /// true indien de persoon beschikbaar is op het moment dat de medeweker heeft voorgelegd en dus als invaller al optreden.
        /// false indien de persoon niet beschikbaar is. De persoon zal vandaag niet meer gecontacteerd wordsen.
        /// </summary>
        public bool IsBeschikbaar { get; set; }

        /// <summary>
        /// INSZ nummer van de persoon die zijn/haar afspraak heeft geannuleerd (en waarvoor deze nieuwe persoon invalt).
        /// Dit veld is erplicht indien IsBeschikbaar true is.
        /// </summary>
        public string ValtInVoorInsz { get; set; }
    }
}
