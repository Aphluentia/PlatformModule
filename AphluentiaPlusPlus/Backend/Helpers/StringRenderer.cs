using System.Text;
using ZXing.Common;
using ZXing.Rendering;
using ZXing;

namespace Backend.Helpers
{
    public class StringRenderer : IBarcodeRenderer<string>
    {
        public string Render(BitMatrix matrix, BarcodeFormat format, string content)
        {
            var result = new StringBuilder();

            for (int y = 0; y < matrix.Height; y++)
            {
                for (int x = 0; x < matrix.Width; x++)
                {
                    result.Append(matrix[x, y] ? "X" : " ");
                }

                result.AppendLine();
            }

            return result.ToString();
        }

        public string Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

