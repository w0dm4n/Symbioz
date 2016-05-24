


















// Generated on 06/04/2015 18:45:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayMerchantInformations : GameRolePlayNamedActorInformations
{

public const short Id = 129;
public override short TypeId
{
    get { return Id; }
}

public sbyte sellType;
        public IEnumerable<Types.HumanOption> options;
        

public GameRolePlayMerchantInformations()
{
}

public GameRolePlayMerchantInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, sbyte sellType, IEnumerable<Types.HumanOption> options)
         : base(contextualId, look, disposition, name)
        {
            this.sellType = sellType;
            this.options = options;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(sellType);
            writer.WriteUShort((ushort)options.Count());
            foreach (var entry in options)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            sellType = reader.ReadSByte();
            if (sellType < 0)
                throw new Exception("Forbidden value on sellType = " + sellType + ", it doesn't respect the following condition : sellType < 0");
            var limit = reader.ReadUShort();
            options = new Types.HumanOption[limit];
            for (int i = 0; i < limit; i++)
            {
                 (options as Types.HumanOption[])[i] = Types.ProtocolTypeManager.GetInstance<Types.HumanOption>(reader.ReadShort());
                 (options as Types.HumanOption[])[i].Deserialize(reader);
            }
            

}


}


}