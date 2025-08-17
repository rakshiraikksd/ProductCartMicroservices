using Microsoft.AspNetCore.Mvc;
using CatalogService.Models;

namespace CatalogService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private static readonly List<Category> _categories = new()
    {
        new Category
        {
            Id = 1,
            Name = "Electronics",
            Description = "Electronic devices and gadgets",
            ImageUrl = "https://example.com/electronics.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Category
        {
            Id = 2,
            Name = "Clothing",
            Description = "Fashion and apparel",
            ImageUrl = "https://example.com/clothing.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new Category
        {
            Id = 3,
            Name = "Books",
            Description = "Books and literature",
            ImageUrl = "https://example.com/books.jpg",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        return Ok(_categories.Where(c => c.IsActive));
    }

    [HttpGet("{id}")]
    public ActionResult<Category> GetCategory(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id && c.IsActive);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    public ActionResult<Category> CreateCategory(Category category)
    {
        category.Id = _categories.Max(c => c.Id) + 1;
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;
        category.IsActive = true;
        _categories.Add(category);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id, Category category)
    {
        var existingCategory = _categories.FirstOrDefault(c => c.Id == id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        existingCategory.ImageUrl = category.ImageUrl;
        existingCategory.IsActive = category.IsActive;
        existingCategory.UpdatedAt = DateTime.UtcNow;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        category.IsActive = false;
        category.UpdatedAt = DateTime.UtcNow;
        return NoContent();
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Category>> SearchCategories([FromQuery] string query)
    {
        var results = _categories.Where(c => 
            c.IsActive && 
            (c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
             c.Description.Contains(query, StringComparison.OrdinalIgnoreCase)));
        return Ok(results);
    }

    [HttpGet("health")]
    public ActionResult<string> Health()
    {
        return Ok("Catalog Service is healthy!");
    }
}


