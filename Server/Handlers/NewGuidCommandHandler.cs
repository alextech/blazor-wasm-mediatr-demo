using System;
using System.Threading;
using System.Threading.Tasks;
using Ata.DeloSled.Shared;
using MediatR;

namespace Ata.DeloSled.Server.Handlers
{
    public class NewGuidCommandHandler : IRequestHandler<NewGuidCommand, CommandResponse<Guid>>
    {
        public async Task<CommandResponse<Guid>> Handle(NewGuidCommand request, CancellationToken cancellationToken)
        {
            return CommandResponse<Guid>.Ok(Guid.NewGuid(), "New guid generated");
        }
    }
}