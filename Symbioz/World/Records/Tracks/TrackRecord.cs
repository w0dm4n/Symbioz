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
    [Table("CharactersTracked")]
    public class TrackRecord : ITable
    {
        public static List<TrackRecord> CharactersTracked = new List<TrackRecord>();
        public static int IdToAdd = 0;

        [Primary]
        public int Id;
        public int TrackedCharacterId;
        public int ItemUID;
        public int Tentative;

        public TrackRecord(int id, int trackedCharacterId, int itemUID, int Tentative)
        {
            this.Id = id;
            this.TrackedCharacterId = trackedCharacterId;
            this.ItemUID = itemUID;
            this.Tentative = 5;
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
            var nextId = 0;
            foreach (var ignored in TrackRecord.CharactersTracked)
            {
                if (ignored.Id > nextId)
                    nextId = ignored.Id;
            }
            nextId++;
            nextId += TrackRecord.IdToAdd;
            TrackRecord.IdToAdd++;
            return (nextId);
        }

        public static void AddTracked(int trackedCharacterId, int itemUID)
        {
            var newElement = new TrackRecord(TrackRecord.PopNextTrackedId(), trackedCharacterId, itemUID, 5);
            SaveTask.AddElement(newElement);
        }

        public static void DeleteTrackedByItemUID(int itemUID)
        {
            foreach (var tracked in TrackRecord.CharactersTracked)
            {
                if (tracked.ItemUID == itemUID)
                {
                    SaveTask.RemoveElement(tracked);
                    break;
                }
            }
        }

        public static TrackRecord GetFromItemuid(int itemUID)
        {
            foreach (var tracked in TrackRecord.CharactersTracked)
            {
                if (tracked.ItemUID == itemUID)
                {
                    return tracked;
                }
            }
            return null;
        }

        public static void DecreaseTentative(int itemUID)
        {
            foreach (var tracked in TrackRecord.CharactersTracked)
            {
                if (tracked.ItemUID == itemUID)
                {
                    tracked.Tentative = tracked.Tentative - 1;
                }
            }
        }
    }
}
