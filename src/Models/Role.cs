using System;
using System.Text.Json.Serialization;

namespace Altinn.TestClient.Models
{
    /// <summary>
    /// A role model from Altinn
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique id of the role.
        /// </summary>
        [JsonPropertyName("RoleId")]
        public int RoleId { get; set; }

        /// <summary>
        /// Specifies the type of role this is. Possible values are Altinn, External and Local.
        /// </summary>
        [JsonPropertyName("RoleType")]
        public string RoleType { get; set; }

        /// <summary>
        /// Unique id of the role definition.
        /// </summary>
        [JsonPropertyName("RoleDefinitionId")]
        public int RoleDefinitionId { get; set; }

        /// <summary>
        /// Name of the role.
        /// </summary>
        [JsonPropertyName("RoleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// Description of the role.
        /// </summary>
        [JsonPropertyName("RoleDescription")]
        public string RoleDescription { get; set; }

        /// <summary>
        /// Specifies who has delegated this role.
        /// </summary>
        [JsonPropertyName("Delegator")]
        public string Delegator { get; set; }

        /// <summary>
        /// The date and time when the role was delegated.
        /// </summary>
        [JsonPropertyName("DelegatedTime")]
        public DateTime DelegatedTime { get; set; }
    }
}