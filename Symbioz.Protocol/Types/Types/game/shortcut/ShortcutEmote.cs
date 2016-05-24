


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ShortcutEmote : Shortcut
{

public const short Id = 389;
public override short TypeId
{
    get { return Id; }
}

public byte emoteId;
        

public ShortcutEmote()
{
}

public ShortcutEmote(sbyte slot, byte emoteId)
         : base(slot)
        {
            this.emoteId = emoteId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteByte(emoteId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            emoteId = reader.ReadByte();
            if ((emoteId < 0) || (emoteId > 255))
                throw new Exception("Forbidden value on emoteId = " + emoteId + ", it doesn't respect the following condition : (emoteId < 0) || (emoteId > 255)");
            

}


}


}