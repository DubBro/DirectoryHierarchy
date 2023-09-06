namespace DirectoryHierarchy.Controllers
{
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly ILogger<FolderController> _logger;

        public FolderController(IFolderService folderService, ILogger<FolderController> logger)
        {
            _folderService = folderService;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(FolderDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string? path)
        {
            try
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
            catch (FolderNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
