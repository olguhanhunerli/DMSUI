using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IDocumentManager
    {
        Task<PagedResultDTO<DocumentListDTO>> GetPagedAsync(int page, int pageSize);
    }
}
