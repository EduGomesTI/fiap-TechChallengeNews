namespace News.Application.Abstractions
{
    public abstract record BaseResponse
    {
        public List<string> Messages { get; set; }

        public BaseResponse()
        {
            Messages = new List<string>();
        }

        public void AddMessage(string message) => Messages.Add(message);

        public void AddMessagges(List<string> messages) => Messages.AddRange(messages);
    }
}