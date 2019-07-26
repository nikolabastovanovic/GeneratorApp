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
    public partial class FormName : Form
    {
        public FormName()
        {
            InitializeComponent();
        }

        private void GenerateButton(object sender, EventArgs e)
        {
            //TextBox textbox = new TextBox();
            
            //textbox.Location = new System.Drawing.Point(10, 25 * 1);
            //textbox.Size = new System.Drawing.Size(80, 20);
            //textbox.Name = "txt_" + (1);
            //this.Controls.Add(textbox);

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

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "header");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderBeginTag(HtmlTextWriterTag.H1);
                writer.Write("My WebSite");
                writer.RenderEndTag(); //end div
                writer.RenderEndTag(); //end h1
                writer.WriteLine();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "navbar");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("Link");
                writer.RenderEndTag(); //end a
                writer.WriteLine();
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("Link");
                writer.RenderEndTag(); //end a
                writer.WriteLine();
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("Link");
                writer.RenderEndTag(); //end a
                writer.WriteLine();
                writer.RenderEndTag(); //end div
                writer.WriteLine();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "side");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderBeginTag(HtmlTextWriterTag.H2);
                writer.Write("About me");
                writer.RenderEndTag();
                writer.WriteLine();
                writer.RenderBeginTag(HtmlTextWriterTag.H5);
                writer.Write("Photo of me:");
                writer.RenderEndTag();
                writer.WriteLine();
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "fakeimg");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:200px;");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write("Image");
                writer.RenderEndTag();
                writer.WriteLine();
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Some text");
                writer.RenderEndTag();
                writer.WriteLine();
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "fakeimg");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:200px;");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write("Advertisement");
                writer.RenderEndTag();
                writer.WriteLine("<br>");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "fakeimg");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:200px;");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write("Advertisement");
                writer.RenderEndTag();
                writer.RenderEndTag(); //end div class side
                writer.WriteLine();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "main");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderBeginTag(HtmlTextWriterTag.H2);
                writer.Write("Title heading 2");
                writer.RenderEndTag();
                writer.WriteLine();
                writer.RenderBeginTag(HtmlTextWriterTag.H5);
                writer.Write("Title description");
                writer.RenderEndTag();
                writer.WriteLine();
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "fakeimg");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:200px;");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write("Image");
                writer.RenderEndTag();
                writer.WriteLine();
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Some text");
                writer.RenderEndTag();
                writer.WriteLine();
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Some text");
                writer.RenderEndTag();
                writer.RenderEndTag(); //end div class main

                writer.RenderEndTag(); //end div class row
                writer.WriteLine();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "footer");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderBeginTag(HtmlTextWriterTag.H2);
                writer.Write("Footer");
                writer.RenderEndTag();
                writer.RenderEndTag(); //end div class footer

                writer.WriteLine();
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

        public string CssString = @"* {
  box-sizing: border-box;
}

/* Style the body */
body {
  font-family: Arial, Helvetica, sans-serif;
  margin: 0;
}

/* Header/logo Title */
.header {
    padding: 10px 16px;
    background: #555;
    color: #f1f1f1;
}

/* Increase the font size of the heading */
.header h1 {
  font-size: 40px;
}

/* Style the top navigation bar */
.navbar {
  overflow: hidden;
  background-color: #333;
}

/* Style the navigation bar links */
.navbar a {
  float: left;
  display: block;
  color: white;
  text-align: center;
  padding: 14px 20px;
  text-decoration: none;
}

/* Right-aligned link */
.navbar a.right {
  float: right;
}

/* Change color on hover */
.navbar a:hover {
  background-color: #ddd;
  color: black;
}

/* Column container */
.row {  
  display: -ms-flexbox; /* IE10 */
  display: flex;
  -ms-flex-wrap: wrap; /* IE10 */
  flex-wrap: wrap;
}

/* Create two unequal columns that sits next to each other */
/* Sidebar/left column */
.side {
  -ms-flex: 30%; /* IE10 */
  flex: 30%;
  background-color: #f1f1f1;
  padding: 20px;
}

/* Main column */
.main {   
  -ms-flex: 70%; /* IE10 */
  flex: 70%;
  background-color: white;
  padding: 20px;
}

/* Fake image, just for this example */
.fakeimg {
  background-color: #aaa;
  width: 100%;
  padding: 20px;
}

/* Footer */
.footer {
  padding: 20px;
  text-align: center;
  background: #ddd;
}";

    }
}
