using MediatR;

namespace News.Application.News.Commands
{
    public sealed record DeleteNewsCommand(int Id) : IRequest<DeleteNewsResponse>;
}