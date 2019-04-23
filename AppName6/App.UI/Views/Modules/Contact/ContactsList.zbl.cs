namespace UI.Modules
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Zebble;

    partial class ContactsList
    {
        public Contact[] Items;

        CachedValue<bool> _ShowPhotoColumn;

        public override async Task OnInitializing()
        {
            Items = await Api.GetContacts();

            await base.OnInitializing();
        }

        public async Task ReloadButtonTapped() => await Reload();

        public async Task Reload() => await List.UpdateSource(Items = await Api.GetContacts());

        partial class Row
        {
            public ContactsList Module => FindParent<ContactsList>();

            public async Task ViewButtonTapped() => await Nav.Forward<Pages.Page1Enter>(new { Item = Item });

            public async Task DeleteButtonTapped()
            {
                if (!(await Alert.Confirm("Are you sure you want to delete this contact?"))) return;

                await Api.Delete(Item);

                await Module.Reload();
            }
        }
    }
}