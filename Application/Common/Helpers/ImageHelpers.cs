using Microsoft.AspNetCore.Http;

namespace Common.Helpers
{
    public static class ImageHelpers
    {
        public static bool IsImage(IFormFile file)
        {
            try
            {
                using (var image = Image.Load(file.OpenReadStream()))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
