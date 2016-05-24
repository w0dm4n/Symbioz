using Symbioz.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Models.Alliances;
using Symbioz.World.Records.Alliances;
using Symbioz.World.Records.Guilds;
using Symbioz.DofusProtocol.Types;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records;

namespace Symbioz.World.Handlers
{
    class AllianceHandler
    {
        [MessageHandler]
        public static void HandleAllianceCreationRequest(AllianceCreationValidMessage message, WorldClient client)
        {
            if (!AllianceProvider.CanCreateAlliance(client, message))
                return;

            client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_OK));
            AllianceProvider.CreateAlliance(client.Character.GetGuild(), message.allianceTag, message.allianceName, message.allianceEmblem);
        }
        [MessageHandler]
        public static void HandleGetAllianceInformations(AllianceInsiderInfoRequestMessage message, WorldClient client)
        {
            AllianceRecord Alliance = client.Character.GetAlliance();
            AllianceInsiderInfoMessage msg = AllianceProvider.GetAllianceInsiderInfoMessage(Alliance);
            client.Send(msg);
        }
        [MessageHandler]
        public static void HandleGetGuildInFactRequest(GuildFactsRequestMessage message, WorldClient client)
        {
            GuildRecord guild = GuildRecord.GetGuild((int)message.guildId);
            if (guild == null)
                return;
            AllianceRecord alliance = AllianceProvider.GetAllianceFromGuild(guild.Id);
            List<CharacterMinimalInformations> membersMinimalInfos = new List<CharacterMinimalInformations>();
            foreach (GuildMember member in CharacterGuildRecord.GetMembers(guild.Id))
            {
                membersMinimalInfos.Add(new CharacterMinimalInformations(member.id, member.level, member.name));
            }
            if (alliance == null)
                client.Send(new GuildFactsMessage(new GuildFactSheetInformations((uint)guild.Id, guild.Name, guild.GetEmblemObject(), (uint)GuildProvider.GetLeader(guild.Id).CharacterId, (byte)guild.Level, (ushort)CharacterGuildRecord.GetMembers(guild.Id).Count()), 0, 0, true, (IEnumerable<CharacterMinimalInformations>)membersMinimalInfos));
            else
                client.Send(new GuildInAllianceFactsMessage(new GuildFactSheetInformations((uint)guild.Id, guild.Name, guild.GetEmblemObject(), (uint)GuildProvider.GetLeader(guild.Id).CharacterId, (byte)guild.Level, (ushort)CharacterGuildRecord.GetMembers(guild.Id).Count()),0,0,true, (IEnumerable<CharacterMinimalInformations>)membersMinimalInfos,new BasicNamedAllianceInformations((uint)alliance.Id,alliance.Tag,alliance.Name)));
        }
        [MessageHandler]
        public static void HandleAllianceKickRequest(AllianceKickRequestMessage message, WorldClient client)
        {
            if(client.Character.HasAlliance && AllianceProvider.GetLeader(client.Character.AllianceId).Id == client.Character.GuildId && GuildProvider.GetLeader(client.Character.GuildId).CharacterId == client.Character.Id)
            {
                AllianceRecord.GetAlliance(client.Character.AllianceId).KickFromAlliance((int)message.kickedId, client);
            }
        }
    }
}
