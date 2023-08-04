using System.Threading;

using FreeSql.DataAnnotations;

using NetTopologySuite.Geometries;

using Npgsql.LegacyPostgis;

namespace QingNing.MultiFreeSql
{
    [ExpressionCall]
    public static class BaseDbFuncExtend
    {
        //必要定义 static + ThreadLocal
        static ThreadLocal<ExpressionCallContext> context = new ThreadLocal<ExpressionCallContext>();
        #region 空间计算谓词-基于PostgisGeometry扩展
        #region 单一谓词

        /// <summary>
        /// PostGIS空间操作单一谓词表达式扩展，仅支持PostGIS
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public static T GeoExpression<T>(this PostgisGeometry @that, GeoSingleFuncEnum geoSingleFuncEnum)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"{geoSingleFuncEnum}({up.ParsedContent["that"]})";
            return default;
        }
        #endregion
        #region 双谓词
        /// <summary>
        /// PostGIS空间操扩展，仅支持PostGIS,返回值为bool作双谓词表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="GeoDoubleFuncEnum">双谓词操作枚举</param>
        /// <param name="argGeoJSON">GeoJSON 字符串</param>
        /// <returns></returns>
        public static T GeoExpression<T>(this PostgisGeometry @that, GeoDoubleFuncEnum geoDoubleFuncEnum, string argGeoJSON)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"{geoDoubleFuncEnum}({up.ParsedContent["that"]},ST_GeomFromGeoJSON({up.ParsedContent["argGeoJSON"]}))";
            return default;
        }
        /// <summary>
        /// PostGIS空间操扩展，仅支持PostGIS,返回值为bool作双谓词表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="GeoDoubleFuncEnum"></param>
        /// <param name="argGeom">postgisGeometry（字段）</param>
        /// <returns></returns>
        public static T GeoExpressionJoin<T>(this PostgisGeometry @that, GeoDoubleFuncEnum geoDoubleFuncEnum, object argGeom)
        {

            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"{geoDoubleFuncEnum}({up.ParsedContent["that"]},{up.ParsedContent["argGeom"]})";
            return default;
        }
        /// <summary>
        /// PostGIS空间操作双谓词表达式扩展，仅支持PostGIS,返回值为GeoJSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="GeoDoubleFuncWithGeomEnum"></param>
        /// <param name="argGeoJSON"></param>
        /// <returns></returns>
        public static T GeoExpression<T>(this PostgisGeometry @that, GeoDoubleFuncWithGeomEnum geoDoubleFuncWithGeomEnum, string argGeoJSON)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"ST_AsGeoJSON({geoDoubleFuncWithGeomEnum}({up.ParsedContent["that"]},ST_GeomFromGeoJSON({up.ParsedContent["argGeoJSON"]})))";
            return default;
        }
        /// <summary>
        /// PostGIS空间操作双谓词表达式扩展，仅支持PostGIS,返回值为GeoJSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="GeoDoubleFuncWithGeomEnum"></param>
        /// <param name="argGeom">postgisGeometry（字段）</param>
        /// <returns></returns>
        public static T GeoExpressionJoin<T>(this PostgisGeometry @that, GeoDoubleFuncWithGeomEnum geoDoubleFuncWithGeomEnum, object argGeom)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"ST_AsGeoJSON({geoDoubleFuncWithGeomEnum}({up.ParsedContent["that"]},{up.ParsedContent["argGeom"]}))";
            return default;
        }
        #endregion
        #endregion
        #region 空间计算谓词-基于NtsGeometry扩展
        #region 单一谓词

        /// <summary>
        /// PostGIS空间操作单一谓词表达式扩展，仅支持PostGIS
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public static T GeoExpression<T>(this Geometry @that, GeoSingleFuncEnum geoSingleFuncEnum)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"{geoSingleFuncEnum}({up.ParsedContent["that"]})";
            return default;
        }
        #endregion
        #region 双谓词
        /// <summary>
        /// PostGIS空间操扩展，仅支持PostGIS,返回值为bool作双谓词表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="GeoDoubleFuncEnum">双谓词操作枚举</param>
        /// <param name="argGeoJSON">GeoJSON 字符串</param>
        /// <returns></returns>
        public static T GeoExpression<T>(this Geometry @that, GeoDoubleFuncEnum geoDoubleFuncEnum, string argGeoJSON)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"{geoDoubleFuncEnum}({up.ParsedContent["that"]},ST_GeomFromGeoJSON({up.ParsedContent["argGeoJSON"]}))";
            return default;
        }
        /// <summary>
        /// PostGIS空间操扩展，仅支持PostGIS,返回值为bool作双谓词表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="GeoDoubleFuncEnum"></param>
        /// <param name="argGeom">Geometry（字段）</param>
        /// <returns></returns>
        public static T GeoExpressionJoin<T>(this Geometry @that, GeoDoubleFuncEnum geoDoubleFuncEnum, object argGeom)
        {

            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"{geoDoubleFuncEnum}({up.ParsedContent["that"]},{up.ParsedContent["argGeom"]})";
            return default;
        }
        /// <summary>
        /// PostGIS空间操作双谓词表达式扩展，仅支持PostGIS,返回值为GeoJSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="GeoDoubleFuncWithGeomEnum"></param>
        /// <param name="argGeoJSON"></param>
        /// <returns></returns>
        public static T GeoExpression<T>(this Geometry @that, GeoDoubleFuncWithGeomEnum geoDoubleFuncWithGeomEnum, string argGeoJSON)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"ST_AsGeoJSON({geoDoubleFuncWithGeomEnum}({up.ParsedContent["that"]},ST_GeomFromGeoJSON({up.ParsedContent["argGeoJSON"]})))";
            return default;
        }
        /// <summary>
        /// PostGIS空间操作双谓词表达式扩展，仅支持PostGIS,返回值为GeoJSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="GeoDoubleFuncWithGeomEnum"></param>
        /// <param name="argGeom">Geometry（字段）</param>
        /// <returns></returns>
        public static T GeoExpressionJoin<T>(this Geometry @that, GeoDoubleFuncWithGeomEnum geoDoubleFuncWithGeomEnum, object argGeom)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                context.Value.Result = $"ST_AsGeoJSON({geoDoubleFuncWithGeomEnum}({up.ParsedContent["that"]},{up.ParsedContent["argGeom"]}))";
            return default;
        }
        #endregion
        #endregion
        #region 其他表达式扩展
        //public static bool FormatDateTime22(this List<string> that, string arg1)
        //{
        //    var up = context.Value;
        //    if (up.DataType == FreeSql.DataType.Sqlite) //重写内容
        //        up.Result = $"date_format({up.ParsedContent["that"]}, {up.ParsedContent["arg1"]})";
        //    return true;
        //}
        /// <summary>
        /// Postgresql in 扩展
        /// </summary>
        /// <param name="that"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool InWithSql<T>(this T @that, T[] arg)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                                                            //context.Value.Result = $"{geoSingleFuncEnum}({up.ParsedContent["that"]})";
                context.Value.Result = $"{up.ParsedContent["that"]} in {up.ParsedContent["arg"]}";
            return true;
        }
        /// <summary>
        /// Postgresql not in 扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool NotInWithSql<T>(this T @that, T[] arg)
        {
            var up = context.Value;
            if (up.DataType == FreeSql.DataType.PostgreSQL) //重写内容
                                                            //context.Value.Result = $"{geoSingleFuncEnum}({up.ParsedContent["that"]})";
                context.Value.Result = $"{up.ParsedContent["that"]} not in {up.ParsedContent["arg"]}";
            return true;
        }

        #endregion

    }
}
