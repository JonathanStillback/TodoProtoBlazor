using System;
using System.Threading.Tasks;
using Models;
using Proto;

namespace Implementations
{
	public class DispatcherActor : IActor
	{
		private Behavior _behavior;
		private bool _isRequest;
		public DispatcherActor(bool isRequest = false)
		{
			_isRequest = isRequest;
			_behavior = new Behavior();
			// if (isRequest)
			// {
			// 	_behavior.Become(Responding);
			// }
		}
		
		public Task ReceiveAsync(IContext context)
		{
			switch(context.Message)
			{
				case Todo t:
					var props = Props.FromProducer(() => new TodoActor());
					var pid = context.Spawn(props);
					if (_isRequest)
					{
						context.ReenterAfter(context.RequestAsync<Todo>(pid, t), task => {
							context.Respond(task.Result);
							return Task.CompletedTask;
						});
					}
					else
						context.Forward(pid);
				break;
				case CreateEntity<Todo> t:
				break;
			}
			return Task.CompletedTask;
		}
		// public record TestRec(int cool, string yeah);
	}
}