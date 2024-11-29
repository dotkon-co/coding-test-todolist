using System.Reflection;

namespace UMBIT.ToDo.BuildingBlocks.Basicos.Enumeradores
{
    public abstract class EnumBase<T> where T : Enum
    {
        public int Identificador { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public EnumBase(T @enum, string descricao)
        {
            Descricao = descricao;
            Nome = @enum.ToString();
            Identificador = Convert.ToInt32(@enum);
        }

        public static IEnumerable<T1> ObtenhaCatalogo<T1>() where T1 : EnumBase<T>
        {
            var campos = typeof(T1).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            var res = campos.Where(m => m.FieldType.IsAssignableTo(typeof(T1))).Select(f => f.GetValue(null)).Cast<T1>();

            return res;
        }

        public static T1 Obtenha<T1>(int identificador) where T1 : EnumBase<T>
        {
            return ObtenhaCatalogo<T1>().SingleOrDefault(enumeradorBase => enumeradorBase.Equals(identificador));
        }

        public static explicit operator int(EnumBase<T> obj)
        {
            return obj.Identificador;
        }

        public static explicit operator string(EnumBase<T> obj)
        {
            return obj.Descricao;
        }

        public static bool operator ==(EnumBase<T> obj1, EnumBase<T> obj2)
        {
            return obj1?.Identificador == obj2?.Identificador;
        }

        public static bool operator !=(EnumBase<T> obj1, EnumBase<T> obj2)
        {
            return obj1?.Identificador != obj2?.Identificador;
        }

        public static bool operator ==(int identificador, EnumBase<T> obj)
        {
            return identificador != obj?.Identificador;
        }

        public static bool operator !=(int identificador, EnumBase<T> obj)
        {
            return identificador != obj?.Identificador;
        }

        public static bool operator ==(EnumBase<T> obj, int identificador)
        {
            return identificador == obj;
        }

        public static bool operator !=(EnumBase<T> obj, int identifiador)
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