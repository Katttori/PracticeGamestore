using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using PracticeGamestore.Filters;

namespace PracticeGamestore.Tests.Unit.Filters;

[TestFixture]
public class BirthdateRestrictionFilterTests
{
    private ActionExecutingContext _context;
    private BirthdateRestrictionFilter _filter;

    [SetUp]
    public void SetUp()
    {
        _context = new ActionExecutingContext(
            new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            controller: new object());
        _filter = new BirthdateRestrictionFilter();
    }

    [Test]
    public void OnActionExecuting_ShouldSetUnderageValueToFalseInHttpContextIfUserIsAdultBasedOnProvidedXBirthdateHeader()
    {
        // Arrange
        _context.HttpContext.Request.Headers.Append("X-Birthdate", "2000-02-02");
        
        // Act
        _filter.OnActionExecuting(_context);
        
        // Assert
        var isAdult = _context.HttpContext.Items["Underage"] as bool? ?? false;
        Assert.That(isAdult, Is.False);
    }
    
    [Test]
    public void OnActionExecuting_ShouldSetUnderageValueToTrueInHttpContextIfUserIsNotAdultBasedOnProvidedXBirthdateHeader()
    {
        // Arrange
        var childBirthDate = DateOnly.FromDateTime(DateTime.UtcNow);
        _context.HttpContext.Request.Headers.Append("X-Birthdate", childBirthDate.ToString());
        
        // Act
        _filter.OnActionExecuting(_context);
        
        // Assert
        var underage = _context.HttpContext.Items["Underage"] as bool? ?? false;
        Assert.That(underage, Is.True);
    }
    
    
    [Test]
    public void OnActionExecuting_ShouldSetUnderageValueToTrueInHttpContextIfXBirthdateHeaderNotProvided()
    {
        // Arrange
        
        // Act
        _filter.OnActionExecuting(_context);
        
        // Assert
        var underage = _context.HttpContext.Items["Underage"] as bool? ?? false;
        Assert.That(underage, Is.True);
    }
    
    [Test]
    public void OnActionExecuting_ShouldSetUnderageValueToTrueInHttpContextIfXBirthdateHeaderValueIsNotValidDate()
    {
        // Arrange
        _context.HttpContext.Request.Headers.Append("X-Birthdate", "I am an adult!!!");

        // Act
        _filter.OnActionExecuting(_context);
        
        // Assert
        var isAdult = _context.HttpContext.Items["Underage"] as bool? ?? false;
        Assert.That(isAdult, Is.True);
    }
}