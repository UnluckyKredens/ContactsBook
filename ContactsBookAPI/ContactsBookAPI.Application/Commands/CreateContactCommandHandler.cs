using ContactsBookAPI.Domain.Entities;
using ContactsBookAPI.Domain.Exceptions;
using ContactsBookAPI.Infrastructure.Repositories.ContactRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;


namespace ContactsBookAPI.Application.Commands
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, int>
    {

        private readonly IContactRepository _contactRepository;


        public CreateContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<int> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            if (await _contactRepository.EmailExistsAsync(request.Email))
            {
                throw new UserOperationException("Email already exists");
            }

            var body = new Contact
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                Zip = request.Zip
            };

            return await _contactRepository.CreateContactAsync(body);
        }
    }
}
