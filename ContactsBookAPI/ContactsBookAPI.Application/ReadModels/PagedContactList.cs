using ContactsBookAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Application.ReadModels
{
    public class PagedContactList
    {
        public required List<Contact> Contacts { get; set; }
        public required int PageSize { get; set; }
        public required int PageNumber { get; set; }
        public required int TotalCount { get; set; }
    }
}
