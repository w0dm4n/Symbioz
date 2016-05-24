


















// Generated on 06/04/2015 18:44:40
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SpellForgottenMessage : Message
{

public const ushort Id = 5834;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<ushort> spellsId;
        public ushort boostPoint;
        

public SpellForgottenMessage()
{
}

public SpellForgottenMessage(IEnumerable<ushort> spellsId, ushort boostPoint)
        {
            this.spellsId = spellsId;
            this.boostPoint = boostPoint;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)spellsId.Count());
            foreach (var entry in spellsId)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteVarUhShort(boostPoint);
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            spellsId = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (spellsId as ushort[])[i] = reader.ReadVarUhShort();
            }
            boostPoint = reader.ReadVarUhShort();
            if (boostPoint < 0)
                throw new Exception("Forbidden value on boostPoint = " + boostPoint + ", it doesn't respect the following condition : boostPoint < 0");
            

}


}


}