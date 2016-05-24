


















// Generated on 06/04/2015 18:44:57
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartOkMountWithOutPaddockMessage : Message
{

public const ushort Id = 5991;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.MountClientData> stabledMountsDescription;
        

public ExchangeStartOkMountWithOutPaddockMessage()
{
}

public ExchangeStartOkMountWithOutPaddockMessage(IEnumerable<Types.MountClientData> stabledMountsDescription)
        {
            this.stabledMountsDescription = stabledMountsDescription;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)stabledMountsDescription.Count());
            foreach (var entry in stabledMountsDescription)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            stabledMountsDescription = new Types.MountClientData[limit];
            for (int i = 0; i < limit; i++)
            {
                 (stabledMountsDescription as Types.MountClientData[])[i] = new Types.MountClientData();
                 (stabledMountsDescription as Types.MountClientData[])[i].Deserialize(reader);
            }
            

}


}


}