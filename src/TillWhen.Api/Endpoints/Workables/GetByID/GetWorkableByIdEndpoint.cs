using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TillWhen.Database.SqlServer;

namespace TillWhen.Api.Endpoints.Workables.GetByID;

public class GetWorkableByIdEndpoint : Endpoint<GetWorkableByIdRequest, GetWorkableByIdResponse, GetWorkableByIdMapper>
{
    public const string Name = "GetProjectById";

    private readonly TillWhenContext _context;

    public GetWorkableByIdEndpoint(TillWhenContext context)
    {
        _context = context;
    }
    
    public override void Configure()
    {
        Get("api/workable/{id}");
        Description(builder => builder.WithName(Name));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetWorkableByIdRequest req, CancellationToken ct)
    {
        var project = await _context.Workables.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (project == null)
        {
            await SendNotFoundAsync(ct);
        }
        else
        {
            await SendAsync(new(Map.FromEntity(project)), cancellation: ct);            
        }
    }
}