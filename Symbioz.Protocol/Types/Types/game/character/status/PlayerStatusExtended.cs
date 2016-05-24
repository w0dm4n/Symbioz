


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PlayerStatusExtended : PlayerStatus
{

public const short Id = 414;
public override short TypeId
{
    get { return Id; }
}

public string message;
        

public PlayerStatusExtended()
{
}

public PlayerStatusExtended(sbyte statusId, string message)
         : base(statusId)
        {
            this.message = message;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(message);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            message = reader.ReadUTF();
            

}


}


}