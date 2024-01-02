using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService
{
    public class PhonePeIntegration
    {
        private readonly string hostUrl = "https://api-preprod.phonepe.com/apis/hermes";
        private readonly string merchantId = "PGTESTPAYUAT"; // Update with your Merchant ID
        private readonly string saltKey = "099eb0cd-02cf-4e2a-8aca-3e6c6aff0399"; // Update with your Salt Key

        // Method to initiate a payment transaction
        public async Task<string> InitiatePaymentAsync(decimal amount, string cardNumber, string cardType, string cardIssuer, int expiryMonth, int expiryYear, string cvv)
        {
            try
            {
                // Prepare payment payload
                var paymentPayload = new
                {
                    merchantId = merchantId,
                    merchantTransactionId = "MT7850590068188104",
                    merchantUserId = "MUID123",
                    amount = 10000,
                    redirectUrl = "https://webhook.site/redirect-url",
                    redirectMode = "REDIRECT",
                    callbackUrl = "https://webhook.site/callback-url",
                    mobileNumber = "9999999999",
                    paymentInstrument = new
                    {
                        type = "PAY_PAGE"
                    }
                };

                // Convert payment payload to JSON
                var paymentPayloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(paymentPayload);
                byte[] bytes = Encoding.UTF8.GetBytes(paymentPayloadJson);

                // Convert the bytes to Base64 string
                string base64EncodedPayload = CalculateSHA256(Convert.ToBase64String(bytes) + "/pg/v1/pay" + saltKey);

                // Create HttpClient
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("X-VERIFY", base64EncodedPayload+"###1");

                    // Send POST request to PhonePe API to initiate payment
                    var response = await httpClient.PostAsync($"{hostUrl}/initiate-payment",
                        new StringContent(paymentPayloadJson, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        // Payment request successful
                        var responseData = await response.Content.ReadAsStringAsync();
                        // Parse responseData as required and return response (payment URL or transaction details)
                        return responseData;
                    }
                    else
                    {
                        // Handle unsuccessful payment request
                        return "Payment initiation failed";
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return "Exception occurred: " + ex.Message;
            }
        }

        public static string CalculateSHA256(string input)
        {
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
    }
}
