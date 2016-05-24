


















// Generated on 06/04/2015 18:45:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismSettingsRequestMessage : Message
{

public const ushort Id = 6437;
public override ushort MessageId
{
    get { return Id; }
}

public ushort subAreaId;
        public sbyte startDefenseTime;
        

public PrismSettingsRequestMessage()
{
}

public PrismSettingsRequestMessage(ushort subAreaId, sbyte startDefenseTime)
        {
            this.subAreaId = subAreaId;
            this.startDefenseTime = startDefenseTime;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(subAreaId);
            writer.WriteSByte(startDefenseTime);
            

}

public override void Deserialize(ICustomDataInput reader)
{

subAreaId = reader.ReadVarUhShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            startDefenseTime = reader.ReadSByte();
            if (startDefenseTime < 0)
                throw new Exception("Forbidden value on startDefenseTime = " + startDefenseTime + ", it doesn't respect the following condition : startDefenseTime < 0");
            

}


}


}