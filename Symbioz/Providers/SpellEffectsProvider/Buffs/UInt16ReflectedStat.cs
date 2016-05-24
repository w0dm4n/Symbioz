using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.SpellEffectsProvider.Buffs
{
    public class UInt16ReflectedStat
    {
        public UInt16ReflectedStat(FieldInfo field,StatsRecord host)
        {
            this.Field = field;
            this.Host = host;
        }
        public string FieldName { get { return Field.Name; } }
        public FieldInfo Field { get; set; }
        public StatsRecord Host { get; set; }
        public void AddValue(short value)
        {
            var added = (short)((short)(Field.GetValue(Host)) + value);
            Field.SetValue(Host, added);
        }
        public void SetValue(short value)
        {
            Field.SetValue(Host, value);
        }
        public short GetValue()
        {
            return (short)Field.GetValue(Host);
        }
    }
}
