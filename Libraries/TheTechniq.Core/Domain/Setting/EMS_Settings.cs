namespace TheTecniQ.Core.Domain.Settings
{
    public class EMS_Settings : BaseEntity
    {
        public string Key { get; set; }

        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}