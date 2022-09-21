using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace UserService.Models
{
    public partial class Purchase
    {
        public int PurchaseId { get; set; }
        public string EmailId { get; set; } = null!;
        public int? BookId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? PaymentMode { get; set; }
        public bool? IsRefund { get; set; }
        public string? PurchaseStatus { get; set; }

        public virtual Book? Book { get; set; }

        public Boolean callPaymentAuzreFunPost()
        {
            bool retVal = false;
            string URL = "https://azurepurchasefunctionapp20220920172058.azurewebsites.net/api/AzurePurchasePost";
            string urlParameters = "?code=QkqWfNqjk4E6yzfyI6G0gZ6dL1MqtKdVzOK-xjNWPWadAzFuLnWtRw==";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            //var myJson = "{ \"EmailId\" : \"" + EmailId + "\"," +
            //                "\"BookId\" : \"" + BookId + "\"," +
            //                "\"PurchaseDate\" : \"" + Convert.ToDateTime(PurchaseDate).ToString("MM/dd/yyyy") + "\"}";

            var myJson = "{ \"EmailId\" : \"" + EmailId + "\"," +
                            "\"BookId\" : \"" + BookId + "\"}";

            // List data response.
            HttpResponseMessage response = client.PostAsync(urlParameters, new StringContent(myJson, Encoding.UTF8, "application/json")).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                //response.Content.ReadAsStringAsync().Result;
                retVal = true;
            }
            else
            {
                //res = response.Content.ReadAsStringAsync().Result;
            }
            client.Dispose();
            return retVal;
        }
    }
}
