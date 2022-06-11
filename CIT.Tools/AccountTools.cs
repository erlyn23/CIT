using CIT.DataAccess.Contracts;
using CIT.Dtos.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Tools
{
    public class AccountTools
    {
        public string FilePath { get => $"{Environment.CurrentDirectory}/wwwroot/utils/confirmation_account_data.json"; }
        private readonly EmailTools _emailTools;

        public AccountTools(EmailTools emailTools)
        {
            _emailTools = emailTools;
        }

        public async Task<EmailVerificationResponse> SendEmailConfirmationAsync(string identificationDocument, string userEmail)
        {
            var emailVerification = new EmailVerificationResponse();
            emailVerification.UserIdentificationDocument = identificationDocument;
            emailVerification.RandomCode = BuildConfirmations.BuildConfirmationCode();
            emailVerification.ExpireDate = DateTime.UtcNow.AddMinutes(30);


            string confirmationUrl = $"https://localhost:44306/Account/Activate?docId={identificationDocument}&code={emailVerification.RandomCode}";
            string subject = "Confirmación de cuenta Sistema Integral de Préstamos";
            string body = $"Hola, para verificar tu cuenta del Sistema Integral de Préstamos ingresa al siguiente enlace: " +
            $"<a href='{confirmationUrl}'>{confirmationUrl}</a>, y expira en los próximos <b>30 minutos.</b>";
            try
            {
                var isEmailSended = await _emailTools.SendEmailWithInfoAsync(userEmail, subject, body);
                if (isEmailSended)
                    CreateJsonFileWithConfirmationData(emailVerification);
                return emailVerification;
            }
            catch
            {
                throw;
            }
        }

        private void CreateJsonFileWithConfirmationData(EmailVerificationResponse emailVerificationResponse)
        {
            var emailVerifications = GetConfirmationDataFromJsonFile();
            EmailVerificationResponse emailVerification = null;

            if (emailVerifications != null)
            {
                emailVerification = emailVerifications
                .Where(e => e.UserIdentificationDocument == emailVerificationResponse.UserIdentificationDocument)
                .FirstOrDefault();
            }
            else
            {
                emailVerifications = new List<EmailVerificationResponse>();
            }

            if (emailVerification != null)
            {
                int toDelete = emailVerifications.IndexOf(emailVerification);
                emailVerifications.RemoveAt(toDelete);

                emailVerification.UserIdentificationDocument = emailVerificationResponse.UserIdentificationDocument;
                emailVerification.RandomCode = emailVerificationResponse.RandomCode;
                emailVerification.ExpireDate = emailVerificationResponse.ExpireDate;
                emailVerifications.Add(emailVerification);
            }
            else
            {
                emailVerifications.Add(emailVerificationResponse);
            }
            string jsonString = JsonConvert.SerializeObject(emailVerifications);
            if (File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, jsonString);
            }
            else
            {
                File.Create(FilePath);
                File.WriteAllText(FilePath, jsonString);
            }
        }
        public List<EmailVerificationResponse> GetConfirmationDataFromJsonFile()
        {
            string jsonContent = string.Empty;
            using (var streamReader = new StreamReader(FilePath)) jsonContent = streamReader.ReadToEnd();
            var emailVerifications = JsonConvert.DeserializeObject<List<EmailVerificationResponse>>(jsonContent);
            return emailVerifications;
        }
    }
}