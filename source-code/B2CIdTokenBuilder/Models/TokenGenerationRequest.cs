namespace B2CIdTokenBuilder.Models
{
    public class TokenGenerationRequest
    {
        public string B2CTenant { get; set; }

        public string B2CPolicy { get; set; }

        public string ClientId { get; set; }

        public string ReplyUrl { get; set; }

        public string Claim1Name { get; set; }
        public string Claim1Value { get; set; }

        public string Claim2Name { get; set; }
        public string Claim2Value { get; set; }

        public string Claim3Name { get; set; }
        public string Claim3Value { get; set; }

        public string Claim4Name { get; set; }
        public string Claim4Value { get; set; }

        public string Claim5Name { get; set; }
        public string Claim5Value { get; set; }

        public string Claim6Name { get; set; }
        public string Claim6Value { get; set; }

        public string Claim7Name { get; set; }
        public string Claim7Value { get; set; }

        public string Claim8Name { get; set; }
        public string Claim8Value { get; set; }
    }
}