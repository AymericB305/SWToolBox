namespace SWToolBox_api.Features.Guilds.DeleteGuild;

public record DeleteGuildRequest(Guid Id);

public static class DeleteGuildMapper
{
    public static DeleteGuildCommand ToCommand(this DeleteGuildRequest request)
    {
        return new DeleteGuildCommand(request.Id);
    }
}