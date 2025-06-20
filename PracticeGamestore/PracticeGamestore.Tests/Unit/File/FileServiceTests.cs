﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.Business.Services.File;
using PracticeGamestore.DataAccess.Repositories.File;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Tests.Unit.File;

public class FileServiceTests
{
    private Mock<IFileRepository> _fileRepository;
    private Mock<IGameRepository> _gameRepository;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IWebHostEnvironment> _webHostEnvironment;
    private Mock<IConfiguration> _configuration;
    private Mock<ILogger<FileService>> _logger;
    private IFileService _fileService;

    [SetUp]
    public void Setup()
    {
        _fileRepository = new Mock<IFileRepository>();
        _gameRepository = new Mock<IGameRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _webHostEnvironment = new Mock<IWebHostEnvironment>();
        _configuration = new Mock<IConfiguration>();
        _logger = new Mock<ILogger<FileService>>();
        
        _webHostEnvironment.Setup(e => e.ContentRootPath).Returns("C:\\app");
        _configuration.Setup(c => c["Storage:GameFilesPath"]).Returns("GameFiles");
        
        _fileService = new FileService(
            _fileRepository.Object,
            _gameRepository.Object,
            _unitOfWork.Object,
            _webHostEnvironment.Object,
            _configuration.Object,
            _logger.Object);
    }

    [Test]
    public async Task GetAll_WhenFilesExist_ShouldReturnAllFiles()
    {
        // Arrange
        var files = TestData.File.GenerateFileEntities();

        _fileRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(files);

        // Act
        var result = (await _fileService.GetAllAsync()).ToList();

        // Assert   
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(files.Count));
        Assert.That(result[0].Id, Is.EqualTo(files[0].Id));
        Assert.That(result[0].GameId, Is.EqualTo(files[0].GameId));
    }

    [Test]
    public async Task GetByIdAsync_WhenFileExists_ShouldReturnFileDto()
    {
        // Arrange
        var file = TestData.File.GenerateFileEntity();

        _fileRepository.Setup(repo => repo.GetByIdAsync(file.Id)).ReturnsAsync(file);

        // Act
        var result = await _fileService.GetByIdAsync(file.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(file.Id));
        Assert.That(result?.GameId, Is.EqualTo(file.GameId));
    }

    [Test]
    public async Task GetByIdAsync_WhenFileDoesNotExists_ShouldReturnNull()
    {
        // Arrange
        _fileRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as DataAccess.Entities.File);

        // Act
        var result = await _fileService.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateAsync_WhenChangesSavedSuccessfully_ShouldAddFile()
    {
        // Arrange
        var fileDto = TestData.File.GenerateFileDto();

        _fileRepository.Setup(p => p.CreateAsync(It.IsAny<DataAccess.Entities.File>()))
            .ReturnsAsync(fileDto.MapToFileEntity().Id);
        _gameRepository
            .Setup(g => g.GetByIdAsync(fileDto.GameId))
            .ReturnsAsync(TestData.Game.GenerateGameEntity(
                TestData.Publisher.GeneratePublisherEntities(),
                TestData.Genre.GenerateGenreEntities(),
                TestData.Platform.GeneratePlatformEntities()));
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _fileService.UploadAsync(fileDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(fileDto.MapToFileEntity().Id));
    }

    [Test]
    public async Task CreateAsync_WhenChangesNotSaved_ShouldReturnNull()
    {
        // Arrange
        var fileDto = TestData.File.GenerateFileDto();

        _fileRepository
            .Setup(p => p.CreateAsync(It.IsAny<DataAccess.Entities.File>()))
            .ReturnsAsync(fileDto.MapToFileEntity().Id);

        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        // Act
        var result = await _fileService.UploadAsync(fileDto);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteAsync_WhenFileIsDeleted_ShouldCallDeleteAndSaveChanges()
    {
        // Arrange
        var fileId = Guid.NewGuid();

        _fileRepository
            .Setup(p => p.DeleteAsync(fileId))
            .Returns(Task.CompletedTask);

        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        var mockFile = TestData.File.GenerateFileEntity(fileId);
        _fileRepository.Setup(r => r.GetByIdAsync(fileId)).ReturnsAsync(mockFile);

        // Act
        await _fileService.DeleteAsync(fileId);

        // Assert
        _fileRepository.Verify(p => p.DeleteAsync(fileId), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}