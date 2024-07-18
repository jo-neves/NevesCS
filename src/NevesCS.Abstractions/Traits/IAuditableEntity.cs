namespace NevesCS.Abstractions.Traits
{
    public interface IAuditableEntity
    {
        public DateTimeOffset CreationDate { get; }

        public DateTimeOffset LastUpdateDate { get; set; }
    }
}
