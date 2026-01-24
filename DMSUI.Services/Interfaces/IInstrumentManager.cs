using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IInstrumentManager
    {
		Task<PagedResultDTO<InstrumentDTO>> GetInstrumentsAsync(int pageNumber, int pageSize);

	}
}
