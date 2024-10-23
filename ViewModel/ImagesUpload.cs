using System.ComponentModel.DataAnnotations;
using Upload_Files_Mega.Helper;

namespace Upload_Files_Mega.ViewModel
{
    public class ImagesUpload
    {
        [Display(Name = "Image")]
        [Required(ErrorMessage = "Pick an Image")]
        [AllowedExtentions(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile MyFile { get; set; }
    }
}
