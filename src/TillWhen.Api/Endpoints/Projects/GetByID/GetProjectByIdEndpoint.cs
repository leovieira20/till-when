using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TillWhen.Database.SqlServer;

namespace TillWhen.Api.Endpoints.Projects.GetByID;

public class GetProjectByIdEndpoint : Endpoint<GetProjectByIdRequest, GetProjectByIdResponse, GetProjectByIdMapper>
{
    public const string Name = "GetProjectById";

    private readonly TillWhenContext _context;

    public GetProjectByIdEndpoint(TillWhenContext context)
    {
        _context = context;
    }
    
    public override void Configure()
    {
        Get("api/projects/{id}");
        Description(builder => builder.WithName(Name));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProjectByIdRequest req, CancellationToken ct)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
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