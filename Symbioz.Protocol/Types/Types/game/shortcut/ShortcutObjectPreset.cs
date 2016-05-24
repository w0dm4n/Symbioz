


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ShortcutObjectPreset : ShortcutObject
{

public const short Id = 370;
public override short TypeId
{
    get { return Id; }
}

public sbyte presetId;
        

public ShortcutObjectPreset()
{
}

public ShortcutObjectPreset(sbyte slot, sbyte presetId)
         : base(slot)
        {
            this.presetId = presetId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(presetId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            

}


}


}