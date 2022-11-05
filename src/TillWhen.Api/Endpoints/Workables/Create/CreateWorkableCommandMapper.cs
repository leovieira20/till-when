using FastEndpoints;
using TillWhen.Application.Workables;

namespace TillWhen.Api.Endpoints.Workables.Create;

public class CreateProjectCommandMapper : Mapper<CreateWorkableRequest, CreateWorkableResponse, CreateWorkable.Command>
{
    public override CreateWorkable.Command ToEntity(CreateWorkableRequest r)
    {
        return new(r.Title, r.Duration);
    }
}