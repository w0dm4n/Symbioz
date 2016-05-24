


















// Generated on 06/04/2015 18:44:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class IdentificationMessage : Message
{

public const ushort Id = 4;
public override ushort MessageId
{
    get { return Id; }
}

public bool autoconnect;
        public bool useCertificate;
        public bool useLoginToken;
        public Types.VersionExtended version;
        public string lang;
        public IEnumerable<byte> credentials;
        public short serverId;
        public long sessionOptionalSalt;
        public IEnumerable<ushort> failedAttempts;
        

public IdentificationMessage()
{
}

public IdentificationMessage(bool autoconnect, bool useCertificate, bool useLoginToken, Types.VersionExtended version, string lang, IEnumerable<byte> credentials, short serverId, long sessionOptionalSalt, IEnumerable<ushort> failedAttempts)
        {
            this.autoconnect = autoconnect;
            this.useCertificate = useCertificate;
            this.useLoginToken = useLoginToken;
            this.version = version;
            this.lang = lang;
            this.credentials = credentials;
            this.serverId = serverId;
            this.sessionOptionalSalt = sessionOptionalSalt;
            this.failedAttempts = failedAttempts;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, autoconnect);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, useCertificate);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, useLoginToken);
            writer.WriteByte(flag1);
            version.Serialize(writer);
            writer.WriteUTF(lang);
            writer.WriteUShort((ushort)credentials.Count());
            foreach (var entry in credentials)
            {
                 writer.WriteByte(entry);
            }
            writer.WriteShort(serverId);
            writer.WriteVarLong(sessionOptionalSalt);
            writer.WriteUShort((ushort)failedAttempts.Count());
            foreach (var entry in failedAttempts)
            {
                 writer.WriteVarUhShort(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

            byte flag1 = reader.ReadByte();
            autoconnect = BooleanByteWrapper.GetFlag(flag1, 0);
            useCertificate = BooleanByteWrapper.GetFlag(flag1, 1);
            useLoginToken = BooleanByteWrapper.GetFlag(flag1, 2);
            version = new Types.VersionExtended();
            version.Deserialize(reader);
            lang = reader.ReadUTF();
            var limit = reader.ReadVarUhShort();
            credentials = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (credentials as byte[])[i] = reader.ReadByte();
            }
            serverId = reader.ReadShort();
            sessionOptionalSalt = reader.ReadVarLong();
            if ((sessionOptionalSalt < -9.007199254740992E15) || (sessionOptionalSalt > 9.007199254740992E15))
                throw new Exception("Forbidden value on sessionOptionalSalt = " + sessionOptionalSalt + ", it doesn't respect the following condition : (sessionOptionalSalt < -9.007199254740992E15) || (sessionOptionalSalt > 9.007199254740992E15)");
            limit = reader.ReadUShort();
            failedAttempts = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (failedAttempts as ushort[])[i] = reader.ReadVarUhShort();
            }
            

}


}


}