using Newtonsoft.Json;
using SqlSugar;

namespace TestWebApi.Entity
{
    [SugarTable]
    public class AppRole
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [JsonProperty, SugarColumn(ColumnName = "role_id", IsPrimaryKey = true, IsIdentity = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [JsonProperty, SugarColumn(ColumnName = "role_name", ColumnDataType = "varchar(60)")]
        public string RoleName { get; set; } = string.Empty;

        [SugarColumn(IsNullable =false)]
        public string Code { get; set; }
    }
}
