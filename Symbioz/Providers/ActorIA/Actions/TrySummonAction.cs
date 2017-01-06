using Symbioz.Enums;
using Symbioz.PathProvider;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Providers.ActorIA.Actions
{
    [IAAction(IAActionsEnum.TrySummon)]
    class TrySummonAction : AbstractIAAction
    {
        public override void Execute(MonsterFighter fighter)
        {
            try
            {
                var spells = fighter.Template.Spells.ConvertAll<SpellRecord>(x => SpellRecord.GetSpell(x));
                SpellRecord summonSpell = null;
                foreach (SpellRecord record in spells)
                {
                    if (summonSpell != null)
                        break;
                    foreach (int s in record.SpellLevels)
                    {
                        SpellLevelRecord lvlrecord = SpellLevelRecord.GetLevel(s);
                        if (record == null)
                            continue;
                        if (lvlrecord.Effects.Find(x => x.BaseEffect.EffectType == EffectsEnum.Eff_Summon) != null)
                        {
                            summonSpell = record;
                            break;
                        }
                    }
                }


                if (summonSpell != null && fighter.SummonCount <= 1)
                {
                    var cells = PathHelper.Getalldirection(fighter.Fight, fighter.CellId);
                    var cell = cells.Find(x => fighter.Fight.IsObstacle(x) == false);

                    if (cell != 0)
                        fighter.CastSpellOnCell(summonSpell.Id, cell);
                    else
                        fighter.Fight.Reply("Unable to summon");
                }
            }
            catch (Exception error)
            {
                Logger.Error(error);
            }
        }
    }
}
