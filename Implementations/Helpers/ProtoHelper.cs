using System;
using System.Threading.Tasks;
using Models;
using Proto;
using Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Proto.DependencyInjection;
using Proto.Extensions;
using Proto.Router;

namespace Implementations
{
	public class ProtoHelper : IProtoHelper
	{
		private readonly ActorSystem _actorSystem;
		private readonly string _actorSendName;
		private readonly string _actorRequestName;
		private PID _pidSend;
		private PID _pidRequest;
		public ProtoHelper(ActorSystem actorSystem)
		{
			_actorSystem = actorSystem;
			_actorSendName = "ProtoSendDispatcher";
			_actorRequestName = "ProtoRequestDispatcher";

			var props = Props.FromProducer(() => new DispatcherActor());
			_pidSend = _actorSystem.Root.SpawnNamed(props, _actorSendName);
			props = Props.FromProducer(() => new DispatcherActor(true));
			_pidRequest = _actorSystem.Root.SpawnNamed(props, _actorRequestName);
		}

		public void SendMessage<TMessage>(TMessage message)
		{
			_actorSystem.Root.Send(_pidSend, message);
		}
		public async Task<TMessage> Request<TMessage>(TMessage message)
		{
			var item = await _actorSystem.Root.RequestAsync<TMessage>(_pidRequest, message);
			return item;
		}

		public Task<TEntity> CreateRequest<TEntity>(CreateEntity<TEntity> message)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> GetRequest<TEntity>(GetEntity<TEntity> message)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> UpdateRequest<TMessage, TEntity>(TMessage message)
		{
			throw new NotImplementedException();
		}

		public Task<TEntity> DeleteRequest<TMessage, TEntity>(TMessage message)
		{
			throw new NotImplementedException();
		}
	}
}