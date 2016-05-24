


















// Generated on 06/04/2015 18:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AccountHouseMessage : Message
{

public const ushort Id = 6315;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.AccountHouseInformations> houses;
        

public AccountHouseMessage()
{
}

public AccountHouseMessage(IEnumerable<Types.AccountHouseInformations> houses)
        {
            this.houses = houses;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)houses.Count());
            foreach (var entry in houses)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            houses = new Types.AccountHouseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (houses as Types.AccountHouseInformations[])[i] = new Types.AccountHouseInformations();
                 (houses as Types.AccountHouseInformations[])[i].Deserialize(reader);
            }
            

}


}


}