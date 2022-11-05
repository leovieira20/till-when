using TillWhen.Api.Endpoints.Workables.Common;

namespace TillWhen.Api.Endpoints.Workables.GetByID;

public class GetWorkableByIdResponse
{
    public GetWorkableByIdResponse(WorkableDto workable)
    {
        Workable = workable;
    }

    public WorkableDto Workable { get; set; }
}