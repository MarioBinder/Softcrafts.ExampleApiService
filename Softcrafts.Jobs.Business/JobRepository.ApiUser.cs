using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Softcrafts.Jobs.Business.Data;
using Softcrafts.Jobs.Entities;
using Softcrafts.Jobs.Entities.Base;
using OCC.Utils.Cryptography;
using OCC.Utils.Cryptography.Models;

namespace Softcrafts.Jobs.Business
{
    public partial class JobRepository
    {
        public SemasResult HeartBeat(bool inklDatabase)
        {
            if (!inklDatabase) return new SemasResult(true, "I'm alive");
            using (var ctx = new JobContext())
            {
                var result = ctx.Jobs.Count();
                if (result > 0)
                    return new SemasResult(true, "I'am alive, the database too");
            }
            return new SemasResult(true, "I'm alive");
        }
    }

    public partial class JobRepository
    {
        public SemasResult IsApiUserAllowed(string username, string passwort)
        {
            using (var ctx = new JobContext())
            {
                var user = ctx.APIUser.AsNoTracking().FirstOrDefault(c => c.Username == username);
                if (user == null)
                    return new SemasResult(false, $"ApiUser {username} not found or allowed");

                var password = new Password
                {
                    InitVector = user.InitVector,
                    SaltValue = user.SaltValue,
                    PassPhrase = user.PassPhrase,
                    KeySize = 256,
                    PlainOrCipherText = username + passwort,
                    PasswordIterations = 6
                };
                var isValid = user.Password == Rijndael.EncryptPassword(password);
                return new SemasResult(isValid, !isValid ? $"ApiUser {username} not found or allowed" : String.Empty);
            }
        }

        public async Task<SemasResult<List<APIUser>>> GetAllApiUsers(Func<APIUser, bool> predicate)
        {
            using (var ctx = new JobContext())
            {
                var permit = await CheckApiUserAdminPermission();
                if (!permit.Success)
                    return new SemasResult<List<APIUser>>(permit.Success, permit.Message);

                var users = await ctx.APIUser.Where(predicate).AsAsyncQueryable().ToListAsync();
                return new SemasResult<List<APIUser>>(true, users);
            }
        }

        public async Task<SemasResult> CreateApiUser(string username, string passwort, bool asAdmin = false)
        {
            using (var ctx = new JobContext())
            {
                var permit = await CheckApiUserAdminPermission();
                if (!permit.Success)
                    return permit;

                var user = ctx.APIUser.AsNoTracking().FirstOrDefault(c => c.Username == username);
                if (user != null)
                    return new SemasResult(true, $"ApiUser {username} already exits, check keepass for the credentials");

                var password = new Password();
                var initVector = password.InitVector = Random(16);
                var saltValue = password.SaltValue = Random(45); //45
                var passPhrase = password.PassPhrase = Random(40); //39
                password.KeySize = 256;
                password.PlainOrCipherText = username + passwort;
                password.PasswordIterations = 6;
                var encryptedPassword = Rijndael.EncryptPassword(password);

                ctx.APIUser.Add(new APIUser()
                {
                    Username = username,
                    Password = encryptedPassword,
                    InitVector = initVector,
                    SaltValue = saltValue,
                    PassPhrase = passPhrase,
                    IsAdmin = asAdmin,
                    CreatedUser = Username,
                    CreatedDate = DateTime.Now,
                });
                var result = await ctx.SaveChangesAsync();
                return new SemasResult(result > 0, result <= 0 ? $"Error in CreateApiUser for {username}" : string.Empty);
            }
        }

        public async Task<SemasResult> SetApiUserAsAdmin(string username)
        {
            var permit = await CheckApiUserAdminPermission();
            if (!permit.Success)
                return permit;

            using (var ctx = new JobContext())
            {
                var user = await ctx.APIUser.FirstOrDefaultAsync(c => c.Username == username);
                if (user == null)
                    return new SemasResult(false, $"ApiUser {username} doesnt exists.");

                user.IsAdmin = true;
                var result = await ctx.SaveChangesAsync();
                return new SemasResult(result > 0, result <= 0 ? $"Error in SetApiUserAsAdmin for {username}" : string.Empty);
            }
        }

        public async Task<SemasResult> RevokeAdminFromApiUser(string username)
        {
            var permit = await CheckApiUserAdminPermission();
            if (!permit.Success)
                return permit;

            using (var ctx = new JobContext())
            {
                var user = await ctx.APIUser.FirstOrDefaultAsync(c => c.Username == username);
                if (user == null)
                    return new SemasResult(false, $"ApiUser {username} doesnt exists.");

                user.IsAdmin = false;
                var result = await ctx.SaveChangesAsync();
                return new SemasResult(result > 0, result <= 0 ? $"Error in RevokeAdminFromApiUser for {username}" : string.Empty);
            }
        }

        public async Task<SemasResult> DeleteApiUser(string username)
        {
            var permit = await CheckApiUserAdminPermission();
            if (!permit.Success)
                return permit;

            using (var ctx = new JobContext())
            {
                var user = await ctx.APIUser.FirstOrDefaultAsync(c => c.Username == username);
                if (user == null)
                    return new SemasResult(false, $"ApiUser {username} doesnt exists.");

                user.IsDeleted = true;
                var result = await ctx.SaveChangesAsync();
                return new SemasResult(result > 0, result <= 0 ? $"Error in DeleteApiUser for {username}" : string.Empty);
            }
        }

        private async Task<SemasResult> CheckApiUserAdminPermission()
        {
            using (var ctx = new JobContext())
            {
                var currentUser = await ctx.APIUser.FirstOrDefaultAsync(c => c.Username == Username);
                if (currentUser == null || !currentUser.IsAdmin)
                    return new SemasResult(false, "Not allowed! You need the admin-role to execute this function.");
                return new SemasResult(true);
            }
        }
    }
}
