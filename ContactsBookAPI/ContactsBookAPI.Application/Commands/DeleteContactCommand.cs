using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsBookAPI.Application.Commands
{
    public class DeleteContactCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
