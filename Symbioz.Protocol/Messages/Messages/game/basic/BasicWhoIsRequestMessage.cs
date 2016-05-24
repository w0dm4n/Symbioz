


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class BasicWhoIsRequestMessage : Message
{

public const ushort Id = 181;
public override ushort MessageId
{
    get { return Id; }
}

public bool verbose;
        public string search;
        

public BasicWhoIsRequestMessage()
{
}

public BasicWhoIsRequestMessage(bool verbose, string search)
        {
            this.verbose = verbose;
            this.search = search;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(verbose);
            writer.WriteUTF(search);
            

}

public override void Deserialize(ICustomDataInput reader)
{

verbose = reader.ReadBoolean();
            search = reader.ReadUTF();
            

}


}


}