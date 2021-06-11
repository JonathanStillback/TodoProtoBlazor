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
			//the message we received
			var msg = context.Message;
			//match message based on type
			if (msg is Todo todo)
			{
				Console.WriteLine($"Proto handling todo: {todo.Name}, but changing name");
				todo.Name = "Proto " + todo.Name;
				context.Respond(todo);
			}
			return Task.CompletedTask;
		}

		public static void HandleTodoByActor(Todo todo)
		{

			var system = new Proto.ActorSystem();
			var context = system.Root;
			var props = Proto.Props.FromProducer(() => new TodoActor());
			var pid = context.Spawn(props);
			context.Send(pid, todo);
		}

		// public record TestRec(int cool, string yeah);
	}
}