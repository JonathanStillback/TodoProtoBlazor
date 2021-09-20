using System;
using System.Threading.Tasks;
using Models;
using Proto;
using Proto.DependencyInjection;
using Proto.Extensions;

namespace Implementations
{
	public class DispatcherRequestActor : IActor
	{
		private readonly ActorSystem _actorSystem;
		private Behavior _behavior;
		public DispatcherRequestActor(ActorSystem actorSystem)
		{
			_actorSystem = actorSystem;
			_behavior = new Behavior();
			// if (isRequest)
			// {
			// 	_behavior.Become(Responding);
			// }
		}
		
		public Task ReceiveAsync(IContext context)
		{
			PID pid;
			switch(context.Message)
			{
				case Todo todoPlain:
					pid = context.Spawn(_actorSystem.DI().PropsFor<TodoActor>());
					context.ReenterAfter(context.RequestAsync<Todo>(pid, todoPlain), task => {
						context.Respond(task.Result);
						return Task.CompletedTask;
					});
				break;
				case DBEntityMessage m:
					if (m.Entity is Todo todo)
					{
						pid = context.Spawn(_actorSystem.DI().PropsFor<TodoActor>());
						if (m.dbChange == DBChange.GetAll)
						{
							context.ReenterAfter(context.RequestAsync<Todo>(pid, m), task => {
								context.Respond(task.Result);
								return Task.CompletedTask;
							});
						}
						else
						{
							context.ReenterAfter(context.RequestAsync<Todo>(pid, m), task => {
								context.Respond(task.Result);
								return Task.CompletedTask;
							});
						}
					}
				break;
			}
			return Task.CompletedTask;
		}
		// public record TestRec(int cool, string yeah);
	}
}