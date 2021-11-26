using Microsoft.AspNetCore.Mvc;
using DemoWebServiceEntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebServiceEntityFramework.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly ILogger<TodoItemsController> _logger;

    private readonly TodoContext _context;

    public TodoItemsController(ILogger<TodoItemsController> logger, TodoContext context)
    {
        _logger = logger;
        _context = context; //quero acessar o meu banco de dados
    }

    [HttpGet] //Get api/TodoItems
    public async Task<IEnumerable<TodoItemDTO>> GetTodoItems() //retorna colecao de todoitemDTO
    {
        return await _context.TodoItems.AsNoTracking().Select(t => new TodoItemDTO(t)).ToListAsync(); //AsNoTracking para que o DbContext não fique observando algo que não está alterando, visto que eu trabalho com o DTO nesse caso
    }                                                                                                 //DbContext observa o TodoItem, e não o TodoItemDTO

    [HttpGet("{id:long}")] //Get api/TodoItems/1
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id) //ActionResult pois 
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if(todoItem == null) return NotFound();
        return new TodoItemDTO(todoItem);
    }


    [HttpGet("notcomplete")] //Get api/TodoItems/notcomplete
    public async Task<IEnumerable<TodoItemDTO>> GetTodoItemsNotComplete()
    {
        return await _context.TodoItems.Where(t => !t.IsComplete).Select(t => new TodoItemDTO(t)).ToListAsync();
    }

    [HttpPost] //POST api/TodoItems
    public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoDTO)
    {
        var todoItem = new TodoItem
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name
        };
        _context.TodoItems.Add(todoItem); //uso o todoItem normal pois é ele que é usado lá no DbContext
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTodoItem), new {id = todoItem.Id}, new TodoItemDTO(todoItem));
    }

    [HttpDelete("{id:long}")]  //DELETE api/TodoItems/1
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if(todoItem == null) return NotFound();

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:long}")] //PUT api/TodoItems/1
    public async Task<ActionResult> UpdateTodoItem(long id, TodoItemDTO todoDTO)
    {
        _logger.LogInformation($"UpdateTodoItem:{todoDTO}");
        var todoItem = await _context.TodoItems.FindAsync(id); //todoItem recebe o todo encontrado
        if(todoItem == null) return NotFound();

        if(id != todoDTO.Id) return BadRequest();

        todoItem.Name = todoDTO.Name;    //faz as manipulacoes atraves do todoItem (pois ele que se comunica com o DbContext)
        todoItem.IsComplete = todoDTO.IsComplete;
        await _context.SaveChangesAsync();
        return NoContent();
    }


}
