using DALSA.SaperaLT.SapClassBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huoyan.XWDalsa.Configurature
{
    class GloableConst
    {
        public const int UNSUPPORTED = -1;       // Not supported
        public const int NO_CONVERSION = 0;       // Supported without conversion
        public const int CONV_TO_MONO8 = 1;       // Supported but converted to MONO8
        public const int CONV_TO_RGB8 = 2;       // Supported but converted to RGB888
        public const int CONV_TO_RGB10 = 3;       // Supported but converted to RGB101010
        public const int CONV_TO_RGB16 = 4;       // Supported but converted to RGB161616

        public static ConversionTable[] m_ConversionTable =
          {
         new ConversionTable(SapFormat.Mono8, new int[] {NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION}),
         new ConversionTable(SapFormat.Int8, new int[] {CONV_TO_MONO8, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, NO_CONVERSION}),
         new ConversionTable(SapFormat.Uint8, new int[] {NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION}),
         new ConversionTable(SapFormat.Mono16, new int[] {CONV_TO_MONO8, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, CONV_TO_MONO8, NO_CONVERSION}),
         new ConversionTable(SapFormat.Int16, new int[] {CONV_TO_MONO8, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, NO_CONVERSION}),
         new ConversionTable(SapFormat.Uint16, new int[] {CONV_TO_MONO8, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, CONV_TO_MONO8, NO_CONVERSION}),
         new ConversionTable(SapFormat.Mono32, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Int32, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Uint32, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Mono64, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Int64, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Uint64, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.RGB5551, new int[] {NO_CONVERSION, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.RGB565, new int[] {NO_CONVERSION, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, NO_CONVERSION}),
         new ConversionTable(SapFormat.RGB888, new int[] {NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION}),
         new ConversionTable(SapFormat.RGBR888, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, CONV_TO_RGB8, CONV_TO_RGB8}),
         new ConversionTable(SapFormat.RGB8888, new int[] {NO_CONVERSION, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION}),
         new ConversionTable(SapFormat.RGB101010, new int[] {NO_CONVERSION, CONV_TO_RGB16, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, NO_CONVERSION}),
         new ConversionTable(SapFormat.RGB161616, new int[] {CONV_TO_RGB10, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, NO_CONVERSION}),
         new ConversionTable(SapFormat.RGB16161616, new int[] {CONV_TO_RGB10, CONV_TO_RGB16, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.HSV, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.UYVY, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, CONV_TO_RGB8, CONV_TO_RGB8}),
         new ConversionTable(SapFormat.YUY2, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, CONV_TO_RGB8, CONV_TO_RGB8}),
         new ConversionTable(SapFormat.YVYU, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, CONV_TO_RGB8, CONV_TO_RGB8}),
         new ConversionTable(SapFormat.YUYV, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, CONV_TO_RGB8, CONV_TO_RGB8}),
         new ConversionTable(SapFormat.Y411, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Y211, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.YUV, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Float, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Complex, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Point, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.FPoint, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Mono1, new int[] {NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.HSI, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.RGBP8, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, NO_CONVERSION}),
         new ConversionTable(SapFormat.RGBP16, new int[] {CONV_TO_RGB10, CONV_TO_RGB16, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.RGBAP8, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.RGBAP16, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.BICOLOR88, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.BICOLOR1616, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.BICOLOR1212, new int[] {CONV_TO_RGB8, CONV_TO_RGB8, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.RGB888_MONO8, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.RGB161616_MONO16, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Mono8P2, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Mono8P3, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Mono8P4, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Mono16P2, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Mono16P3, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED}),
         new ConversionTable(SapFormat.Mono16P4, new int[] {UNSUPPORTED, UNSUPPORTED, NO_CONVERSION, NO_CONVERSION, UNSUPPORTED, UNSUPPORTED})
      };
    }

    class ConversionTable
    {
        public SapFormat m_BufferFormat;
        // Support for saving respectively in BMP, TIFF, CRC, RAW, JPEG and JPEG2000
        public int[] m_FileFormat;

        public ConversionTable(SapFormat bufferFormat, int[] fileFormat)
        {
            m_BufferFormat = bufferFormat;
            m_FileFormat = fileFormat;
        }
    };

    /// <summary>
    /// Enum of server category
    /// </summary>
    public enum ServerCategory
    {
        ServerAll,
        ServerAcq,
        ServerAcqDevice
    };
    

}
