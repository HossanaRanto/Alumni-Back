namespace Alumni_Back.Helpers
{
    public static class PaginationHelper
    {
        public static IEnumerable<T> Pagination<T>(this IEnumerable<T> source,int offset,int limit)
        {
            IEnumerable<T> result=Enumerable.Empty<T>();
            if (source.Count() < limit)
            {
                limit=source.Count();
            }
            for(int i=offset; i<limit && limit>0; i++)
            {
                result.Append(source.ElementAt(i));
            }
            return result;
        }
    }
}
