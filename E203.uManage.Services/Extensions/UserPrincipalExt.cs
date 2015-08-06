﻿using E203.uManage.Services.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace E203.uManage.Services.Extensions
{
    public static class UserPrincipalExt
    {
        public static User AsUser(this UserPrincipal userPrincipal)
        {
            return UserPrincipalToUser(userPrincipal);
        }

        public static IEnumerable<User> AsUserList(this IEnumerable<UserPrincipal> userPrincipals)
        {
            return userPrincipals.Select(UserPrincipalToUser);
        }

        private static User UserPrincipalToUser(UserPrincipal userPrincipal)
        {
            if (userPrincipal == null)
                throw new ArgumentNullException("userPrincipal");

            // Uses most of the built-in properties available as part of the UserPrincipal Object
            // https://msdn.microsoft.com/en-us/library/system.directoryservices.accountmanagement.userprincipal

            return new User
            {
                // ReSharper disable once PossibleInvalidOperationException 
                // This should only be null when the context type is Machine
                UserId = userPrincipal.Guid.GetValueOrDefault(),
                UserPrincipalName = userPrincipal.UserPrincipalName,
                NtUserName = userPrincipal.SamAccountName,
                DistinguishedName = userPrincipal.DistinguishedName,
                AccountIsLocked = userPrincipal.IsAccountLockedOut(),
                AccountIsEnabled = userPrincipal.Enabled,
                AccountIsExpired = userPrincipal.AccountExpirationDate.HasValue && userPrincipal.AccountExpirationDate.Value <= DateTime.UtcNow,
                AccountWillExpire = userPrincipal.AccountExpirationDate.HasValue,
                AccountExpirationDate = userPrincipal.AccountExpirationDate,
                //PasswordIsExpired // TODO: Needs directory information to determine
                PasswordWillExpire = userPrincipal.PasswordNeverExpires, // TODO: This is not definitive, just a high level check
                //PasswordExpirationDate // TODO: Needs directory information to determine
                PasswordLastSetDate = userPrincipal.LastPasswordSet,
                FirstName = userPrincipal.GivenName,
                MiddleName = userPrincipal.MiddleName,
                LastName = userPrincipal.Surname,
                DisplayName = userPrincipal.DisplayName,
                Email = userPrincipal.EmailAddress
            };
        }
    }
}