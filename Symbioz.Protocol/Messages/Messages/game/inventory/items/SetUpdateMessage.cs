


















// Generated on 06/04/2015 18:45:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SetUpdateMessage : Message
{

public const ushort Id = 5503;
public override ushort MessageId
{
    get { return Id; }
}

public ushort setId;
        public IEnumerable<ushort> setObjects;
        public IEnumerable<Types.ObjectEffect> setEffects;
        

public SetUpdateMessage()
{
}

public SetUpdateMessage(ushort setId, IEnumerable<ushort> setObjects, IEnumerable<Types.ObjectEffect> setEffects)
        {
            this.setId = setId;
            this.setObjects = setObjects;
            this.setEffects = setEffects;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(setId);
            writer.WriteUShort((ushort)setObjects.Count());
            foreach (var entry in setObjects)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)setEffects.Count());
            foreach (var entry in setEffects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

setId = reader.ReadVarUhShort();
            if (setId < 0)
                throw new Exception("Forbidden value on setId = " + setId + ", it doesn't respect the following condition : setId < 0");
            var limit = reader.ReadUShort();
            setObjects = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (setObjects as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            setEffects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 (setEffects as Types.ObjectEffect[])[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 (setEffects as Types.ObjectEffect[])[i].Deserialize(reader);
            }
            

}


}


}