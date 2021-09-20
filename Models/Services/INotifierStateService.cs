using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
	public interface INotifierStateService<T>
	{
			public List<T> ValuesList {get;}

			Task AddTolist(T value);

			// event Func<Task> Notify;
			public void Subscribe(Func<Task> eventHandler);
	}
}