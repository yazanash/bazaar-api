using Bazaar.app.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Bazaar.app.Services
{
    public class WebPImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WebPImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        } 
        public async Task<string> SaveImageAsWebP(IFormFile file,string folderName ,string fileNameWithoutExtension)
        {
            var rootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(rootPath, folderName);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{fileNameWithoutExtension}.webp";
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                if (image.Width > 1080)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(1080, 0),
                        Mode = ResizeMode.Max
                    }));
                }

                await image.SaveAsWebpAsync(filePath, new WebpEncoder
                {
                    Quality = 80,
                    Method = WebpEncodingMethod.BestQuality
                });
            }
            return $"{folderName}/{fileName}";
        }
        public void MoveFile(string relativeSourcePath, string relativeDestinationPath)
        {
            var sourceFullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativeSourcePath);
            var destFullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativeDestinationPath);

            var destDirectory = Path.GetDirectoryName(destFullPath);
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory!);
            }
            if (System.IO.File.Exists(sourceFullPath))
            {
                if (System.IO.File.Exists(destFullPath))
                    System.IO.File.Delete(destFullPath);
                System.IO.File.Move(sourceFullPath, destFullPath);
            }
        }
        public void DeleteImage(string image)
        {
            var parts = image.Split('/');
            if (parts.Length < 2) throw new InvalidFileFormatException();

            string folder = parts[0];
            string fileName = Path.GetFileName(parts[1]);

            var allowedFolders = new[] { "temp", "ads" };
            if (!allowedFolders.Contains(folder.ToLower()))
                throw new FolderAccessDeniedException();

            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);

            }
        }

    }
}
