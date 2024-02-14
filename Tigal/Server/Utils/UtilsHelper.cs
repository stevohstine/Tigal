using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace Tigal.Server.Utils
{
    public static class UtilsHelper
    {
        public static string GenerateNewCode(int CodeLength)
        {
            Random random = new Random();
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < CodeLength; i++)
            {
                output.Append(random.Next(0, 9));
            }

            return output.ToString();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string SaveImage(string ImgStr, string ImgName)
        {
            string imageName = ImgName + ".jpg";
            var filePath = Path.Combine( Directory.GetCurrentDirectory(), "Images/Properties", $"{imageName}");

            byte[] imageBytes = Convert.FromBase64String(ImgStr);
            System.IO.File.WriteAllBytes(filePath, imageBytes);
            var fileUrl = $"https://backend.tigal.africa/Images/Properties/{imageName}";
            return fileUrl;
        }
    }
}
