using ContactsBookAPI.Domain.Exceptions;
using ContactsBookAPI.Infrastructure.Repositories.ContactRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Application.Commands
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, int>
    {
        private readonly IContactRepository _contactRepository;

        public DeleteContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<int> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var user = await _contactRepository.GetContactByIdAsync(request.Id);

            if(user == null)
            {
                throw new UserOperationException("User not exist");
            }

            await _contactRepository.DeleteContactAsync(request.Id);
            return request.Id;

        }
    }
}
