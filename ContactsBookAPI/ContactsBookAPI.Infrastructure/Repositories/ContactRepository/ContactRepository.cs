using ContactsBookAPI.Domain.Entities;
using ContactsBookAPI.Domain.Exceptions;
using ContactsBookAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace ContactsBookAPI.Infrastructure.Repositories.ContactRepository
{
    public class ContactRepository(DataContext _context) : IContactRepository
    {

        public async Task<int> CreateContactAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);

            await _context.SaveChangesAsync();

            return contact.Id;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var contact = await _context.Contacts.AnyAsync(c => c.Email == email);

            if (contact)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public async Task DeleteContactAsync(int id)
        {
            _context.Contacts.Where(c => c.Id == id).ExecuteDelete();

            await _context.SaveChangesAsync();
        }
        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

            return contact;
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            var oldContact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contact.Id);

            if (oldContact is null)
            {
                throw new KeyNotFoundException($"Contact {contact.Id} not found");
            }

            oldContact.FirstName = contact.FirstName;
            oldContact.LastName = contact.LastName;
            oldContact.Email = contact.Email;
            oldContact.PhoneNumber = contact.PhoneNumber;
            oldContact.Address = contact.Address;
            oldContact.City = contact.City;
            oldContact.Zip = contact.Zip;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Contact>> GetPagedContactListAsync(int pageNumber, int size, string? searchedPhrase)
        {
            var query = _context.Contacts.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchedPhrase))
            {
                var search = searchedPhrase.ToLower().Trim();

                query = query.Where(c => c.Email.ToLower().Contains(search));
            }

            query = query.OrderBy(c => c.Id);

            var contacts = await query.Skip((pageNumber - 1) * size).Take(size).ToListAsync();

            return contacts;

        }

        public async Task<int> GetTotalContactsCountAsync(string searchedPhraze)
        {
            if (searchedPhraze.Trim().Length == 0)
            {
                return await _context.Contacts.CountAsync();
            }

            else
            {
                var search = searchedPhraze.ToLower().Trim();

                return await _context.Contacts.Where(c => c.Email.ToLower().Contains(search)).CountAsync();
            }
        }
    }
}
