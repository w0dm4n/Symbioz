


















// Generated on 06/04/2015 18:44:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ServersListMessage : Message
{

public const ushort Id = 30;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.GameServerInformations> servers;
        public ushort alreadyConnectedToServerId;
        public bool canCreateNewCharacter;
        

public ServersListMessage()
{
}

public ServersListMessage(IEnumerable<Types.GameServerInformations> servers, ushort alreadyConnectedToServerId, bool canCreateNewCharacter)
        {
            this.servers = servers;
            this.alreadyConnectedToServerId = alreadyConnectedToServerId;
            this.canCreateNewCharacter = canCreateNewCharacter;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)servers.Count());
            foreach (var entry in servers)
            {
                 entry.Serialize(writer);
            }
            writer.WriteVarUhShort(alreadyConnectedToServerId);
            writer.WriteBoolean(canCreateNewCharacter);
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            servers = new Types.GameServerInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (servers as Types.GameServerInformations[])[i] = new Types.GameServerInformations();
                 (servers as Types.GameServerInformations[])[i].Deserialize(reader);
            }
            alreadyConnectedToServerId = reader.ReadVarUhShort();
            if (alreadyConnectedToServerId < 0)
                throw new Exception("Forbidden value on alreadyConnectedToServerId = " + alreadyConnectedToServerId + ", it doesn't respect the following condition : alreadyConnectedToServerId < 0");
            canCreateNewCharacter = reader.ReadBoolean();
            

}


}


}