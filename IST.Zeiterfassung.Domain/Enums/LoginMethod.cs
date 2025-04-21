using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Domain.Enums
{
    public enum LoginMethod
    {
        Keine = 0,
        Passwort = 1,
        NFC = 2,
        QRCode = 4,
        Token = 8
    }
}
