using System.Threading.Tasks;

namespace Models
{
	public interface IProtoHelper
	{
		void SendMessage<TMessage>(TMessage message);
		Task<TMessage> Request<TMessage>(TMessage message);
		Task<TEntity> DBRequest<TEntity>(DBEntityMessage message);
	}
	public record DBEntityMessage(object Entity, DBChange dbChange);
	public enum DBChange
	{
		Create,
		Get,
		Update,
		Delete
	}

}