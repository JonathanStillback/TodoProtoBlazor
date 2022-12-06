using System.Threading.Tasks;

namespace Models
{
	public interface IProtoClient
	{
		void SendMessage<TMessage>(TMessage message);
		Task<TMessage> Request<TMessage>(TMessage message);
		Task<TEntity> DBRequest<TEntity>(DBEntityMessage message);
		// Task<TEntity> DBRequest<TEntity>(DBCommand retrieval);
		Task<TEntity> DBRequest<TEntity>(DBRetrievalMessage message);
	}
	// public enum DBChange
	// {
	// 	Create,
	// 	Update,
	// 	Delete
	// }

	// public enum DBRetrieval
	// {
	// 	Get,
	// 	GetAll
	// }

	public enum DBCommand
	{
		Create,
		Update,
		Delete,
		Get,
		GetAll
	}

	public record DBEntityMessage(object Entity, DBCommand dbCommand);
	public record DBRetrievalMessage(DBCommand dbCommand);

}