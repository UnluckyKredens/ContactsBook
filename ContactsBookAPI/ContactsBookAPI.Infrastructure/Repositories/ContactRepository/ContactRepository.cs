using ContactsBookAPI.Domain.Entities;
using ContactsBookAPI.Domain.Exceptions;
using ContactsBookAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace ContactsBookAPI.Infrastructure.Repositories.ContactRepository
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext _context;

        public ContactRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<int> CreateContactAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<bool> EmailExists(string email)
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
            await _context.Contacts.Where<Contact>(c => c.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }


        public async Task<Contact> GetContactByIdAsync(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id); 

            return contact!;
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            var Oldcontact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == contact.Id);

            if (Oldcontact is null)
            {
                throw new KeyNotFoundException($"Contact {contact.Id} not found");
            }

            Oldcontact.FirstName = contact.FirstName;
            Oldcontact.LastName = contact.LastName;
            Oldcontact.Email = contact.Email;
            Oldcontact.PhoneNumber = contact.PhoneNumber;

            await _context.SaveChangesAsync();
  
        }

        public async Task<List<Contact>> GetPagedContactList(int pageNumber, int size, string? searchedPhrase)
        {
            var query = _context.Contacts.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(searchedPhrase))
            {
                var search = searchedPhrase.ToLower().Trim();
                query = query.Where(c =>  c.Email.ToLower().Contains(search));
            }

            query = query.OrderBy(c => c.Id);

            var contacts = await query.Skip((pageNumber - 1) * size).Take(size).ToListAsync();

            return contacts;

        }
    }
}
