using System.Text.Json.Serialization;

namespace Altinn.TestClient.Models
{
    /// <summary>
    /// The ReporteeConversion response from Altinn reportees endpoind.
    /// </summary>
    public class Reportee
    {
        /// <summary>
        /// The reportee id
        /// </summary>
        [JsonPropertyName("ReporteeId")]
        public string ReporteeId { get; set; }

        /// <summary>
        /// The full name of the person or organization.
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Person or organization
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Social Security Number Masked
        /// </summary>
        [JsonPropertyName("SocialSecurityNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SocialSecurityNumber { get; set; }

        /// <summary>
        /// The Organization Number
        /// </summary>
        [JsonPropertyName("OrganizationNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string OrganizationNumber { get; set; }

        /// <summary>
        /// The Organization Number of the Parent organization, if any.
        /// </summary>
        [JsonPropertyName("ParentOrganizationNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ParentOrganizationNumber { get; set; }

        /// <summary>
        /// The Organization Form
        /// </summary>
        [JsonPropertyName("OrganizationForm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string OrganizationForm { get; set; }

        /// <summary>
        /// The status of the organization
        /// </summary>
        [JsonPropertyName("Status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Status { get; set; }
    }
}