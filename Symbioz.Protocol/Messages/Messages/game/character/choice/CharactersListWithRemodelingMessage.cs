


















// Generated on 06/04/2015 18:44:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharactersListWithRemodelingMessage : CharactersListMessage
{

public const ushort Id = 6550;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.CharacterToRemodelInformations> charactersToRemodel;
        

public CharactersListWithRemodelingMessage()
{
}

public CharactersListWithRemodelingMessage(IEnumerable<Types.CharacterBaseInformations> characters, bool hasStartupActions, IEnumerable<Types.CharacterToRemodelInformations> charactersToRemodel)
         : base(characters, hasStartupActions)
        {
            this.charactersToRemodel = charactersToRemodel;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)charactersToRemodel.Count());
            foreach (var entry in charactersToRemodel)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            charactersToRemodel = new Types.CharacterToRemodelInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (charactersToRemodel as Types.CharacterToRemodelInformations[])[i] = new Types.CharacterToRemodelInformations();
                 (charactersToRemodel as Types.CharacterToRemodelInformations[])[i].Deserialize(reader);
            }
            

}


}


}