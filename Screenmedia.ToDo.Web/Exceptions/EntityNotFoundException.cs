using System;

namespace Screenmedia.ToDo.Web.Exceptions
{
    public class EntityNotFoundException<TEntity> : Exception where TEntity : class
    {
        public EntityNotFoundException(object id) : base($"{typeof(TEntity).Name} {id} does not exist", innerException: null)
        {
        }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
