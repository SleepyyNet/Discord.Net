﻿using Discord.API.Rest;
using System;
using System.Threading.Tasks;
using Model = Discord.API.User;

namespace Discord
{
    internal class SelfUser : User, ISelfUser
    {        
        public string Email { get; private set; }
        public bool IsVerified { get; private set; }

        public SelfUser(DiscordClient discord, Model model)
            : base(model)
        {
        }
        public override void Update(Model model, UpdateSource source)
        {
            if (source == UpdateSource.Rest && IsAttached) return;

            base.Update(model, source);

            Email = model.Email;
            IsVerified = model.IsVerified;
        }
        
        public async Task UpdateAsync()
        {
            if (IsAttached) throw new NotSupportedException();

            var model = await Discord.ApiClient.GetSelfAsync().ConfigureAwait(false);
            Update(model, UpdateSource.Rest);
        }
        public async Task ModifyAsync(Action<ModifyCurrentUserParams> func)
        {
            if (func != null) throw new NullReferenceException(nameof(func));

            var args = new ModifyCurrentUserParams();
            func(args);
            var model = await Discord.ApiClient.ModifySelfAsync(args).ConfigureAwait(false);
            Update(model, UpdateSource.Rest);
        }
    }
}