using System;
using System.Collections;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace LooksLikeIt.NET
{
    public class ImageHasher : IImageHasher
    {
        private const int DHashImageSizeInPixels = 8;
        private const int PHashImageSizeInPixels = 32;

        private Color GetContrastColor(Image<Rgba32> image)
        {
            long r, g , b;
            r = g = b = 0;
            long t = 0;
            for (int i = 0; i < image.Height; ++i)
            {
                var pixelsRowSpan = image.GetPixelRowSpan(i);
                for (int j = 0; j < image.Width; ++j)
                {
                    r += pixelsRowSpan[j].R;
                    g += pixelsRowSpan[j].G;
                    b += pixelsRowSpan[j].B;
                    ++t;
                }
            }

            float total = image.Height * image.Width;
            return new Rgba32(
                (byte)(r / total),
                (byte)(g / total),
                (byte)(b / total));
        }

        private long ComputeImageDHash(Image<Rgba32> image, Rgba32 averageColor)
        {
            int averageColorSum = averageColor.R + averageColor.G + averageColor.B;
            BitArray result = new BitArray(64);
            for (int i = 0; i < image.Height; ++i)
            {
                var pixelRowSpan = image.GetPixelRowSpan(i);
                for (int j = 0; j < image.Width; j++)
                {
                    int currentColorSum = pixelRowSpan[j].R + pixelRowSpan[j].G + pixelRowSpan[j].B;
                    result[i * image.Width + j] = currentColorSum >= averageColorSum;
                }
            }

            var bytes = new byte[sizeof(long)];
            result.CopyTo(bytes, 0);
            return BitConverter.ToInt64(bytes);
        }
        
        public string GetDHash(string pathToImage)
        {
            using var image = Image.Load<Rgba32>(pathToImage);
            image.Mutate(x => x.Resize(DHashImageSizeInPixels, DHashImageSizeInPixels));
            image.Mutate(x => x.Grayscale());
            image.SaveAsPng("1gray.png");
            var averageColor = GetContrastColor(image);
            return ComputeImageDHash(image, averageColor)
                .ToString("x8");
        }

        public string GetPHash(string pathToImage)
        {
            throw new System.NotImplementedException();
        }
    }
}