namespace BookStoreServer.WebApi.Dtos;

public sealed record SaveCommentDto(
    int OrderId,
    string Comment,
    short Raiting);

