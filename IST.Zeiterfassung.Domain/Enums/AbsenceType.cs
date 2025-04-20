using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Domain.Enums
{
    public enum AbsenceType
    {
        [Display(Name = "Urlaub")]
        Urlaub = 0,

        [Display(Name = "Krankmeldung")]
        Krankheit = 1,

        [Display(Name = "Home Office")]
        HomeOffice = 2,

        [Display(Name = "Sonderurlaub")]
        Sonderurlaub = 3
    }

}
