namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Data
{
    public class TrackEvent
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime CriadoEm { get; set; }
        public string DataOriginSerial { get; set; }
        public string DataEditedSerial { get; set; }
        public int StatusEdited { get; set; }
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }


        public TrackEvent(Guid transactionId, string dataOriginSerial, string dataEditedSerial, int statusEdited, string assemblyName, string typeName)
        {
            Id = Guid.NewGuid();
            CriadoEm = DateTime.Now;
            TransactionId = transactionId;
            DataOriginSerial = dataOriginSerial;
            DataEditedSerial = dataEditedSerial;
            StatusEdited = statusEdited;
            AssemblyName = assemblyName;
            TypeName = typeName;
        }
    }
}
