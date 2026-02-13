using ContactsBookAPI.Domain.Entities;
using ContactsBookAPI.Domain.Exceptions;
using ContactsBookAPI.Infrastructure.Repositories.ContactRepository;
using MediatR;


namespace ContactsBookAPI.Application.Commands
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, int>
    {
        private readonly IContactRepository _contactRepository;
        public UpdateContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<int> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {

            var existingContact = await _contactRepository.GetContactByIdAsync(request.Id);

            if (existingContact is null)
            {
                throw new UserOperationException($"Contact with id {request.Id} not found.");
            }

            if (existingContact?.Email != request.Email)
            {
                if (await _contactRepository.EmailExistsAsync(request.Email))
                {
                    throw new UserOperationException($"A contact with email {request.Email} already exists.");
                }
            }

            var contact = new Contact
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                Zip = request.Zip
            };
            await _contactRepository.UpdateContactAsync(contact);

            return request.Id;
        }
    }
}
