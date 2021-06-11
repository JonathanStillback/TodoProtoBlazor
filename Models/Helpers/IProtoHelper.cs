using System.Threading.Tasks;

namespace Models
{
	public interface IProtoHelper
	{
		void SendMessage<TMessage>(TMessage message);
		Task<TMessage> Request<TMessage>(TMessage message);
		Task<TEntity> CreateRequest<TEntity>(CreateEntity<TEntity> message);
		Task<TEntity> GetRequest<TEntity>(GetEntity<TEntity> message);
		Task<TEntity> UpdateRequest<TMessage, TEntity>(TMessage message);
		Task<TEntity> DeleteRequest<TMessage, TEntity>(TMessage message);
	}
	public record CreateEntity<TEntity>(TEntity Entity);
	public record GetEntity<TEntity>(TEntity Entity);
	public record UpdateEntity<TEntity>(TEntity Entity);
	public record DeleteEntity<TEntity>(TEntity Entity);

}