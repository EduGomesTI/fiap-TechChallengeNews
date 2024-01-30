namespace SendMail.Options
{
    public sealed class MassTransitOptions
    {
        public string Queue { get; set; }

        public string Server { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}