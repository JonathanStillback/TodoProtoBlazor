using System;
using System.Threading.Tasks;
using Models;
using Proto;
using Microsoft.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Proto.DependencyInjection;
using Proto.Extensions;
using Proto.Router;
using Microsoft.Extensions.Configuration;

namespace Implementations
{
	public class ProtoClient : IProtoClient
	{
		private readonly ActorSystem _actorSystem;
		private readonly string _actorSendName;
		private readonly string _actorRequestName;
		private PID _pidSend;
		private PID _pidRequest;
		public ProtoClient(ActorSystem actorSystem)
		{
			_actorSystem = actorSystem;
			_actorSendName = "ProtoSendDispatcher";
			_actorRequestName = "ProtoRequestDispatcher";

			var props = _actorSystem.DI().PropsFor<DispatcherSendActor>();
			_pidSend = _actorSystem.Root.SpawnNamed(props, _actorSendName);
			props = _actorSystem.DI().PropsFor<DispatcherRequestActor>();
			_pidRequest = _actorSystem.Root.SpawnNamed(props, _actorRequestName);
		}

		public void SendMessage<TMessage>(TMessage message)
		{
			_actorSystem.Root.Send(_pidSend, message);
		}
		public async Task<TEntity> Request<TEntity>(TEntity entity)
		{
			var item = await _actorSystem.Root.RequestAsync<TEntity>(_pidRequest, entity);
			return item;
		}

		public async Task<TEntity> DBRequest<TEntity>(DBEntityMessage message)
		{
			var item = await _actorSystem.Root.RequestAsync<TEntity>(_pidRequest, message);
			return item;
		}
	}
}