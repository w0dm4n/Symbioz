


















// Generated on 06/04/2015 18:45:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ActorAlignmentInformations
{

public const short Id = 201;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte alignmentSide;
        public sbyte alignmentValue;
        public sbyte alignmentGrade;
        public uint characterPower;
        

public ActorAlignmentInformations()
{
}

public ActorAlignmentInformations(sbyte alignmentSide, sbyte alignmentValue, sbyte alignmentGrade, uint characterPower)
        {
            this.alignmentSide = alignmentSide;
            this.alignmentValue = alignmentValue;
            this.alignmentGrade = alignmentGrade;
            this.characterPower = characterPower;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(alignmentSide);
            writer.WriteSByte(alignmentValue);
            writer.WriteSByte(alignmentGrade);
            writer.WriteVarUhInt(characterPower);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

alignmentSide = reader.ReadSByte();
            alignmentValue = reader.ReadSByte();
            if (alignmentValue < 0)
                throw new Exception("Forbidden value on alignmentValue = " + alignmentValue + ", it doesn't respect the following condition : alignmentValue < 0");
            alignmentGrade = reader.ReadSByte();
            if (alignmentGrade < 0)
                throw new Exception("Forbidden value on alignmentGrade = " + alignmentGrade + ", it doesn't respect the following condition : alignmentGrade < 0");
            characterPower = reader.ReadVarUhInt();
            if (characterPower < 0)
                throw new Exception("Forbidden value on characterPower = " + characterPower + ", it doesn't respect the following condition : characterPower < 0");
            

}


}


}