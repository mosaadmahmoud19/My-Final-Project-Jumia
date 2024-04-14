using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace My_Final_Project.Controllers
{
    public class ImagesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImagesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UploadPhoto(IFormFile Img)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            if (request.ContentType is null || !request.ContentType.Contains("multipart/form-data"))
            {
                throw new ArgumentException("Wrong content type");
            }
            if (Img == null || Img.Length <= 0)
            {
                throw new ArgumentException("No File Found");
            }
            var allowedExtensions = new string[] { ".jpg", ".svg", ".png", ".jpeg" , "webp" };
            if (!allowedExtensions.Any(ext => Img.FileName.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException("Not Valid Extension");
            }
            if (Img.Length > 3_000_000)
            {
                throw new ArgumentException("Max Size exceeded");
            }
            var projectFolder = Directory.GetCurrentDirectory();
            var relativeImagesPath = Path.Combine("wwwroot", "Images");
            var fullImagesPath = Path.Combine(projectFolder, relativeImagesPath);
            var fileName = $"{Guid.NewGuid()}_{Img.FileName}";
            var fullImagePath = Path.Combine(fullImagesPath, fileName);
            using (var stream = new FileStream(fullImagePath, FileMode.Create))
            {
                Img.CopyTo(stream);
            }
            var url = $"{request.Scheme}://{request.Host}/wwwroot/Images/{fileName}";
            return url;
        }
    }
}
