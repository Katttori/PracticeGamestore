using Microsoft.AspNetCore.Http;
using Moq;
using PracticeGamestore.Business.Mappers;
using PracticeGamestore.DataAccess.Entities;

namespace PracticeGamestore.Tests.TestData;

public static class File
{
    public static List<PracticeGamestore.DataAccess.Entities.File> GenerateFileEntities()
    {
        var games = Game.GenerateGameEntities();

        return new List<PracticeGamestore.DataAccess.Entities.File>
        {
            new()
            {
                Id = Guid.NewGuid(),
                GameId = games.First().Id,
                Size = 1024,
                Path = "GameFiles/gamefile1.pdf",
                CreationDate = new DateTime(2024, 5, 10),
                Type = "application/pdf",
                Game = games.First(g => g.Id == games.First().Id)
            },
            new()
            {
                Id = Guid.NewGuid(),
                GameId = games.First(g => g.Name == "Mystic Forest Adventure").Id,
                Size = 2048,
                Path = "GameFiles/gamefile2.exe",
                CreationDate = new DateTime(2024, 6, 15),
                Type = "application/x-msdownload",
                Game = games.First(g => g.Name == "Mystic Forest Adventure")
            }
        };
    }

    public static PracticeGamestore.DataAccess.Entities.File GenerateFileEntity(Guid? id = null, Guid? gameId = null)
    {
        var realId = id ?? Guid.NewGuid();
        var realGameId = gameId ?? Guid.NewGuid();

        return new PracticeGamestore.DataAccess.Entities.File
        {
            Id = realId,
            GameId = realGameId,
            Size = 3072,
            Path = "GameFiles/sample.dat",
            CreationDate = DateTime.UtcNow,
            Type = "application/octet-stream",
            Game = new PracticeGamestore.DataAccess.Entities.Game
            {
                Id = realGameId,
                Name = "Sample Game",
                Description = "This is a sample game for testing purposes.",
                ReleaseDate = DateTime.UtcNow.AddYears(-1),
                Price = 19.99m,
                Rating = 4.5f,
                Key = "SAMPLE-KEY-1234",
            }
        };
    }

    public static PracticeGamestore.Business.DataTransferObjects.FileDto GenerateFileDto(Guid? id = null,
        Guid? gameId = null)
    {
        var realId = id ?? Guid.NewGuid();
        var realGameId = gameId ?? Guid.NewGuid();

        // MOCKED file
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("testfile.dat");
        mockFile.Setup(f => f.Length).Returns(4096);
        mockFile.Setup(f => f.ContentType).Returns("application/octet-stream");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 0x00, 0x01 }));
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns<Stream, CancellationToken>((stream, _) => stream.WriteAsync(new byte[] { 0x00, 0x01 }, 0, 2));

        return new PracticeGamestore.Business.DataTransferObjects.FileDto
        {
            Id = realId,
            GameId = realGameId,
            Size = 4096,
            Path = $"GameFiles/{realId}.dat",
            CreationDate = DateTime.UtcNow,
            Type = "application/octet-stream",
            File = mockFile.Object
        };
    }
    
    public static List<PracticeGamestore.Business.DataTransferObjects.FileDto> GenerateFileDtos()
    {
        return GenerateFileEntities().Select(file => file.MapToFileDto()).ToList();
    }
}