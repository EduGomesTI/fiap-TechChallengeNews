using MediatR;

namespace News.Application.News.Queries
{
    public sealed record GetAllNewsQuery : IRequest<IEnumerable<GetAllNewsResponse>>
    {
    }
}