using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LawyerTimeTracker.Utils
{
    public static class IFormFileUtils
    {
        public static async Task<byte[]> GetImage(IFormFile image)
        {
            using MemoryStream ms = new MemoryStream();
            await image.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}