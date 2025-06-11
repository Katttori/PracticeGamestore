using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Services.File;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.File;

namespace PracticeGamestore.Controllers;

[ApiController]
[Route("files")]
public class FileController(IFileService fileService, ILogger<FileController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var files = await fileService.GetAllAsync();
        return Ok(files);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var file  = await fileService.GetByIdAsync(id);
        if (file is null)
        {
            logger.LogError("File with id: {Id} was not found.", id);
            return NotFound(ErrorMessages.NotFound("File", id));
        }
        
        var bytes = await fileService.ReadPhysicalFileAsync(file.Path);
        return File(bytes, file.Type, Path.GetFileName(file.Path));
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] FileRequestModel request)
    {
        var id = await fileService.UploadAsync(request.MapToFileDto());
        if (id is null)
        {
            logger.LogError("Failed to upload file for request: {Request}", request);
            return BadRequest(ErrorMessages.FailedFileUpload);
        }
        
        logger.LogInformation("Uploaded file with id: {Id}", id);
        return CreatedAtAction(nameof(Download), new { id }, id);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var file = await fileService.GetByIdAsync(id);
        if (file is null)
        {
            logger.LogError("File with id: {Id} was not found.", id);
            return NotFound(ErrorMessages.NotFound("File", id));
        }
        
        await fileService.DeleteAsync(id);
        logger.LogInformation("Deleted file with id: {Id}", id);
        return NoContent();
    }
}