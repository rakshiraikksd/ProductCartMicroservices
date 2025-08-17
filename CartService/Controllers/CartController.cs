using Microsoft.AspNetCore.Mvc;
using CartService.Models;

namespace CartService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private static readonly Dictionary<string, Cart> _carts = new();

    [HttpGet("{userId}")]
    public ActionResult<Cart> GetCart(string userId)
    {
        if (!_carts.ContainsKey(userId))
        {
            _carts[userId] = new Cart { UserId = userId };
        }
        return Ok(_carts[userId]);
    }

    [HttpPost("{userId}/items")]
    public ActionResult<CartItem> AddItem(string userId, [FromBody] CartItem item)
    {
        if (!_carts.ContainsKey(userId))
        {
            _carts[userId] = new Cart { UserId = userId };
        }

        var cart = _carts[userId];
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
            existingItem.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            item.Id = cart.Items.Count > 0 ? cart.Items.Max(i => i.Id) + 1 : 1;
            item.UserId = userId;
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            cart.Items.Add(item);
        }

        return Ok(item);
    }

    [HttpPut("{userId}/items/{itemId}")]
    public IActionResult UpdateItem(string userId, int itemId, [FromBody] CartItem item)
    {
        if (!_carts.ContainsKey(userId))
        {
            return NotFound("Cart not found");
        }

        var cart = _carts[userId];
        var existingItem = cart.Items.FirstOrDefault(i => i.Id == itemId);

        if (existingItem == null)
        {
            return NotFound("Item not found");
        }

        existingItem.Quantity = item.Quantity;
        existingItem.UpdatedAt = DateTime.UtcNow;

        return NoContent();
    }

    [HttpDelete("{userId}/items/{itemId}")]
    public IActionResult RemoveItem(string userId, int itemId)
    {
        if (!_carts.ContainsKey(userId))
        {
            return NotFound("Cart not found");
        }

        var cart = _carts[userId];
        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
        {
            return NotFound("Item not found");
        }

        cart.Items.Remove(item);
        return NoContent();
    }

    [HttpDelete("{userId}")]
    public IActionResult ClearCart(string userId)
    {
        if (!_carts.ContainsKey(userId))
        {
            return NotFound("Cart not found");
        }

        _carts[userId].Items.Clear();
        return NoContent();
    }

    [HttpGet("{userId}/summary")]
    public ActionResult<object> GetCartSummary(string userId)
    {
        if (!_carts.ContainsKey(userId))
        {
            return Ok(new { TotalItems = 0, TotalAmount = 0.0m });
        }

        var cart = _carts[userId];
        return Ok(new { 
            TotalItems = cart.TotalItems, 
            TotalAmount = cart.TotalAmount,
            ItemCount = cart.Items.Count
        });
    }

    [HttpGet("health")]
    public ActionResult<string> Health()
    {
        return Ok("Cart Service is healthy!");
    }
}


