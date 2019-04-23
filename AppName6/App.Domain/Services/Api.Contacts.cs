namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Zebble;

    partial class Api
    {
        #region Dummy API Response
        static Contact[] DummyApiContactsResponse;
        static Random Random = new Random();
        static string[] FakeFirstNames = "Nicola,Gabriel,Leroy,Shaw,Tad,Miriam,Melita,Quiana,Felicia,Lindsay,Rae,Signe".Split(',').ToArray();
        static string[] FakeLastNames = "Pickford,Gabel,Benes,Roosevelt,Colegrove,Heitman,Fritsche,Roberds,Flury,Vail,Kendra,Arens,Carbone".Split(',').ToArray();

        static IEnumerable<Contact> CreateDummyApiContactsResponse()
        {
            for (var i = 0; i < 100; i++)
            {
                var randomName = FakeFirstNames.PickRandom();
                yield return new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = randomName + " " + FakeLastNames.PickRandom(),
                    DateOfBirth = LocalTime.Today.AddDays(-Random.Next(10000, 20000)),
                    Email = randomName + Random.Next(100, 999) + "@" + new[] { "gmail", "yahoo", "hotmail" }.PickRandom() + ".com",
                    Tel = "0" + Random.Next(100000000, 900000000),
                    Type = new ContactType { Name = DummyContactTypes.PickRandom() }
                };
            }
        }
        #endregion

        public static Task<Contact[]> GetContacts()
        {
            // TODO: return Get<Contact[]>("api/v1/contacts");

            if (DummyApiContactsResponse == null)
                DummyApiContactsResponse = CreateDummyApiContactsResponse().ToArray();

            return Task.FromResult(DummyApiContactsResponse);
        }

        internal static Task<bool> Save(Contact item) => Post("api/v1/contacts/save", item);

        public static Task Delete(Contact contact) => Delete("api/v1/contacts/delete", new { Id = contact.Id });
    }
}

namespace Zebble.Data { }