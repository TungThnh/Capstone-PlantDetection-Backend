using Newtonsoft.Json;

namespace Domain.Models.Firebase
{
    public class FirebaseTokenModel
    {
        public string Issuer { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public long ExpirationTimeSeconds { get; set; }
        public long IssuedAtTimeSeconds { get; set; }
        public string Uid { get; set; } = null!;
        public string TenantId { get; set; } = null!;
        public ClaimsModel Claims { get; set; } = null!;

        public FirebaseTokenModel()
        {
            // Constructor không tham số
        }

        public FirebaseTokenModel(
            string issuer,
            string subject,
            string audience,
            long expirationTimeSeconds,
            long issuedAtTimeSeconds,
            string uid,
            string tenantId,
            ClaimsModel claims)
        {
            Issuer = issuer;
            Subject = subject;
            Audience = audience;
            ExpirationTimeSeconds = expirationTimeSeconds;
            IssuedAtTimeSeconds = issuedAtTimeSeconds;
            Uid = uid;
            TenantId = tenantId;
            Claims = claims;
        }
    }

    public class ClaimsModel
    {
        public string Picture { get; set; } = null!;
        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }
        public long AuthTime { get; set; }
        public string Email { get; set; } = null!;
        public FirebaseModel Firebase { get; set; } = null!;
        public string Name { get; set; } = null!;
        [JsonProperty("user_id")]
        public string UserId { get; set; } = null!;

        public ClaimsModel()
        {
            // Constructor không tham số
        }

        public ClaimsModel(
            string picture,
            bool emailVerified,
            long authTime,
            string email,
            FirebaseModel firebase,
            string name,
            string userId)
        {
            Picture = picture;
            EmailVerified = emailVerified;
            AuthTime = authTime;
            Email = email;
            Firebase = firebase;
            Name = name;
            UserId = userId;
        }
    }

    public class FirebaseModel
    {
        public IdentitiesModel Identities { get; set; } = null!;
        [JsonProperty("sign_in_provider")]
        public string SignInProvider { get; set; } = null!;

        public FirebaseModel()
        {
            // Constructor không tham số
        }

        public FirebaseModel(IdentitiesModel identities, string signInProvider)
        {
            Identities = identities;
            SignInProvider = signInProvider;
        }
    }

    public class IdentitiesModel
    {
        [JsonProperty("google.com")]
        public string[] GoogleCom { get; set; } = null!;
        public string[] Email { get; set; } = null!;

        public IdentitiesModel()
        {
            // Constructor không tham số
        }

        public IdentitiesModel(string[] googleCom, string[] email)
        {
            GoogleCom = googleCom;
            Email = email;
        }
    }


}

