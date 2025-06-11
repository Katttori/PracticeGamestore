using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PracticeGamestore.Business.Services.File;
using PracticeGamestore.Business.Services.Location;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.File;

namespace PracticeGamestore.Controllers;

[ApiController]
[Route("files")]
public class FileController(
    IFileService fileService,
    ILocationService locationService,
    ILogger<FileController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromHeader(Name = "X-Location-Country"), Required]
        string country,
        [FromHeader(Name = "X-User-Email"), Required]
        string email)
    {
        await locationService.HandleLocationAccessAsync(country, email);
        
        var files = await fileService.GetAllAsync();
        return Ok(files);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(
        [FromHeader(Name = "X-Location-Country"), Required]
        string country,
        [FromHeader(Name = "X-User-Email"), Required]
        string email,
        Guid id)
    {
        await locationService.HandleLocationAccessAsync(country, email);
        
        var file  = await fileService.GetByIdAsync(id);
        if (file is null)
        {
            logger.LogError("File with id: {Id} was not found.", id);
            return NotFound($"File with id: {id} was not found.");
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
            return BadRequest("Failed to upload file.");
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
            return NotFound($"File with id: {id} was not found.");
        }
        
        await fileService.DeleteAsync(id);
        logger.LogInformation("Deleted file with id: {Id}", id);
        return NoContent();
    }
}