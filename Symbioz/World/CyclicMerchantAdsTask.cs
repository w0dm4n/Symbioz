using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.World.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World
{
    public static class CyclicMerchantAdsTask
    {
        public const short AdsInstanceInterval = 20000;

        private static System.Timers.Timer CyclicMerchantAdsTimer { get; set; }

        [StartupInvoke("CyclicMerchantAdsTask", StartupInvokeType.Cyclics)]
        public static void Start()
        {
            CyclicMerchantAdsTimer = new System.Timers.Timer(AdsInstanceInterval);
            CyclicMerchantAdsTimer.Elapsed += CyclicMerchantAdsTimer_Elapsed;
            CyclicMerchantAdsTimer.Start();
        }

        private static void CyclicMerchantAdsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (var mapAndMerchants in LoadMapsAndMerchants())
            {
                MapRecord mapRecord = MapRecord.GetMap(mapAndMerchants.Key);
                if (mapRecord != null && mapRecord.Instance != null && mapRecord.Instance.Clients.Count > 0)
                {
                    foreach (var merchant in mapAndMerchants.Value)
                    {
                        if (merchant.IsMerchantMode && !string.IsNullOrEmpty(merchant.MerchantMessage))
                            mapRecord.Instance.Send(new ChatServerMessage((sbyte)ChatActivableChannelsEnum.CHANNEL_GLOBAL, merchant.MerchantMessage, 1, string.Empty, merchant.Id, merchant.Name, merchant.AccountId));
                    }
                }
            }
        }

        private static Dictionary<int, List<CharacterRecord>> LoadMapsAndMerchants()
        {
            Dictionary<int, List<CharacterRecord>> mapsAndMerchants = new Dictionary<int, List<CharacterRecord>>();
            foreach (var character in CharacterRecord.Characters)
            {
                if (character.IsMerchantMode)
                {
                    if (mapsAndMerchants.ContainsKey(character.MapId))
                    {
                        mapsAndMerchants[character.MapId].Add(character);
                    }
                    else
                    {
                        mapsAndMerchants.Add(character.MapId, new List<CharacterRecord>() { character });
                    }
                }
            }
            return mapsAndMerchants;
        }
    }
}
