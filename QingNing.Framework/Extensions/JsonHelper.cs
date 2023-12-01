using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Data;

namespace QingNing.Extensions;
public static class JsonHelper
{
    #region ToJson


    public static object ToJson(this string Json)
    {
        return Json == null ? null : JsonConvert.DeserializeObject(Json);
    }
    public static string ToJson(this object obj)
    {
        var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
        return JsonConvert.SerializeObject(obj, timeConverter);
    }
    public static string ToJson(this object obj, string datetimeformats)
    {
        var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
        return JsonConvert.SerializeObject(obj, timeConverter);
    }

    /// <summary>
    ///     将对象序列化为JSON字符串，支持存在循环引用的对象
    /// </summary>
    /// <typeparam name="T">动态类型</typeparam>
    /// <param name="value">动态类型对象</param>
    /// <param name="tolower">是否将属性转为首字母小写形式</param>
    /// <returns>JSON字符串</returns>
    public static string ToJson<T>(this T value, bool tolower = false)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        if (!tolower)
            return JsonConvert.SerializeObject(value, settings);
        settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        return JsonConvert.SerializeObject(value, settings);
    }

    #endregion

    #region ToObject<T>   字符串转json对象
    /// <summary>
    ///     字符串转换为json对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="tolower"></param>
    /// <returns></returns>
    public static T ToObject<T>(this string value, bool tolower = true)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        if (!tolower)
            return JsonConvert.DeserializeObject<T>(value, settings);
        settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        return JsonConvert.DeserializeObject<T>(value, settings);
    }

    public static T ToObject<T>(this string Json)
    {
        return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
    }

    #endregion


    public static List<T> ToList<T>(this string Json)
    {
        return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
    }
    public static DataTable ToTable(this string Json)
    {
        return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
    }
    public static JObject ToJObject(this string Json)
    {
        return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
    }
    public static Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
    {
        if (string.IsNullOrEmpty(jsonStr))
            return new Dictionary<TKey, TValue>();

        Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

        return jsonDict;

    }
}
