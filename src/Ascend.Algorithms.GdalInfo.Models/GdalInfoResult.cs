
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascend.Algorithms.GdalInfo
{
    public class GDALInfoProjection
    {

        public string Wkt { get; set; }
        public string Proj4 { get; set; }

        public string Raw { get; set; }
    }
    public class GDALInfoGCP
    {
        public GDALInfoGCP()
        {

        }
       
        public double GCPLine { get; set; }
        public double GCPPixel { get; set; }
        public double GCPX { get; set; }
        public double GCPY { get; set; }
        public double GCPZ { get; set; }
        public string Id { get; set; }
        public string Info { get; set; }
    }
    public class GDALInfoBandInfo
    {
        public GDALInfoBandInfo()
        {

        }
        
        public int DataType { get; set; }
        public string DataTypeStr { get; set; }
        public string ColorInterpretation { get; set; }
        public int XSize { get; set; }
        public int YSize { get; set; }
        public int BlockXSize { get; set; }
        public int BlockYSize { get; set; }
    }
    public class GDALCornerCoordinates
    {
        public double[] UpperLeft { get; set; }
        public double[] LowerLeft { get; set; }
        public double[] UpperRight { get; set; }
        public double[] LowerRight { get; set; }
        public double[] Center { get; set; }
    }
    public class GDALInfoResult
    {
        public GDALInfoResult()
        {
            MetaData = new Dictionary<string, string>();
            ImageStructureMetaData = new Dictionary<string, string>();
        }
        public GDALInfoProjection Projection { get; set; }
        public int RasterCount { get; set; }
        public int[] RasterSize { get; set; }

        public Dictionary<string, string> MetaData { get; set; }
        public Dictionary<string, string> ImageStructureMetaData { get; set; }

        public GDALCornerCoordinates CornerCoordinates { get; set; }
        public double[] ADFGeoTransform { get; set; }

        public GDALInfoBandInfo[] Bands { get; set; }


        public GDALInfoProjection GCPProjection { get; set; }
        public GDALInfoGCP[] GCPs { get; set; }
        public double[] GCPsToGeoTransform { get; set; }

    }
    public class GDALInfoInput
    {
        public string GdalDataSource { get; set; }
    }

}
