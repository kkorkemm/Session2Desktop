using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session2Desktop.Base
{
    public partial class Assets
    {
        public string LastClosedEM
        {
            get
            {
                EmergencyMaintenances emergencyMaintenances = EmergencyMaintenances.ToList().OrderBy(p => p.ID).LastOrDefault();

                if (emergencyMaintenances != null && emergencyMaintenances.EMEndDate != null)
                    return emergencyMaintenances.EMEndDate.ToString();
                else
                    return "--";
            }
        }

        public int CountEm => EmergencyMaintenances.Count();
    }
}
