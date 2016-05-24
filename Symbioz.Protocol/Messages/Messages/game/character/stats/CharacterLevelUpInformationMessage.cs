


















// Generated on 06/04/2015 18:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterLevelUpInformationMessage : CharacterLevelUpMessage
{

public const ushort Id = 6076;
public override ushort MessageId
{
    get { return Id; }
}

public string name;
public uint id;
        

public CharacterLevelUpInformationMessage()
{
}

public CharacterLevelUpInformationMessage(byte newLevel, string name, uint id)
         : base(newLevel)
        {
            this.name = name;
            this.id = id;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(name);
            writer.WriteVarUhInt(id);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            name = reader.ReadUTF();
            id = reader.ReadVarUhInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            

}


}


}