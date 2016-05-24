using Symbioz.DofusProtocol.Types;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Parties.Dungeon
{
    public class DungeonPartyProvider : Singleton<DungeonPartyProvider>
    {
        private List<DungeonPartyCharacter> m_loggedDungeonParyCharacters = new List<DungeonPartyCharacter>();

        private object m_locker = new object();

        public void AddCharacter(Character character, List<ushort> dungeonsId)
        {
            lock (m_locker)
                m_loggedDungeonParyCharacters.Add(new DungeonPartyCharacter(character, dungeonsId));
        }

        public void UpdateCharacter(Character character, List<ushort> dungeonsIds)
        {
            lock (m_locker)
            {
                DungeonPartyCharacter dpc = m_loggedDungeonParyCharacters.Find(x => x.Character == character);
                dpc.DungeonsIds = dungeonsIds;
            }
        }

        public void RemoveCharacter(Character character)
        {
            lock (m_locker)
            m_loggedDungeonParyCharacters.Remove(GetDPCByCharacterId(character.Id));
            if(character != null && character.Client != null)
                character.Client.Send(new DungeonPartyFinderRegisterSuccessMessage((IEnumerable<ushort>)new List<ushort> { 0 }));
        }

        public DungeonPartyCharacter GetDPCByCharacterId(int characterId)
        {
            return m_loggedDungeonParyCharacters.Find(x => x.Character.Id == characterId);
        }

        public IEnumerable<DungeonPartyFinderPlayer> GetCharactersForDungeon(ushort dungeonId)
        {
            foreach (DungeonPartyCharacter dpc in m_loggedDungeonParyCharacters)
            {
                if (dpc.DungeonsIds.Contains(dungeonId))
                    yield return dpc.GetDungeonPartyFinderPlayer();
            }
        }
    }
}
