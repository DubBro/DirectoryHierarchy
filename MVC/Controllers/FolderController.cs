using MVC.Models.ViewModels;

namespace MVC.Controllers
{
    public class FolderController : Controller
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        public async Task<IActionResult> Index(string? path)
        {
            var data = await _folderService.GetFolderAsync(path);

            if (data == null)
            {
                return View("Error");
            }

            var sf = new List<SubFolderViewModel>();

            if (data.Id == 0)
            {
                foreach (var subfolder in data.SubFolders)
                {
                    sf.Add(new SubFolderViewModel()
                    {
                        Name = subfolder.Name,
                        Link = new Uri($"{Request.GetEncodedUrl()}{HttpUtility.UrlEncode(subfolder.Name)}"),
                    });
                }
            }
            else
            {
                foreach (var subfolder in data.SubFolders)
                {
                    sf.Add(new SubFolderViewModel()
                    {
                        Name = subfolder.Name,
                        Link = new Uri($"{Request.GetEncodedUrl()}/{HttpUtility.UrlEncode(subfolder.Name)}"),
                    });
                }
            }

            var vm = new IndexViewModel()
            {
                Title = data.Name,
                SubFolders = sf,
            };

            return View(vm);
        }
    }
}