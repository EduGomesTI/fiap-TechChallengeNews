namespace News.Infrastructure.Options
{
    public sealed class AzureServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string QueueName { get; set; } = string.Empty;
    }
}