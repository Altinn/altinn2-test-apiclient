using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Altinn.TestClient.Models
{
    /// <summary>
    /// A right model from Altinn
    /// </summary>
    public class RightHolder
    {
        /// <summary>
        /// The ID of the user holding the rights. The value is globally unique in Altinn and represents a person, organization or enterprise user.
        /// </summary>
        [JsonPropertyName("RightHolderId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RightHolderId { get; set; }

        /// <summary>
        /// The name of the right holder. Person name, name of organization or username of an enterprise user.
        /// </summary>
        [JsonPropertyName("Name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Name { get; set; }

        /// <summary>
        /// The surname of the reportee. Visible only for persons. Required input when performing delegation.
        /// </summary>
        [JsonPropertyName("LastName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LastName { get; set; }

        /// <summary>
        /// The username of an enterprise user or person with a username. Not visible in output, but required input when delegating a right to an enterprise user.
        /// </summary>
        [JsonPropertyName("UserName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserName { get; set; }

        /// <summary>
        /// Required input when performing delegation. The value is used to notify the entity receiving the new rights.
        /// </summary>
        [JsonPropertyName("Email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Email { get; set; }

        /// <summary>
        /// The social security number of the reportee if a person. This is in most cases hidden. Can be used in place of username when delegating rights to a person.
        /// </summary>
        [JsonPropertyName("SocialSecurityNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SocialSecurityNumber { get; set; }

        /// <summary>
        /// The organization number of the reportee. This is required input when delegating rights to an organization.
        /// </summary>
        [JsonPropertyName("OrganizationNumber")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string OrganizationNumber { get; set; }

        /// <summary>
        /// A list of existing roles given to the rights holder.
        /// </summary>
        [JsonPropertyName("Roles")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Roles Roles { get; set; }

        /// <summary>
        /// A list of existing rights given to the rights holder. (Most rights in this list has been given through a role.)
        /// </summary>
        [JsonPropertyName("Rights")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Rights Rights { get; set; }

        /// <summary>
        /// Struct of Roles and Rights lists used when making new delegations.
        /// </summary>
        [JsonPropertyName("_embedded")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public NewDelegations NewDelegations { get; set; }
    }

    public class Roles
    {
        [JsonPropertyName("_embedded")]
        public RolesEmbedded Embedded { get; set; }
    }

    public class RolesEmbedded
    {
        [JsonPropertyName("roles")]
        public List<Role> Roles { get; set; }
    }

    public class Rights
    {
        [JsonPropertyName("_embedded")]
        public RightsEmbedded Embedded { get; set; }
    }

    public class RightsEmbedded
    {
        [JsonPropertyName("rights")]
        public List<Right> Rights { get; set; }
    }

    public class NewDelegations
    {
        [JsonPropertyName("rights")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Right> Rights { get; set; }

        [JsonPropertyName("roles")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Role> Roles { get; set; }

    }
}