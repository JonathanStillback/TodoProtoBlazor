using System;
using System.Threading.Tasks;
using Models;
using Proto;
using Proto.DependencyInjection;

namespace Implementations
{
	public class DispatcherSendActor : IActor
	{
		private readonly ActorSystem _actorSystem;
		private Behavior _behavior;
		public DispatcherSendActor(ActorSystem actorSystem)
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
					// _actorSystem.
					pid = context.Spawn(_actorSystem.DI().PropsFor<TodoActor>());
					context.Forward(pid);
				break;
			}
			return Task.CompletedTask;
		}
		// public record TestRec(int cool, string yeah);
	}
}