using ContactsBookAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Infrastructure.Repositories.ContactRepository
{
    public interface IContactRepository
    {
        Task<int> CreateContactAsync(Contact contact);
        Task<bool> EmailExists(string email);
        Task<Contact> GetContactByIdAsync(int id);
        Task<List<Contact>> GetPagedContactList(int size, int pageNumber, string? searchedPhraze);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
    }
}
