﻿using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.ORM;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records.Alliances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Guilds
{
    [Table("Guilds")]
    public class GuildRecord : ITable
    {
        static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static List<GuildRecord> Guilds = new List<GuildRecord>();

        [Primary]
        public int Id;

        public string Name;

        public ushort SymbolShape;

        public int SymbolColor;

        public sbyte BackgroundShape;

        public int BackgroundColor;

        [Update]
        public ushort Level;

        [Update]
        public ulong Experience;

        [Update]
        public int MaxTaxCollectors;

        public DateTime CreationDate;

        [Update]
        public string GuildWelcomeMessage;

        public GuildRecord(int id, string name, ushort symbolShape, int symbolColor,
            sbyte backgroundShape, int backgroundColor, ushort level, ulong experience, int maxTaxCollectors, DateTime creationDate, string guildWelcomeMessage)
        {
            this.Id = id;
            this.Name = name;
            this.SymbolShape = symbolShape;
            this.SymbolColor = symbolColor;
            this.BackgroundShape = backgroundShape;
            this.BackgroundColor = backgroundColor;
            this.Level = level;
            this.Experience = experience;
            this.MaxTaxCollectors = maxTaxCollectors;
            this.CreationDate = creationDate;
            this.GuildWelcomeMessage = guildWelcomeMessage;
        }

        public GuildEmblem GetEmblemObject()
        {
            return new GuildEmblem(SymbolShape, SymbolColor, BackgroundShape, BackgroundColor);
        }

        public GuildInformations GetGuildInformations()
        {
            return new GuildInformations((uint)Id, Name, this.GetEmblemObject());
        }

        public bool IsInAlliance
        {
            get
            {
                return GuildAllianceRecord.GuildsAlliances.FirstOrDefault(x => x.GuildId == this.Id) != null ? true : false;
            }
        }

        public BasicGuildInformations GetBasicInformations()
        {
            return new BasicGuildInformations((uint)Id, Name);
        }

        public static GuildRecord GetGuild(int id)
        {
            return Guilds.Find(x => x.Id == id);
        }

        public static bool CanCreateGuild(string guildName)
        {
            return Guilds.Find(x => x.Name == guildName) == null;
        }

        public static int PopNextId()
        {
            Locker.EnterReadLock();
            try
            {
                var ids = Guilds.ConvertAll<int>(x => x.Id);
                ids.Sort();
                return ids.Count == 0 ? 1 : ids.Last() + 1;
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        public CharacterGuildRecord GetLeader()
        {
            foreach (CharacterGuildRecord member in CharacterGuildRecord.CharactersGuilds.FindAll(x => x.GuildId == this.Id))
            {
                if (member.Rights == (uint)GuildRightsBitEnum.GUILD_RIGHT_BOSS)
                {
                    return member;
                }
            }
            return null;
        }

        public static GuildRecord GetGuildForSave(int GuildId)
        {
            foreach (var guild in Guilds)
            {
                if (guild.Id == GuildId)
                    return guild;
            }
            return null;
        }

        [BeforeSave]
        public static void BeforeSave()
        {
            var GuildsCopy = new List<GuildRecord>();
            foreach (var guild in Guilds)
                GuildsCopy.Add(guild);

            foreach (var guild in GuildsCopy)
            {
                SaveTask.UpdateElement(GetGuildForSave(guild.Id));
            }
            //Guilds.ForEach(x => SaveTask.UpdateElement(x));
        }

        #region Extended

        public void Send(Message message)
        {
            GuildProvider.Instance.GetClients(this.Id).ForEach(x => x.Send(message));
        }

        public void SendChatMessage(string message)
        {
            this.Send(new TextInformationMessage(0, 0, new string[] { message }));
        }

        #endregion
    }
}
