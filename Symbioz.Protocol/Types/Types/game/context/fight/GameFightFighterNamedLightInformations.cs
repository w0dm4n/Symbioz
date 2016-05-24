


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameFightFighterNamedLightInformations : GameFightFighterLightInformations
{

public const short Id = 456;
public override short TypeId
{
    get { return Id; }
}

public string name;
        

public GameFightFighterNamedLightInformations()
{
}

public GameFightFighterNamedLightInformations(bool sex, bool alive, int id, sbyte wave, ushort level, sbyte breed, string name)
         : base(sex, alive, id, wave, level, breed)
        {
            this.name = name;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(name);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            name = reader.ReadUTF();
            

}


}


}