using DMSUI.Entities.DTOs.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CAPA
{
    public class CAPACreateLookupsDTO
    {
        public List<LookupItemDTO> RootCauseMethods { get; set; } = new();
        public List<LookupItemDTO> Owners { get; set; } = new();
    }
}
