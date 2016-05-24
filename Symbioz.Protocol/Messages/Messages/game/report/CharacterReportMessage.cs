


















// Generated on 06/04/2015 18:45:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterReportMessage : Message
{

public const ushort Id = 6079;
public override ushort MessageId
{
    get { return Id; }
}

public uint reportedId;
        public sbyte reason;
        

public CharacterReportMessage()
{
}

public CharacterReportMessage(uint reportedId, sbyte reason)
        {
            this.reportedId = reportedId;
            this.reason = reason;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(reportedId);
            writer.WriteSByte(reason);
            

}

public override void Deserialize(ICustomDataInput reader)
{

reportedId = reader.ReadVarUhInt();
            if (reportedId < 0)
                throw new Exception("Forbidden value on reportedId = " + reportedId + ", it doesn't respect the following condition : reportedId < 0");
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
            

}


}


}