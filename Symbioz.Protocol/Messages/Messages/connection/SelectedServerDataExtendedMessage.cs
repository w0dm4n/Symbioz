


















// Generated on 06/04/2015 18:44:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

    public class SelectedServerDataExtendedMessage : SelectedServerDataMessage
    {

        public const ushort Id = 6469;
        public override ushort MessageId
        {
            get { return Id; }
        }

        public IEnumerable<ushort> serverIds;


        public SelectedServerDataExtendedMessage()
        {
        }

        public SelectedServerDataExtendedMessage(ushort serverId, string address, ushort port, bool canCreateNewCharacter, IEnumerable<byte> ticket, IEnumerable<ushort> serverIds)
            : base(serverId, address, port, canCreateNewCharacter, ticket)
        {
            this.serverIds = serverIds;
        }


        public override void Serialize(ICustomDataOutput writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)serverIds.Count());
            foreach (var entry in serverIds)
            {
                writer.WriteVarUhShort(entry);
            }
        }

        public override void Deserialize(ICustomDataInput reader)
        {

            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            serverIds = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                (serverIds as ushort[])[i] = reader.ReadVarUhShort();
            }


        }


    }


}