using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2Desktop.Base
{
    public partial class Employees
    {
        public string FullName => FirstName + " " + LastName;
    }
}
