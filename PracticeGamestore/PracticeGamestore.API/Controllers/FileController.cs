using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using PracticeGamestore.Business.Constants;
using Microsoft.AspNetCore.Authorization;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Services.File;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Mappers;
using PracticeGamestore.Models.File;

namespace PracticeGamestore.Controllers;

[ApiController]
[Route("files")]
public class FileController(
    IFileService fileService,
    IHeaderHandleService headerHandleService,
    ILogger<FileController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Manager))]
    public async Task<IActionResult> GetAll(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
        var files = await fileService.GetAllAsync();
        return Ok(files);
    }

    [HttpGet("{id:guid}/download")]
    [Authorize(Roles = nameof(UserRole.User))]
    public async Task<IActionResult> Download(
        [FromHeader(Name = HeaderNames.LocationCountry), Required] string country,
        [FromHeader(Name = HeaderNames.UserEmail), Required] string email,
        Guid id)
    {
        await headerHandleService.CheckAccessAsync(country, email);
        
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
    [Authorize(Roles = nameof(UserRole.Manager))]
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
    [Authorize(Roles = nameof(UserRole.Admin))]
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