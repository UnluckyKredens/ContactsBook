using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Application.Commands
{
    public class UpdateContactCommand : IRequest<int>
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Zip { get; set; }
    }
}
