using ContactsBookAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Infrastructure.Repositories.ContactRepository
{
    public interface IContactRepository
    {
        Task<int> CreateContactAsync(Contact contact);
        Task<bool> EmailExistsAsync(string email);
        Task<Contact?> GetContactByIdAsync(int id);
        Task<List<Contact>> GetPagedContactListAsync(int size, int pageNumber, string? searchedPhraze);
        Task<int> GetTotalContactsCountAsync(string searchedPhraze);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
    }
}
