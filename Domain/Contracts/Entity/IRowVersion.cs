namespace Domain.Contracts.Entity
{
    public interface IRowVersion
    {
        public byte[] RowVersion { get; set; }
    }
}
