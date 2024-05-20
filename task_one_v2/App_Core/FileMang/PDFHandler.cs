using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.AspNetCore.Hosting; 

namespace task_one_v2.App_Core.FileHelper
{
    public class PdfData
    {
        public string ChefName { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string Ingredients { get; set; }
        public string Procedure { get; set; }
        public string RecipeImg { get; set; }
    }

    public static class PDFHandler
    {
        public static void GeneratePdfOrder(IWebHostEnvironment hostingEnvironment, PdfData recipeData)
        {
            string pdfPath = Path.Combine(hostingEnvironment.WebRootPath, "PDF", "Recipe.pdf");

            using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document doc = new Document())
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Load favicon image
                    string faviconPath = Path.Combine(hostingEnvironment.WebRootPath, "Globalimg", "favicon.jpg");
                    Image favicon = Image.GetInstance(faviconPath);
                    favicon.ScaleAbsolute(175, 175);

                    // Create a circular clipping path for the favicon image
                    PdfTemplate mask1 = writer.DirectContent.CreateTemplate(175, 175);
                    mask1.Ellipse(0, 0, 175, 175);
                    mask1.Clip();
                    mask1.NewPath();
                    mask1.AddImage(favicon, 175, 0, 0, 175, 0, 0);

                    Image clippedImage1 = Image.GetInstance(mask1);
                    clippedImage1.Alignment = Image.ALIGN_CENTER;
                    doc.Add(clippedImage1);
                    doc.Add(new Paragraph("\n")); 

                    string recipeImagePath = Path.Combine(hostingEnvironment.WebRootPath, "Images", recipeData.RecipeImg);
                    if (!string.IsNullOrEmpty(recipeImagePath) && File.Exists(recipeImagePath))
                    {
                        Image recipeImage = Image.GetInstance(recipeImagePath);
                        recipeImage.ScaleToFit(120, 120);

                        PdfTemplate mask2 = writer.DirectContent.CreateTemplate(175, 175);
                        mask2.Ellipse(0, 0, 175, 175);
                        mask2.Clip();
                        mask2.NewPath();
                        mask2.AddImage(recipeImage, 175, 0, 0, 175, 0, 0);

                        Image clippedImage2 = Image.GetInstance(mask2);
                        clippedImage2.Alignment = Image.LEFT_ALIGN;
                        doc.Add(clippedImage2);
                        doc.Add(new Paragraph("\n"));
                        doc.Add(new Paragraph("\n")); 
                        doc.Add(new Paragraph("\n")); 

                    }

                    doc.AddAuthor(recipeData.ChefName);
                    doc.Add(new Paragraph($"Recipe Name: {recipeData.RecipeName}"));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"Description: {recipeData.Description}"));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"Category Name: {recipeData.CategoryName}"));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"Ingredients: {recipeData.Ingredients}"));
                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"Steps: {recipeData.Procedure}"));
                    doc.Close();
                }
            }
        }
    }
}
