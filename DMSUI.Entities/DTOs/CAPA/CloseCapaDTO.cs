using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Entities.DTOs.CAPA
{
    public class CloseCapaDTO
    {
        public string? ClosureEvidence { get; set; }
        public string? EffectivenessResult { get; set; }
        public string? EffectivenessCheck { get; set; }
        public int? EffectivenessCheckedBy { get; set; }
        public DateTimeOffset? EffectivenessCheckedAt { get; set; }
    }
}
