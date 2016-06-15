using Symbioz.Core;
using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.Providers;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Tracks
{
    [Table("CharactersTracked", true)]
    class TracksRecord : ITable
    {
        public static List<TracksRecord> CharactersTracked = new List<TracksRecord>();
        public static int IdToAdd = 0;

        [Primary]
        public int Id;
        public int TrackedCharacterId;
        public int ItemUID;

        public TracksRecord(int id, int trackedCharacterId, int itemUID)
        {
            this.Id = id;
            this.TrackedCharacterId = trackedCharacterId;
            this.ItemUID = itemUID;
        }

        public static int GetCharacterIdTrackedFromItemUID(int UID)
        {
            foreach (var tracked in CharactersTracked)
            {
                if (tracked.ItemUID == UID)
                    return tracked.TrackedCharacterId;
            }
            return 0;
        }

        public static int PopNextTrackedId()
        {
            var Id = 0;
            foreach (var Ignored in TracksRecord.CharactersTracked)
            {
                if (Ignored.Id > Id)
                    Id = Ignored.Id;
            }
            Id++;
            Id += TracksRecord.IdToAdd;
            TracksRecord.IdToAdd++;
            return (Id);
        }

        public static void AddTracked(int trackedCharacterId, int itemUID)
        {
            var newElement = new TracksRecord(TracksRecord.PopNextTrackedId(), trackedCharacterId, itemUID);
            TracksRecord.CharactersTracked.Add(newElement);
            SaveTask.AddElement(newElement);
        }

        public static void DeleteTrackedByItemUID(int itemUID)
        {
            foreach (var tracked in TracksRecord.CharactersTracked)
            {
                if (tracked.ItemUID == itemUID)
                {
                    TracksRecord.CharactersTracked.Remove(tracked);
                    SaveTask.RemoveElement(tracked);
                    break;
                }
            }
        }
    }
}
