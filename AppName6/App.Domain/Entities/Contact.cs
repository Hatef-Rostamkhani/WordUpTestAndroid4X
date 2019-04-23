namespace Domain
{
    using System;

    public class Contact
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public string Tel { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }

        public ContactType Type { get; set; }
    }
}
