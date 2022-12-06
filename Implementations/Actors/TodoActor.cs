using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models;
using Proto;

namespace Implementations
{
	public class TodoActor : IActor
	{
		private readonly IDBProvider<Todo> _dbProvider;

		public TodoActor(IDBProvider<Todo> dbProvider)
		{
			_dbProvider = dbProvider;
		}
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
				case DBEntityMessage m when m.Entity is Todo t:
					Console.WriteLine($"Todo {m.dbCommand} handled by Proto");
					switch(m.dbCommand)
					{
						case DBCommand.Create:
							var id = _dbProvider.Create(t);
							t.Id = id;
							context.Respond(t);
						break;
						case DBCommand.Update:
							var todo = _dbProvider.Update(t);
							context.Respond(todo);
						break;
						case DBCommand.GetAll:
							var todos = _dbProvider.GetAll();
							context.Respond(todos);
						break;
					}
				break;
			};
			return Task.CompletedTask;
		}


		// public record TestRec(int cool, string yeah);
	}
}