using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.World.Models;
using Symbioz.World.Models.Alliances;
using Symbioz.World.Models.Fights.FightsTypes;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Alliances
{
    [Table("alliances",true)]
    public class AllianceRecord : ITable
    {

        static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static List<AllianceRecord> Alliances = new List<AllianceRecord>();

        [Primary]
        public int Id;
        [Update]
        public string Name;
        [Update]
        public string Tag;
        [Update]
        public int SymbolColor;
        [Update]
        public ushort SymbolShape;
        [Update]
        public int BackgroundColor;
        [Update]
        public sbyte BackgroundShape;
        [Update]
        public int LeaderGuildId;
        public DateTime CreationDate;
        [Update]
        public string AllianceWelcomeMessage;
        [Ignore]
        List<FightAvAPrism> Fights = new List<FightAvAPrism>();

        public AllianceRecord(int id, string name, string tag, int symbolColor, ushort symbolShape, int backgroundColor, sbyte backgroundShape, int leader, DateTime creationDate, string allianceWelcomeMessage)
        {
            this.Id = id;
            this.Name = name;
            this.Tag = tag;
            this.SymbolColor = symbolColor;
            this.SymbolShape = symbolShape;
            this.BackgroundColor = backgroundColor;
            this.BackgroundShape = backgroundShape;
            this.LeaderGuildId = leader;
            this.CreationDate = creationDate;
            this.AllianceWelcomeMessage = allianceWelcomeMessage;
        }

        public GuildEmblem GetEmblemObject()
        {
            return new GuildEmblem(SymbolShape, SymbolColor, BackgroundShape, BackgroundColor);
        }

        public AllianceInformations GetAllianceInformations()
        {
            return new AllianceInformations((uint)Id, Tag, Name, new GuildEmblem(SymbolShape, SymbolColor, BackgroundShape, BackgroundColor));
        }

        public BasicAllianceInformations GetBasicInformations()
        {
            return new BasicAllianceInformations((uint)Id, Tag);
        }

        public BasicNamedAllianceInformations GetBasicNamedAllianceInformations()
        {
            return new BasicNamedAllianceInformations((uint)this.Id, this.Tag, this.Name);
        }

        public bool KickFromAlliance(int guildId, WorldClient by)
        {
            if(GuildRecord.GetGuild(guildId) != null)
            {
                GuildAllianceRecord member = GuildAllianceRecord.GetCharacterAlliance(guildId);
                if(member != null || member.AllianceId == this.Id)
                {
                    List<CharacterGuildRecord> charactersGuildRecord = CharacterGuildRecord.CharactersGuilds.FindAll(x => x.GuildId == guildId);
                    foreach(CharacterGuildRecord characterGuild in charactersGuildRecord)
                    {
                        Character character = WorldServer.Instance.GetOnlineClient(characterGuild.CharacterId).Character;
                        AllianceRecord.OnCharacterLeftAlliance(character);
                    }
                    member.RemoveElement();
                    if(AllianceRecord.CountGuildInAlliance(member.AllianceId) < 1)
                    {
                        AllianceRecord.DeleteAlliance(member.AllianceId);
                    }
                    return true;
                }
            }
            return false;
        }

        public static void OnCharacterLeftAlliance(Character character)
        {
            if (WorldServer.Instance.IsConnected(character.Id))
            {
                WorldClient client = WorldServer.Instance.GetOnlineClient(character.Id);
                client.Send(new AllianceLeftMessage());
                client.Character.ForgetEmote(AllianceProvider.EMOTE_ID);
                client.Character.HumanOptions.RemoveAll(x => x is HumanOptionAlliance);
                client.Character.RefreshOnMapInstance();
            }
            else
            {
                character.Record.KnownEmotes.RemoveAll(x => x.Equals(AllianceProvider.EMOTE_ID));
            }
        }

        public static void OnCharacterJoinAlliance(CharacterRecord character, AllianceRecord alliance)
        {
            if (WorldServer.Instance.IsConnected(character.Id))
            {
                WorldClient client = WorldServer.Instance.GetOnlineClient(character.Id);
                client.Send(new AllianceJoinedMessage(alliance.GetAllianceInformations(), true));
                client.Character.LearnEmote(AllianceProvider.EMOTE_ID);
                client.Character.HumanOptions.Add(new HumanOptionAlliance(alliance.GetAllianceInformations(),(sbyte)0));
                client.Character.SetAllianceAndGuildLook();
                client.Character.RefreshOnMapInstance();
            }
            else
            {
                character.KnownEmotes.Add(AllianceProvider.EMOTE_ID);
            }
        }

        public static bool NameTaked(string allianceName)
        {
            return Alliances.Find(x => x.Name == allianceName) != null;
        }

        public static bool TagTaked(string allianceTag)
        {
            return Alliances.Find(x => x.Tag == allianceTag) != null;
        }

        public static AllianceRecord GetAlliance(int allianceId)
        {
            return Alliances.Find(x => x.Id == allianceId);
        }

        public static int CountGuildInAlliance(int allianceId)
        {
            return GuildAllianceRecord.GuildsAlliances.Count(x => x.AllianceId == allianceId);
        }

        public static void DeleteAlliance(int allianceId)
        {
            AllianceRecord alliance = AllianceRecord.GetAlliance(allianceId);
            List<GuildAllianceRecord> members = GuildAllianceRecord.GuildsAlliances.FindAll(x => x.AllianceId == allianceId);
            foreach(GuildAllianceRecord member in members)
            {
                foreach(WorldClient client in WorldServer.Instance.GetAllClientsOnline().FindAll(x=>x.Character.GuildId == member.GuildId))
                {
                    AllianceRecord.OnCharacterLeftAlliance(client.Character);
                }
                member.RemoveElement();
            }
        }

        public static int PopNextId()
        {
            Locker.EnterReadLock();
            try
            {
                var ids = Alliances.ConvertAll<int>(x => x.Id);
                ids.Sort();
                return ids.Count == 0 ? 1 : ids.Last() + 1;
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        [BeforeSave]
        public static void BeforeSave()
        {
            Alliances.ForEach(x => SaveTask.UpdateElement(x));
        }

        #region Extended

        public void Send(Message message)
        {
            AllianceProvider.GetClients(this.Id).ForEach(x => x.Send(message));
        }

        public void SendChatMessage(string message)
        {
            this.Send(new TextInformationMessage(0, 0, new string[] { message }));
        }

        #endregion

        public void AddPrismFight(FightAvAPrism fight)
        {
            this.Fights.Add(fight);
            this.Send(new PrismFightAddedMessage(fight.GetPrismFightersInformation()));
        }

        public void RemovePrismFight(FightAvAPrism fight)
        {
            if (this.Fights.Contains(fight))
            {
                this.Fights.Remove(fight);
                this.Send(new PrismFightRemovedMessage((ushort)fight.GetPrism().SubAreaId));
            }
        }
    }
}
