


















// Generated on 06/04/2015 18:44:57
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartOkMountMessage : ExchangeStartOkMountWithOutPaddockMessage
{

public const ushort Id = 5979;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.MountClientData> paddockedMountsDescription;
        

public ExchangeStartOkMountMessage()
{
}

public ExchangeStartOkMountMessage(IEnumerable<Types.MountClientData> stabledMountsDescription, IEnumerable<Types.MountClientData> paddockedMountsDescription)
         : base(stabledMountsDescription)
        {
            this.paddockedMountsDescription = paddockedMountsDescription;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)paddockedMountsDescription.Count());
            foreach (var entry in paddockedMountsDescription)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            paddockedMountsDescription = new Types.MountClientData[limit];
            for (int i = 0; i < limit; i++)
            {
                 (paddockedMountsDescription as Types.MountClientData[])[i] = new Types.MountClientData();
                 (paddockedMountsDescription as Types.MountClientData[])[i].Deserialize(reader);
            }
            

}


}


}