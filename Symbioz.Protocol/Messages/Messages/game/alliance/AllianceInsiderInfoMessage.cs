


















// Generated on 06/04/2015 18:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceInsiderInfoMessage : Message
{

public const ushort Id = 6403;
public override ushort MessageId
{
    get { return Id; }
}

public Types.AllianceFactSheetInformations allianceInfos;
        public IEnumerable<Types.GuildInsiderFactSheetInformations> guilds;
        public IEnumerable<Types.PrismSubareaEmptyInfo> prisms;
        

public AllianceInsiderInfoMessage()
{
}

public AllianceInsiderInfoMessage(Types.AllianceFactSheetInformations allianceInfos, IEnumerable<Types.GuildInsiderFactSheetInformations> guilds, IEnumerable<Types.PrismSubareaEmptyInfo> prisms)
        {
            this.allianceInfos = allianceInfos;
            this.guilds = guilds;
            this.prisms = prisms;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

allianceInfos.Serialize(writer);
            writer.WriteUShort((ushort)guilds.Count());
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)prisms.Count());
            foreach (var entry in prisms)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

allianceInfos = new Types.AllianceFactSheetInformations();
            allianceInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            guilds = new Types.GuildInsiderFactSheetInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guilds as Types.GuildInsiderFactSheetInformations[])[i] = new Types.GuildInsiderFactSheetInformations();
                 (guilds as Types.GuildInsiderFactSheetInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            prisms = new Types.PrismSubareaEmptyInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (prisms as Types.PrismSubareaEmptyInfo[])[i] = Types.ProtocolTypeManager.GetInstance<Types.PrismSubareaEmptyInfo>(reader.ReadShort());
                 (prisms as Types.PrismSubareaEmptyInfo[])[i].Deserialize(reader);
            }
            

}


}


}