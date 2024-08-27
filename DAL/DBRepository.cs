using ContactBook.Models;
using ContactBook.Models.Base;
using ContactBook.ViewModels;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Dynamic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
namespace ContactBook.DAL
{
    internal class DBRepository
    {
        private SqliteDataAccess _sqlite;

        public DBRepository()
        {
            _sqlite = new SqliteDataAccess();
        }


        [Obsolete]
        public async void AddAdamWest(SelectedContact contact)
        {
            var lightContact = new Contact()
            {
                FirstName = contact.BaseContact.FirstName,
                LastName = contact.BaseContact.LastName,
            };
            var g = await _sqlite.SaveDataAsync<Contact>(lightContact);
        }

        public async Task<int> GetContactsCount()
        {
            var count = await _sqlite.GetCountOf<int>("SELECT COUNT(id) FROM contact");
            return count;
        }

        public async Task<SelectedContact> SaveContactAsync(SelectedContact createdContact)
        {
            var contact = await _sqlite.SaveDataAsync(createdContact.BaseContact);

            var relationship = await _sqlite.SaveDataAsync(createdContact.Relation);

            foreach (var email in createdContact.Emails) 
            { 
                await _sqlite.SaveDataAsync(email.UnderlyingEntity);
            }

            foreach (var address in createdContact.Addresses)
            {
                await _sqlite.SaveDataAsync(address.UnderlyingEntity);
            }

            foreach (var number in createdContact.Numbers)
            {
                await _sqlite.SaveDataAsync(number.UnderlyingEntity);
            }

            return createdContact;
        }

        /// <summary>
        /// Retrieves all elements in tSource where the id equals the specified one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tSource"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<T> RetrieveWhereId<T>(List<T> tSource, int id) where T : BaseEntityForeign
        {
            if (tSource.Where(x => x.ContactId == id).Count() > 0)
                return tSource.Where(x => x.ContactId == id).ToList();
            else
                return new List<T>();
        }

        /// <summary>
        /// Retrieves the first element in tSource where the id equals the specified one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tSource"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private T RetrieveFirstWhereId<T>(List<T> tSource, int id) 
            where T : BaseEntityForeign, new()
        {
            var list = RetrieveWhereId(tSource, id);

            if (list.Count > 0) 
                return list[0];
            else 
                return new T();
        }

        /// <summary>
        /// Asynchronously retrieves a list of contacts.
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectedContact>> GetContactsAsync()
        {
            var fullContacts = new List<SelectedContact>();

            var contacts = await _sqlite.SelectDataAsync<Contact>("SELECT * FROM contact");

            var numbers = await _sqlite.SelectDataAsync<Number>("SELECT * FROM number");

            var emails = await _sqlite.SelectDataAsync<Email>("SELECT * FROM email");

            var addresses = await _sqlite.SelectDataAsync<Address>("SELECT * FROM address");

            var relation = await _sqlite.SelectDataAsync<Relation>("SELECT * FROM relation");

            foreach (var contact in contacts)
            {
                var fullContact = new SelectedContact(
                    contact,
                    RetrieveFirstWhereId(relation, contact.Id),
                    RetrieveWhereId(emails, contact.Id),
                    RetrieveWhereId(addresses, contact.Id),
                    RetrieveWhereId(numbers, contact.Id)
                    );
                fullContacts.Add(fullContact);
            }

            return fullContacts;
        }
        /// <summary>
        /// Asynchronously retrieves a contact whose id equals the specified one.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SelectedContact> GetContactAsync(int id)
        {
            var contacts = await _sqlite.SelectDataAsync<Contact>($"SELECT * FROM contact where id = {id}");
            var contact = contacts.First();

            var numbers = await _sqlite.SelectDataAsync<Number>($"SELECT * FROM number where contact_id = {id}");

            var emails = await _sqlite.SelectDataAsync<Email>($"SELECT * FROM email where contact_id = {id}");

            var addresses = await _sqlite.SelectDataAsync<Address>($"SELECT * FROM address where contact_id = {id}");

            var relation = await _sqlite.SelectDataAsync<Relation>($"SELECT * FROM relation where contact_id = {id}");

            var fullContact = new SelectedContact(
                contact, 
                RetrieveFirstWhereId(relation, contact.Id),
                RetrieveWhereId(emails, contact.Id),
                RetrieveWhereId(addresses, contact.Id),
                RetrieveWhereId(numbers, contact.Id)
                );
            

            return fullContact;
        }








        //private (string, string) ToCommandValues(Dictionary<string, object> properties)
        //{
        //    foreach (var property in properties)
        //    {

        //    }

        //    return (string.Empty, string.Empty);
        //}





        //public async Task<IEnumerable<FullContactViewModel>> SelectAllContactViewModels<T>() where T : FullContactViewModel
        //{
        //    var command = new NpgsqlCommand($@"SELECT * FROM {typeof(T).Name} WHERE id=@id");
        //    command.Parameters.AddWithValue("id", id);
        //    command.
        //    var result = await InteractWithDBAsync<IEnumerable<T>>(() => ReadEntitiesAsync<T>(command));

        //    return result;
        //}



        //public async Task<bool> InsertEntityAsync<T>(T entity) where T : BaseEntity
        //{   
        //    bool result = await InteractWithDBAsync<bool>(() => InsertEntity<T>(entity));

        //    return result;
        //}

        //public async Task<T> SelectEntityAsync<T>(int id)
        //{

        //    var command = new NpgsqlCommand($@"SELECT * FROM {typeof(T).Name} WHERE id=@id");
        //    command.Parameters.AddWithValue("id", id);

        //    var result = await InteractWithDBAsync<IEnumerable<T>>(() => ReadEntitiesAsync<T>(command));

        //    return result.First();
        //}




    }
}
