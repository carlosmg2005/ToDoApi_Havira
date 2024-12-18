using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Para autorizar o acesso
using System.Collections.Generic;
using System.Linq;
using System;

[Authorize] // Exige autenticação JWT
[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private static List<ToDo> todos = new List<ToDo>();
    private static int currentId = 1;

    [HttpPost]
    public IActionResult Create([FromBody] ToDo todo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Obter o UserId do token JWT
        var userIdClaim = User.FindFirst("sub");
        if (userIdClaim == null)
        {
            return Unauthorized("Usuário não autenticado.");
        }

        todo.UserId = int.Parse(userIdClaim.Value); // Associar ao usuário autenticado
        todo.Id = currentId++;
        todo.CreatedAt = DateTime.UtcNow;
        todos.Add(todo);

        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Obter o UserId do token JWT
        var userIdClaim = User.FindFirst("sub");
        if (userIdClaim == null)
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var userId = int.Parse(userIdClaim.Value);
        var userTodos = todos.Where(t => t.UserId == userId).ToList();

        return Ok(userTodos);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        // Obter o UserId do token JWT
        var userIdClaim = User.FindFirst("sub");
        if (userIdClaim == null)
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var userId = int.Parse(userIdClaim.Value);
        var todo = todos.FirstOrDefault(t => t.Id == id && t.UserId == userId);

        if (todo == null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ToDo updatedTodo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Obter o UserId do token JWT
        var userIdClaim = User.FindFirst("sub");
        if (userIdClaim == null)
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var userId = int.Parse(userIdClaim.Value);
        var todo = todos.FirstOrDefault(t => t.Id == id && t.UserId == userId);

        if (todo == null)
        {
            return NotFound();
        }

        todo.Title = updatedTodo.Title;
        todo.Description = updatedTodo.Description;
        todo.CompletedAt = updatedTodo.CompletedAt;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Obter o UserId do token JWT
        var userIdClaim = User.FindFirst("sub");
        if (userIdClaim == null)
        {
            return Unauthorized("Usuário não autenticado.");
        }

        var userId = int.Parse(userIdClaim.Value);
        var todo = todos.FirstOrDefault(t => t.Id == id && t.UserId == userId);

        if (todo == null)
        {
            return NotFound();
        }

        todos.Remove(todo);
        return NoContent();
    }
}