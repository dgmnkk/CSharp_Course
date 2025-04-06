using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Humeniuk.Exceptions
{
    public class MoreThan135YOException : Exception
    {
        public MoreThan135YOException() : base("Застаріла дата народження, більше 135 років")
        {
        }
    }
}
