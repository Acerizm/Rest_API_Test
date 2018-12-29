using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/ToDo")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoContext _context;

        //public constructor
        public ToDoController(ToDoContext context)
        {
            _context = context;

            if(_context.TodoItems.Count() == 0)
            {
                //Create a new ToDo item if collection is empty
                //which means you cant delete all ToDoItems

                _context.TodoItems.Add(new TodoItem { Name = "Item 1" });
                _context.SaveChanges();
            }
        }

        //GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Todo/5
        //{id} will be mapped to the "id" parameter
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }
    }
}
