using Shader.Helper;
using Symbioz.Auth;
using Symbioz.Auth.Handlers;
using Symbioz.Auth.Models;
using Symbioz.Auth.Records;
using Symbioz.Core;
using Symbioz.Core.Startup;
using Symbioz.DofusProtocol.Messages;
using Symbioz.DofusProtocol.Types;
using Symbioz.Enums;
using Symbioz.Helper;
using Symbioz.Network.Clients;
using Symbioz.Network.Servers;
using Symbioz.ORM;
using Symbioz.Providers.DataWriter;
using Symbioz.Providers.Maps;
using Symbioz.World.Models;
using Symbioz.World.Models.Alliances;
using Symbioz.World.Models.Fights.Marks;
using Symbioz.World.Models.Guilds;
using Symbioz.World.PathProvider;
using Symbioz.World.Records;
using Symbioz.World.Records.Guilds;
using Symbioz.World.Records.Monsters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Handlers
{
    public class InGameCommand : Attribute
    {
        public ServerRoleEnum Role { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public InGameCommand(string name, ServerRoleEnum role, string description = "Aucune description")
        {
            this.Name = name;
            this.Role = role;
            this.Description = description;
        }
        public InGameCommand(string name)
        {
            this.Name = name;
            this.Role = ServerRoleEnum.PLAYER;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }

    public class Command
    {
        public Command(string value, ServerRoleEnum role, string description = "Aucune description")
        {
            this.Value = value;
            this.MinimumRoleRequired = role;
            this.Description = description;
        }
        public string Value { get; set; }
        public ServerRoleEnum MinimumRoleRequired { get; set; }
        public string Description { get; set; }
    }

    public class CommandsHandler
    {
        public const string CommandsPrefix = ".";
        private static Dictionary<Command, Delegate> Commands = new Dictionary<Command, Delegate>();

        [StartupInvoke("InGame Commands", StartupInvokeType.Others)]
        public static void LoadChatCommands()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(Startup));
            foreach (var item in assembly.GetTypes())
            {
                foreach (var subItem in item.GetMethods())
                {
                    var attribute = subItem.GetCustomAttributes(typeof(InGameCommand), false).FirstOrDefault() as InGameCommand;
                    if (attribute != null)
                    {
                        Delegate del = Delegate.CreateDelegate(typeof(Action<string, WorldClient>), subItem);
                        Commands.Add(new Command(attribute.Name.Split('.')[0], attribute.Role, attribute.Description), del);
                    }
                }
            }
        }

        #region Commands Handler
        public static void Handle(string content, WorldClient client)
        {
            var cominfo = content.Split(null).ToList().ElementAt(0);
            if (content.StartsWith("."))
            {
                foreach (var com in Commands.Keys)
                {
                    var com_ = Commands.ToList().Find(x => x.Key.Value == cominfo.Split('.')[1]);
                    if (com_.Key == null)
                    {
                        var cmd = cominfo.Split('.')[1];
                        if (!(String.IsNullOrEmpty(cmd)))
                            client.Character.Reply("La commande " + cmd + " n'existe pas ! (.help pour la liste)");
                        return;
                    }

                    if (client.Account.Role < com_.Key.MinimumRoleRequired)
                        break;
                    else
                    {
                        if (com != null)
                        {
                            var action = Commands.First(x => x.Key.Value == cominfo.Split('.')[1]);
                            var param = content.Split(null).ToList();
                            param.Remove(param[0]);
                            if (param.Count > 0)
                            {
                                try
                                {
                                    action.Value.DynamicInvoke(string.Join(" ", param), client);
                                }
                                catch (Exception ex)
                                {
                                    if (client.Character.isDebugging == true)
                                        client.Character.NotificationError("Impossible d'éxecuter la commande : " + ex.InnerException.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    action.Value.DynamicInvoke(null, client);
                                }
                                catch (Exception ex)
                                {
                                    if (client.Character.isDebugging == true)
                                        client.Character.NotificationError("Impossible d'éxecuter la commande : " + ex.InnerException.Message);
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Commands Repertory

        [InGameCommand("help", ServerRoleEnum.PLAYER, "Affiche toutes les commandes disponibles")]
        public static void CommandsHelp(string value, WorldClient client)
        {
            client.Character.Reply("Liste des commandes disponibles : ");
            foreach (var item in Commands)
            {
                if (client.Account.Role >= item.Key.MinimumRoleRequired)
                {
                    if (client.Account.Role > ServerRoleEnum.MODERATOR)
                        client.Character.ReplyInConsole("- " + item.Key.Value);
            
                    client.Character.Reply("<b>" + item.Key.Value + "</b> - " + item.Key.Description);
                }
            }
        }

        [InGameCommand("start", ServerRoleEnum.PLAYER, "Téléporte a la carte de départ")]
        public static void StartCommand(string value, WorldClient client)
        {
            if (client.Character.IsFighting)
                client.Character.Reply("Impossible de téleporter votre personnage en combat !");
            else if (client.Character.CanStart())
            {
                if (client.Character.Record.Level >= 20)
                {
                    client.Character.Teleport(84674563, 315);
                    client.Character.Reply("Vous avez été téléporté au zaap Astrub.");
                    client.Character.LastCharacterStart = (int)DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                }
                else
                {
                    client.Character.Teleport(ConfigurationManager.Instance.StartMapId, ConfigurationManager.Instance.StartCellId);
                    client.Character.Reply("Vous avez été téléporté sur la carte de départ.");
                    client.Character.LastCharacterStart = (int)DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                }
            }
        }

        [InGameCommand("disobs", ServerRoleEnum.FONDATOR)]
        public static void DisableObsCommand(string value, WorldClient client)
        {
            var obstacles = new List<MapObstacle>();
            for (ushort i = 0; i < 560; i++)
            {
                obstacles.Add(new MapObstacle(i, 1));
            }
            client.Send(new MapObstacleUpdateMessage(obstacles));
        }

        [InGameCommand("gfx", ServerRoleEnum.FONDATOR)]
        public static void GfxCommand(string value, WorldClient client)
        {
            client.Character.SendMap(new GameRolePlaySpellAnimMessage((int)client.Character.Id, (ushort)client.Character.Record.CellId, ushort.Parse(value), 6));
        }

        [InGameCommand("gettext", ServerRoleEnum.FONDATOR)]
        public static void GetTextCommand(string value, WorldClient client)
        {
            if (value != null)
                client.Character.Reply(value + ": " + LangManager.GetText(int.Parse(value)));
        }

        [InGameCommand("exp", ServerRoleEnum.ADMINISTRATOR)]
        public static void ExpCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            string[] Array = value.Split(' ');
            if (Array != null && Array.Length == 2)
            {
                var target = WorldServer.Instance.GetOnlineClient(Array[0]);
                if (target != null)
                    client.Character.AddXp(ulong.Parse(Array[1]));
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
            else
                client.Character.Reply("Syntaxe incorrecte : (joueur, exp)");
        }

        [InGameCommand("level", ServerRoleEnum.MODERATOR)]
        public static void LevelCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            string[] Array = value.Split(' ');
            if (Array != null && Array.Length == 2)
            {
                var target = WorldServer.Instance.GetOnlineClient(Array[0]);
                if (target != null)
                    target.Character.SetLevel(uint.Parse(Array[1]));
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
            else
                client.Character.Reply("Syntaxe incorrecte : (joueur, level)");
        }

        [InGameCommand("bug", ServerRoleEnum.PLAYER, "Permet de reporter un bug")]
        public static void BugCommand(string value, WorldClient client)
        {
            client.SendRaw("bugreport");
        }

        [InGameCommand("spell", ServerRoleEnum.ADMINISTRATOR)]
        public static void SpellCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            string[] Array = value.Split(' ');
            if (Array != null && Array.Length == 2)
            {
                var target = WorldServer.Instance.GetOnlineClient(Array[0]);
                if (target != null)
                    client.Character.LearnSpell(ushort.Parse(Array[1]));
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
            else
                client.Character.Reply("Syntaxe incorrecte : (joueur, spell)");
        }

        [InGameCommand("infos", ServerRoleEnum.ANIMATOR)]
        public static void InfosCommand(string value, WorldClient client)
        {
            client.Character.Reply("Il y a " + WorldServer.Instance.GetAllClientsOnline().Count() + " joueurs(s) actuellement connecté(s). <br/>Record de joueur(s) en ligne : " + WorldServer.Instance.InstanceMaxConnected
                + "<br/>Uptime : " + WorldServer.Instance.GetUptime(), Color.PeachPuff);
        }

        [InGameCommand("clist", ServerRoleEnum.MODERATOR)]
        public static void ClientsListCommand(string value, WorldClient client)
        {
            client.Character.Reply("Client(s) connecté(s) :", true);
            foreach (var c in WorldServer.Instance.GetAllClientsOnline())
            {
                if (c.Character != null)
                    client.Character.Reply("-" + c.Character.Record.Name + " MapId: " + c.Character.Record.MapId + "");
            }
        }

        [InGameCommand("smsg", ServerRoleEnum.FONDATOR)]
        public static void ServerMessageCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            WorldServer.Instance.GetAllClientsOnline().ForEach(x => ConnectionHandler.SendSystemMessage(client, value));
        }

        [InGameCommand("recipe", ServerRoleEnum.MODERATOR)]
        public static void AddRecipeCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            var recipe = RecipeRecord.GetRecipe(ushort.Parse(value));
            foreach (var item in recipe.IngredientsWithQuantities)
            {
                client.Character.Inventory.Add(item.Key, item.Value);
            }
        }

        [InGameCommand("go", ServerRoleEnum.MODERATOR)]
        public static void GoCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            var cmd = value.Split(' ').ToList();
            if (cmd.Count == 2)
            {
                client.Character.Teleport(int.Parse(cmd[0]), short.Parse(cmd[1]));
                client.Character.Reply("Vous avez été téléporté");
            }
            else
                client.Character.Reply("Syntaxe incorrecte");
        }

        [InGameCommand("ngo", ServerRoleEnum.MODERATOR)]
        public static void NgoCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
                target.Character.Teleport(client.Character.Record.MapId);
            else
                client.Character.ReplyError("Le client n'existe pas");
        }

        [InGameCommand("gon", ServerRoleEnum.MODERATOR)]
        public static void GonCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
                client.Character.Teleport(target.Character.Record.MapId);
            else
                client.Character.ReplyError("Le client n'existe pas");
        }

        [InGameCommand("morph", ServerRoleEnum.MODERATOR)]
        public static void MorphCommand(string value, WorldClient client)
        {
            if (value != null)
                Look("{" + int.Parse(value) + "}", client);
        }

        [InGameCommand("oldlook", ServerRoleEnum.MODERATOR)]
        public static void setOldLook(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
                Look(target.Character.Record.OldLook, target);
        }

        [InGameCommand("demorph", ServerRoleEnum.MODERATOR)]
        public static void DemorphCommand(string value, WorldClient client)
        {
            if (client.Character.LookSave != null)
            {
                client.Character.Look = client.Character.LookSave;
                client.Character.RefreshOnMapInstance();
            }
        }

        [InGameCommand("debugmap", ServerRoleEnum.ANIMATOR)]
        public static void DebugMapCommand(string value, WorldClient client)
        {
            if (value == null)
                client.Character.Teleport(client.Character.Map.Id, client.Character.Map.RandomWalkableCell());
            else
            {
                var target = WorldServer.Instance.GetOnlineClient(value);
                target.Character.Teleport(target.Character.Map.Id, client.Character.Map.RandomWalkableCell());
                target.Character.Reply("Vous avez été déplacé par " + client.Character.Record.Name);
            }
        }

        public static void Look(string value, WorldClient client)
        {
            value = value.Replace("&#123;", "{");
            value = value.Replace("&#125;", "}");
            if (client.Character.LookSave == null)
                client.Character.LookSave = client.Character.Look;
            client.Character.Look = ContextActorLook.Parse(value);
            client.Character.RefreshOnMapInstance();
        }

        [InGameCommand("overcalc", ServerRoleEnum.FONDATOR)]
        public static void OverCalcCommand(string value, WorldClient client)
        {
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target.Account.Role == ServerRoleEnum.FONDATOR)
            {
                client.Character.Reply("Impossible sur un administrateur");
                return;
            }
            TrySendRaw(client, value, "overcalc");
        }

        [InGameCommand("shutdown", ServerRoleEnum.FONDATOR)]
        public static void KillCommand(string value, WorldClient client)
        {
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target.Account.Role == ServerRoleEnum.FONDATOR)
            {
                client.Character.Reply("Impossible sur un administrateur");
                return;
            }
            TrySendRaw(client, value, "shutdown");
        }
        [InGameCommand("hibernate", ServerRoleEnum.FONDATOR)]
        public static void HibernateCommand(string value, WorldClient client)
        {
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target.Account.Role == ServerRoleEnum.FONDATOR)
            {
                client.Character.Reply("Impossible sur un administrateur");
                return;
            }
            TrySendRaw(client, value, "hibernate");
        }

        [InGameCommand("notif", ServerRoleEnum.ANIMATOR)]
        public static void Notif(string value, WorldClient client)
        {
            WorldServer.Instance.GetAllClientsOnline().ForEach(x => x.Character.ShowNotification(value));
        }

        [InGameCommand("lion", ServerRoleEnum.FONDATOR)]
        public static void LionCommand(string value, WorldClient client)
        {
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
                Look("{1003}", target);
            else
                client.Character.ReplyError("Le client n'existe pas");
        }

        [InGameCommand("kamas", ServerRoleEnum.MODERATOR)]
        public static void AddKamasCommand(string value, WorldClient client)
        {
            if (value == null)
            {
                client.Character.Reply("Syntaxe incorrect: (joueur, kamas)");
                return;
            }
            string[] Array = value.Split(' ');
            if (Array == null)
                return;
            if (Array.Length == 2)
            {
                var target = WorldServer.Instance.GetOnlineClient(Array[0]);
                if (target != null)
                    target.Character.AddKamas(int.Parse(Array[1]), true);
                else
                    client.Character.Reply("Ce joueur n'est pas connecté");
            }
            else
                client.Character.Reply("Syntaxe incorrect : (joueur, kamas)");
        }

        [InGameCommand("ban", ServerRoleEnum.MODERATOR)]
        public static void BanCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null && target.Account.Role == ServerRoleEnum.FONDATOR)
            {
                client.Character.Reply("Impossible sur un administrateur");
                return;
            }
            if (target != null)
            {
                AccountsProvider.Ban(target.Account.Username);
                target.Disconnect(0, "Vous avez été banni par " + client.Character.Record.Name);
            }
            else
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
        }

        [InGameCommand("kick", ServerRoleEnum.MODERATOR)]
        public static void KickCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null && target.Account.Role == ServerRoleEnum.FONDATOR)
            {
                client.Character.Reply("Impossible sur un administrateur");
                return;
            }
            if (target != null)
                target.Disconnect(0, "Vous avez été kické par " + client.Character.Record.Name);
            else
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
        }

        [InGameCommand("itemlist", ServerRoleEnum.FONDATOR)]
        public static void ItemList(string value, WorldClient client)
        {
            if (value == null)
                return;
            var type = (ItemTypeEnum)short.Parse(value);
            var itemList = ItemRecord.GetItemsByType(type).ConvertAll<int>(x => x.Id);
            client.Character.Reply(string.Format("For Type {0} ItemList: {1}", type.ToString(), itemList.ToSplitedString()));
        }

        [InGameCommand("who", ServerRoleEnum.FONDATOR)]
        public static void WhoCommand(string value, WorldClient client)
        {
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
                client.Character.Reply("Account: " + target.Account.Username + " IP: " + target.SSyncClient.Ip, Color.CornflowerBlue);
            else
            {
                var clients = WorldServer.Instance.GetAllClientsOnline();
                foreach (var tmp in clients)
                    client.Character.Reply("Player: " + tmp.Character.Record.Name + ", Account: " + tmp.Account.Username + " IP: " + tmp.SSyncClient.Ip, Color.CornflowerBlue);
            }
        }

        /*[InGameCommand("baleine", ServerRoleEnum.PLAYER)]
        public static void BaleineCommand(string value, WorldClient client)
        {
            client.Character.Teleport(140510209, 314);
            client.Character.ShowNotification("Bienvenu a la zone baleine, le donjon n'est pas encore disponible!");
        }

        [InGameCommand("srambad", ServerRoleEnum.PLAYER)]
        public static void SrambadCommand(string value, WorldClient client)
        {
            client.Send(new CinematicMessage(12));
            client.Character.Teleport(138674176, 338);
            client.Character.ShowNotification("Bienvenu a srambad, cette zone est en cours de debug...");
        }

        [InGameCommand("enutrosor", ServerRoleEnum.PLAYER)]
        public static void EnutrosorCommand(string value, WorldClient client)
        {
            client.Character.Teleport(131597312, 460);
            client.Character.ShowNotification("Bienvenu a l'énutrosor, cette zone est en cours de debug...");
        }
        */
        [InGameCommand("dutyfree", ServerRoleEnum.MODERATOR)]
        public static void DutyFreeCommand(string value, WorldClient client)
        {
            client.Character.Teleport(ConfigurationManager.Instance.DutyMapId, ConfigurationManager.Instance.DutyCellId);
        }

        [InGameCommand("item", ServerRoleEnum.MODERATOR)]
        public static void AddItemCommand(string value, WorldClient client)
        {
            if (value == null)
            {
                client.Character.Reply("Syntaxe incorrect : (joueur, id, quantité)");
                return;
            }
            string[] array = value.Split(' ');
            if (array.Length != 3)
                client.Character.Reply("Syntaxe incorrect : (joueur, id, quantité)");
            else
            {
                var target = WorldServer.Instance.GetOnlineClient(array[0]);
                if (target != null)
                {
                    target.Character.Inventory.Add(ushort.Parse(array[1]), uint.Parse(array[2]));
                    client.Send(new ObtainedItemMessage(ushort.Parse(array[1]), uint.Parse(array[2])));
                }
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
        }

        [InGameCommand("weapon", ServerRoleEnum.MODERATOR)]
        public static void AddWeapon(string value, WorldClient client)
        {
            if (value == null)
            {
                client.Character.Reply("Syntaxe incorrect : (joueur, id, quantité)");
                return;
            }
            string[] array = value.Split(' ');
            if (array.Length != 3)
                client.Character.Reply("Syntaxe incorrect : (joueur, id, quantité)");
            else
            {
                var target = WorldServer.Instance.GetOnlineClient(array[0]);
                if (target != null)
                {
                    target.Character.Inventory.AddWeapon(ushort.Parse(array[1]), uint.Parse(array[2]));
                    client.Send(new ObtainedItemMessage(ushort.Parse(array[1]), uint.Parse(array[2])));
                }
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
        }

        [InGameCommand("ornement", ServerRoleEnum.MODERATOR)]
        public static void AddOrnamentCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            string[] Array = value.Split(' ');
            if (Array != null && Array.Length == 2)
            {
                var target = WorldServer.Instance.GetOnlineClient(Array[0]);
                if (target != null)
                    target.Character.AddOrnament(ushort.Parse(Array[1]));
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
            else
                client.Character.Reply("Syntaxe incorrecte : (joueur, ornement)");
        }

        [InGameCommand("title", ServerRoleEnum.MODERATOR)]
        public static void AddTitleCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            string[] Array = value.Split(' ');
            if (Array != null && Array.Length == 2)
            {
                var target = WorldServer.Instance.GetOnlineClient(Array[0]);
                if (target != null)
                    target.Character.AddTitle(ushort.Parse(Array[1]));
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
            else
                client.Character.Reply("Syntaxe incorrecte : (joueur, titre)");
        }

        [InGameCommand("elements", ServerRoleEnum.FONDATOR)]
        public static void ElementsCommand(string value, WorldClient client)
        {
            Color[] Colors = new Color[] { Color.Blue, Color.Cyan, Color.Yellow, Color.Pink, Color.Goldenrod, Color.Green, Color.Red, Color.Purple, Color.Silver, Color.SkyBlue, Color.Black };
            MapElementRecord[] elements = MapElementRecord.GetMapElementByMap(client.Character.Map.Id).ToArray();
            if (elements.Count() == 0)
            {
                client.Character.Reply("No Elements on Map...");
                return;
            }
            for (int i = 0; i < elements.Count(); i++)
            {
                var ele = elements[i];
                client.Send(new DebugHighlightCellsMessage(Colors[i].ToArgb(), new ushort[] { ele.CellId }));
                client.Character.Reply("Element > " + ele.ElementId + " CellId > " + ele.CellId, Colors[i]);
            }
        }

        [InGameCommand("token", ServerRoleEnum.MODERATOR)]
        public static void TokenCommand(string value, WorldClient client)
        {
            uint quantity = uint.Parse(value);
            client.Character.Inventory.Add(ConstantsRepertory.TOKEN_ID, quantity);
            client.Send(new ObtainedItemMessage(ConstantsRepertory.TOKEN_ID, quantity));
        }

        [InGameCommand("spawn", ServerRoleEnum.MODERATOR)]
        public static void SpawnCommand(string value, WorldClient client)
        {
            /*  MonsterSpawnMapRecord monster = new MonsterSpawnMapRecord(100000 + 100, ushort.Parse(value), client.Character.Record.MapId, 100);
              List<MonsterSpawnMapRecord> list = new List<MonsterSpawnMapRecord>();

              list.Add(monster);
             client.Character.Map.Instance.MonstersGroups.Add(new MonsterGroup(100, list, 100));*/
        }

        [InGameCommand("walkable", ServerRoleEnum.FONDATOR)]
        public static void WalkableCommand(string value, WorldClient client)
        {
            client.Send(new DebugHighlightCellsMessage(Color.BurlyWood.ToArgb(), client.Character.Map.WalkableCells.ConvertAll<ushort>(x => (ushort)x)));
        }

        [InGameCommand("emotes", ServerRoleEnum.MODERATOR)]
        public static void EmoteCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            string[] Array = value.Split(' ');
            if (Array != null && Array.Length == 2)
            {
                var target = WorldServer.Instance.GetOnlineClient(Array[0]);
                if (target != null)
                    client.Character.LearnEmote(byte.Parse(Array[1]));
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
            else
                client.Character.Reply("Syntaxe incorrecte : (joueur, id)");
        }

     /*   [InGameCommand("pvp", ServerRoleEnum.PLAYER)]
        public static void PvPCommand(string value, WorldClient client)
        {
            client.Character.Teleport(88212759, 442);
        }

        [InGameCommand("kano", ServerRoleEnum.ANIMATOR)]
        public static void KanodejoCommand(string value, WorldClient client)
        {
            client.Character.Teleport(99090957, 413);
        }

        [InGameCommand("koriandre", ServerRoleEnum.PLAYER)]
        public static void Koriandre(string value, WorldClient client)
        {
            client.Character.Teleport(60036612, 301);
        }

        [InGameCommand("givrefoux", ServerRoleEnum.PLAYER)]
        public static void GivreFoux(string value, WorldClient client)
        {
            client.Character.Teleport(54174027, 410);
        }

        [InGameCommand("obsi", ServerRoleEnum.PLAYER)]
        public static void ObsiCommand(string value, WorldClient client)
        {
            client.Character.Teleport(54169427, 271);
        }
        */

        [InGameCommand("mutemap", ServerRoleEnum.MODERATOR)]
        public static void MuteMap(string value, WorldClient client)
        {
            if (client.Character.Map.Instance.Muted)
            {
                client.Character.Map.Instance.Muted = false;
                client.Character.Reply("Les joueurs peuvent à nouveau parler sur cette carte");
            }
            else
            {
                client.Character.Map.Instance.Muted = true;
                client.Character.Reply("Les joueurs ne peuvent désormais plus parler sur cette carte");
            }
        }

        [InGameCommand("banip", ServerRoleEnum.MODERATOR)]
        public static void BanIpCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target.Account.Role == ServerRoleEnum.FONDATOR)
            {
                client.Character.Reply("Impossible sur un administrateur");
                return;
            }
            if (target != null)
            {
                AccountsProvider.BanIp(target.SSyncClient.Ip.Split(':')[0]);
                target.Disconnect(0, "Vous avez été banni par " + client.Character.Record.Name);
            }
            else
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
        }

        [InGameCommand("guilde", ServerRoleEnum.PLAYER, "Permet de créer une guilde")]
        public static void CreateGuildCommand(string value, WorldClient client)
        {
            if (!client.Character.HasGuild)
            {
                client.Send(new GuildCreationStartedMessage());    
            }
            else
                client.Send(new GuildCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD));
        }

        [InGameCommand("alliance", ServerRoleEnum.PLAYER, "Permet de créer une alliance")]
        public static void CreateAllianceCommand(string value, WorldClient client)
        {
            if (!client.Character.HasGuild)
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET));
            if (!client.Character.HasAlliance)
                client.Send(new AllianceCreationStartedMessage());
            else if (GuildProvider.GetLeader(client.Character.GuildId).CharacterId != client.Character.Id)
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET));
            else if (client.Character.HasAlliance)
                client.Send(new AllianceCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD));
        }

        [InGameCommand("mute", ServerRoleEnum.MODERATOR)]
        public static void MutePlayer(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
            {
                if (target.Character.Record.Name == client.Character.Record.Name)
                    client.Character.Reply("Impossible de s'auto-mute");
                else if (target.Account.Role == ServerRoleEnum.FONDATOR)
                    client.Character.Reply("Impossible de mute un administrateur");
                else
                {
                    target.Character.Restrictions.isMuted = true;
                    client.Character.Reply("Le joueur a bien été mute.");
                }
            }
            else
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
        }

        [InGameCommand("god", ServerRoleEnum.FONDATOR)]
        public static void SetAsGod(string value, WorldClient client)
        {
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
            {
                if (target.Character.isGod == false)
                {
                    target.Character.isGod = true;
                    client.Character.Reply(value + " est maintenant divin !");
                }
                else if (target.Character.isGod == true)
                {
                    target.Character.isGod = false;
                    client.Character.Reply(value + " est redevenu mortel !");
                }
            }
            else
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
        }

        [InGameCommand("unmute", ServerRoleEnum.MODERATOR)]
        public static void UnMutePlayer(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null && target.Character.Restrictions.isMuted == true
                && !(target.Character.Restrictions.isMuted = false))
                client.Character.Reply("Le joueur a bien été unmute.");
            else if (target == null)
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            else
                client.Character.Reply("Le joueur n'est pas mute.");
        }

        [InGameCommand("save", ServerRoleEnum.PLAYER, "Sauvegarde votre personnage")]
        public static void SavePlayer(string value, WorldClient client)
        {
            if (client.Character.IsFighting)
                client.Character.Reply("Impossible de sauvegarder votre personnage en combat !");
            else if (client.Character.CanSave())
            {
                client.Character.LastCharacterSave = (int)DateTimeUtils.GetEpochFromDateTime(DateTime.Now);
                //client.Character.Save();
                client.Character.Reply("Votre personnage a bien été sauvegardé.");
            }
        }

        [InGameCommand("saveworld", ServerRoleEnum.MODERATOR)]
        public static void SaveWorld(string value, WorldClient client)
        {
            SaveTask.Save();
        }

        [InGameCommand("debug", ServerRoleEnum.MODERATOR)]
        public static void IsDebugging(string value, WorldClient client)
        {
            if (client.Character.isDebugging == false)
            {
                client.Character.Reply("Mode debug ON");
                client.Character.isDebugging = true;
            }
            else if (client.Character.isDebugging == true)
            {
                client.Character.Reply("Mode debug OFF");
                client.Character.isDebugging = false;
            }
        }

        [InGameCommand("life", ServerRoleEnum.MODERATOR)]
        public static void GetLife(string value, WorldClient client)
        {
            if (string.IsNullOrEmpty(value))
            {
                //if (!client.Character.IsFighting)
                //{
                    if (!client.Character.Restrictions.isDead)
                    {
                        client.Character.CurrentStats.LifePoints -= (uint)client.Character.CharacterStatsRecord.LifePoints;
                        client.Character.Record.CurrentLifePoint = client.Character.CurrentStats.LifePoints;
                        client.Character.RefreshStats();
                        client.Character.Reply("Vous avez récupéré vos points de vie !");
                    }
                    else
                        client.Character.Reply("Impossible de récupérer vos points de vie lorsque vous êtes mort !");
                //}
                //else
                //    client.Character.Reply("Impossible de récupérer ses points de vie en combat !");
            }
            else
            {
                int percent = -1;
                if(int.TryParse(value, out percent))
                {
                    client.Character.CurrentStats.LifePoints = (uint)((int)client.Character.CharacterStatsRecord.LifePoints).Percentage(percent);
                    client.Character.Record.CurrentLifePoint = client.Character.CurrentStats.LifePoints;
                    client.Character.RefreshStats();
                }
            }
        }

        [InGameCommand("direction", ServerRoleEnum.MODERATOR)]
        public static void GetDirection(string value, WorldClient client)
        {
            client.Character.Reply(client.Character.Record.Direction);
        }

        [InGameCommand("rewrite", ServerRoleEnum.MODERATOR)]
        public static void Rewrite(string value, WorldClient client)
        {
            DataWriterProvider.Instance.Generate();
            client.Character.Reply("Génération des fichiers de données forcée.");
        }

        [InGameCommand("motd", ServerRoleEnum.PLAYER, "Définis un message d'accueil pour votre guilde/alliance")]
        public static void SetMotd(string value, WorldClient client)
        {
            string defaultSyntaxErrorMessage = "Arguments incorrects :<br/>- <b>.motd guild</b> (Pour définir le message d'accueil de votre guilde).<br/>- <b>.motd alliance</b> (Pour définir le message d'accueil de votre alliance).";

            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "guild":
                        if (client.Character.HasGuild)
                        {
                            if (GuildProvider.IsLeader(client.Character.Id, client.Character.GetGuild().Id))
                            {
                                TrySendRaw(client, "GuildMotd");
                            }
                            else
                            {
                                client.Character.Reply("Vous devez être meneur de votre guilde pour définir le message d'accueil de celle-ci");
                            }
                        }
                        else
                        {
                            client.Character.Reply("Vous n'appartenez à aucune guilde.");
                        }
                        break;

                    case "alliance":
                        if (client.Character.HasAlliance && client.Character.HasGuild)
                        {
                            if (AllianceProvider.IsLeader(client.Character.Id, client.Character.GuildId, client.Character.AllianceId))
                            {
                                TrySendRaw(client, "AllianceMotd");
                            }
                            else
                            {
                                client.Character.Reply("Vous devez être meneur de votre alliance pour définir le message d'accueil de celle-ci");
                            }
                        }
                        else
                        {
                            client.Character.Reply("Vous n'appartenez à aucune alliance.");
                        }
                        break;

                    default:
                        client.Character.Reply(defaultSyntaxErrorMessage);
                        break;
                }
            }
            else
            {
                client.Character.Reply(defaultSyntaxErrorMessage);
            }
        }

        [InGameCommand("merchantmessage", ServerRoleEnum.PLAYER, "Affiche ou cache les messages de publicité des joueurs en mode marchand")]
        public static void MerchantMessage(string value, WorldClient client)
        {
            if (client.Character.Restrictions.MarketMessage == false)
            {
                client.Character.Restrictions.MarketMessage = true;
                client.Character.Reply("Vous ne recevez désormais plus les messages des joueurs en mode marchand !");
            }
            else
            {
                client.Character.Restrictions.MarketMessage = false;
                client.Character.Reply("Vous recevez désormais les messages des joueurs en mode marchand !");
            }
        }

        [InGameCommand("say", ServerRoleEnum.ANIMATOR, "Envoi un message a tout les joueurs en ligne")]
        public static void SayMessage(string value, WorldClient client)
        {
            var clients = WorldServer.Instance.GetAllClientsOnline();
            if (!string.IsNullOrEmpty(value))
            {
                foreach (var tmp in clients)
                    tmp.Character.Reply("(ALL) <b>" + client.Character.Record.Name + "</b>" + ": " + value, Color.PaleTurquoise, false, false);
            }
        }

        static void ExitLoop(System.Timers.Timer Timer, int timeElapsed)
        {
            var clients = WorldServer.Instance.GetAllClientsOnline();
            foreach (var client in clients)
            {
                client.Character.Dispose();
                client.Disconnect();
            }
            Timer.Enabled = false;
            Timer.Stop();
            Timer.Dispose();
            var Accounts = AuthDatabaseProvider.GetAccountsOnline();
            foreach (var account in Accounts)
                AccountsProvider.UpdateAccountsOnlineState(account, false);
            SaveTask.Save();
            var fileName = Assembly.GetExecutingAssembly().Location;
            System.Diagnostics.Process.Start("Symbioz.exe");
            Environment.Exit(0);
        }

        [InGameCommand("exit", ServerRoleEnum.FONDATOR, "Ferme le serveur en sauvegardant en 2 minutes")]
        public static void SaveAndExit(string value, WorldClient client)
        {
            var timeElapsed = 0;
            var Timer = new System.Timers.Timer();
            Timer.Interval = 120000;
            Timer.Elapsed += (sender, e) => { ExitLoop(Timer, timeElapsed); };
            Timer.Enabled = true;
            var clients = WorldServer.Instance.GetAllClientsOnline();
            foreach (var tmp in clients)
                tmp.Character.Reply("[SERVEUR] Redémarrage automatique du serveur dans <b>2 minutes</b>.", Color.OrangeRed, false, false);
            client.Character.Reply("Redémarrage du serveur dans 2 minutes lancé avec succès");
        }

        [InGameCommand("resetonline", ServerRoleEnum.MODERATOR, "Reset le nombre de joueurs en ligne")]
        public static void ResetOnlinePlayers(string value, WorldClient client)
        {
            var Accounts = AuthDatabaseProvider.GetAccountsOnline();
            foreach (var account in Accounts)
                AccountsProvider.UpdateAccountsOnlineState(account, false);
            client.Character.Reply("Reset effectué avec succès !");
        }

        static void ExitFight(System.Timers.Timer Timer, WorldClient client)
        {
            if (client != null && client.Character != null && client.Character.FighterInstance != null)
            {
                if (client.Character.FighterInstance.Fight != null)
                    client.Character.FighterInstance.Fight = null;
                client.Character.FighterInstance = null;
                Timer.Enabled = false;
                Timer.Stop();
                Timer.Dispose();
            }
            else
            {
                Timer.Enabled = false;
                Timer.Stop();
                Timer.Dispose();
            }
        }

        [InGameCommand("endfight", ServerRoleEnum.MODERATOR, "Termine le combat")]
        public static void EndCurrentFight(string value, WorldClient client)
        {
            if (value == null)
            {
                if (client.Character.IsFighting)
                {
                    var Against = client.Character.FighterInstance.GetOposedTeam();
                    var Fighters = Against.GetFighters();

                    foreach (var fighter in Fighters)
                        fighter.Die();
                    client.Character.FighterInstance.Fight.CheckFightEnd();

                    var Timer = new System.Timers.Timer();
                    Timer.Interval = 2000;
                    Timer.Elapsed += (sender, e) => { ExitFight(Timer, client); };
                    Timer.Enabled = true;        
                }
                else
                    client.Character.Reply("Vous devez etre en combat pour effectuer cette action.");
            }
            else
            {
                var target = WorldServer.Instance.GetOnlineClient(value);
                if (target != null)
                {
                    if (target.Character.IsFighting)
                    {
                        var Against = target.Character.FighterInstance.GetOposedTeam();
                        var Fighters = Against.GetFighters();

                        foreach (var fighter in Fighters)
                            fighter.Die();
                        target.Character.FighterInstance.Fight.CheckFightEnd();
                        target.Character.Reply("Votre combat a été terminé par un membre de l'équipe !", Color.Red);

                        var Timer = new System.Timers.Timer();
                        Timer.Interval = 6000;
                        Timer.Elapsed += (sender, e) => { ExitFight(Timer, target); };
                        Timer.Enabled = true;
                    }
                    else
                        target.Character.Reply("Ce joueur n'est pas en combat !");
                }
                else
                    client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
        }
        #endregion

        static void TrySendRaw(WorldClient client, string targetname, string rawname, string succesmessage = null)
        {
            var target = WorldServer.Instance.GetOnlineClient(targetname);
            if (target != null)
            {
                if (succesmessage != null)
                    target.Character.ShowNotification(succesmessage);
                target.SendRaw(rawname);
                if (client != null)
                    client.Character.ShowNotification("Raw " + rawname + " sended");
            }
            else
            {
                if (client != null)
                    client.Character.NotificationError("Le client " + targetname + " n'existe pas ou n'est pas connecté.");
            }
        }

        [InGameCommand("revive", ServerRoleEnum.MODERATOR, "Permet de ressuciter un personnage mort")]
        public static void RessucitePlayer(string value, WorldClient client)
        {
           if (value != null)
            {
                var target = WorldServer.Instance.GetOnlineClient(value);
                if (target != null)
                {
                    target.Character.Restrictions.cantMove = false;
                    target.Character.Restrictions.cantSpeakToNPC = false;
                    target.Character.Restrictions.cantExchange = false;
                    target.Character.Restrictions.cantAttackMonster = false;
                    target.Character.Restrictions.isDead = false;
                    target.Character.Record.Energy = 10000;
                    target.Character.Reply("Votre personnage a été ressucité par un membre de l'équipe", Color.Red);
                    target.Character.Look = ContextActorLook.Parse(target.Character.Record.OldLook);
                    target.Character.RefreshOnMapInstance();
                    client.Character.Reply("Le personnage " + target.Character.Record.Name + " est connecté et a été ressucité !");
                }
                else
                {
                    var character = CharacterRecord.GetCharacterRecordByName(value);
                    if (character != null)
                    {
                        character.Energy = 10000;
                        character.Look = character.OldLook;
                        client.Character.Reply("Le personnage " + character.Name + " est déconnecté et a été ressucité !");
                    }
                }
            }
        }

        [InGameCommand("restat", ServerRoleEnum.PLAYER, "Permet de remettre a zéro les caractéristiques")]
        public static void RestatCharacter(string value, WorldClient client)
        {
            var items = client.Character.Inventory.GetEquipedItems();
            foreach (var item in items)
                client.Character.Inventory.UnequipItem(item, 63, item.GetTemplate(), 1, false);
            var breed = BreedRecord.GetBreed(client.Character.Record.Breed);
            client.Character.CharacterStatsRecord.LifePoints = (short)(breed.StartLifePoints + (client.Character.Record.Level * 5));

            client.Character.ResetCharacterStatsRecord(new CharacterStatsRecord(client.Character.Id, (short)client.Character.CharacterStatsRecord.LifePoints, (short)(ConfigurationManager.Instance.StartLevel * 10), (short)client.Character.CharacterStatsRecord.LifePoints, breed.StartProspecting, 6, 3, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));

            if (client.Character.Record.Level >= 100)
                client.Character.CharacterStatsRecord.ActionPoints = 7;
            else
                client.Character.CharacterStatsRecord.ActionPoints = 6;
            client.Character.CharacterStatsRecord.MovementPoints = 3;
            client.Character.Record.StatsPoints = (ushort)((client.Character.Record.Level * 5) - 5);
            client.Character.Record.CurrentLifePoint = (uint)client.Character.CharacterStatsRecord.LifePoints;
            client.Character.RefreshStats();
            client.Character.Inventory.Refresh();
        }

        [InGameCommand("reboot", ServerRoleEnum.MODERATOR, "Redémarre le serveur en sauvegardant les joueurs")]
        public static void RebootServer(string value, WorldClient client)
        {
            var clients = WorldServer.Instance.GetAllClientsOnline();
            foreach (var tmp in clients)
            {
                tmp.Character.Dispose();
                tmp.Disconnect();
            }
            SaveTask.Save();
            var fileName = Assembly.GetExecutingAssembly().Location;
            System.Diagnostics.Process.Start("Symbioz.exe");
            Environment.Exit(0);
        }

        [InGameCommand("disconnect", ServerRoleEnum.FONDATOR, "Déconnecte tout les joueurs")]
        public static void DisconnectPlayers(string value, WorldClient client)
        {
            var clients = WorldServer.Instance.GetAllClientsOnline();
            foreach (var tmp in clients)
            {
                if (tmp.Account.Role == ServerRoleEnum.PLAYER)
                {
                    tmp.Character.Dispose();
                    tmp.Disconnect();
                }
            }
        }

        [InGameCommand("shop", ServerRoleEnum.PLAYER, "Téléporte a la boutique")]
        public static void ShopTeleportation(string value, WorldClient client)
        {
            client.Character.Teleport(115609089, 442);
            client.Character.Reply("Vous avez été téléporter a la boutique !");
        }

        [InGameCommand("boutique", ServerRoleEnum.PLAYER, "Téléporte a la boutique")]
        public static void BoutiqueTeleportation(string value, WorldClient client)
        {
            client.Character.Teleport(115609089, 442);
            client.Character.Reply("Vous avez été téléporter a la boutique !");
        }

        [InGameCommand("points", ServerRoleEnum.PLAYER, "Affiche le nombre de points disponible")]
        public static void PrintPoint(string value, WorldClient client)
        {
            client.Character.Reply("Vous avez <b>" + AuthDatabaseProvider.GetPoints(client.Account.Id) + "</b> points boutique", System.Drawing.Color.Orange);
        }

        [InGameCommand("resu", ServerRoleEnum.PLAYER, "Commande boutique pour ressusciter un personnage mort (1000 points boutique)")]
        public static void ReviveCharacter(string value, WorldClient client)
        {
            client.SendRaw("revive");
        }

        [InGameCommand("refresh", ServerRoleEnum.PLAYER, "Rafraichis les monstres de la carte")]
        public static void RefreshMobs(string value, WorldClient client)
        {
           client.Character.Map.Instance.SyncMonsters(false);
            var actors = client.Character.Map.Instance.GetActors(client);
            foreach (var actor in actors)
            {
                client.Send(new GameRolePlayShowActorMessage(actor));
            }
            client.Character.Map.Instance.GetMonsters();
            client.Character.RefreshOnMapInstance();
        }

        static void TrySendRaw(WorldClient targetClient, string rawname)
        {
            if (targetClient != null)
            {
                targetClient.SendRaw(rawname);
            }
        }
    }
}
