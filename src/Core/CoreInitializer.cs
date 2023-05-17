using AutoMapper;
using Core.Mappings;
using Core.Services;

namespace Core;

public class CoreInitializer : IInitializer
{
    private readonly IMapper mapper;

    public CoreInitializer(IMapper mapper)
        => this.mapper = mapper;

    public int RunningOrder => 0;

    public void Run()
    {
        MappingApi.Initialize(this.mapper);
    }
}
