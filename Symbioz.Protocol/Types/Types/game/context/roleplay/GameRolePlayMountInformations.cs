


















// Generated on 06/04/2015 18:45:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameRolePlayMountInformations : GameRolePlayNamedActorInformations
{

public const short Id = 180;
public override short TypeId
{
    get { return Id; }
}

public string ownerName;
        public byte level;
        

public GameRolePlayMountInformations()
{
}

public GameRolePlayMountInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, string ownerName, byte level)
         : base(contextualId, look, disposition, name)
        {
            this.ownerName = ownerName;
            this.level = level;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(ownerName);
            writer.WriteByte(level);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            ownerName = reader.ReadUTF();
            level = reader.ReadByte();
            if ((level < 0) || (level > 255))
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : (level < 0) || (level > 255)");
            

}


}


}