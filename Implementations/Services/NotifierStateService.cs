using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using System.Linq;

namespace Implementations
{
	public class NotifierStateService<T> : INotifierStateService<T>
	{
		private readonly List<T> values = new List<T>();
		private readonly IDBProvider<T> _dbProvider;
		private readonly IProtoClient _protoClient;

		public List<T> ValuesList => values;

			public NotifierStateService(IDBProvider<T> dbProvider, IProtoClient protoClient)
			{
				Console.WriteLine("Initializing new NotifierStateService");
				_dbProvider = dbProvider;
				_protoClient = protoClient;

				_protoClient.DBRequest<T>(new DBRetrievalMessage(DBCommand.GetAll));
				values = _dbProvider.GetAll().ToList();
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