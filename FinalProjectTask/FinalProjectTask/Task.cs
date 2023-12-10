namespace FinalProjectTask
{
	public class Task
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public DateTime DueDate { get; set; }
		public Boolean IsCompleted { get; set; }

		public DateTime CompletionDate { get; set; }
	}
}
