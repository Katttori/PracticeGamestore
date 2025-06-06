using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Services.File;
using PracticeGamestore.Controllers;
using PracticeGamestore.Models.File;

namespace PracticeGamestore.Tests.Unit.File;

public class FileControllerTests
{
    private Mock<IFileService> _fileService;
    private Mock<IPhysicalFileService> _physicalFileService;
    private Mock<ILogger<FileController>> _logger;
    private FileController _controller;

    [SetUp]
    public void Setup()
    {
        _fileService = new Mock<IFileService>();
        _physicalFileService = new Mock<IPhysicalFileService>();
        _logger = new Mock<ILogger<FileController>>();
        _controller = new FileController(_fileService.Object, _physicalFileService.Object, _logger.Object);
    }

    [Test]
    public async Task GetAllFiles_ShouldReturnOkWithFiles()
    {
        var dtos = TestData.File.GenerateFileDtos();
        _fileService.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

        var result = await _controller.GetAll();
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
    public async Task Download_ShouldReturnFileResult_WhenFileExists()
    {
        var dto = TestData.File.GenerateFileDto();
        _fileService.Setup(s => s.GetByIdAsync(dto.Id!.Value)).ReturnsAsync(dto);
        _physicalFileService.Setup(s => s.ReadFileAsync(dto.Path))
            .ReturnsAsync(Encoding.UTF8.GetBytes("test content"));

        // simulate file on disk
        var dummyContent = Encoding.UTF8.GetBytes("test content");
        Directory.CreateDirectory(Path.GetDirectoryName(dto.Path)!);
        await System.IO.File.WriteAllBytesAsync(dto.Path, dummyContent);

        var result = await _controller.Download(dto.Id.Value);
        var fileResult = result as FileContentResult;

        Assert.Multiple(() =>
        {
            Assert.That(fileResult, Is.Not.Null);
            Assert.That(fileResult!.FileContents, Is.EqualTo(dummyContent));
            Assert.That(fileResult.ContentType, Is.EqualTo(dto.Type));
        });

        System.IO.File.Delete(dto.Path);
    }

    [Test]
    public async Task Download_ShouldReturnNotFound_WhenMissing()
    {
        var id = Guid.NewGuid();
        _fileService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((FileDto?)null);

        var result = await _controller.Download(id);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task Upload_ShouldReturnCreated_WhenSuccess()
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
    public async Task Upload_ShouldReturnBadRequest_WhenFailed()
    {
        var dto = TestData.File.GenerateFileDto();
        var model = new FileRequestModel { GameId = dto.GameId, File = dto.File };
        _fileService.Setup(s => s.UploadAsync(It.IsAny<FileDto>())).ReturnsAsync((Guid?)null);

        var result = await _controller.Upload(model);
        var badRequest = result as BadRequestObjectResult;

        Assert.That(badRequest, Is.Not.Null);
    }

    [Test]
    public async Task Delete_ShouldReturnNoContent_WhenSuccess()
    {
        var dto = TestData.File.GenerateFileDto();
        _fileService.Setup(s => s.GetByIdAsync(dto.Id!.Value)).ReturnsAsync(dto);

        var result = await _controller.Delete(dto.Id.Value);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Delete_ShouldReturnNotFound_WhenMissing()
    {
        var id = Guid.NewGuid();
        _fileService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((FileDto?)null);

        var result = await _controller.Delete(id);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }
}