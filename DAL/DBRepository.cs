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
using System.Windows.Media.Imaging;
namespace ContactBook.DAL
{
    internal class DBRepository
    {
        private SqliteDataAccess _sql;
        private SqliteDataAccess _sqlite;

        public DBRepository()
        {
           _sql = new SqliteDataAccess();
            _sqlite = new SqliteDataAccess();
        }

        public List<SelectedContact> GetSqliteContactAsync()
        {
            var contactList = _sqlite.LoadContacts();
            var fullContacts = new List<SelectedContact>();
            foreach (var contact in contactList)
            {
                var fullContact = new SelectedContact()
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    ImageUrl = contact.Image,
                };
                fullContacts.Add(fullContact);
            }
            return fullContacts;
        }

        public void AddAdamWest()
        {
            _sqlite.SaveContact(new Contact() { FirstName = "wdam", LastName = "west"});
        }

        public async Task<List<SelectedContact>> GetContactsAsync()
        {
            var fullContacts = new List<SelectedContact>();

            var contacts = await _sql.SelectDataAsync<Contact, Contact>("SELECT * FROM contact", new Contact { });

            var numbers = await _sql.SelectDataAsync<Number, Number>("SELECT * FROM number", new Number { });

            var emails = await _sql.SelectDataAsync<Email, Email>("SELECT * FROM email", new Email { });

            var addresses = await _sql.SelectDataAsync<Address, Address>("SELECT * FROM address", new Address { });

            var relation = await _sql.SelectDataAsync<Relation, Relation>("SELECT * FROM relation", new Relation { });

            foreach (var contact in contacts)
            {
                var fullContact = new SelectedContact()
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    ImageUrl = contact.Image,
                    Numbers = RetrieveWhereId(numbers, contact.Id),
                    Emails = RetrieveWhereId(emails, contact.Id),
                    Addresses = RetrieveWhereId(addresses, contact.Id),
                    Relation = RetrieveFirstWhereId(relation, contact.Id),
                };
                fullContacts.Add(fullContact);
            }

            return fullContacts;
        }

        private List<T> RetrieveWhereId<T>(List<T> tSource, int id) where T : BaseEntityForeign
        {
            if (tSource.Where(x => x.ContactId == id).Count() > 0)
                return tSource.Where(x => x.ContactId == id).ToList();
            else
                return new List<T>();
        }

        private T RetrieveFirstWhereId<T>(List<T> tSource, int id) 
            where T : BaseEntityForeign, new()
        {
            var list = RetrieveWhereId(tSource, id);

            if (list.Count > 0) 
                return list[0];
            else 
                return new T();
        }


        public async Task<SelectedContact> GetContactAsync(int id)
        {
            var contacts = await _sql.SelectDataAsync<Contact, Contact>($"SELECT * FROM contact where id={id}", new Contact { });
            var contact = contacts.First();

            var numbers = await _sql.SelectDataAsync<Number, Number>($"SELECT * FROM number where contact_id={id}", new Number { });

            var emails = await _sql.SelectDataAsync<Email, Email>($"SELECT * FROM email contact_id={id}", new Email { });

            var addresses = await _sql.SelectDataAsync<Address, Address>($"SELECT * FROM address contact_id={id}", new Address { });

            var relation = await _sql.SelectDataAsync<Relation, Relation>($"SELECT * FROM relation contact_id={id}", new Relation { });

            var fullContact = new SelectedContact()
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                ImageUrl = contact.Image,
                Numbers = RetrieveWhereId(numbers, contact.Id),
                Emails = RetrieveWhereId(emails, contact.Id),
                Addresses = RetrieveWhereId(addresses, contact.Id),
                Relation = RetrieveFirstWhereId(relation, contact.Id),
            };

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
