using MediatR;

namespace News.Application.News.Queries
{
    public sealed record GetNewsByIdQuery(int Id) : IRequest<GetNewsByIdResponse>;
}