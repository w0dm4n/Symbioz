


















// Generated on 06/04/2015 18:45:30
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class NamedPartyTeam
{

public const short Id = 469;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte teamId;
        public string partyName;
        

public NamedPartyTeam()
{
}

public NamedPartyTeam(sbyte teamId, string partyName)
        {
            this.teamId = teamId;
            this.partyName = partyName;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(teamId);
            writer.WriteUTF(partyName);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

teamId = reader.ReadSByte();
            if (teamId < 0)
                throw new Exception("Forbidden value on teamId = " + teamId + ", it doesn't respect the following condition : teamId < 0");
            partyName = reader.ReadUTF();
            

}


}


}