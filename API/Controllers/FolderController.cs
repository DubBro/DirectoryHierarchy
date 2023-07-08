namespace DirectoryHierarchy.Controllers
{
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(FolderDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string? path)
        {
            if (path == null)
            {
                var rootFolders = await _folderService.GetRootFoldersAsync();

                var result = new FolderDTO()
                {
                    Id = 0,
                    Name = "Root",
                    SubFolders = rootFolders,
                    ParentId = null,
                };

                return Ok(result);
            }
            else
            {
                var result = await _folderService.GetFolderAsync(path);
                return Ok(result);
            }
        }
    }
}
