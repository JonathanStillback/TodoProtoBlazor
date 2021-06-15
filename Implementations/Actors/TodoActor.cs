using System;
using System.Threading.Tasks;
using Models;
using Proto;

namespace Implementations
{
	public class TodoActor : IActor
	{
		//the receive function, invoked by the runtime whenever a message
		//should be processed
		public Task ReceiveAsync(IContext context)
		{
			switch (context.Message)
			{
				case Todo todo:
					Console.WriteLine($"Proto handling todo: {todo.Name}, but changing name");
					todo.Name = "Proto " + todo.Name;
					context.Respond(todo);
				break;
				case DBEntityMessage m when m.Entity is Todo t && m.dbChange == DBChange.Create:
					Console.WriteLine("Handled by Proto todo create");
					context.Respond(t);
				break;
			};
			return Task.CompletedTask;
		}


		// public record TestRec(int cool, string yeah);
	}
}