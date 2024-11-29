using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UMBIT.ToDo.BuildingBlocks.Basicos.Enumeradores
{
    public abstract class EnumeradorBase<T> where T : EnumeradorBase<T>
    {
        public int Identificador { get; private set; }
        public string Descricao { get; private set; }

        protected EnumeradorBase() { }

        public EnumeradorBase(int identificador, string descricao)
        {
            Identificador = identificador;
            Descricao = descricao;
        }

        public EnumeradorBase(string descricao)
        {
            Identificador = descricao.GetHashCode();
            Descricao = descricao;
        }

        public static IEnumerable<T> ObtenhaCatalogo()
        {
            var campos = typeof(T).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly);

            return campos.Where(m => m.FieldType == typeof(T) && m.DeclaringType.BaseType == typeof(T).BaseType).Select(f => f.GetValue(null)).Cast<T>();
        }

        public static T Obtenha(int identificador)
        {
            return ObtenhaCatalogo().SingleOrDefault(enumeradorBase => enumeradorBase.Equals(identificador));
        }

        public static explicit operator int(EnumeradorBase<T> obj)
        {
            return obj.Identificador;
        }

        public static explicit operator string(EnumeradorBase<T> obj)
        {
            return obj.Descricao;
        }

        public static bool operator ==(EnumeradorBase<T> obj1, EnumeradorBase<T> obj2)
        {
            return obj1?.Identificador == obj2?.Identificador;
        }

        public static bool operator !=(EnumeradorBase<T> obj1, EnumeradorBase<T> obj2)
        {
            return obj1?.Identificador != obj2?.Identificador;
        }

        public static bool operator ==(int identificador, EnumeradorBase<T> obj)
        {
            return identificador != obj?.Identificador;
        }

        public static bool operator !=(int identificador, EnumeradorBase<T> obj)
        {
            return identificador != obj?.Identificador;
        }

        public static bool operator ==(EnumeradorBase<T> obj, int identificador)
        {
            return identificador == obj;
        }

        public static bool operator !=(EnumeradorBase<T> obj, int identifiador)
        {
            return identifiador != obj;
        }

        public override bool Equals(object obj)
        {
            if (obj is int identificador)
                return Identificador == identificador;
            else if (obj is string descricao)
                return Descricao == descricao;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Identificador.GetHashCode();
        }

        public override string ToString()
        {
            return Descricao;
        }
    }
}
