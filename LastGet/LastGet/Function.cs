using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LastGet
{
    public class AirCode
    {
        public string aircode;
    }



    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        private string tableName = "AirCode";

        public async Task<AirCode> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            string aircode = "";

            Dictionary<string, string> myDictionary = (Dictionary<string, string>)input.QueryStringParameters;

            myDictionary.TryGetValue("aircode", out aircode);

            GetItemResponse res = await client.GetItemAsync(tableName, new Dictionary<string, AttributeValue>
            {
                { "aircode", new AttributeValue { S = aircode } }
            }
            );

            Document myDoc = Document.FromAttributeMap(res.Item);

            AirCode myCode = JsonConvert.DeserializeObject<AirCode>(myDoc.ToJson());

            return myCode;
        }
    }
}
