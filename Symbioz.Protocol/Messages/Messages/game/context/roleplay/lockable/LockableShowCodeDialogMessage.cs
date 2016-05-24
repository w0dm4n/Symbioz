


















// Generated on 06/04/2015 18:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class LockableShowCodeDialogMessage : Message
{

public const ushort Id = 5740;
public override ushort MessageId
{
    get { return Id; }
}

public bool changeOrUse;
        public sbyte codeSize;
        

public LockableShowCodeDialogMessage()
{
}

public LockableShowCodeDialogMessage(bool changeOrUse, sbyte codeSize)
        {
            this.changeOrUse = changeOrUse;
            this.codeSize = codeSize;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(changeOrUse);
            writer.WriteSByte(codeSize);
            

}

public override void Deserialize(ICustomDataInput reader)
{

changeOrUse = reader.ReadBoolean();
            codeSize = reader.ReadSByte();
            if (codeSize < 0)
                throw new Exception("Forbidden value on codeSize = " + codeSize + ", it doesn't respect the following condition : codeSize < 0");
            

}


}


}