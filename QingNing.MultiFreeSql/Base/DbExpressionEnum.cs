using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QingNing.MultiFreeSql
{
    /// <summary>
    /// PostGIS单一谓词函数枚举
    /// </summary>
    public enum GeoSingleFuncEnum
    {
        [Display(Name = "ST_AsText")]
        ST_AsText = 0,
        [Display(Name = "ST_AsGeoJSON")]
        ST_AsGeoJSON = 1,
        [Display(Name = "ST_Area")]
        ST_Area = 2,
        [Display(Name = "ST_Length")]
        ST_Length = 3,
        [Display(Name = "ST_Boundary")]
        ST_Boundary = 4
    }
    /// <summary>
    /// PostGIS单一谓词函数枚举,返回值为Bool
    /// </summary>
    public enum GeoDoubleFuncEnum
    {
        [Display(Name = "ST_Contains")]
        ST_Contains = 0,
        [Display(Name = "ST_Covers")]
        ST_Covers = 1,
        [Display(Name = "ST_CoveredBy")]
        ST_CoveredBy = 2,
        [Display(Name = "ST_Intersects")]
        ST_Intersects = 3,
        [Display(Name = "ST_Touches")]
        ST_Touches = 4,
        [Display(Name = "ST_Within")]
        ST_Within = 5,
        [Display(Name = "ST_Disjoint")]
        ST_Disjoint = 6,
        [Display(Name = "ST_Crosses")]
        ST_Crosses = 7,
    }
    /// <summary>
    /// PostGIS单一谓词函数枚举,返回值为GeoJSON
    /// </summary>
    public enum GeoDoubleFuncWithGeomEnum
    {
        [Display(Name = "ST_Intersection")]
        ST_Intersection = 0,
        [Display(Name = "ST_Difference")]
        ST_Difference = 1
    }

}
