namespace FitnessApi.Mappers.IMappers
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        TSource MapBack(TDestination destination);
    }
}
