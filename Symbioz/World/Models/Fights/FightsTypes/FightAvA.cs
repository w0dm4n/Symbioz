using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Network.Clients;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights.FightsTypes
{
    class FightAvA : Fight
    {
        public FightAvA(int id, MapRecord map, FightTeam blueteam, FightTeam redteam, short fightcellid)
            : base(id, map, blueteam, redteam, fightcellid)
        {

        }

        public override FightTypeEnum FightType
        {
            get
            {
                return base.FightType;//TODO type
            }
        }

        public override bool AddFightSword
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override FightCommonInformations GetFightCommonInformations()
        {
            throw new NotImplementedException();
        }

        public override void ShowFightResults(List<FightResultListEntry> results, WorldClient client)
        {
            throw new NotImplementedException();
        }

        public override void TryJoin(WorldClient client, int mainfighterid)
        {
            throw new NotImplementedException();
        }
    }
}
