


















// Generated on 06/04/2015 18:44:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class DebugInClientMessage : Message
{

public const ushort Id = 6028;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte level;
        public string message;
        

public DebugInClientMessage()
{
}

public DebugInClientMessage(sbyte level, string message)
        {
            this.level = level;
            this.message = message;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(level);
            writer.WriteUTF(message);
            

}

public override void Deserialize(ICustomDataInput reader)
{

level = reader.ReadSByte();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            message = reader.ReadUTF();
            

}


}


}