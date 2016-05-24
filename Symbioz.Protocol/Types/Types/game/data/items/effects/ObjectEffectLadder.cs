


















// Generated on 06/04/2015 18:45:33
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ObjectEffectLadder : ObjectEffectCreature
{

public const short Id = 81;
public override short TypeId
{
    get { return Id; }
}

public uint monsterCount { get; set; }
        

public ObjectEffectLadder()
{
}

public ObjectEffectLadder(ushort actionId, ushort monsterFamilyId, uint monsterCount)
         : base(actionId, monsterFamilyId)
        {
            this.monsterCount = monsterCount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(monsterCount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            monsterCount = reader.ReadVarUhInt();
            if (monsterCount < 0)
                throw new Exception("Forbidden value on monsterCount = " + monsterCount + ", it doesn't respect the following condition : monsterCount < 0");
            

}


}


}