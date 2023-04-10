using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Web.Helper;
namespace Web.Helper
{
    public class ImageManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public List<string> UploadedFile(List<IFormFile> images)
        {
            List<string> uniqueFileNames = new List<string>();

            if (images != null)
            {
                foreach (var img in images)
                {
                    if (img.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "MediaUpload");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        string extension = Path.GetExtension(img.FileName);

                        string uniqueImg = "_" + Guid.NewGuid().ToString() + extension;
                        uniqueFileNames.Add(uniqueImg);
                        string filePath = Path.Combine(uploadsFolder, uniqueImg);
                        using (var image = Image.Load(img.OpenReadStream()))
                        {
                            string newSize = new ImageCompress().ResizeImage(image, 610, 383);
                            string[] aSize = newSize.Split(",");
                            image.Mutate(h => h.Resize(Convert.ToInt32(aSize[1]), Convert.ToInt32(aSize[0])));
                            image.Save(filePath);
                        }
                    }
                }
            }
            return uniqueFileNames;
        }
        public bool DeleteImage(string ImageName)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath + "/MediaUpload/" + ImageName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
