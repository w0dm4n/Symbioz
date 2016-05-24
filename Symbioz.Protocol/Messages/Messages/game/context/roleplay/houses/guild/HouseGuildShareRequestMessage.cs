


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class HouseGuildShareRequestMessage : Message
{

public const ushort Id = 5704;
public override ushort MessageId
{
    get { return Id; }
}

public bool enable;
        public uint rights;
        

public HouseGuildShareRequestMessage()
{
}

public HouseGuildShareRequestMessage(bool enable, uint rights)
        {
            this.enable = enable;
            this.rights = rights;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(enable);
            writer.WriteVarUhInt(rights);
            

}

public override void Deserialize(ICustomDataInput reader)
{

enable = reader.ReadBoolean();
            rights = reader.ReadVarUhInt();
            if (rights < 0)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0");
            

}


}


}