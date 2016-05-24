using Symbioz.DofusProtocol.Types;
using Symbioz.Network.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Parties.Dungeon
{
    public class DungeonPartyCharacter
    {
        public Character Character;

        public List<ushort> DungeonsIds;

        public DungeonPartyCharacter(Character character, List<ushort> dungeonId)
        {
            this.Character = character;

            this.DungeonsIds = dungeonId;
        }


        public DungeonPartyFinderPlayer GetDungeonPartyFinderPlayer()
        {
            return new DungeonPartyFinderPlayer((uint)this.Character.Id, this.Character.Record.Name,
                this.Character.Record.Breed, this.Character.Record.Sex, this.Character.Record.Level);
        }
    }
}