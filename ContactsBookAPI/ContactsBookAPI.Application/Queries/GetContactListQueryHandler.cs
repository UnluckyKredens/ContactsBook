using ContactsBookAPI.Application.ReadModels;
using ContactsBookAPI.Domain.Exceptions;
using ContactsBookAPI.Infrastructure.Repositories.ContactRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Application.Queries
{
    public class GetContactListQueryHandler : IRequestHandler<GetContactListQuery, PagedContactList>
    {

        private readonly IContactRepository _contactRepository;

        public GetContactListQueryHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<PagedContactList> Handle(GetContactListQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber ?? 1;

            var pageSize = request.PageSize ?? 15;

            var search = request.Search ?? "";

            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new UserOperationException("Page number must be greater than 0.");
            }

            var list = await _contactRepository.GetPagedContactListAsync(pageNumber, pageSize, search);

            var totalCount = await _contactRepository.GetTotalContactsCountAsync(search);

            return new PagedContactList
            {
                Contacts = list,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
