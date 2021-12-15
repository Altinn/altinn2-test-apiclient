using System.Text.Json.Serialization;

namespace Altinn.TestClient.Models
{
    /// <summary>
    /// A right model from Altinn
    /// </summary>
    public class Right
    {
        /// <summary>
        /// A unique id for the specific right.
        /// </summary>
        [JsonPropertyName("RightID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int RightID { get; set; }
        
        /// <summary>
        /// Specifies the type of right. Possible values are Message, Service and SystemResource.
        /// </summary>
        [JsonPropertyName("RightType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RightType { get; set; }

        /// <summary>
        /// Id of the system resource. Visible only for a right of type SystemResource.
        /// </summary>
        [JsonPropertyName("SystemResourceID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SystemResourceID { get; set; }

        /// <summary>
        /// Part 1/2 of the id of a specific service. Visible only for a right of type Service.
        /// </summary>
        [JsonPropertyName("ServiceCode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ServiceCode { get; set; }

        /// <summary>
        /// Part 2/2 of the id of a specific service. Visible only for a right of type Service.
        /// </summary>
        [JsonPropertyName("ServiceEditionCode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ServiceEditionCode { get; set; }

        /// <summary>
        /// Value used to identify a specific message.
        /// </summary>
        [JsonPropertyName("MessageID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MessageID { get; set; }

        /// <summary>
        /// Action supported by the right. Possible values are Read, Write, Sign, ArchiveRead and ArchiveDelete.
        /// </summary>
        [JsonPropertyName("Action")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Action { get; set; }

        /// <summary>
        /// Specifies the way the right is given. Possible values are PartyRights, RoleTypeRights, ReporteeElementRights and DirectlyDelegatedRights.
        /// </summary>
        [JsonPropertyName("RightSourceType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RightSourceType { get; set; }
    }
}