using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace TestWebApi.Entity
{
    [JsonObject(MemberSerialization.OptIn), Table(Name = "app_role")]
    public class AppRole
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [JsonProperty, Column(Name = "role_id", IsPrimary = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [JsonProperty, Column(Name = "role_name", DbType = "varchar(60)")]
        public string RoleName { get; set; } = string.Empty;
    }
}
