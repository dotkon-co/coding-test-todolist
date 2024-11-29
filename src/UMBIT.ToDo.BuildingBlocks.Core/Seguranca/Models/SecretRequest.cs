using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models
{
    public class SecretRequest
    {
        [JsonPropertyName("kid")]
        public string Kid { get; set; }

    }
}
