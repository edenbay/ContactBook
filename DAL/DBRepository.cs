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
        private SqlDataAccess _sql;
       
        public DBRepository()
        {
           _sql = new SqlDataAccess();
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
                    ImageUrl = contact.ImageUrl,
                    Numbers = numbers.Where(x => x.ContactId == contact.Id).ToList(),
                    Emails = emails.Where(x => x.ContactId == contact.Id).ToList(),
                    Addresses = addresses.Where(x => x.ContactId == contact.Id).ToList(),
                    Relation = relation.Where(x => x.ContactId == contact.Id).First(),
                };
                fullContacts.Add(fullContact);
            }

            return fullContacts;
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
                ImageUrl = contact.ImageUrl,
                Numbers = numbers.Where(x => x.ContactId == contact.Id).ToList(),
                Emails = emails.Where(x => x.ContactId == contact.Id).ToList(),
                Addresses = addresses.Where(x => x.ContactId == contact.Id).ToList(),
                Relation = relation.Where(x => x.ContactId == contact.Id).First(),
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
