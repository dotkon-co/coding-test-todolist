using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Interfaces
{
    public interface IPermissao
    {
        public int Identificador { get; internal set; }

        public string Assembly { get; }
        public string Nome { get; internal set; }
        public string IdentificadorCompleto { get; }
        public string Descricao { get; internal set; }


    }
}
