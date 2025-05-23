﻿using System.Security.Claims;
using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(User user, IEnumerable<Claim>? additionalClaims = null);
}
