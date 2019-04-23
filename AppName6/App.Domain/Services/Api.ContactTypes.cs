namespace Domain
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    partial class Api
    {
        static string[] DummyContactTypes = "Family,Friend,Business,Other".Split(',').ToArray();

        public static Task<ContactType[]> GetContactTypes()
        {
            // TODO: return Get<ContactType[]>("api/v1/contact-types");

            if (DummyApiContactsResponse == null)
                DummyApiContactsResponse = CreateDummyApiContactsResponse().ToArray();

            return Task.FromResult(DummyContactTypes.Select(x => new ContactType { Name = x }).ToArray());
        }
    }
}