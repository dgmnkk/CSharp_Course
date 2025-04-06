using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Humeniuk.Exceptions
{
    public class UnknownDataException : Exception
    {
        public UnknownDataException() : base("Невідомі дані")
        {
        }
    }
}
