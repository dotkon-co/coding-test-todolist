using System;

namespace UMBIT.ToDo.BuildingBlocks.SignalR.Basicos.Atributos
{
    public class HubPathAttribute : Attribute
    {
        public string Path { get; set; }
        public HubPathAttribute(string path = "/hub")
        {
            Path = path;
        }
    }
}
