using Symbioz.DofusProtocol.Messages;
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
    public class FightArena : Fight
    {
        public const ushort ARENA_ITEM_ID = 12736;

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_PVP_ARENA; }
        }

        public override bool AddFightSword
        {
            get { return false; ; }
        }

        public FightArena(int id, MapRecord map, FightTeam blueTeam, FightTeam redTeam, short fightCellId) : base(id, map, blueTeam, redTeam, fightCellId) { }

        public override FightCommonInformations GetFightCommonInformations()
        {
            throw new NotImplementedException();
        }

        public override void TryJoin(WorldClient client, int mainfighterid)
        {
            throw new Exception("A client try to join an Arena Fight!");
        }


        public override void ShowFightResults(List<FightResultListEntry> results, WorldClient client)
        {
           
            client.Send(new GameFightEndMessage((ushort)TimeLine.m_round, 0, 0, results, new NamedPartyTeamWithOutcome[0]));
        }
        public override void OnFightEnded(TeamColorEnum winner)
        {


            FightTeam winners = null;
            FightTeam loosers = null;
            if (winner == RedTeam.TeamColor)
            {
                winners = RedTeam;
                loosers = BlueTeam;
            }
            else if (winner == BlueTeam.TeamColor)
            {
                winners = BlueTeam;
                loosers = RedTeam;
            }

            GetAllCharacterFighters(true).ForEach(x => x.Client.Character.Record.ArenaFightCount++);

            winners.GetCharacterFighters(true).ForEach(x => x.Client.Character.Record.ArenaVictoryCount++);

            winners.GetCharacterFighters(true).ForEach(x => x.Client.Character.Record.ActualRank += (ushort)(x.Client.Character.Record.Level)); // Check for an algo

            foreach (var _winner in winners.GetCharacterFighters(true))
            {
               if (_winner.Client.Character.Record.ActualRank > _winner.Client.Character.Record.MaxRank)
               {
                   _winner.Client.Character.Record.MaxRank = _winner.Client.Character.Record.ActualRank;
                   _winner.Client.Character.Record.BestDailyRank = _winner.Client.Character.Record.ActualRank;
               }
            }

            loosers.GetCharacterFighters(true).ForEach(x => x.Client.Character.Record.ActualRank -= (ushort)(x.Client.Character.Record.Level));


            GetAllCharacterFighters(true).ForEach(x => x.Client.Character.RefreshArenasInfos());
            base.OnFightEnded(winner);
        }

    }
}
