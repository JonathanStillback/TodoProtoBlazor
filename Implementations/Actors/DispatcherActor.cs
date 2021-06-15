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
			PID pid;
			switch(context.Message)
			{
				case Todo todoPlain:
					pid = context.Spawn(Props.FromProducer(() => new TodoActor()));
					if (_isRequest)
					{
						context.ReenterAfter(context.RequestAsync<Todo>(pid, todoPlain), task => {
							context.Respond(task.Result);
							return Task.CompletedTask;
						});
					}
					else
						context.Forward(pid);
				break;
				case DBEntityMessage m:
					if (m.Entity is Todo todo)
					{
						pid = context.Spawn(Props.FromProducer(() => new TodoActor()));
						context.ReenterAfter(context.RequestAsync<Todo>(pid, m), task => {
							context.Respond(task.Result);
							return Task.CompletedTask;
						});
					}
				break;
			}
			return Task.CompletedTask;
		}
		// public record TestRec(int cool, string yeah);
	}
}