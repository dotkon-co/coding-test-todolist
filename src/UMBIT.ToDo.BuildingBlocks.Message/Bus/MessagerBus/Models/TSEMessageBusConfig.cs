namespace UMBIT.ToDo.BuildingBlocks.Message.Bus.MessagerBus.Models
{
    public class UMBITMessageBusConfig
    {
        public int? Port { get; set; }
        public string? Host { get; set; }
        public string? Senha { get; set; }
        public string? Usuario { get; set; }
        public string Service { get; set; }
    }
}
