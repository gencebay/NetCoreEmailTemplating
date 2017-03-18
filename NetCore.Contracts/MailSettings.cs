namespace NetCore.Contracts
{
    public class MailSettings
    {
        public string Host { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string From { get; set; }
        public int Port { get; set; }

        public MailSettings()
        {

        }
    }
}
