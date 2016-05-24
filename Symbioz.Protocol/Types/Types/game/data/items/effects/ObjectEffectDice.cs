


















// Generated on 06/04/2015 18:45:33
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

    public class ObjectEffectDice : ObjectEffect
    {

        public const short Id = 73;
        public override short TypeId
        {
            get { return Id; }
        }

        public ushort diceNum { get; set; }
        public ushort diceSide { get; set; }
        public ushort diceConst { get; set; }


        public ObjectEffectDice()
        {
        }

        public ObjectEffectDice(ushort actionId, ushort diceNum, ushort diceSide, ushort diceConst)
            : base(actionId)
        {
            this.diceNum = diceNum;
            this.diceSide = diceSide;
            this.diceConst = diceConst;
        }


        public override void Serialize(ICustomDataOutput writer)
        {

            base.Serialize(writer);
            writer.WriteVarUhShort(diceNum);
            writer.WriteVarUhShort(diceSide);
            writer.WriteVarUhShort(diceConst);


        }

        public override void Deserialize(ICustomDataInput reader)
        {

            base.Deserialize(reader);
            diceNum = reader.ReadVarUhShort();
            if (diceNum < 0)
                throw new Exception("Forbidden value on diceNum = " + diceNum + ", it doesn't respect the following condition : diceNum < 0");
            diceSide = reader.ReadVarUhShort();
            if (diceSide < 0)
                throw new Exception("Forbidden value on diceSide = " + diceSide + ", it doesn't respect the following condition : diceSide < 0");
            diceConst = reader.ReadVarUhShort();
            if (diceConst < 0)
                throw new Exception("Forbidden value on diceConst = " + diceConst + ", it doesn't respect the following condition : diceConst < 0");


        }


    }


}