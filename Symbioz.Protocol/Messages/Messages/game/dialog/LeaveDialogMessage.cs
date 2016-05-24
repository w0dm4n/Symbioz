


















// Generated on 06/04/2015 18:44:41
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class LeaveDialogMessage : Message
{

public const ushort Id = 5502;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte dialogType;
        

public LeaveDialogMessage()
{
}

public LeaveDialogMessage(sbyte dialogType)
        {
            this.dialogType = dialogType;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(dialogType);
            

}

public override void Deserialize(ICustomDataInput reader)
{

dialogType = reader.ReadSByte();
            if (dialogType < 0)
                throw new Exception("Forbidden value on dialogType = " + dialogType + ", it doesn't respect the following condition : dialogType < 0");
            

}


}


}