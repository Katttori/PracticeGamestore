using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.File;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.File;

namespace PracticeGamestore.Tests.Unit.File;

public class FileControllerTests
{
    private Mock<IFileService> _fileService;
    private Mock<IHeaderHandleService> _headerHandleService;
    private Mock<ILogger<FileController>> _logger;
    private FileController _controller;

    private const string CountryHeader = "Ukraine";
    private const string UserEmailHeader = "test@gmail.com";

    [SetUp]
    public void Setup()
    {
        _fileService = new Mock<IFileService>();
        _headerHandleService = new Mock<IHeaderHandleService>();
        _logger = new Mock<ILogger<FileController>>();
        _controller = new FileController(_fileService.Object, _headerHandleService.Object, _logger.Object);
    }

    [Test]
    public async Task GetAllFiles_WhenFilesExist_ShouldReturnOkWithFiles()
    {
        var dtos = TestData.File.GenerateFileDtos();
        _fileService.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

        var result = await _controller.GetAll(CountryHeader, UserEmailHeader);
        var ok = result as OkObjectResult;
        var value = ok?.Value as IEnumerable<FileDto>;

        Assert.Multiple(() =>
        {
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.StatusCode, Is.EqualTo(200));
            Assert.That(value, Is.Not.Null);
            Assert.That(value!.Count(), Is.EqualTo(dtos.Count));
        });
    }

    [Test]
    public async Task Download_WhenFileExists_ShouldReturnFileResult()
    {
        var dto = TestData.File.GenerateFileDto();
        
        _fileService.Setup(s => s.GetByIdAsync(dto.Id!.Value)).ReturnsAsync(dto);
        _fileService.Setup(s => s.ReadPhysicalFileAsync(dto.Path)).ReturnsAsync(Encoding.UTF8.GetBytes("File content"));
        
        var result = await _controller.Download(CountryHeader, UserEmailHeader, dto.Id.Value);
        var fileResult = result as FileContentResult;
        
        Assert.Multiple(() =>
        {
            Assert.That(fileResult, Is.Not.Null);
            Assert.That(fileResult!.FileContents, Is.Not.Null);
            Assert.That(fileResult.FileContents.Length, Is.GreaterThan(0));
            Assert.That(fileResult.ContentType, Is.EqualTo(dto.Type));
            Assert.That(fileResult.FileDownloadName, Is.EqualTo(Path.GetFileName(dto.Path)));
        });
    }

    [Test]
    public async Task Download_WhenMissing_ShouldReturnNotFound()
    {
        var id = Guid.NewGuid();
        _fileService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((FileDto?)null);

        var result = await _controller.Download(CountryHeader, UserEmailHeader, id);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Upload_WhenSuccess_ShouldReturnCreated()
    {
        var dto = TestData.File.GenerateFileDto();
        var model = new FileRequestModel { GameId = dto.GameId, File = dto.File };
        _fileService.Setup(s => s.UploadAsync(It.IsAny<FileDto>())).ReturnsAsync(dto.Id);

        var result = await _controller.Upload(model);
        var created = result as CreatedAtActionResult;

        Assert.Multiple(() =>
        {
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.StatusCode, Is.EqualTo(201));
            Assert.That(created.RouteValues!["id"], Is.EqualTo(dto.Id));
        });
    }

    [Test]
    public async Task Upload_WhenFailed_ShouldReturnBadRequest()
    {
        var dto = TestData.File.GenerateFileDto();
        var model = new FileRequestModel { GameId = dto.GameId, File = dto.File };
        _fileService.Setup(s => s.UploadAsync(It.IsAny<FileDto>())).ReturnsAsync((Guid?)null);

        var result = await _controller.Upload(model);
        var badRequest = result as BadRequestObjectResult;

        Assert.That(badRequest, Is.Not.Null);
    }

    [Test]
    public async Task Delete_WhenSuccess_ShouldReturnNoContent()
    {
        var dto = TestData.File.GenerateFileDto();
        _fileService.Setup(s => s.GetByIdAsync(dto.Id!.Value)).ReturnsAsync(dto);

        var result = await _controller.Delete(dto.Id.Value);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Delete_WhenMissing_ShouldReturnNotFound()
    {
        var id = Guid.NewGuid();
        _fileService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((FileDto?)null);

        var result = await _controller.Delete(id);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
}