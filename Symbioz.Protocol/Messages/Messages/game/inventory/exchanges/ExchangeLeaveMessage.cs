


















// Generated on 06/04/2015 18:44:52
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeLeaveMessage : LeaveDialogMessage
{

public const ushort Id = 5628;
public override ushort MessageId
{
    get { return Id; }
}

public bool success;
        

public ExchangeLeaveMessage()
{
}

public ExchangeLeaveMessage(sbyte dialogType, bool success)
         : base(dialogType)
        {
            this.success = success;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(success);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            success = reader.ReadBoolean();
            

}


}


}