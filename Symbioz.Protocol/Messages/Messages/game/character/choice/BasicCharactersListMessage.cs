


















// Generated on 06/04/2015 18:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class BasicCharactersListMessage : Message
{

public const ushort Id = 6475;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.CharacterBaseInformations> characters;
        

public BasicCharactersListMessage()
{
}

public BasicCharactersListMessage(IEnumerable<Types.CharacterBaseInformations> characters)
        {
            this.characters = characters;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)characters.Count());
            foreach (var entry in characters)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            characters = new Types.CharacterBaseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (characters as Types.CharacterBaseInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterBaseInformations>(reader.ReadShort());
                 (characters as Types.CharacterBaseInformations[])[i].Deserialize(reader);
            }
            

}


}


}