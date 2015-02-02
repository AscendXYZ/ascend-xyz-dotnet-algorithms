using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenIDConnectAuthenticationLibrary;
using Ascend.Algorithms.Client.Core;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Ascend.Algorithms.GdalInfo.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            return;
            var blob = "";
            var authContext = new OpenIDConnectContext("https://identity-staging.ascend.xyz/connect/authorize", "6994A4A8-0E65-4FED-A82B-C684A0DD1758", "oob://localhost/wpfclient");

                var authenticationResult = authContext
                    .RequestTokenAsync("alg.execute", "token").Result;
                var token = authenticationResult.AccessToken;
    

            var algClient = new AlgorithmManagementClient(
                new AlgorithmCredentials { BearerToken = token },
                new Uri("https://alg-staging.services.ascend.xyz/api/"));


            var uri = algClient.RunAlgorithmAsync("Gdal.GdalInfo",
                "1.0.0-pre-20150201-02",
                "Ascend.Algorithms.GdalAlgorithms",
                JsonConvert.SerializeObject(new GDALInfoInput
                {
                    GdalDataSource = "/vsicurl/" + blob,

                }), new CancellationToken()).GetAwaiter().GetResult();


  
                Thread.Sleep(10000);
               var status= algClient.GetStatusAsync<GDALInfoResult>(uri, new CancellationToken()).GetAwaiter().GetResult();

               
           
        }
    }
}
