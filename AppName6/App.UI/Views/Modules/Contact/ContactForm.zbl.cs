namespace UI.Modules
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Zebble;

    partial class ContactForm
    {
        public Contact Item;

        public ContactForm() => Item = Nav.Param<Contact>("Item") ?? new Contact();

        public override async Task OnInitializing()
        {
            await base.OnInitializing();

            Type.Control.DataSource = await Api.GetContactTypes();
            LoadData();
        }

        public async Task SaveButtonTapped()
        {
            PopulateModel();
            if (await Api.Save(Item)) await Nav.Back();
        }

        internal void LoadData()
        {
            Name.Value = Item.Name;
            Email.Value = Item.Email;
            Tel.Value = Item.Tel;
            Notes.Value = Item.Notes;
            DateOfBirth.Value = Item.DateOfBirth;
            Type.Value = Item.Type;
        }

        internal void PopulateModel()
        {
            Item.Name = Name.GetValue<string>();
            Item.Email = Email.GetValue<string>();
            Item.Tel = Tel.GetValue<string>();
            Item.Notes = Notes.GetValue<string>();
            Item.DateOfBirth = DateOfBirth.GetValue<DateTime?>();
            Item.Type = Type.GetValue<ContactType>();
        }
    }
}