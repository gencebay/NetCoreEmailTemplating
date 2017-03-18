namespace NetCore.Contracts
{
    public class EmailMessageContext
    {
        public bool IsHtml { get; set; }

        public string Subject { get; set; }

        public string To { get; set; }
    }
}
