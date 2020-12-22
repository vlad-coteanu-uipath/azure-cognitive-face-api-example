using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FacialCognitiveServicesExample
{
    class FacialCognitiveService
    {

        public static FacialCognitiveService Instance = new FacialCognitiveService();

        private static string AZURE_FACIAL_COGNITIVE_SERVICE_KEY = "b03a73f929e7452ca2eca23a42765267";
        private static string AZURE_FACIAL_COGNITIVE_SERVICE_ENDPOINT = "https://facialcognitivesolution.cognitiveservices.azure.com/";

        private FacialCognitiveService() { }

        private bool IsCandidateInGroup(IFaceClient faceClient, string personGroupId, string candidatePath)
        {
            Console.WriteLine("Test if candidate image " + candidatePath + " contains faces belonging to people from person group " + personGroupId);
            using (Stream s = File.OpenRead(candidatePath))
            {
                var faces = faceClient.Face.DetectWithStreamAsync(s).GetAwaiter().GetResult();
                var faceIds = faces.Select(face => (Guid?)face.FaceId).ToList();

                var results = faceClient.Face.IdentifyAsync(faceIds, personGroupId).GetAwaiter().GetResult();
                foreach (var identifyResult in results)
                {
                    Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Count == 0)
                    {
                        Console.WriteLine("No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = faceClient.PersonGroupPerson.GetAsync(personGroupId, candidateId).GetAwaiter().GetResult();
                        Console.WriteLine("Identified as {0}", person.Name);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ValidateImages(string happyImagePath, string sadImagePath, string surpriseImagePath) 
        {
            IFaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(AZURE_FACIAL_COGNITIVE_SERVICE_KEY))
            {
                Endpoint = AZURE_FACIAL_COGNITIVE_SERVICE_ENDPOINT
            };
            faceClient.Endpoint = AZURE_FACIAL_COGNITIVE_SERVICE_ENDPOINT;

            /*
                The validator should first check if the three images contain the same person
                and then if the sentiment analysis provides the expected result.
             */

            Console.WriteLine("Step 1: Create an empty person group");
            string personGroupId = "test-person-group-vlad-coteanu-id-9";
            string groupName = "test-person-group-vlad-coteanu-9";
            faceClient.PersonGroup.CreateAsync(personGroupId, groupName).GetAwaiter().GetResult();

            Console.WriteLine("Step 2: Add the person in the first picture to the group");
            string firstPersonName = "vlad-coteanu-first-person";
            var createPersonResult = faceClient.PersonGroupPerson.CreateAsync(personGroupId, firstPersonName).GetAwaiter().GetResult();

            Console.WriteLine("Step 3: Register the first face to our person");
            using (Stream s = File.OpenRead(happyImagePath))
            {
                faceClient.PersonGroupPerson.AddFaceFromStreamAsync(personGroupId, createPersonResult.PersonId, s).GetAwaiter().GetResult();
            }

            Console.WriteLine("Step 4: Train the person group");
            faceClient.PersonGroup.TrainAsync(personGroupId).GetAwaiter().GetResult();

            Console.WriteLine("Step 5: Check train status and proceed after it is finished");
            while(true)
            {
                var trainingStatus = faceClient.PersonGroup.GetTrainingStatusAsync(personGroupId).GetAwaiter().GetResult();
                if(trainingStatus.Status != TrainingStatusType.Running)
                {
                    break;
                }
                Console.WriteLine("Still training...");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Training is over");
            Console.WriteLine("Step 6: Check if the two pictures contain the face of the person registered");
            if(!IsCandidateInGroup(faceClient, personGroupId, sadImagePath) || !IsCandidateInGroup(faceClient, personGroupId, surpriseImagePath)) 
            {
                Console.WriteLine("Same person check failed");
                MessageBox.Show("The person is not the same in all pictures");
                faceClient.PersonGroup.DeleteAsync(personGroupId).GetAwaiter().GetResult();
                return false;
            }

            Console.WriteLine("Step 7: Perform the sentiment analysis part");
            try
            {

                IList<FaceAttributeType?> faceAttributes =
                    new FaceAttributeType?[]
                    {
                        FaceAttributeType.Emotion,
                    };

                using (Stream s = File.OpenRead(happyImagePath))
                {
                    var faceDetectionResult = faceClient.Face.DetectWithStreamAsync(s, true, false, faceAttributes).GetAwaiter().GetResult();
                    double happiness = faceDetectionResult[0].FaceAttributes.Emotion.Happiness;
                    Console.WriteLine("Happiness degree in the happiness image: " + happiness);
                    if (happiness < 0.75)
                    {
                        MessageBox.Show("The face in the happy image is not happy enough");
                        faceClient.PersonGroup.DeleteAsync(personGroupId).GetAwaiter().GetResult();
                        return false;
                    }
                }

                using (Stream s = File.OpenRead(sadImagePath))
                {
                    var faceDetectionResult = faceClient.Face.DetectWithStreamAsync(s, true, false, faceAttributes).GetAwaiter().GetResult();
                    double sadness = faceDetectionResult[0].FaceAttributes.Emotion.Sadness;
                    Console.WriteLine("Sadness degree in the sad image: " + sadness);
                    if (sadness < 0.75)
                    {
                        MessageBox.Show("The face in the sad image is not sad enough");
                        faceClient.PersonGroup.DeleteAsync(personGroupId).GetAwaiter().GetResult();
                        return false;
                    }
                }

                using (Stream s = File.OpenRead(surpriseImagePath))
                {
                    Console.WriteLine(surpriseImagePath);
                    var faceDetectionResult = faceClient.Face.DetectWithStreamAsync(s, true, false, faceAttributes).GetAwaiter().GetResult();
                    double surprise = faceDetectionResult[0].FaceAttributes.Emotion.Surprise;
                    Console.WriteLine("Surprise degree in the surprise image: " + surprise);
                    if (surprise < 0.75)
                    {
                        MessageBox.Show("The face in the surprise image is not surprise enough");
                        faceClient.PersonGroup.DeleteAsync(personGroupId).GetAwaiter().GetResult();
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                faceClient.PersonGroup.DeleteAsync(personGroupId).GetAwaiter().GetResult();
                return false;
            }

            faceClient.PersonGroup.DeleteAsync(personGroupId).GetAwaiter().GetResult();

            Console.WriteLine("Validation passed");
            return true;
        }



    }
}
