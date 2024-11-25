using FastEndpoints;

namespace SWToolBox_api.Features.Monsters;

public sealed class MonstersGroup : Group
{
    public MonstersGroup()
    {
        Configure("monsters", ep =>
        {
            ep.AllowAnonymous();
        });
    }    
}