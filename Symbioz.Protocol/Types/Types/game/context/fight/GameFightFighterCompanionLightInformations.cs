


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameFightFighterCompanionLightInformations : GameFightFighterLightInformations
{

public const short Id = 454;
public override short TypeId
{
    get { return Id; }
}

public sbyte companionId;
        public int masterId;
        

public GameFightFighterCompanionLightInformations()
{
}

public GameFightFighterCompanionLightInformations(bool sex, bool alive, int id, sbyte wave, ushort level, sbyte breed, sbyte companionId, int masterId)
         : base(sex, alive, id, wave, level, breed)
        {
            this.companionId = companionId;
            this.masterId = masterId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(companionId);
            writer.WriteInt(masterId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            companionId = reader.ReadSByte();
            if (companionId < 0)
                throw new Exception("Forbidden value on companionId = " + companionId + ", it doesn't respect the following condition : companionId < 0");
            masterId = reader.ReadInt();
            

}


}


}