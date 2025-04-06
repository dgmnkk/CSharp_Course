using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Humeniuk.Exceptions
{
    public class FutureDateBirthException : Exception
    {
        public FutureDateBirthException() : base("Дата народження в майбутньому")
        {
        }
    }
}
