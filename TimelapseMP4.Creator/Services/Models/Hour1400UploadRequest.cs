// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace TimelapseMP4Creator.Services.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class Hour1400UploadRequest
    {
        /// <summary>
        /// Initializes a new instance of the Hour1400UploadRequest class.
        /// </summary>
        public Hour1400UploadRequest()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Hour1400UploadRequest class.
        /// </summary>
        public Hour1400UploadRequest(IFormFile file = default(IFormFile), string fileName = default(string), string secret = default(string))
        {
            File = file;
            FileName = fileName;
            Secret = secret;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "file")]
        public IFormFile File { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "secret")]
        public string Secret { get; set; }

    }
}