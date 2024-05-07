using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        public readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Todo>> GetTodos()
        {
            var todos = await _context.Todos.AsNoTracking().ToListAsync();
            return todos;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound("ToDo item not found");
            }
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.AddAsync(todo);
            await _context.SaveChangesAsync();

            return Ok("ToDo item created");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Todo todo)
        {
            var existingTodo = await _context.Todos.FindAsync(id);
            if (existingTodo == null)
            {
                return NotFound("ToDo item not found");
            }

            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsCompleted = todo.IsCompleted;

            await _context.SaveChangesAsync();

            return Ok("ToDo item updated");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound("ToDo item not found");
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok("ToDo item deleted");
        }
    }
}