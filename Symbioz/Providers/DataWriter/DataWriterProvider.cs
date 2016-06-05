using Shader.Helper;
using Symbioz.Core;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Helper;
using Symbioz.Utils;
using Symbioz.World.Models.Alliances;
using Symbioz.World.Models.Guilds;
using Symbioz.World.Records.Alliances;
using Symbioz.World.Records.Guilds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Symbioz.Providers.DataWriter
{
    public class DataWriterProvider : Singleton<DataWriterProvider>
    {
        private CustomDataWriter DataWriter;
        private Timer CyclicWritingFilesTask;
        private ConsoleColor DefaultColor = ConsoleColor.Yellow;


        public void Init()
        {
            this.DataWriter = new CustomDataWriter();

            this.Generate();

            this.CyclicWritingFilesTask = new Timer(ConfigurationManager.Instance.CyclicFileGenerationInterval);
            this.CyclicWritingFilesTask.Elapsed += (sender, e) => this.Generate();
            this.CyclicWritingFilesTask.Start();
        }

        public void Stop()
        {
            this.CyclicWritingFilesTask.Stop();
        }

        public void Generate()
        {
            Logger.Write(string.Format("Génération des fichiers de données ..."), DefaultColor);
            this.GenerateGuildListMessage(ConfigurationManager.Instance.ServerId);
            this.GenerateGuildVersatileInfoListMessage(ConfigurationManager.Instance.ServerId);
            this.GenerateAllianceListMessage(ConfigurationManager.Instance.ServerId);
            this.GenerateAllianceVersatileInfoListMessage(ConfigurationManager.Instance.ServerId);
            Logger.Write(string.Format("Génération des fichiers de données terminée !"), DefaultColor);
        }

        #region Files Generation

        private void GenerateGuildListMessage(int serverId)
        {
            string fileName = string.Format("GuildListMessage.{0}.data", serverId);
            Logger.Write(string.Format("Génération du fichier '{0}' ...", fileName), DefaultColor);

            List<GuildInformations> guildInformations = new List<GuildInformations>();
            foreach (var guild in GuildRecord.Guilds)
            {
                guildInformations.Add(guild.GetGuildInformations());
            }

            GuildListMessage guildListMessage = new GuildListMessage(guildInformations);
            guildListMessage.Pack(this.DataWriter);

            File.WriteAllBytes(ConfigurationManager.Instance.CyclicFileGenerationPath + fileName, DataWriter.Data);
            Logger.Write(string.Format("Génération du fichier '{0}' terminé", fileName), DefaultColor);
        }

        private void GenerateGuildVersatileInfoListMessage(int serverId)
        {
            string fileName = string.Format("GuildVersatileInfoListMessage.{0}.data", serverId);
            Logger.Write(string.Format("Génération du fichier '{0}' ...", fileName), DefaultColor);

            List<GuildVersatileInformations> guildVersatileInformations = new List<GuildVersatileInformations>();
            foreach (var guild in GuildRecord.Guilds)
            {
                GuildVersatileInformations guildVersatileInformation = new GuildVersatileInformations((uint)guild.Id,
                    (uint)GuildProvider.GetLeader(guild.Id).CharacterId, (byte)guild.Level,
                    (byte)CharacterGuildRecord.GetMembers(guild.Id).Length);
                guildVersatileInformations.Add(guildVersatileInformation);
            }

            GuildVersatileInfoListMessage guildVersatileInfoListMessage = new GuildVersatileInfoListMessage(guildVersatileInformations);
            guildVersatileInfoListMessage.Pack(this.DataWriter);

            File.WriteAllBytes(ConfigurationManager.Instance.CyclicFileGenerationPath + fileName, DataWriter.Data);
            Logger.Write(string.Format("Génération du fichier '{0}' terminé", fileName), DefaultColor);
        }

        private void GenerateAllianceListMessage(int serverId)
        {
            string fileName = string.Format("AllianceListMessage.{0}.data", serverId);
            Logger.Write(string.Format("Génération du fichier '{0}' ...", fileName), DefaultColor);

            List<AllianceFactSheetInformations> allianceFactSheetInformations = new List<AllianceFactSheetInformations>();
            foreach (var alliance in AllianceRecord.Alliances)
            {
                AllianceFactSheetInformations allianceFactSheetInformation = new AllianceFactSheetInformations((uint)alliance.Id,
                    alliance.Tag, alliance.Name, alliance.GetEmblemObject(), DateTimeUtils.GetEpochFromDateTime(alliance.CreationDate));
                allianceFactSheetInformations.Add(allianceFactSheetInformation);
            }

            AllianceListMessage allianceListMessage = new AllianceListMessage(allianceFactSheetInformations);
            allianceListMessage.Pack(this.DataWriter);

            File.WriteAllBytes(ConfigurationManager.Instance.CyclicFileGenerationPath + fileName, DataWriter.Data);
            Logger.Write(string.Format("Génération du fichier '{0}' terminé", fileName), DefaultColor);
        }

        private void GenerateAllianceVersatileInfoListMessage(int serverId)
        {
            string fileName = string.Format("AllianceVersatileInfoListMessage.{0}.data", serverId);
            Logger.Write(string.Format("Génération du fichier '{0}' ...", fileName), DefaultColor);

            List<AllianceVersatileInformations> allianceVersatileInformations = new List<AllianceVersatileInformations>();
            foreach (var alliance in AllianceRecord.Alliances)
            {
                AllianceVersatileInformations allianceVersatileInformation = new AllianceVersatileInformations((uint)alliance.Id,
                    (ushort)AllianceProvider.GetGuildsCount(alliance.Id), (ushort)AllianceProvider.GetMembersCount(alliance.Id), 0);
                allianceVersatileInformations.Add(allianceVersatileInformation);
            }

            AllianceVersatileInfoListMessage allianceVersatileInfoListMessage = new AllianceVersatileInfoListMessage(allianceVersatileInformations);
            allianceVersatileInfoListMessage.Pack(this.DataWriter);

            File.WriteAllBytes(ConfigurationManager.Instance.CyclicFileGenerationPath + fileName, DataWriter.Data);
            Logger.Write(string.Format("Génération du fichier '{0}' terminé", fileName), DefaultColor);
        }

        #endregion
    }
}
