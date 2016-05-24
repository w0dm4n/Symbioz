


















// Generated on 06/04/2015 18:44:58
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class UpdateMountBoostMessage : Message
{

public const ushort Id = 6179;
public override ushort MessageId
{
    get { return Id; }
}

public int rideId;
        public IEnumerable<Types.UpdateMountBoost> boostToUpdateList;
        

public UpdateMountBoostMessage()
{
}

public UpdateMountBoostMessage(int rideId, IEnumerable<Types.UpdateMountBoost> boostToUpdateList)
        {
            this.rideId = rideId;
            this.boostToUpdateList = boostToUpdateList;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarInt(rideId);
            writer.WriteUShort((ushort)boostToUpdateList.Count());
            foreach (var entry in boostToUpdateList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

rideId = reader.ReadVarInt();
            var limit = reader.ReadUShort();
            boostToUpdateList = new Types.UpdateMountBoost[limit];
            for (int i = 0; i < limit; i++)
            {
                 (boostToUpdateList as Types.UpdateMountBoost[])[i] = Types.ProtocolTypeManager.GetInstance<Types.UpdateMountBoost>(reader.ReadShort());
                 (boostToUpdateList as Types.UpdateMountBoost[])[i].Deserialize(reader);
            }
            

}


}


}