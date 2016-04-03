#region Using directives

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPECentral.Data.EF5;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

#endregion

namespace SalesOrderParser
{
    public static class PdfParser
    {
        public static async Task<SalesOrderDetail> ParseSalesOrderAsync(string path, string mailId, string orderNumberExpr, string deliveryExpr,
            string buyerExpr, string drawingNumberExpr)
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    var parsedText = ExtractText(path);

                    if (string.IsNullOrWhiteSpace(parsedText))
                    {
                        // TODO: throw custom exception here
                        return null;
                    }

                    var deliveryRegex = new Regex(deliveryExpr);
                    var buyerRegegx = new Regex(buyerExpr);
                    var orderRegex = new Regex(orderNumberExpr);
                    var dwgRegex = new Regex(drawingNumberExpr);

                    var deliveryMatch = deliveryRegex.Match(parsedText);
                    var orderNumberMatch = orderRegex.Match(parsedText);
                    var buyerMatch = buyerRegegx.Match(parsedText);
                    var drawingMatch = dwgRegex.Match(parsedText);

                    byte[] photoBytes = null;
                    

                    var result = new SalesOrderDetail
                    {
                        OrderNumber = orderNumberMatch.Success ? orderNumberMatch.Value.Trim() : "UNABLE TO PARSE!",
                        MailId = mailId,
                        DeliveryDate =
                            deliveryMatch.Success ? DateTime.Parse(deliveryMatch.Value) : DateTime.MinValue,
                        DrawingNumber = drawingMatch.Success ? drawingMatch.Value : "UNABLE TO PARSE!",
                        Buyer = buyerMatch.Success ? buyerMatch.Value : "UNABLE TO PARSE!",
                        FileName = path
                    };

                    return result;
                }
                catch (Exception ex)
                {
                    // TODO: throw custom exception here
                }

                return null;
            });
        }

        private static string ExtractText(string path)
        {
            var textBuilder = new StringBuilder();

            try
            {
                var pdfReader = new PdfReader(path);
                for (var page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    var currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
                    textBuilder.Append(Environment.NewLine);
                    textBuilder.Append("\n Page Number:" + page);
                    textBuilder.Append(Environment.NewLine);
                    currentText =
                        Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8,
                            Encoding.Default.GetBytes(currentText)));
                    textBuilder.Append(currentText);
                }

                pdfReader.Close();
            }
            catch (Exception ex)
            {
                // TODO: throw custom exception here
            }

            return textBuilder.ToString();
        }
    }
}