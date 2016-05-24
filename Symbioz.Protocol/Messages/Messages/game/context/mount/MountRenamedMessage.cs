


















// Generated on 06/04/2015 18:44:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MountRenamedMessage : Message
{

public const ushort Id = 5983;
public override ushort MessageId
{
    get { return Id; }
}

public int mountId;
        public string name;
        

public MountRenamedMessage()
{
}

public MountRenamedMessage(int mountId, string name)
        {
            this.mountId = mountId;
            this.name = name;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarInt(mountId);
            writer.WriteUTF(name);
            

}

public override void Deserialize(ICustomDataInput reader)
{

mountId = reader.ReadVarInt();
            name = reader.ReadUTF();
            

}


}


}