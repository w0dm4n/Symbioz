


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ShortcutSmiley : Shortcut
{

public const short Id = 388;
public override short TypeId
{
    get { return Id; }
}

public sbyte smileyId;
        

public ShortcutSmiley()
{
}

public ShortcutSmiley(sbyte slot, sbyte smileyId)
         : base(slot)
        {
            this.smileyId = smileyId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(smileyId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            smileyId = reader.ReadSByte();
            if (smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + smileyId + ", it doesn't respect the following condition : smileyId < 0");
            

}


}


}