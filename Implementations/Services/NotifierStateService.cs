using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Implementations
{
	public class NotifierStateService<T> : INotifierStateService<T>
	{
			private readonly List<T> values = new List<T>();
			public IReadOnlyList<T> ValuesList => values;

			public NotifierStateService()
			{
				Console.WriteLine("Initializing new NotifierStateService");
			}

			public async Task AddTolist(T value)
			{
					values.Add(value);
					if (Notify != null)
					{
							await Notify?.Invoke();
					}

			}

			private event Func<Task> Notify;

			public void Subscribe(Func<Task> eventHandler)
			{
				var name = eventHandler.Target.ToString();
				if (_eventHandlers.Contains(name))
					return;
				_eventHandlers.Add(name);
				Notify += eventHandler;
			}

			private HashSet<string> _eventHandlers = new HashSet<string>();
	}
}