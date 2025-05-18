using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using TallerIDWM_Backend.Src.Models;

namespace TallerIDWM_Backend.Src.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> Filter(this IQueryable<User> query, bool? isActive, DateOnly? from, DateOnly? to)
        {
            if (isActive.HasValue)
                query = query.Where(u => u.Active == isActive.Value);

            if (from.HasValue)
                query = query.Where(u => u.Registered >= from.Value);

            if (to.HasValue)
                query = query.Where(u => u.Registered <= to.Value);

            return query;
        }

        public static IQueryable<User> Search(this IQueryable<User> query, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return query;

            var lower = search.Trim().ToLower();

            return query.Where(u =>
                u.FirstName.ToLower().Contains(lower) ||
                u.LastName.ToLower().Contains(lower) ||
                (u.Email != null && u.Email.ToLower().Contains(lower))
            );
        }

        public static IQueryable<User> Sort(this IQueryable<User> query, string? orderBy)
        {
            return orderBy switch
            {
                "name" => query.OrderBy(u => u.FirstName),
                "nameDesc" => query.OrderByDescending(u => u.FirstName),
                "date" => query.OrderBy(u => u.Registered),
                "dateDesc" => query.OrderByDescending(u => u.Registered),
                _ => query.OrderByDescending(u => u.Registered)
            };
        }
    }
}