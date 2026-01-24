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

		public async Task<PagedResultDTO<InstrumentDTO>> GetInstrumentsAsync(int pageNumber, int pageSize)
		{
			return await _instrumentApiClient.GetInstrumentsAsync(pageNumber, pageSize);
		}
	}
}
