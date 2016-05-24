


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

    public class FightLoot
    {

        public const short Id = 41;
        public virtual short TypeId
        {
            get { return Id; }
        }

        public List<ushort> objects;
        public uint kamas;


        public FightLoot()
        {
        }

        public FightLoot(List<ushort> objects, uint kamas)
        {
            this.objects = objects;
            this.kamas = kamas;
        }


        public virtual void Serialize(ICustomDataOutput writer)
        {

            writer.WriteUShort((ushort)objects.Count());
            foreach (var entry in objects)
            {
                writer.WriteVarUhShort(entry);
            }
            writer.WriteVarUhInt(kamas);


        }

        public virtual void Deserialize(ICustomDataInput reader)
        {

            var limit = reader.ReadUShort();
            objects = new ushort[limit].ToList();
            for (int i = 0; i < limit; i++)
            {
                (objects.ToArray() as ushort[])[i] = reader.ReadVarUhShort();
            }
            kamas = reader.ReadVarUhInt();
            if (kamas < 0)
                throw new Exception("Forbidden value on kamas = " + kamas + ", it doesn't respect the following condition : kamas < 0");


        }


    }


}