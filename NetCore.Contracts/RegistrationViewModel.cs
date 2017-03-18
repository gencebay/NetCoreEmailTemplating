using System;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Contracts
{
    public class RegistrationViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string EMail { get; set; }
    }

    public class RegistrationEmailViewModel : EmailMessageContext
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}