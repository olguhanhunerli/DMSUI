using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface IDocumentAttachmentManager
    {
        Task UploadMultipleAsync(int documentId, List<IFormFile> files);

    }
}
