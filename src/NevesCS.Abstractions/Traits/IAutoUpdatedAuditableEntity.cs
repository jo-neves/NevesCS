namespace NevesCS.Abstractions.Traits
{
    public interface IAutoUpdatedAuditableEntity
    {
        public DateTimeOffset CreationDate { get; }

        public DateTimeOffset LastUpdateDate { get; set; }
    }
}
