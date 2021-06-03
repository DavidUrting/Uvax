using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uvax.Web.Models
{
    public class VaccinatieRegistratie
    {
        /// <summary>
        /// INSZ nummer van de persoon die gecontacteerd werd.
        /// </summary>
        public string Insz { get; set; }

        /// <summary>
        /// true indien de persoon beschikbaar is op het moment dat de medeweker heeft voorgelegd.
        /// false indien de persoon niet beschikbaar is.
        /// </summary>
        public bool IsBeschikbaar { get; set; }
    }
}
