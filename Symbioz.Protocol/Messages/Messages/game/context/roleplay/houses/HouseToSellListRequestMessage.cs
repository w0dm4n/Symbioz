


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class HouseToSellListRequestMessage : Message
{

public const ushort Id = 6139;
public override ushort MessageId
{
    get { return Id; }
}

public ushort pageIndex;
        

public HouseToSellListRequestMessage()
{
}

public HouseToSellListRequestMessage(ushort pageIndex)
        {
            this.pageIndex = pageIndex;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(pageIndex);
            

}

public override void Deserialize(ICustomDataInput reader)
{

pageIndex = reader.ReadVarUhShort();
            if (pageIndex < 0)
                throw new Exception("Forbidden value on pageIndex = " + pageIndex + ", it doesn't respect the following condition : pageIndex < 0");
            

}


}


}