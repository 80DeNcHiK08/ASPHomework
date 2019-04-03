using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Homework.Models;
using System.Threading.Tasks;

namespace Homework.Controllers
{
    //[Route("api/todo")]
    public class TodoController : Controller
    {
        public TodoController(ITodoRepository todoItems)
        {
            TodoItems = todoItems;
        }
        public ITodoRepository TodoItems { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return TodoItems.GetAll();
        }

        [HttpPost]
        public IActionResult Create(string Name, bool IsComplete)
        {
            TodoItem item = new TodoItem { TodoName = Name, IsComplete = IsComplete };
            if (item == null)
            {
                return BadRequest();
            }
            TodoItems.Add(item);
            return new NoContentResult();
        }

        public IActionResult Update(string id, string Name)
        {
            TodoItem item = TodoItems.Find(id);
            if (item == null)
            {
                return BadRequest();
            }

            var todo = TodoItems.Find(item.TodoId);
            if (todo == null)
            {
                return NotFound();
            }

            TodoItems.Update(todo, Name);
            return new NoContentResult();
        }

        public IActionResult Delete(string id)
        {
            var todo = TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            TodoItems.Remove(id);
            return new NoContentResult();
        }
    }
}