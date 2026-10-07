using Microsoft.AspNetCore.Components.Forms;

namespace GLMS.POE.Backend.Services.Interfaces
{
    public interface IFileService
    {
        Task<(string path, string fileName)> SaveContractFileAsync(IBrowserFile file);

        bool IsValidPdf(string fileName);

        void DeleteFile(string path);
    }
}
