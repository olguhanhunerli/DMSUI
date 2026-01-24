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
		Task<PagedResultDTO<InstrumentDTO>> GetDeletedByInstrumentsAsync(int pageNumber, int pageSize);
		Task<InstrumentDTO> GetInstrumentByIdAsync(int instrumentId);
		Task<InstrumentDTO> GetInstrumentDeletedByIdAsync(int instrumentId);
		Task CreateInstrumentAsync(CreateInstrumentDTO dto);
		Task UpdateInstrumentAsync(int id, UpdateInstrumentDTO dto);
		Task DeleteInstrumentAsync(int id);
		Task RollBackInstrumentAsync(int id);
		Task ToggleInstrumentActiveAsync(int id, bool isActive);

	}
}
