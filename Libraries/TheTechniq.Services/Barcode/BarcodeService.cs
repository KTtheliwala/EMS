using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using System.Collections.Generic;
using System.Data;
using LinqToDB.Data;

using TheTecniQ.Core;
using TheTecniQ.Core.Domain.Logging;
using TheTecniQ.Data;
using LinqToDB.Linq.Builder;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Data.DataProviders;

using TheTecniQ.Core.Domain.User;
using TheTecniQ.Core.Infrastructure;
using TheTecniQ.Services.Common;
using System.Drawing;
using System.IO;
using ZXing;
using ZXing.Common;
using System.Drawing.Imaging;


namespace TheTecniQ.Services.Barcode
{ 
        public class BarcodeService
        {
            public byte[] GenerateBarcode(string content)
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128, // You can change the format as needed
                    Options = new EncodingOptions
                    {
                        Height = 45,
                        Width = 200,
                        Margin = 0
                    }
                };

                var pixelData = writer.Write(content);

                using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
                {
                    using (var ms = new MemoryStream())
                    {
                        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                        try
                        {
                            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                        }
                        finally
                        {
                            bitmap.UnlockBits(bitmapData);
                        }

                        bitmap.Save(ms, ImageFormat.Png);
                        return ms.ToArray();
                    }
                }
            }
        }
    
}