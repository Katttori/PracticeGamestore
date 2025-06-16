using Microsoft.AspNetCore.Http;
using Moq;
using PracticeGamestore.Business.Mappers;

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
                Path = "GameFiles/gamefile1.zip",
                CreationDate = new DateTime(2024, 5, 10),
                Type = "application/zip",
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
            Path = "GameFiles/sample.zip",
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

    public static Business.DataTransferObjects.FileDto GenerateFileDto(Guid? id = null,
        Guid? gameId = null)
    {
        var realId = id ?? Guid.NewGuid();
        var realGameId = gameId ?? Guid.NewGuid();
        
        return new Business.DataTransferObjects.FileDto
        {
            Id = realId,
            GameId = realGameId,
            Size = 4096,
            Path = $"GameFiles/{realId}.zip",
            CreationDate = DateTime.UtcNow,
            Type = "application/octet-stream",
            File = GenerateFile("testfile.zip", 4096)
        };
    }
    
    public static List<Business.DataTransferObjects.FileDto> GenerateFileDtos()
    {
        return GenerateFileEntities().Select(file => file.MapToFileDto()).ToList();
    }
    
    public static IFormFile GenerateFile(string fileNameWithExtension, long length)
    {
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns(fileNameWithExtension);
        mockFile.Setup(f => f.Length).Returns(length);
        mockFile.Setup(f => f.ContentType).Returns("application/octet-stream");
        mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream([0x00, 0x01]));
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns<Stream, CancellationToken>((stream, _) => stream.WriteAsync([0x00, 0x01], 0, 2));
        return mockFile.Object;
    }
}