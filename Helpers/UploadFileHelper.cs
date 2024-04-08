namespace TrainingFPT.Helpers
{
    public static class UploadFileHelper
    {
        public static string UploadFile(IFormFile file, string? nameFolder)
        {
            string uniqueFileName = string.Empty;
            try
            {
                string pathUploadServer = string.Empty;
                switch (nameFolder)
                {
                    case "image":
                        pathUploadServer = "wwwroot\\uploads\\images";
                        break;
                    case "videos":
                        pathUploadServer = "wwwroot\\uploads\\videos";
                        break;
                    case "audios":
                        pathUploadServer = "wwwroot\\uploads\\audios";
                        break;
                    case "documents":
                        pathUploadServer = "wwwroot\\uploads\\documents";
                        break;
                    default:
                        pathUploadServer = "wwwroot\\uploads\\images";
                        break;
                }
                
                string extension = Path.GetExtension(file.FileName);
                string uniqueStr = Guid.NewGuid().ToString();
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                string fileNameUpload = uniqueStr + "-" + time + extension;

                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadServer, fileNameUpload);
                var stream = new FileStream(uploadPath, FileMode.Create);
                file.CopyToAsync(stream);
                uniqueFileName = fileNameUpload;

            }
            catch (Exception ex) 
            {
                uniqueFileName = ex.Message.ToString();

            }

            return uniqueFileName;
        }
    }
}
