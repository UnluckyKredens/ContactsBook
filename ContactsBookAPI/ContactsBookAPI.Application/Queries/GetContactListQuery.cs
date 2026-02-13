using ContactsBookAPI.Application.ReadModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Application.Queries
{
    public class GetContactListQuery : IRequest<PagedContactList>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Search { get; set; }
    }
}
