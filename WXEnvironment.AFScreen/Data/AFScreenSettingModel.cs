using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using WXEnvironment.MongoDB;

#pragma warning disable CS8603 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.

namespace WXEnvironment.AFScreen.Data
{
    /// <summary>
    /// 表单/大屏的配置信息
    /// <para>a：不存入数据库，返回显示</para>
    /// <para>b：存入数据库，不返回</para>
    /// </summary>
    [BsonIgnoreExtraElements]
    public class AFScreenSettingModel : BaseData
    {

        /// <summary>
        /// Info-Id
        /// </summary>
        [BsonElement("info_id")]
        public string? InfoId { get; set; }

        /// <summary>
        /// Info-名称
        /// </summary>
        [BsonElement("info_name")]
        public string? InfoName { get; set; }
        /// <summary>
        /// Info-宽
        /// </summary>
        [BsonElement("info_width")]
        public int InfoWidth { get; set; }
        /// <summary>
        /// Info-高
        /// </summary>
        [BsonElement("info_height")]
        public int InfoHeight { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        [BsonElement("belong_user_id")]
        public string? BelongUserId { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        [BsonElement("belong_user_name")]
        public string? BelongUserName { get; set; }

        /// <summary>
        /// 背景类型  color | image
        /// </summary>
        [BsonElement("bg_type")]
        public string? BgType { get; set; }
        /// <summary>
        /// 背景颜色
        /// </summary>
        [BsonElement("bg_color")]
        public string? BgColor { get; set; }
        /// <summary>
        /// 背景图片地址
        /// </summary>
        [BsonElement("bg_image")]
        public string? BgImage { get; set; }

        /// <summary>
        /// 背景粒子效果
        /// </summary>
        [BsonElement("bg_particles")]
        public string? BgParticles { get; set; }

        /// <summary>
        /// 激活控件
        /// </summary>
        [BsonElement("active_element_id")]
        public string? ActiveElementId { get; set; }

        /// <summary>
        /// 额外字段
        /// <para>BsonExtraElements：额外的字段统一至此</para>
        /// <para>JsonExtensionData：序列化JSON时展开到顶级(只能是System.Text.Json)</para>
        /// </summary>
        [BsonExtraElements]
        [JsonExtensionData]
        public Dictionary<string, object> ExtraFields { get; set; } = [];

    }

    /// <summary>
    /// 搜索
    /// </summary>
    public class QueryAFScreenSetting : BaseQuery<AFScreenSettingModel>
    {
        /// <summary>
        /// Info-Id
        /// </summary>
        public string? InfoId { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public string? BelongUserId { get; set; }

        /// <summary>
        /// 背景类型  color | image
        /// </summary>
        public string? BgType { get; set; }

        /// <summary>
        /// 处理搜索
        /// </summary>
        /// <param name="Filter"></param>
        /// <param name="filters"></param>
        protected override void FillFilters(FilterDefinitionBuilder<AFScreenSettingModel> Filter, List<FilterDefinition<AFScreenSettingModel>> filters)
        {
            filters.Add(Filter.EqIfNotNull(c => c.FlagDelete, false));

            filters.Add(Filter.EqIfNotEmpty(c => c.Id, this.Id));
            filters.Add(Filter.InIfNotNull(c => c.Id, this.Ids));
            filters.Add(Filter.EqIfNotEmpty(c => c.DocVersion, this.DocVersion));
            filters.Add(Filter.InIfNotNull(c => c.DocVersion, this.DocVersions));

            filters.Add(Filter.EqIfNotEmpty(c => c.InfoId, this.InfoId));
            filters.Add(Filter.EqIfNotEmpty(c => c.BelongUserId, this.BelongUserId));
            filters.Add(Filter.EqIfNotEmpty(c => c.BgType, this.BgType));

            filters.Add(Filter.OrIfNotNull(
                Filter.Like(c => c.BelongUserName, KeyWord)
            ));
        }
    }
}
