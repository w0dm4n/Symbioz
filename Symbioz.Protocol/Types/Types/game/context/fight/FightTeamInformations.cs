


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

    public class FightTeamInformations : AbstractFightTeamInformations
    {

        public const short Id = 33;
        public override short TypeId
        {
            get { return Id; }
        }

        public List<Types.FightTeamMemberInformations> teamMembers;


        public FightTeamInformations()
        {
        }

        public FightTeamInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, sbyte nbWaves, IEnumerable<Types.FightTeamMemberInformations> teamMembers)
            : base(teamId, leaderId, teamSide, teamTypeId, nbWaves)
        {
            this.teamMembers = teamMembers.ToList();
        }


        public override void Serialize(ICustomDataOutput writer)
        {

            base.Serialize(writer);
            writer.WriteUShort((ushort)teamMembers.Count());
            foreach (var entry in teamMembers)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }


        }

        public override void Deserialize(ICustomDataInput reader)
        {

            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var members = new Types.FightTeamMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                (members as Types.FightTeamMemberInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.FightTeamMemberInformations>(reader.ReadShort());
                (members as Types.FightTeamMemberInformations[])[i].Deserialize(reader);
            }
            teamMembers = members.ToList();


        }


    }


}