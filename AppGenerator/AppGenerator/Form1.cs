using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace AppGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateButton(object sender, EventArgs e)
        {
            string generatedHtmlString = string.Empty;

            StringBuilder sb = new StringBuilder("<!DOCTYPE html>" + Environment.NewLine);

            StringWriter stringWriter = new StringWriter(sb);

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);
                writer.RenderBeginTag(HtmlTextWriterTag.Head);

                writer.AddAttribute("charset", "utf-8");
                writer.RenderBeginTag(HtmlTextWriterTag.Meta);
                writer.RenderEndTag(); //end meta
                writer.WriteLine();

                writer.RenderBeginTag(HtmlTextWriterTag.Title);
                writer.Write("Base.html");
                writer.RenderEndTag(); //end title
                writer.RenderEndTag(); //end head
                writer.WriteLine();

                writer.RenderBeginTag(HtmlTextWriterTag.Body);


                writer.RenderEndTag(); //end body
                writer.RenderEndTag(); //end html
            }
            generatedHtmlString = stringWriter.ToString();

            #region Izmena csproj fajla
            string csprojPath = @"C:\Users\nikola.bastovanovic\source\repos\GeneratedWebApp\GeneratedWebApp\GeneratedWebApp.csproj";
            string csprojEdited = File.ReadAllText(csprojPath);
            if (csprojEdited.Contains(@"<Content Include=""base.html""") == false) //zakucan base.html
            {
                int positionToIncludeHTML = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
                csprojEdited = csprojEdited.Insert(positionToIncludeHTML, @"<Content Include=""base.html""" + " />" + Environment.NewLine + "\t"); //zakucan base.html
                File.WriteAllText(csprojPath, csprojEdited);
            }
            #endregion

            #region Kreiranje fajla base.html
            string path = @"C:\Users\nikola.bastovanovic\source\repos\GeneratedWebApp\GeneratedWebApp\base.html";

            if (File.Exists(path))
            {
                File.Delete(path);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(generatedHtmlString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(generatedHtmlString);
                }
            }
            #endregion
        }
    }
}
