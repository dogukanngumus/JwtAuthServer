using AutoMapper;

namespace Service.Mappings;

public class ObjectMapper
{
    private static Lazy<IMapper> _lazyMapper = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(configure =>
        {
            configure.AddProfile<MappingProfiles>();
        });
        return config.CreateMapper();
    });
    public static IMapper Mapper => _lazyMapper.Value;
}