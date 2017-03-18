using System.Collections.Generic;

namespace NetCore.Contracts
{
    public class WebResult
    {
        public string Content { get; set; }
        public ResultState ResultState { get; set; }
        public List<ModelValidationResult> Validations { get; set; }

        public string State
        {
            get
            {
                switch (ResultState)
                {
                    case ResultState.Success:
                        return "success";
                    case ResultState.Warning:
                        return "warning";
                    case ResultState.Error:
                        return "danger";
                    default:
                        return "default";
                }
            }
        }

        public WebResult()
        {
            Validations = new List<ModelValidationResult>();
        }
    }
}
