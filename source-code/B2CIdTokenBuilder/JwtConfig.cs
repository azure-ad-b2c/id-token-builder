﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace B2CIdTokenBuilder
{
    public static class JwtConfig
    {
        private static Uri GetCurrentUrl() => HttpContext.Current?.Request?.Url;

        private static Lazy<X509SigningCredentials> signingCredentials = new Lazy<X509SigningCredentials>(() =>
        {
            string certThumbprint = ConfigurationManager.AppSettings["SigningCertThumbprint"];
            string certAlgorithm = ConfigurationManager.AppSettings["SigningCertAlgorithm"];

            using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, false);

                if (certCollection.Count < 1)
                {
                    throw new Exception("Could not find signing certificate in certificate store.");
                }

                X509Certificate2 certificate = certCollection[0];
                certStore.Close();

                return new X509SigningCredentials(certificate, certAlgorithm);
            }
        });

        public static X509SigningCredentials JwtSigningCredentials => signingCredentials.Value;

        public static string JwtIssuer => $"{GetCurrentUrl()?.Scheme}://{GetCurrentUrl()?.Authority}{HttpContext.Current?.Request?.ApplicationPath}";

        public static int JwtExpirationDays => 7;
    }
}