


















// Generated on 06/04/2015 18:45:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class UpdateMapPlayersAgressableStatusMessage : Message
{

public const ushort Id = 6454;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<uint> playerIds;
        public IEnumerable<sbyte> enable;
        

public UpdateMapPlayersAgressableStatusMessage()
{
}

public UpdateMapPlayersAgressableStatusMessage(IEnumerable<uint> playerIds, IEnumerable<sbyte> enable)
        {
            this.playerIds = playerIds;
            this.enable = enable;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)playerIds.Count());
            foreach (var entry in playerIds)
            {
                 writer.WriteVarUhInt(entry);
            }
            writer.WriteUShort((ushort)enable.Count());
            foreach (var entry in enable)
            {
                 writer.WriteSByte(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            playerIds = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (playerIds as uint[])[i] = reader.ReadVarUhInt();
            }
            limit = reader.ReadUShort();
            enable = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (enable as sbyte[])[i] = reader.ReadSByte();
            }
            

}


}


}