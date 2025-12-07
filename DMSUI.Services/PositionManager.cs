using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Position;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
	public class PositionManager : IPositionManager
	{
		private readonly IPositionApiClient _positionApiClient;

		public PositionManager(IPositionApiClient positionApiClient)
		{
			_positionApiClient = positionApiClient;
		}

		public async Task<bool> AddPositionAsync(PositionCreateDTO position)
		{
			return await _positionApiClient.AddPositionAsync(position);	
		}

		public async Task<List<PositionListDTO>> GetAllPositionsAsync()
		{
			return await _positionApiClient.GetAllPositionsAsync();
		}

		public async Task<PositionDetailDTO> GetPositionByIdAsync(int id)
		{
			return await _positionApiClient.GetPositionByIdAsync(id);
		}
	}
}
