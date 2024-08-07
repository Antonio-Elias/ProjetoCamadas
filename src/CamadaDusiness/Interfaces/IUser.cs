﻿

using System.Security.Claims;

namespace CamadaBusiness.Interfaces;

public interface IUser
{
    string Name { get; }
    Guid GetUserId();
    string GetUserEmail();
    bool IsAuthenticated();
    bool IsInRole(string role);
    public IEnumerable<Claim> GetClaimsIdentity();
}
