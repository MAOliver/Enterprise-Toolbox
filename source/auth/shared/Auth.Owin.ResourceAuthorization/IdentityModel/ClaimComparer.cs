using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Auth.Owin.ResourceAuthorization.IdentityModel
{
    public class ClaimComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;
            if (y == null) return false;

            return (x.Type == y.Type &&
                    x.Value == y.Value);
        }

        public int GetHashCode(Claim claim)
        {
            if (object.ReferenceEquals(claim, null)) return 0;

            var type = claim.Type;
            var val = claim.Value;

            var typeHash = type != null ? type.GetHashCode() : 0;
            var valueHash = val != null ? val.GetHashCode() : 0;

            return typeHash ^ valueHash;
        }
    }
}