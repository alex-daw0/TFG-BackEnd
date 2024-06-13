namespace backend2.Token {
    public class JwtSettings {
        public string? KeySecret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }

        public JwtSettings( ) {
        }

        public JwtSettings(string keySecret, string issuer, string audience) {
            KeySecret = keySecret;
            Issuer = issuer;
            Audience = audience;
        }
    }
}
