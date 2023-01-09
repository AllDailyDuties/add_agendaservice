namespace AllDailyDuties_AgendaService.Models.Shared
{
    public class GenericObj<T>
    {
        public Guid id { get; set; }
        public T obj { get; set; }
        public GenericObj(Guid _id, T _obj)
        {
            id = _id;
            obj = _obj;
        }

    }
}
