using Symbioz.World.Records;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA.Actions
{
    [IAAction(IAActionsEnum.BuffTeam)]
    class BuffTeamAction : AbstractIAAction
    {
        public override void Execute(World.Models.Fights.Fighters.MonsterFighter fighter)
        {
            try
            {
                var spells = fighter.Template.Spells.ConvertAll<SpellRecord>(x => SpellRecord.GetSpell(x));

                if (fighter.GetOposedTeam().LowerProchFighter(fighter, 20).FighterStats.LifePercentage <= 20 && spells.FindAll(x => x.Category == SpellCategoryEnum.Damages).Count > 0)
                    return;
                int po = 2;
                foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Heal))
                {
                    foreach (int s in spell.SpellLevels)
                    {
                        SpellLevelRecord record = SpellLevelRecord.GetLevel(s);
                        if (record == null)
                            continue;
                        if (po < record.MaxRange)
                            po = record.MaxRange;
                    }
                }
                foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Buff))
                {
                    foreach (int s in spell.SpellLevels)
                    {
                        SpellLevelRecord record = SpellLevelRecord.GetLevel(s);
                        if (record == null)
                            continue;
                        if (po < record.MaxRange)
                            po = record.MaxRange;
                    }
                }
                var target = fighter.Team.LowerProchFighter(fighter, po);
                foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Heal))
                {
                    CastAction.TryCast(fighter, spell.Id, target);
                }
                foreach (var spell in spells.FindAll(x => x.Category == SpellCategoryEnum.Buff))
                {
                    CastAction.TryCast(fighter, spell.Id, target);
                }
            }
            catch (Exception error)
            {
                Logger.Error(error);
            }
        }
    }
}
