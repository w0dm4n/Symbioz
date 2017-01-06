using MySql.Data.MySqlClient;
using Symbioz.Auth.Models;
using Symbioz.Core;
using Symbioz.Network.Clients;
using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.ORM;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Threading;

namespace Symbioz.Auth.Records
{
    class AccountsProvider     
    {
        static ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        public static void CreateAccountInformation(int accountid,int startbankkamas)
        {

            Monitor.Enter(AuthDatabaseProvider.AuthConnection);
            try
            {
                AuthDatabaseProvider.Insert("AccountsInformations", new List<string>() { "Id", "BankKamas" }, new List<string>() { accountid.ToString(), startbankkamas.ToString() });

            }
            finally
            {
                Monitor.Exit(AuthDatabaseProvider.AuthConnection);
            }
        } 

        public static void Ban(string accountName)
        {
            Locker.EnterReadLock();
            try
            {
                AuthDatabaseProvider.Update("accounts", "Banned", "True", "Username", accountName);
            }
            finally
            {
                Locker.ExitReadLock();
            }
           
        }

        public static void BanIp(string ip)
        {
            Locker.EnterReadLock();
            try
            {
                BanIpRecord.Add(ip);
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        public static Account GetAccountFromDb(string username)
        {
            Monitor.Enter(AuthDatabaseProvider.AuthConnection);
            try
            {
                Account account = null;
                string query = "SELECT * FROM Accounts WHERE Username = '" + username + "'";
                MySqlDataReader dataReader = new MySqlCommand(query, AuthDatabaseProvider.AuthConnection).ExecuteReader();
                while (dataReader.Read())
                {
                    account = new Account();
                    account.Username = dataReader["Username"].ToString();
                    account.Password = dataReader["Password"].ToString();
                    account.Nickname = dataReader["Nickname"].ToString();
                    account.Role = (ServerRoleEnum)dataReader.GetInt32("Role");
                    account.Id = dataReader.GetInt32("Id");
                    account.Banned = dataReader.GetBoolean("Banned");
                    account.MaxCharactersCount = dataReader.GetInt32("MaxCharactersCount");
                    account.PointsCount = dataReader.GetInt32("PointCount");
                }
                dataReader.Close();
                return account;
            }
            finally
            {
                Monitor.Exit(AuthDatabaseProvider.AuthConnection);
            }
        }

        public static Account GetAccountFromDb(int accountId)
        {
            Monitor.Enter(AuthDatabaseProvider.AuthConnection);
            try
            {
                Account account = null;
                string query = "SELECT * FROM Accounts WHERE Id = '" + accountId + "'";
                MySqlDataReader dataReader = new MySqlCommand(query, AuthDatabaseProvider.AuthConnection).ExecuteReader();
                while (dataReader.Read())
                {
                    account = new Account();
                    account.Username = dataReader["Username"].ToString();
                    account.Password = dataReader["Password"].ToString();
                    account.Nickname = dataReader["Nickname"].ToString();
                    account.Role = (ServerRoleEnum)dataReader.GetInt32("Role");
                    account.Id = dataReader.GetInt32("Id");
                    account.Banned = dataReader.GetBoolean("Banned");
                    account.MaxCharactersCount = dataReader.GetInt32("MaxCharactersCount");
                    account.WarnOnFriendConnection = dataReader.GetInt32("WarnOnFriendConnection") == 1 ? true : false;
                    account.PointsCount = dataReader.GetInt32("PointCount");
                }
                dataReader.Close();
                return account;
            }
            finally
            {
                Monitor.Exit(AuthDatabaseProvider.AuthConnection);
            }
        }

        public static bool UpdateAccountsWarningEvent(Account account)
        {
            bool updated = false;
            try
            {
                AuthDatabaseProvider.Update("accounts", "WarnOnFriendConnection", account.WarnOnFriendConnection ? "1" : "0", "Id", account.Id.ToString());
                //TODO: Warns ...
                updated = true;
            }
            catch(Exception ex)
            {
                Logger.Error(ex.Message);
            }
            
            return updated;
        }

        public static bool UpdateAccountsOnlineState(int accountId, bool isOnline)
        {
            return true;
            /*bool updated = false;
            Locker.EnterReadLock();
            try
            {
                AuthDatabaseProvider.Update("accounts", "IsOnline", isOnline ? "1" : "0", "Id", accountId.ToString());
                updated = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                Locker.ExitReadLock();
            }
            return updated;*/
        }

        public static bool RemovePoints(Account account)
        {
            Locker.EnterReadLock();
            try
            {
                AuthDatabaseProvider.Update("accounts", "PointCount", "0", "Id", account.Id.ToString());
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                Locker.ExitReadLock();
            }
        }

        public static Account DecryptCredentialsFromClient(AuthClient client, string salt, IEnumerable<byte> credentials)
        {
            try
            {
                string login;
                string password;

                using (var reader = new BigEndianReader(credentials.ToArray()))
                {
                    login = reader.ReadUTF();
                    password = reader.ReadUTF();
                    client.AesKey = reader.ReadBytes(32);
                }
                var account = new Account();
                account.Username = login;
                account.Password = password;
                return account;

            }
            catch (Exception e)
            {
                Logger.Error("[Decrypt Credentials] " + e.ToString());
                return null;
            }
        }

        static byte[] AESEncrypt(byte[] data, byte[] key)
        {
            var iv = key.Take(16).ToArray();
            try
            {
                using (var rijndaelManaged = new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
                {
                    ICryptoTransform crypto = rijndaelManaged.CreateEncryptor();
                    return crypto.TransformFinalBlock(data, 0, data.Length);
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        public static byte[] EncryptTicket(string ticket, byte[] key)
        {
            var writer = new BigEndianWriter();
            writer.WriteByte((byte)ticket.Length);
            writer.WriteUTFBytes(ticket);
            return AESEncrypt(writer.Data, key);
        }

        public static bool CheckAndApplyNickname(AuthClient client, string nickname)
        {
            Monitor.Enter(AuthDatabaseProvider.AuthConnection);
            try
            {
                if (AuthDatabaseProvider.SelectData("Accounts", "Nickname", nickname, "Nickname") != string.Empty)
                    return false;
                else
                {
                    AuthDatabaseProvider.Update("Accounts", "Nickname", nickname, "Id", client.Account.Id.ToString());
                    client.Account.Nickname = nickname;
                    return true;
                }
            }
            finally
            {
                Monitor.Exit(AuthDatabaseProvider.AuthConnection);
            }
        }
    }
}
