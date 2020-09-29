using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using SeleniumProject.POCO;
using SeleniumProject.Settings;
using System.IO;
using System.Net;
using TechTalk.SpecFlow;

namespace SeleniumProject.StepDefinition
{
    [Binding]
    public sealed class PetStoreBasicOperationsSteps : BaseDefinition
    {
        long id;
        readonly RestClient client;
        IRestResponse response;
        readonly string jsonForPetCreation;
        string jsonForPetUpdate;

        public PetStoreBasicOperationsSteps()
        {
            jsonForPetCreation = File.ReadAllText("JSONFiles/PetDetails.json");
            client = ObjectRepository.Client;
        }

        [Given(@"user creates a pet")]
        public void GivenUserCreatesAPet()
        {
            Logger.Info("Creating a pet");
            response = client.Execute(CreateRequest(Method.POST).AddParameter("application/json", jsonForPetCreation, ParameterType.RequestBody));
            Logger.Info($"Response: {response.Content}");
            //map response to POCO
            var petDetailsResponseMappedToObject = new JsonDeserializer().Deserialize<PetDetails>(response);
            //set id for use in next steps
            id = petDetailsResponseMappedToObject.id;
            //if status code is not OK then dont proceed with remainder of tests
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [When(@"user calls GET method")]
        public void WhenUserCallsGETMethod()
        {
            response = client.Execute(new RestRequest($"/pet/{id}", Method.GET));
            Logger.Info($"Response: {response.Content}");
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"user can read the pet details which match the details that were used to create pet")]
        public void ThenUserCanReadThePetDetailsWhichMatchTheDetailsThatWereUsedToCreatePet()
        {
            //convert jsonString to POCO
            var petDetailsFromJson = JsonConvert.DeserializeObject<PetDetails>(jsonForPetCreation);
            //convert response to POCO
            var responsePetDetails = new JsonDeserializer().Deserialize<PetDetails>(response);

            //compare values
            responsePetDetails.category.name.Should().Be(petDetailsFromJson.category.name);
            responsePetDetails.name.Should().Be(petDetailsFromJson.name);
            responsePetDetails.tags[0].name.Should().Be(petDetailsFromJson.tags[0].name);
        }

        [Then(@"user updates pet details")]
        public void ThenUserUpdatesPetDetails()
        {
            jsonForPetUpdate = CreateUpdatedJsonToUpdatePet();
            response = client.Execute(CreateRequest(Method.PUT).AddParameter("application/json", jsonForPetUpdate, ParameterType.RequestBody));
            Logger.Info($"Response: {response.Content}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Then(@"user can read the pet details which match the details that were used to update pet")]
        public void ThenUserCanReadThePetDetailsWhichMatchTheDetailsThatWereUsedToUpdatePet()
        {
            var updatePetDetails = JsonConvert.DeserializeObject<PetDetails>(jsonForPetUpdate);
            var responsePetDetails = new JsonDeserializer().Deserialize<PetDetails>(response);
            responsePetDetails.status.Should().Be(updatePetDetails.status);
        }


        [Then(@"delete pet details when complete")]
        public void ThenDeletePetDetailsWhenComplete()
        {
            //carry out delete
            client.Execute(new RestRequest($"/pet/{id}", Method.DELETE));

            //check deletion with a get request
            client.Execute(new RestRequest($"/pet/{id}", Method.GET)).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #region private Methods
        private RestRequest CreateRequest(Method method)
        {
            return new RestRequest("/pet", method);
        }

        private string CreateUpdatedJsonToUpdatePet()
        {
            //get json and deserialise to POCO
            var updatePetDetailsMappedToObject = JsonConvert.DeserializeObject<PetDetails>(jsonForPetCreation);
            //change ID to created pet id
            updatePetDetailsMappedToObject.id = id;
            //change availability to sold
            updatePetDetailsMappedToObject.status = "Sold";
            //serialise back to json
            return JsonConvert.SerializeObject(updatePetDetailsMappedToObject);
        }
        #endregion
    }
}
