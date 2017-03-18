using System.Collections.Generic;

namespace NetCore.Contracts
{
    public class ModelValidationResult
    {
        public string Name { get; set; }
        public List<string> Messages { get; set; }
        public string AllMessages
        {
            get
            {
                return string.Join(",", Messages.ToArray());
            }
        }

        public ModelValidationResult()
        {
            Messages = new List<string>();
        }
    }
}
