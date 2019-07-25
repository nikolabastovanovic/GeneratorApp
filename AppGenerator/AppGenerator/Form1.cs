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
                writer.WriteLine();

                writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "HelpCSS.css");
                writer.RenderBeginTag(HtmlTextWriterTag.Link);
                writer.RenderEndTag(); //end link
                writer.RenderEndTag(); //end head
                writer.WriteLine();
                

                writer.RenderBeginTag(HtmlTextWriterTag.Body);

                writer.AddAttribute(HtmlTextWriterAttribute.Src, "JavaScript.js");
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.RenderEndTag(); //end script
                writer.RenderEndTag(); //end body
                writer.RenderEndTag(); //end html
            }
            generatedHtmlString = stringWriter.ToString();

            #region Izmena csproj fajla
            string csprojPath = @"C:\Users\nikola.bastovanovic\source\repos\GeneratedWebApp\GeneratedWebApp\GeneratedWebApp.csproj";
            string csprojEdited = File.ReadAllText(csprojPath);
            int positionToIncludeHTML = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
            if (csprojEdited.Contains(@"<Content Include=""base.html""") == false) //zakucan base.html
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeHTML, @"<Content Include=""base.html""" + " />" + Environment.NewLine + "\t"); //zakucan base.html
                File.WriteAllText(csprojPath, csprojEdited);
            }
            if (csprojEdited.Contains(@"<Content Include=""JavaScript.js"" />") == false)
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeHTML, @"<Content Include=""JavaScript.js""" + " />" + Environment.NewLine + "\t"); //zakucan base.html
                File.WriteAllText(csprojPath, csprojEdited);
            }
            if (csprojEdited.Contains(@"<Content Include=""HelpCSS.css"" />") == false)
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeHTML, @"<Content Include=""HelpCSS.css""" + " />" + Environment.NewLine + "\t"); //zakucan base.html
                File.WriteAllText(csprojPath, csprojEdited);
            }
            #endregion

            #region Kreiranje generisanih fajlova
            string path = @"C:\Users\nikola.bastovanovic\source\repos\GeneratedWebApp\GeneratedWebApp\base.html";
            string javaScriptPath = @"C:\Users\nikola.bastovanovic\source\repos\GeneratedWebApp\GeneratedWebApp\JavaScript.js";
            string cssPath = @"C:\Users\nikola.bastovanovic\source\repos\GeneratedWebApp\GeneratedWebApp\HelpCSS.css";

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

            if (File.Exists(javaScriptPath))
            {
                File.Delete(javaScriptPath);
                using (StreamWriter sw = File.CreateText(javaScriptPath))
                {
                    sw.Write(JavaScript);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(javaScriptPath))
                {
                    sw.Write(JavaScript);
                }
            }

            if (File.Exists(cssPath))
            {
                File.Delete(cssPath);
                using (StreamWriter sw = File.CreateText(cssPath))
                {
                    sw.Write(CssString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(cssPath))
                {
                    sw.Write(CssString);
                }
            }
            #endregion
        }

        public string JavaScript =@"window.onscroll = function () {myFunction()};
    var header = document.getElementById(""myHeader"");
    var sticky = header.offsetTop;
    
    function myFunction()
    {
        if (window.pageYOffset > sticky)
        {
            header.classList.add(""sticky"");
        }
        else
        {
            header.classList.remove(""sticky"");
        }
    }";

        public string CssString = @"body {
    margin: 0;
    font-family: Arial, Helvetica, sans-serif;
}
.top-container {
    background-color: #f1f1f1;
    padding: 30px;
    text-align: center;
}

.header {
    padding: 10px 16px;
    background: #555;
    color: #f1f1f1;
}

.content {
    padding: 16px;
}

.sticky {
    position: fixed;
    top: 0;
    width: 100%;
}

.sticky + .content {
    padding-top: 102px;
}";
    }
}
