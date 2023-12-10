using Microsoft.AspNetCore.Mvc;

namespace FinalProjectTask.Controllers
{
	[ApiController]
	[Route("api/tasks")]
	public class TasksController : ControllerBase
	{
		private static List<Task> tasks = new List<Task>();

		[HttpGet]
		public IActionResult GetAllTasks()
		{
			return Ok(tasks);
		}

		[HttpGet("completed")]
		public IActionResult GetAllCompletedTasks()
		{
			List<Task> completedTasks = new List<Task>();

			foreach (var task in tasks)
			{
				if (task.IsCompleted)
				{
					completedTasks.Add(task);
				}
			}

			return Ok(completedTasks);
		}

		[HttpGet("{id}")]
		public IActionResult GetTaskById(int id)
		{
			foreach(var task in tasks)
			{
				if(task.Id == id)
				{
					return Ok(task);
				}
			}
			return Ok(); 

		}

		[HttpPost]
		public IActionResult AddTask([FromBody] Task newTask)
		{
			newTask.Id = tasks.Count + 1;
			tasks.Add(newTask);
			return Ok(newTask);
		}

		[HttpDelete("{id}")]
		public IActionResult RemoveTask(int id)
		{
			var result = false; 
			foreach (var task in tasks)
			{
				if (task.Id == id)
				{
					result = tasks.Remove(task); 
				}
			}

			if (result == false)
				return NotFound();

			return Ok();
		}

		[HttpPut("{id}")]
		public IActionResult EditTask(int id, [FromBody] Task updatedTask)
		{
			Task taskToUpdate = null;

			foreach (var task in tasks)
			{
				if (task.Id == id)
				{
					taskToUpdate = task;
					break;
				}
			}

			if (taskToUpdate == null)
				return NotFound();

			taskToUpdate.Description = updatedTask.Description;
			taskToUpdate.DueDate = updatedTask.DueDate;

			return Ok(taskToUpdate);
		}

		[HttpPost("{id}/complete")]
		public IActionResult MarkTaskAsCompleted(int id)
		{
			Task taskToComplete = null;

			foreach (var task in tasks)
			{
				if (task.Id == id)
				{
					taskToComplete = task;
					break;
				}
			}

			if (taskToComplete == null)
				return NotFound();

			taskToComplete.IsCompleted = true;
			taskToComplete.CompletionDate = DateTime.Now;

			return Ok(taskToComplete);
		}


		[HttpGet("sorted")]
		public IActionResult GetSortedTasks(string sortBy)
		{
			IEnumerable<Task> sortedTasks = tasks;

			
			if (!string.IsNullOrEmpty(sortBy))
			{
				switch (sortBy.ToLower())
				{
					case "description":
						sortedTasks = tasks.OrderBy(t => t.Description);
						break;
					case "duedate":
						sortedTasks = tasks.OrderBy(t => t.DueDate);
						break;
					case "id":
						sortedTasks = tasks.OrderBy(t => t.Id);
						break; 

				}
			}

			return Ok(sortedTasks);
		}

		[HttpGet("search")]
		public IActionResult SearchTasks(string searchTerm)
		{
			if (string.IsNullOrEmpty(searchTerm))
				return BadRequest("Search term is required.");

			var searchResults = new List<Task>();

			foreach (var task in tasks)
			{
				if (task.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
				{
					searchResults.Add(task);
				}
			}

			return Ok(searchResults);
		}

		[HttpGet("filtered")]
		public IActionResult GetFilteredTasks(bool? isCompleted, DateTime? dueDate, DateTime? completionDate)
		{
			var filteredTasks = new List<Task>();

		
			foreach (var task in tasks)
			{
				if ((!isCompleted.HasValue || task.IsCompleted == isCompleted.Value) &&
					(!dueDate.HasValue || task.DueDate.Date == dueDate.Value.Date) &&
					(!completionDate.HasValue || (task.CompletionDate.Date == completionDate.Value.Date)))
				{
					filteredTasks.Add(task);
				}
			}

			return Ok(filteredTasks);
		}
	}
}

