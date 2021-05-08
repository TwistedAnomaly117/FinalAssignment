using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using System.Dynamic;
using System.Net.Http;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace FinalAssignment
{
    [Serializable]
    public class GeoCode
    {
        public string aircode;
        public string weatherObservation;
    }
    
    public class Function
    {
        
        private static AmazonDynamoDBClient tableClient = new AmazonDynamoDBClient();
        public static readonly HttpClient client = new HttpClient();
        private string tableName = "AirCode";
        public async Task<ExpandoObject> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            

            //Table table = Table.LoadTable(tableClient, tableName);

            Dictionary<string, string> myDict = (Dictionary<string, string>)input.QueryStringParameters;

            string call = await client.GetStringAsync("http://api.geonames.org/weatherIcaoJSON?ICAO=" + myDict.First().Value + "&username=agoedeke");

            dynamic postCode = JsonConvert.DeserializeObject<ExpandoObject>(call);



           

            Dictionary<string, AttributeValue> myDictionary = new Dictionary<string, AttributeValue>();


            myDictionary.Add("aircode", new AttributeValue() { S = myDict.First().Value });


            PutItemRequest myRequest = new PutItemRequest(tableName, myDictionary);

            PutItemResponse res = await tableClient.PutItemAsync(myRequest);



            return postCode;

        }
    }

}
