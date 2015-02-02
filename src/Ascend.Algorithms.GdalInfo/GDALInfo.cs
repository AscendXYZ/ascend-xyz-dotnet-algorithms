/******************************************************************************
 * $Id: GDALInfo.cs 18705 2010-02-02 21:11:43Z tamas $
 *
 * Name:     GDALInfo.cs
 * Project:  GDAL CSharp Interface
 * Purpose:  A sample app to read GDAL raster data information.
 * Author:   Tamas Szekeres, szekerest@gmail.com
 *
 ******************************************************************************
 * Copyright (c) 2007, Tamas Szekeres
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 *****************************************************************************/

using System;

using OSGeo.GDAL;
using OSGeo.OSR;
using Ascend.Algorithms.Interface;
using Ascend.Algorithms.GdalInfo;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

/**

 * <p>Title: GDAL C# GDALRead example.</p>
 * <p>Description: A sample app to read GDAL raster data information.</p>
 * @author Tamas Szekeres (szekerest@gmail.com)
 * @version 1.0
 */



/// <summary>
/// A C# based sample to read GDAL raster data information.
/// </summary> 

namespace Ascend.Algorithms.GdalInfo
{
    public class GDALInfoAlgorithm : AscendAlgorithm<GDALInfoInput, GDALInfoResult>
    {





        private static double[] GDALInfoGetPosition(Dataset ds, double x, double y)
        {
            double[] adfGeoTransform = new double[6];
            double dfGeoX, dfGeoY;
            ds.GetGeoTransform(adfGeoTransform);

            dfGeoX = adfGeoTransform[0] + adfGeoTransform[1] * x + adfGeoTransform[2] * y;
            dfGeoY = adfGeoTransform[3] + adfGeoTransform[4] * x + adfGeoTransform[5] * y;
            return new[] { dfGeoX, dfGeoY };
            //return dfGeoX.ToString() + ", " + dfGeoY.ToString();
        }

        public override Task<GDALInfoResult> RunAlgorithmAsync()
        {

            var result = new GDALInfoResult();
            try
            {
                /* -------------------------------------------------------------------- */
                /*      Register driver(s).                                             */
                /* -------------------------------------------------------------------- */
                Gdal.AllRegister();

                /* -------------------------------------------------------------------- */
                /*      Open dataset.                                                   */
                /* -------------------------------------------------------------------- */
                Dataset ds = Gdal.Open(this.Payload.GdalDataSource, Access.GA_ReadOnly);

                if (ds == null)
                {
                    Console.WriteLine("Can't open " + Payload.GdalDataSource);
                    throw new Exception("Cant open: " + Payload.GdalDataSource);
                }

                Console.WriteLine("Raster dataset parameters:");
                Console.WriteLine("  Projection: " + ds.GetProjectionRef());
                Console.WriteLine("  RasterCount: " + ds.RasterCount);
                Console.WriteLine("  RasterSize (" + ds.RasterXSize + "," + ds.RasterYSize + ")");


                result.RasterCount = ds.RasterCount;
                result.RasterSize = new[] { ds.RasterXSize, ds.RasterYSize };

                /* -------------------------------------------------------------------- */
                /*      Get driver                                                      */
                /* -------------------------------------------------------------------- */
                Driver drv = ds.GetDriver();

                if (drv == null)
                {
                    Console.WriteLine("Can't get driver.");
                    System.Environment.Exit(-1);
                }

                Console.WriteLine("Using driver " + drv.LongName);

                /* -------------------------------------------------------------------- */
                /*      Get metadata                                                    */
                /* -------------------------------------------------------------------- */
                string[] metadata = ds.GetMetadata("");
                if (metadata.Length > 0)
                {
                    Console.WriteLine("  Metadata:");
                    for (int iMeta = 0; iMeta < metadata.Length; iMeta++)
                    {
                        Console.WriteLine("    " + iMeta + ":  " + metadata[iMeta]);
                        var meta = metadata[iMeta].Split('=');
                        result.MetaData.Add(meta[0], string.Join("=", meta.Skip(1)));
                    }
                    Console.WriteLine("");
                }

                /* -------------------------------------------------------------------- */
                /*      Report "IMAGE_STRUCTURE" metadata.                              */
                /* -------------------------------------------------------------------- */
                metadata = ds.GetMetadata("IMAGE_STRUCTURE");
                if (metadata.Length > 0)
                {
                    Console.WriteLine("  Image Structure Metadata:");
                    for (int iMeta = 0; iMeta < metadata.Length; iMeta++)
                    {
                        Console.WriteLine("    " + iMeta + ":  " + metadata[iMeta]);
                        var meta = metadata[iMeta].Split('=');
                        result.ImageStructureMetaData.Add(meta[0], string.Join("=", meta.Skip(1)));
                    }
                    Console.WriteLine("");
                }

                /* -------------------------------------------------------------------- */
                /*      Report subdatasets.                                             */
                /* -------------------------------------------------------------------- */
                metadata = ds.GetMetadata("SUBDATASETS");
                if (metadata.Length > 0)
                {
                    Console.WriteLine("  Subdatasets:");
                    for (int iMeta = 0; iMeta < metadata.Length; iMeta++)
                    {
                        Console.WriteLine("    " + iMeta + ":  " + metadata[iMeta]);
                    }
                    Console.WriteLine("");
                }

                /* -------------------------------------------------------------------- */
                /*      Report geolocation.                                             */
                /* -------------------------------------------------------------------- */
                metadata = ds.GetMetadata("GEOLOCATION");
                if (metadata.Length > 0)
                {
                    Console.WriteLine("  Geolocation:");
                    for (int iMeta = 0; iMeta < metadata.Length; iMeta++)
                    {
                        Console.WriteLine("    " + iMeta + ":  " + metadata[iMeta]);
                    }
                    Console.WriteLine("");
                }

                /* -------------------------------------------------------------------- */
                /*      Report corners.                                                 */
                /* -------------------------------------------------------------------- */
                Console.WriteLine("Corner Coordinates:");
                Console.WriteLine("  Upper Left (" + GDALInfoGetPosition(ds, 0.0, 0.0) + ")");
                Console.WriteLine("  Lower Left (" + GDALInfoGetPosition(ds, 0.0, ds.RasterYSize) + ")");
                Console.WriteLine("  Upper Right (" + GDALInfoGetPosition(ds, ds.RasterXSize, 0.0) + ")");
                Console.WriteLine("  Lower Right (" + GDALInfoGetPosition(ds, ds.RasterXSize, ds.RasterYSize) + ")");
                Console.WriteLine("  Center (" + GDALInfoGetPosition(ds, ds.RasterXSize / 2, ds.RasterYSize / 2) + ")");
                Console.WriteLine("");

                result.ADFGeoTransform = new double[6];
                ds.GetGeoTransform(result.ADFGeoTransform);

                result.CornerCoordinates = new GDALCornerCoordinates
                {
                    UpperLeft = GDALInfoGetPosition(ds, 0.0, 0.0),
                    LowerLeft = GDALInfoGetPosition(ds, 0.0, ds.RasterYSize),
                    UpperRight = GDALInfoGetPosition(ds, ds.RasterXSize, 0.0),
                    LowerRight = GDALInfoGetPosition(ds, ds.RasterXSize, ds.RasterYSize),
                    Center = GDALInfoGetPosition(ds, ds.RasterXSize / 2, ds.RasterYSize / 2)

                };

                /* -------------------------------------------------------------------- */
                /*      Report projection.                                              */
                /* -------------------------------------------------------------------- */
                string projection = ds.GetProjectionRef();
                result.Projection = GetProjection(projection);

                /* -------------------------------------------------------------------- */
                /*      Report GCPs.                                                    */
                /* -------------------------------------------------------------------- */
                if (ds.GetGCPCount() > 0)
                {
                    Console.WriteLine("GCP Projection: ", ds.GetGCPProjection());
                    result.GCPProjection = GetProjection(ds.GetGCPProjection());
                    GCP[] GCPs = ds.GetGCPs();
                    result.GCPs = GCPs.Select(g => g.AsGDALInfoGCP()).ToArray();
                    for (int i = 0; i < ds.GetGCPCount(); i++)
                    {
                        Console.WriteLine("GCP[" + i + "]: Id=" + GCPs[i].Id + ", Info=" + GCPs[i].Info);
                        Console.WriteLine("          (" + GCPs[i].GCPPixel + "," + GCPs[i].GCPLine + ") -> ("
                                    + GCPs[i].GCPX + "," + GCPs[i].GCPY + "," + GCPs[i].GCPZ + ")");
                        Console.WriteLine("");
                    }
                    Console.WriteLine("");

                    result.GCPsToGeoTransform = new double[6];
                    Gdal.GCPsToGeoTransform(GCPs, result.GCPsToGeoTransform, 0);
                    Console.WriteLine("GCP Equivalent geotransformation parameters: ", ds.GetGCPProjection());
                    for (int i = 0; i < 6; i++)
                        Console.WriteLine("t[" + i + "] = " + result.GCPsToGeoTransform[i].ToString());
                    Console.WriteLine("");
                }

                /* -------------------------------------------------------------------- */
                /*      Get raster band                                                 */
                /* -------------------------------------------------------------------- */
                var bands = new List<GDALInfoBandInfo>();
                for (int iBand = 1; iBand <= ds.RasterCount; iBand++)
                {
                    Band band = ds.GetRasterBand(iBand);
                    Console.WriteLine("Band " + iBand + " :");
                    Console.WriteLine("   DataType: " + Gdal.GetDataTypeName(band.DataType));
                    Console.WriteLine("   ColorInterpretation: " + Gdal.GetColorInterpretationName(band.GetRasterColorInterpretation()));
                    ColorTable ct = band.GetRasterColorTable();
                    if (ct != null)
                        Console.WriteLine("   Band has a color table with " + ct.GetCount() + " entries.");

                    Console.WriteLine("   Description: " + band.GetDescription());
                    Console.WriteLine("   Size (" + band.XSize + "," + band.YSize + ")");
                    int BlockXSize, BlockYSize;
                    band.GetBlockSize(out BlockXSize, out BlockYSize);
                    Console.WriteLine("   BlockSize (" + BlockXSize + "," + BlockYSize + ")");
                    double val;
                    int hasval;
                    band.GetMinimum(out val, out hasval);
                    if (hasval != 0) Console.WriteLine("   Minimum: " + val.ToString());
                    band.GetMaximum(out val, out hasval);
                    if (hasval != 0) Console.WriteLine("   Maximum: " + val.ToString());
                    band.GetNoDataValue(out val, out hasval);
                    if (hasval != 0) Console.WriteLine("   NoDataValue: " + val.ToString());
                    band.GetOffset(out val, out hasval);
                    if (hasval != 0) Console.WriteLine("   Offset: " + val.ToString());
                    band.GetScale(out val, out hasval);
                    if (hasval != 0) Console.WriteLine("   Scale: " + val.ToString());

                    for (int iOver = 0; iOver < band.GetOverviewCount(); iOver++)
                    {
                        Band over = band.GetOverview(iOver);
                        Console.WriteLine("      OverView " + iOver + " :");
                        Console.WriteLine("         DataType: " + over.DataType);
                        Console.WriteLine("         Size (" + over.XSize + "," + over.YSize + ")");
                        Console.WriteLine("         PaletteInterp: " + over.GetRasterColorInterpretation().ToString());
                    }
                    bands.Add(band.AsGDALInfoBandInfo());
                }
                result.Bands = bands.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine("Application error: " + e.Message);
            }

            return Task.FromResult(result);
        }

        private static GDALInfoProjection GetProjection(string projection)
        {
            var proj = new GDALInfoProjection();
            proj.Raw = projection;

            if (projection != null)
            {
                SpatialReference srs = new SpatialReference(null);
                if (srs.ImportFromWkt(ref projection) == 0)
                {
                    string wkt;
                    srs.ExportToPrettyWkt(out wkt, 0);
                    proj.Wkt = wkt;
                    srs.ExportToProj4(out wkt);
                    proj.Proj4 = wkt;
                }
                else
                {

                }
            }
            return proj;
        }
    }
}