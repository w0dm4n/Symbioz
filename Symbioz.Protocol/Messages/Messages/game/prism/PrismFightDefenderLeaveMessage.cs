


















// Generated on 06/04/2015 18:45:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismFightDefenderLeaveMessage : Message
{

public const ushort Id = 5892;
public override ushort MessageId
{
    get { return Id; }
}

public ushort subAreaId;
        public ushort fightId;
        public uint fighterToRemoveId;
        

public PrismFightDefenderLeaveMessage()
{
}

public PrismFightDefenderLeaveMessage(ushort subAreaId, ushort fightId, uint fighterToRemoveId)
        {
            this.subAreaId = subAreaId;
            this.fightId = fightId;
            this.fighterToRemoveId = fighterToRemoveId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(subAreaId);
            writer.WriteVarUhShort(fightId);
            writer.WriteVarUhInt(fighterToRemoveId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

subAreaId = reader.ReadVarUhShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            fightId = reader.ReadVarUhShort();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            fighterToRemoveId = reader.ReadVarUhInt();
            if (fighterToRemoveId < 0)
                throw new Exception("Forbidden value on fighterToRemoveId = " + fighterToRemoveId + ", it doesn't respect the following condition : fighterToRemoveId < 0");
            

}


}


}