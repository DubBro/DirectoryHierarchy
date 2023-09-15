using MVC.Models.ViewModels;

namespace MVC.Controllers
{
    public class FolderController : Controller
    {
        private readonly IFolderService _folderService;
        private readonly ILogger<FolderController> _logger;

        public FolderController(IFolderService folderService, ILogger<FolderController> logger)
        {
            _folderService = folderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? path)
        {
            try
            {
                var data = await _folderService.GetFolderAsync(path);

                var vm = MapFolderModelToIndexViewModel(data, Request);

                ViewData["Path"] = HttpUtility.UrlDecode(Request.Path).Trim('/');

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        [Route("{action}")]
        public async Task<IActionResult> CreateFolder(string? path, string folderName)
        {
            try
            {
                ViewData["Url"] = $"{Request.Scheme}://{Request.Host.Value}";
                ViewData["NewFolder"] = folderName;
                ViewData["Path"] = path == null ? "" : path.Replace(" ", "+");

                await _folderService.CreateFolderAsync(path, folderName);

                return View("CreateFolderSuccess");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("CreateFolderFailed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View("Error");
            }
        }

        private static IndexViewModel MapFolderModelToIndexViewModel(Folder folder, HttpRequest request)
        {
            var sf = new List<SubFolderViewModel>();

            if (folder.Id == 0)
            {
                foreach (var subfolder in folder.SubFolders)
                {
                    sf.Add(new SubFolderViewModel()
                    {
                        Name = subfolder.Name,
                        Link = new Uri($"{request.GetEncodedUrl()}{HttpUtility.UrlEncode(subfolder.Name)}"),
                    });
                }
            }
            else
            {
                foreach (var subfolder in folder.SubFolders)
                {
                    sf.Add(new SubFolderViewModel()
                    {
                        Name = subfolder.Name,
                        Link = new Uri($"{request.GetEncodedUrl()}/{HttpUtility.UrlEncode(subfolder.Name)}"),
                    });
                }
            }

            return new IndexViewModel()
            {
                Title = folder.Name,
                SubFolders = sf,
            };
        }
    }
}