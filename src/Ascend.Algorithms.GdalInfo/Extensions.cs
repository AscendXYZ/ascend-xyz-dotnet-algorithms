using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascend.Algorithms.GdalInfo
{
    public static class Extensions
    {
        public static GDALInfoGCP AsGDALInfoGCP(this GCP gcp)
        {
            return new GDALInfoGCP{
                GCPLine = gcp.GCPLine,
                GCPPixel = gcp.GCPPixel,
                GCPX = gcp.GCPX,
                GCPY = gcp.GCPY,
                GCPZ = gcp.GCPZ,
                Id = gcp.Id,
                Info = gcp.Info,
            };
        }

        public static GDALInfoBandInfo AsGDALInfoBandInfo(this Band band)
        {
            int blockXSize, blockYSize;
            band.GetBlockSize(out blockXSize, out blockYSize);

            return new GDALInfoBandInfo
            {
                DataTypeStr = Gdal.GetDataTypeName(band.DataType),
                ColorInterpretation = Gdal.GetColorInterpretationName(band.GetRasterColorInterpretation()),
                XSize = band.XSize,
                YSize = band.YSize,
                BlockXSize = blockXSize,
                BlockYSize = blockYSize
            };
        }
    }
}
