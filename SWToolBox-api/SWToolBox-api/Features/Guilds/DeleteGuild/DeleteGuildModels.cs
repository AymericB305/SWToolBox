namespace SWToolBox_api.Features.Guilds.DeleteGuild;

public record DeleteGuildRequest(Guid Id);
public record DeleteGuildResponse(bool IsSuccess, string? ErrorMessage);

public static class DeleteGuildMapper
{
    public static DeleteGuildCommand ToCommand(this DeleteGuildRequest request)
    {
        return new DeleteGuildCommand(request.Id);
    }
}