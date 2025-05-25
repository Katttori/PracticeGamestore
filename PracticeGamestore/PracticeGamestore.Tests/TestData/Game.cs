using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.DataAccess.Enums;
using PracticeGamestore.Models.Game;

namespace PracticeGamestore.Tests.TestData;

public static class Game
{
    public static List<DataAccess.Entities.Game> GenerateGameEntities()
    {
        var publishers = Publisher.GeneratePublisherEntities();
        var platforms = Platform.GeneratePlatformEntities();
        var genres = Genre.GenerateGenreEntities();
        var cyberWarriorsId = Guid.NewGuid();
        var mysticForestId = Guid.NewGuid();
        var spaceColonyId = Guid.NewGuid();
        var dragonsLegacyId = Guid.NewGuid();
        var streetRacerId = Guid.NewGuid();
        var fifaChampionsId = Guid.NewGuid();
        var cityArchitectId = Guid.NewGuid();
        var hauntedMansionId = Guid.NewGuid();
        var puzzleMasterId = Guid.NewGuid();
        var galacticWarfareId = Guid.NewGuid();
        var wildWestId = Guid.NewGuid();
        var cyberShooterId = Guid.NewGuid();
        
        var games = new List<DataAccess.Entities.Game>
        {
            new ()
            {
                Id = cyberWarriorsId,
                Name = "Cyber Warriors 2077",
                Key = "4uiru78rh6x84",
                Price = 59.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
                Description = "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
                Rating = 4.5,
                AgeRating = AgeRating.EighteenPlus,
                ReleaseDate = new DateTime(2023, 11, 15),
                PublisherId = publishers[0].Id,
                Publisher = publishers[0], 
                GamePlatforms =
                [
                    new() { GameId = cyberWarriorsId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = cyberWarriorsId, PlatformId = platforms[1].Id, Platform = platforms[1] }
                ],
                GameGenres = [new() { GameId = cyberWarriorsId, GenreId = genres[0].Id, Genre = genres[0] }]
            },
            new ()
            {
                Id = mysticForestId,
                Name = "Mystic Forest Adventure",
                Key = "kuy32fe7367636872ey",
                Price = 29.99m,
                Description = "A magical journey through enchanted forests where you solve puzzles and befriend mystical creatures.",
                Rating = 4.2,
                AgeRating = AgeRating.ThreePlus,
                ReleaseDate = new DateTime(2024, 3, 8),
                PublisherId = publishers[1].Id,
                Publisher = publishers[1],
                GamePlatforms = [new() { GameId = mysticForestId, PlatformId = platforms[0].Id, Platform = platforms[0] }],
                GameGenres = [new() { GameId = mysticForestId, GenreId = genres[1].Id, Genre = genres[1] }]
            },
            new ()
            {
                Id = spaceColonyId,
                Name = "Space Colony Builder",
                Key = "35467568467987809807",
                Price = 39.99m,
                Description = "Build and manage your own space colony on distant planets while dealing with resource management and alien threats.",
                Rating = 4.7,
                AgeRating = AgeRating.TwelvePlus,
                ReleaseDate = new DateTime(2024, 1, 22),
                PublisherId = publishers[2].Id,
                Publisher = publishers[2],
                GamePlatforms = [new() { GameId = spaceColonyId, PlatformId = platforms[1].Id, Platform = platforms[1] }],
                GameGenres =
                [
                    new() { GameId = spaceColonyId, GenreId = genres[0].Id, Genre = genres[0] },
                    new() { GameId = spaceColonyId, GenreId = genres[1].Id, Genre = genres[1] }
                ]
            },
            new ()
            {
                Id = dragonsLegacyId,
                Name = "Dragon's Legacy",
                Key = "dragon-legacy-2024",
                Price = 69.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0B],
                Description = "An epic fantasy RPG where you embark on a quest to save the realm from an ancient dragon threat.",
                Rating = 4.8,
                AgeRating = AgeRating.SixteenPlus,
                ReleaseDate = new DateTime(2024, 5, 12),
                PublisherId = publishers[3].Id,
                Publisher = publishers[3],
                GamePlatforms =
                [
                    new() { GameId = dragonsLegacyId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = dragonsLegacyId, PlatformId = platforms[1].Id, Platform = platforms[1] },
                    new() { GameId = dragonsLegacyId, PlatformId = platforms[2].Id, Platform = platforms[2] }
                ],
                GameGenres = 
                [
                    new() { GameId = dragonsLegacyId, GenreId = genres[2].Id, Genre = genres[2] },
                    new() { GameId = dragonsLegacyId, GenreId = genres[4].Id, Genre = genres[4] }
                ]
            },
            
            new ()
            {
                Id = streetRacerId,
                Name = "Street Racer Ultimate",
                Key = "street-racer-ultimate",
                Price = 49.99m,
                Description = "High-octane street racing with customizable cars and underground tournaments.",
                Rating = 4.3,
                AgeRating = AgeRating.TwelvePlus,
                ReleaseDate = new DateTime(2024, 2, 14),
                PublisherId = publishers[0].Id,
                Publisher = publishers[0],
                GamePlatforms =
                [
                    new() { GameId = streetRacerId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = streetRacerId, PlatformId = platforms[2].Id, Platform = platforms[2] }
                ],
                GameGenres = [new() { GameId = streetRacerId, GenreId = genres[5].Id, Genre = genres[5] }]
            },

            new ()
            {
                Id = fifaChampionsId,
                Name = "FIFA Champions 2025",
                Key = "fifa-champions-2025",
                Price = 59.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0C],
                Description = "The ultimate football simulation with realistic gameplay and all your favorite teams.",
                Rating = 4.1,
                AgeRating = AgeRating.ThreePlus,
                ReleaseDate = new DateTime(2024, 9, 22),
                PublisherId = publishers[0].Id,
                Publisher = publishers[0],
                GamePlatforms =
                [
                    new() { GameId = fifaChampionsId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = fifaChampionsId, PlatformId = platforms[1].Id, Platform = platforms[1] },
                    new() { GameId = fifaChampionsId, PlatformId = platforms[2].Id, Platform = platforms[2] }
                ],
                GameGenres = [new() { GameId = fifaChampionsId, GenreId = genres[6].Id, Genre = genres[6] }]
            },

            new ()
            {
                Id = cityArchitectId,
                Name = "City Architect Simulator",
                Key = "city-architect-sim",
                Price = 34.99m,
                Description = "Design and build the city of your dreams with advanced urban planning tools.",
                Rating = 4.6,
                AgeRating = AgeRating.SevenPlus,
                ReleaseDate = new DateTime(2024, 4, 18),
                PublisherId = publishers[1].Id,
                Publisher = publishers[1],
                GamePlatforms = [new() { GameId = cityArchitectId, PlatformId = platforms[0].Id, Platform = platforms[0] }],
                GameGenres = 
                [
                    new() { GameId = cityArchitectId, GenreId = genres[3].Id, Genre = genres[3] },
                    new() { GameId = cityArchitectId, GenreId = genres[7].Id, Genre = genres[7] }
                ]
            },

            new ()
            {
                Id = hauntedMansionId,
                Name = "Haunted Mansion Mystery",
                Key = "haunted-mansion-mystery",
                Price = 24.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0D],
                Description = "Explore a cursed mansion and uncover dark secrets in this spine-chilling horror adventure.",
                Rating = 4.0,
                AgeRating = AgeRating.EighteenPlus,
                ReleaseDate = new DateTime(2024, 10, 31),
                PublisherId = publishers[7].Id,
                Publisher = publishers[7],
                GamePlatforms =
                [
                    new() { GameId = hauntedMansionId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = hauntedMansionId, PlatformId = platforms[4].Id, Platform = platforms[4] }
                ],
                GameGenres = 
                [
                    new() { GameId = hauntedMansionId, GenreId = genres[8].Id, Genre = genres[8] },
                    new() { GameId = hauntedMansionId, GenreId = genres[4].Id, Genre = genres[4] }
                ]
            },

            new ()
            {
                Id = puzzleMasterId,
                Name = "Puzzle Master Collection",
                Key = "puzzle-master-collection",
                Price = 19.99m,
                Description = "A collection of challenging puzzles that will test your logical thinking and problem-solving skills.",
                Rating = 4.4,
                AgeRating = AgeRating.ThreePlus,
                ReleaseDate = new DateTime(2024, 6, 5),
                PublisherId = publishers[5].Id,
                Publisher = publishers[5],
                GamePlatforms =
                [
                    new() { GameId = puzzleMasterId, PlatformId = platforms[3].Id, Platform = platforms[3] },
                    new() { GameId = puzzleMasterId, PlatformId = platforms[0].Id, Platform = platforms[0] }
                ],
                GameGenres = [new() { GameId = puzzleMasterId, GenreId = genres[9].Id, Genre = genres[9] }]
            },

            new ()
            {
                Id = galacticWarfareId,
                Name = "Galactic Warfare",
                Key = "galactic-warfare-2024",
                Price = 54.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0E],
                Description = "Command vast space fleets in epic battles across the galaxy in this real-time strategy masterpiece.",
                Rating = 4.7,
                AgeRating = AgeRating.TwelvePlus,
                ReleaseDate = new DateTime(2024, 7, 20),
                PublisherId = publishers[2].Id,
                Publisher = publishers[2],
                GamePlatforms =
                [
                    new() { GameId = galacticWarfareId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = galacticWarfareId, PlatformId = platforms[4].Id, Platform = platforms[4] }
                ],
                GameGenres = 
                [
                    new() { GameId = galacticWarfareId, GenreId = genres[3].Id, Genre = genres[3] },
                    new() { GameId = galacticWarfareId, GenreId = genres[0].Id, Genre = genres[0] }
                ]
            },

            new ()
            {
                Id = wildWestId,
                Name = "Wild West Outlaws",
                Key = "wild-west-outlaws",
                Price = 59.99m,
                Description = "Live the life of an outlaw in the American frontier with horse riding, gunfights, and moral choices.",
                Rating = 4.9,
                AgeRating = AgeRating.EighteenPlus,
                ReleaseDate = new DateTime(2024, 8, 15),
                PublisherId = publishers[4].Id,
                Publisher = publishers[4],
                GamePlatforms =
                [
                    new() { GameId = wildWestId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = wildWestId, PlatformId = platforms[1].Id, Platform = platforms[1] },
                    new() { GameId = wildWestId, PlatformId = platforms[2].Id, Platform = platforms[2] }
                ],
                GameGenres = 
                [
                    new() { GameId = wildWestId, GenreId = genres[0].Id, Genre = genres[0] },
                    new() { GameId = wildWestId, GenreId = genres[4].Id, Genre = genres[4] }
                ]
            },

            new ()
            {
                Id = cyberShooterId,
                Name = "Cyber Shooter Arena",
                Key = "cyber-shooter-arena",
                Price = 29.99m,
                Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0F],
                Description = "Fast-paced multiplayer FPS set in a neon-lit cyberpunk world with advanced weaponry.",
                Rating = 4.2,
                AgeRating = AgeRating.SixteenPlus,
                ReleaseDate = new DateTime(2024, 12, 1),
                PublisherId = publishers[6].Id,
                Publisher = publishers[6],
                GamePlatforms =
                [
                    new() { GameId = cyberShooterId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                    new() { GameId = cyberShooterId, PlatformId = platforms[4].Id, Platform = platforms[4] }
                ],
                GameGenres = 
                [
                    new() { GameId = cyberShooterId, GenreId = genres[1].Id, Genre = genres[1] },
                    new() { GameId = cyberShooterId, GenreId = genres[0].Id, Genre = genres[0] }
                ]
            }
        };
        return games;
    }

    public static DataAccess.Entities.Game GenerateGameEntity(List<DataAccess.Entities.Publisher> publishers, List<DataAccess.Entities.Genre> genres, List<DataAccess.Entities.Platform> platforms, Guid? id = null)
    { 
        var realId = id ?? Guid.NewGuid();
        return new()
        {
            Id = realId,
            Name = "Cyber Warriors 2077",
            Key = "4uiru78rh6x84",
            Price = 59.99m,
            Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
            Description =
                "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
            Rating = 4.5,
            AgeRating = AgeRating.EighteenPlus,
            ReleaseDate = new DateTime(2023, 11, 15),
            PublisherId = publishers[0].Id,
            Publisher = publishers[0],
            GamePlatforms =
            [
                new() { GameId = realId, PlatformId = platforms[0].Id, Platform = platforms[0] },
                new() { GameId = realId, PlatformId = platforms[1].Id, Platform = platforms[1] }
            ],
            GameGenres = [new() { GameId = realId, GenreId = genres[0].Id, Genre = genres[0] }]
        };
    }
    
    public static GameRequestDto GenerateGameRequestModel(DataAccess.Entities.Game game)
    { 
        return new GameRequestDto(
            game.Id,
            game.Name,
            game.Key,
            game.Price,
            game.Picture,
            game.Description,
            game.Rating,
            (int)game.AgeRating,
            game.ReleaseDate,
            game.PublisherId,
            game.GameGenres.Select(gg => gg.GenreId).ToList(),
            game.GamePlatforms.Select(gp => gp.PlatformId).ToList()
        );
    }
    
    public static List<GameResponseDto> GenerateGameResponseDtos()
    {
        var gameResponseDtos = new List<GameResponseDto>
        {
            new (
                Guid.NewGuid(),
                "Cyber Warriors 2077",
                "4uiru78rh6x84",
                59.99m,
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
                "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
                4.5,
                AgeRating.EighteenPlus,
                new DateTime(2023, 11, 15),
                new(Guid.NewGuid(), "Electronic Arts", "American video game company", "https://www.ea.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "PS5", "PlayStation 5")],
                [new(Guid.NewGuid(), "Action")]
            ),
            new (
                Guid.NewGuid(),
                "Mystic Forest Adventure",
                "kuy32fe7367636872ey",
                29.99m,
                null,
                "A magical journey through enchanted forests where you solve puzzles and befriend mystical creatures.",
                4.2,
                AgeRating.ThreePlus,
                new DateTime(2024, 3, 8),
                new(Guid.NewGuid(), "Ubisoft", "French video game company", "https://www.ubisoft.com"),
                [new(Guid.NewGuid(), "PC", "Personal Computer")],
                [new(Guid.NewGuid(), "FPS")]
            ),
            new (
                Guid.NewGuid(),
                "Space Colony Builder",
                "35467568467987809807",
                39.99m,
                null,
                "Build and manage your own space colony on distant planets while dealing with resource management and alien threats.",
                4.7,
                AgeRating.TwelvePlus,
                new DateTime(2024, 1, 22),
                new(Guid.NewGuid(), "Activision Blizzard", "American video game holding company", "https://www.activisionblizzard.com"),
                [new(Guid.NewGuid(), "PS5", "PlayStation 5")],
                [new(Guid.NewGuid(), "Action"), new(Guid.NewGuid(), "FPS")]
            )
        };
        return gameResponseDtos;
    }
    
    public static GameResponseDto GenerateGameResponseDto()
    {
        return new(
            Guid.NewGuid(),
            "Cyber Warriors 2077",
            "4uiru78rh6x84",
            59.99m,
            [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
            "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
            4.5,
            AgeRating.EighteenPlus,
            new DateTime(2023, 11, 15),
            new(Guid.NewGuid(), "Electronic Arts", "American video game company", "https://www.ea.com"),
            [new(Guid.NewGuid(), "PC", "Personal Computer"), new(Guid.NewGuid(), "PS5", "PlayStation 5")],
            [new(Guid.NewGuid(), "Action")]
        );
    }
    
    public static GameRequestModel GenerateGameRequestModel()
    {
        return new()
        {
            Name = "Cyber Warriors 2077",
            Key = "4uiru78rh6x84",
            Price = 59.99m,
            Picture = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
            Description = "A futuristic action RPG set in a dystopian cyberpunk world where you fight against corporate overlords.",
            Rating = 4.5,
            AgeRating = (int)AgeRating.EighteenPlus,
            ReleaseDate = new DateTime(2023, 11, 15),
            PublisherId = Guid.NewGuid(),
            GenreIds = [Guid.NewGuid()],
            PlatformIds = [Guid.NewGuid(), Guid.NewGuid()]
        };
    }
    
}