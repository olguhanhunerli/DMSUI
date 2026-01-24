using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Instruments;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
	public class InstrumentManager : IInstrumentManager
	{
		private readonly IInstrumentApiClient _instrumentApiClient;

		public InstrumentManager(IInstrumentApiClient instrumentApiClient)
		{
			_instrumentApiClient = instrumentApiClient;
		}

		public async Task CreateInstrumentAsync(CreateInstrumentDTO dto)
		{
			await _instrumentApiClient.CreateInstrumentAsync(dto);
		}

		public async Task DeleteInstrumentAsync(int id)
		{
			await _instrumentApiClient.DeleteInstrumentAsync(id);
		}

		public async Task<PagedResultDTO<InstrumentDTO>> GetDeletedByInstrumentsAsync(int pageNumber, int pageSize)
		{
			return await _instrumentApiClient.GetDeletedByInstrumentsAsync(pageNumber, pageSize);
		}

		public async Task<InstrumentDTO> GetInstrumentByIdAsync(int instrumentId)
		{
			return await _instrumentApiClient.GetInstrumentByIdAsync(instrumentId);
		}

		public async Task<InstrumentDTO> GetInstrumentDeletedByIdAsync(int instrumentId)
		{
			return await _instrumentApiClient.GetInstrumentDeletedByIdAsync(instrumentId);
		}

		public async Task<PagedResultDTO<InstrumentDTO>> GetInstrumentsAsync(int pageNumber, int pageSize)
		{
			return await _instrumentApiClient.GetInstrumentsAsync(pageNumber, pageSize);
		}

		public async Task RollBackInstrumentAsync(int id)
		{
			await _instrumentApiClient.RollBackInstrumentAsync(id);
		}

		public async Task ToggleInstrumentActiveAsync(int id, bool isActive)
		{
			await _instrumentApiClient.ToggleInstrumentActiveAsync(id, isActive);
		}

		public async Task UpdateInstrumentAsync(int id,UpdateInstrumentDTO dto)
		{
			await _instrumentApiClient.UpdateInstrumentAsync(id,dto);
		}
	}
}
