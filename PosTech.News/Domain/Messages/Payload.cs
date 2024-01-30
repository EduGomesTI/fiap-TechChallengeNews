using News.Domain.Enuns;

namespace News.Domain.Messages
{
    public sealed record Payload<T>
    {
        public Payload(CrudTypes type, T data)
        {
            Id = Guid.NewGuid();
            Type = type;
            Data = data;
        }

        public Guid Id { get; init; }
        public CrudTypes Type { get; init; }
        public T Data { get; init; }
    }
}