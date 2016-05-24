


















// Generated on 06/04/2015 18:44:02
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class IdentificationAccountForceMessage : IdentificationMessage
{

public const ushort Id = 6119;
public override ushort MessageId
{
    get { return Id; }
}

public string forcedAccountLogin;
        

public IdentificationAccountForceMessage()
{
}

public IdentificationAccountForceMessage(bool autoconnect, bool useCertificate, bool useLoginToken, Types.VersionExtended version, string lang, IEnumerable<byte> credentials, short serverId,long sessionOptionalSalt, IEnumerable<ushort> failedAttempts, string forcedAccountLogin)
         : base(autoconnect, useCertificate, useLoginToken, version, lang, credentials, serverId, sessionOptionalSalt, failedAttempts)
        {
            this.forcedAccountLogin = forcedAccountLogin;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(forcedAccountLogin);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            forcedAccountLogin = reader.ReadUTF();
            

}


}


}