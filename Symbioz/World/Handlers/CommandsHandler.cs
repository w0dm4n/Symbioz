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
        public InGameCommand(string name, ServerRoleEnum role)
        {
            this.Name = name;
            this.Role = role;
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
        public Command(string value, ServerRoleEnum role)
        {
            this.Value = value;
            this.MinimumRoleRequired = role;
        }
        public string Value { get; set; }
        public ServerRoleEnum MinimumRoleRequired { get; set; }
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
                        Commands.Add(new Command(attribute.Name.Split('.')[0], attribute.Role), del);
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
                            client.Character.Reply("La commande " + cmd + " n'existe pas !");
                        return;
                    }

                    if (client.Account.Role < com_.Key.MinimumRoleRequired)
                    {
                        client.Character.Reply("Vous ne disposez pas des droits nécessaires pour exécuter cette commande.");
                        break;
                    }
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

        [InGameCommand("help", ServerRoleEnum.PLAYER)]
        public static void CommandsHelp(string value, WorldClient client)
        {
            client.Character.Reply("Commandes :");
            foreach (var item in Commands)
            {
                if (client.Account.Role >= item.Key.MinimumRoleRequired)
                    if (client.Account.Role > ServerRoleEnum.MODERATOR)
                    {
                        client.Character.ReplyInConsole("- " + item.Key.Value);
                    }
                client.Character.Reply("- " + item.Key.Value);
            }
        }

        [InGameCommand("start", ServerRoleEnum.PLAYER)]
        public static void StartCommand(string value, WorldClient client)
        {

            client.Character.Teleport(ConfigurationManager.Instance.StartMapId, ConfigurationManager.Instance.StartCellId);
            client.Character.Reply("Vous avez été téléporté sur la carte de départ.");
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
            client.Character.Reply(value + ": " + LangManager.GetText(int.Parse(value)));
        }

        [InGameCommand("exp", ServerRoleEnum.ADMINISTRATOR)]
        public static void ExpCommand(string value, WorldClient client)
        {
            client.Character.AddXp(ulong.Parse(value));
        }

        [InGameCommand("level", ServerRoleEnum.MODERATOR)]
        public static void LevelCommand(string value, WorldClient client)
        {
            uint level = uint.Parse(value);
            client.Character.SetLevel(level);

        }

        [InGameCommand("bug", ServerRoleEnum.PLAYER)]
        public static void BugCommand(string value, WorldClient client)
        {
            client.SendRaw("bugreport");
        }

        [InGameCommand("event", ServerRoleEnum.PLAYER)]
        public static void EventCommad(string value, WorldClient client)
        {
            client.Character.Teleport(144443396, 393);
            client.Character.ShowNotification("Vous avez été téléporté sur la carte \"Event\".");
        }

        [InGameCommand("spell", ServerRoleEnum.ADMINISTRATOR)]
        public static void SpellCommand(string value, WorldClient client)
        {
            client.Character.LearnSpell(ushort.Parse(value));
        }

        [InGameCommand("infos", ServerRoleEnum.PLAYER)]
        public static void InfosCommand(string value, WorldClient client)
        {
            client.Character.Reply("Il y a " + WorldServer.Instance.WorldClients.Count() + " client(s) actuellement connecté(s). <br/>Record : " + WorldServer.Instance.InstanceMaxConnected);
        }

        [InGameCommand("clist", ServerRoleEnum.ADMINISTRATOR)]
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
            WorldServer.Instance.GetAllClientsOnline().ForEach(x => ConnectionHandler.SendSystemMessage(client, value));
        }

        [InGameCommand("recipe", ServerRoleEnum.MODERATOR)]
        public static void AddRecipeCommand(string value, WorldClient client)
        {
            var recipe = RecipeRecord.GetRecipe(ushort.Parse(value));
            foreach (var item in recipe.IngredientsWithQuantities)
            {
                client.Character.Inventory.Add(item.Key, item.Value);
            }
        }

        [InGameCommand("go", ServerRoleEnum.MODERATOR)]
        public static void GoCommand(string value, WorldClient client)
        {
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
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
                target.Character.Teleport(client.Character.Record.MapId);
            else
                client.Character.ReplyError("Le client n'existe pas");
        }

        [InGameCommand("gon", ServerRoleEnum.MODERATOR)]
        public static void GonCommand(string value, WorldClient client)
        {
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
                client.Character.Teleport(target.Character.Record.MapId);
            else
                client.Character.ReplyError("Le client n'existe pas");
        }

        [InGameCommand("morph", ServerRoleEnum.MODERATOR)]
        public static void MorphCommand(string value, WorldClient client)
        {
            Look("{" + int.Parse(value) + "}", client);
        }

        [InGameCommand("debugmap", ServerRoleEnum.ANIMATOR)]
        public static void DebugMapCommand(string value, WorldClient client)
        {
            if (value == null)
            {
                client.Character.Teleport(client.Character.Map.Id, client.Character.Map.RandomWalkableCell());
            }
            else
            {
                var target = WorldServer.Instance.GetOnlineClient(value);
                target.Character.Teleport(target.Character.Map.Id, client.Character.Map.RandomWalkableCell());
                target.Character.Reply("Vous avez été déplacé par " + client.Character.Record.Name);
            }
        }

        [InGameCommand("look", ServerRoleEnum.ADMINISTRATOR)]
        public static void Look(string value, WorldClient client)
        {
            value = value.Replace("&#123;", "{");
            value = value.Replace("&#125;", "}");
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
            if (target == null)
                return;
            if (target.Account.Role == ServerRoleEnum.FONDATOR)
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
            {
                client.Character.Reply("Le client n'existe pas");
            }
        }

        [InGameCommand("kick", ServerRoleEnum.MODERATOR)]
        public static void KickCommand(string value, WorldClient client)
        {
            if (value == null)
                return;
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target == null)
                return;
            if (target.Account.Role == ServerRoleEnum.FONDATOR)
            {
                client.Character.Reply("Impossible sur un administrateur");
                return;
            }
            if (target != null)
            {
                target.Disconnect(0, "Vous avez été kické par " + client.Character.Record.Name);
            }
            else
            {
                client.Character.Reply("Le client n'existe pas");
            }
        }

        [InGameCommand("itemlist", ServerRoleEnum.FONDATOR)]
        public static void ItemList(string value, WorldClient client)
        {
            var type = (ItemTypeEnum)short.Parse(value);
            var itemList = ItemRecord.GetItemsByType(type).ConvertAll<int>(x => x.Id);
            client.Character.Reply(string.Format("For Type {0} ItemList: {1}", type.ToString(), itemList.ToSplitedString()));
        }

        [InGameCommand("who", ServerRoleEnum.FONDATOR)]
        public static void WhoCommand(string value, WorldClient client)
        {
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null)
            {
                client.Character.Reply("Account: " + target.Account.Username + " IP: " + target.SSyncClient.Ip, Color.CornflowerBlue);
            }
            else
            {
                client.Character.Reply("le joueur n'existe pas ou n'est pas connecté");
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
                    client.Character.Reply("Le joueur n'est pas connecté !");
            }
        }

        [InGameCommand("weapon", ServerRoleEnum.MODERATOR)]
        public static void AddWeapon(string value, WorldClient client)
        {
            client.Character.Inventory.AddWeapon(ushort.Parse(value), 1);
            client.Send(new ObtainedItemMessage(ushort.Parse(value), 1));
        }

        [InGameCommand("ornament", ServerRoleEnum.MODERATOR)]
        public static void AddOrnamentCommand(string value, WorldClient client)
        {
            client.Character.AddOrnament(ushort.Parse(value));
        }

        [InGameCommand("title", ServerRoleEnum.MODERATOR)]
        public static void AddTitleCommand(string value, WorldClient client)
        {
            client.Character.AddTitle(ushort.Parse(value));
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
            client.Character.LearnEmote(byte.Parse(value));
        }

        [InGameCommand("pvp", ServerRoleEnum.PLAYER)]
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
            {
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            }
        }

        [InGameCommand("guild", ServerRoleEnum.PLAYER)]
        public static void CreateGuildCommand(string value, WorldClient client)
        {
            if (!client.Character.HasGuild)
                client.Send(new GuildCreationStartedMessage());
            else
                client.Send(new GuildCreationResultMessage((sbyte)GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD));
        }

        [InGameCommand("alliance", ServerRoleEnum.PLAYER)]
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
            var target = WorldServer.Instance.GetOnlineClient(value);
            if (target != null && target.Character.Restrictions.isMuted == true
                && !(target.Character.Restrictions.isMuted = false))
                client.Character.Reply("Le joueur a bien été unmute.");
            else if (target == null)
                client.Character.Reply("Le joueur n'existe pas ou n'est pas connecté");
            else
                client.Character.Reply("Le joueur n'est pas mute.");
        }
        [InGameCommand("save", ServerRoleEnum.PLAYER)]
        public static void SavePlayer(string value, WorldClient client)
        {
            if (client.Character.IsFighting)
                client.Character.Reply("Impossible de sauvegarder votre personnage en combat !");
            else
                client.Character.Save();
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
            if (!client.Character.IsFighting)
            {
                if (!client.Character.Restrictions.isDead)
                {
                    client.Character.CurrentStats.LifePoints = (uint)client.Character.StatsRecord.LifePoints;
                    client.Character.Record.CurrentLifePoint = client.Character.CurrentStats.LifePoints;
                    client.Character.RefreshStats();
                    client.Character.Reply("Vous avez récupérer vos points de vie !");
                }
                else
                    client.Character.Reply("Impossible de récupérer vos points de vie en étant mort !");
            }
            else
                client.Character.Reply("Impossible de récupérer ses points de vie en combat !");
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
    #endregion

    static void TrySendRaw(WorldClient client, string targetname, string rawname, string succesmessage = null)
        {
            var target = WorldServer.Instance.GetOnlineClient(targetname);
            if (target != null)
            {
                if (succesmessage != null)
                    target.Character.ShowNotification(succesmessage);
                target.SendRaw(rawname);
                client.Character.ShowNotification("Raw " + rawname + " sended");
            }
            else
                client.Character.NotificationError("Le client " + targetname + " n'existe pas ou n'est pas connecté.");
        }
    }
}
