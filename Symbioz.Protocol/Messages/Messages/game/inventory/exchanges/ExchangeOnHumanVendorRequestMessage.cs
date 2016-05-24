


















// Generated on 06/04/2015 18:44:54
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeOnHumanVendorRequestMessage : Message
{

public const ushort Id = 5772;
public override ushort MessageId
{
    get { return Id; }
}

public uint humanVendorId;
        public ushort humanVendorCell;
        

public ExchangeOnHumanVendorRequestMessage()
{
}

public ExchangeOnHumanVendorRequestMessage(uint humanVendorId, ushort humanVendorCell)
        {
            this.humanVendorId = humanVendorId;
            this.humanVendorCell = humanVendorCell;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(humanVendorId);
            writer.WriteVarUhShort(humanVendorCell);
            

}

public override void Deserialize(ICustomDataInput reader)
{

humanVendorId = reader.ReadVarUhInt();
            if (humanVendorId < 0)
                throw new Exception("Forbidden value on humanVendorId = " + humanVendorId + ", it doesn't respect the following condition : humanVendorId < 0");
            humanVendorCell = reader.ReadVarUhShort();
            if ((humanVendorCell < 0) || (humanVendorCell > 559))
                throw new Exception("Forbidden value on humanVendorCell = " + humanVendorCell + ", it doesn't respect the following condition : (humanVendorCell < 0) || (humanVendorCell > 559)");
            

}


}


}