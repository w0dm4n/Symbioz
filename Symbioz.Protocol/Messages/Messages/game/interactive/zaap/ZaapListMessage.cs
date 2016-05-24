


















// Generated on 06/04/2015 18:44:49
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ZaapListMessage : TeleportDestinationsListMessage
{

public const ushort Id = 1604;
public override ushort MessageId
{
    get { return Id; }
}

public int spawnMapId;
        

public ZaapListMessage()
{
}

public ZaapListMessage(sbyte teleporterType, IEnumerable<int> mapIds, IEnumerable<ushort> subAreaIds, IEnumerable<ushort> costs, IEnumerable<sbyte> destTeleporterType, int spawnMapId)
         : base(teleporterType, mapIds, subAreaIds, costs, destTeleporterType)
        {
            this.spawnMapId = spawnMapId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(spawnMapId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            spawnMapId = reader.ReadInt();
            if (spawnMapId < 0)
                throw new Exception("Forbidden value on spawnMapId = " + spawnMapId + ", it doesn't respect the following condition : spawnMapId < 0");
            

}


}


}