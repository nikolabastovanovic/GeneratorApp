using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace AppGenerator
{
    public partial class FormGenerator : Form
    {
        public FormGenerator()
        {
            InitializeComponent();
            comboBoxPagesCollection.Text = "Choose page to add content..";
            comboBoxPagesCollection.Enabled = false;
        }

        private void GenerateButton(object sender, EventArgs e)
        {
            string pages = txtPageNames.Text;
            int countComas = Regex.Matches(txtPageNames.Text, ",").Count;
            string[] pageNames = new string[countComas + 1];

            //Uklanjanje praznih karaktera 
            for (int i = 0; i < pageNames.Length; i++)
            {
                string pageName = string.Empty;
                if (txtPageNames.Text.Contains(",") == true)
                {
                    pageName = txtPageNames.Text.Substring(0, txtPageNames.Text.IndexOf(","));
                    txtPageNames.Text = txtPageNames.Text.Substring(txtPageNames.Text.IndexOf(",") + 1);
                }
                else
                {
                    pageName = txtPageNames.Text;
                }

                pageNames[i] = pageName.Trim();
            }
            txtPageNames.Text = pages;

            string generatedHtmlString = string.Empty;

            for (int y = 0; y < pageNames.Length; y++)
            {
                //Mora biti u petlji kako se kod ne bi ponavljao i kreirao sve vise puta..
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
                    writer.Write("Page Title" + pageNames[y]);
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
                    for (int menuTabs = 0; menuTabs < pageNames.Length; menuTabs++)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, pageNames[menuTabs] + ".html");
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(pageNames[menuTabs]);
                        writer.RenderEndTag(); //end a
                        writer.WriteLine();
                    }
                    writer.RenderEndTag(); //end div
                    writer.WriteLine();

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    if (checkBoxCreateSideBar.Checked)
                    {
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

                        //Tekst iz generisanog textbox-a
                        TextBox sideTxtBox = this.Controls.Find("txtSideContent" + pageNames[y], true).FirstOrDefault() as TextBox;

                        writer.Write(sideTxtBox.Text);

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
                    }

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
                    //Tekst iz generisanog textbox-a
                    TextBox mainTxtBox = this.Controls.Find("txtContent" + pageNames[y], true).FirstOrDefault() as TextBox;
                    writer.Write(mainTxtBox.Text);
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
                if (csprojEdited.Contains(@"<Content Include=""" + pageNames[y] + ".html\"") == false) //zakucan base.html
                {
                    csprojEdited = csprojEdited.Insert(positionToIncludeHTML, @"<Content Include=""" + pageNames[y] + ".html\"" + "" + " />" + Environment.NewLine + "\t"); //zakucan base.html
                    File.WriteAllText(csprojPath, csprojEdited);
                }
                if (csprojEdited.Contains(@"<Content Include=""JavaScript.js"" />") == false)
                {
                    csprojEdited = csprojEdited.Insert(positionToIncludeHTML, @"<Content Include=""JavaScript.js""" + " />" + Environment.NewLine + "\t"); //zakucan JavaScript.js
                    File.WriteAllText(csprojPath, csprojEdited);
                }
                if (csprojEdited.Contains(@"<Content Include=""HelpCSS.css"" />") == false)
                {
                    csprojEdited = csprojEdited.Insert(positionToIncludeHTML, @"<Content Include=""HelpCSS.css""" + " />" + Environment.NewLine + "\t"); //zakucan HelpCSS.css
                    File.WriteAllText(csprojPath, csprojEdited);
                }
                #endregion

                #region Kreiranje generisanih fajlova
                string path = @"C:\Users\nikola.bastovanovic\source\repos\GeneratedWebApp\GeneratedWebApp\" + pageNames[y] + ".html";
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
                generatedHtmlString = "";
            }
        }

        public string JavaScript = @"window.onscroll = function () {myFunction()};
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

        private void ComboBoxPagesCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string choosen = comboBoxPagesCollection.SelectedItem.ToString();
            buttonInsertImage.Visible = true;
            //KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.K);
            //TxtPageNames_KeyPress(null, key);
            if (choosen.Contains("side bar"))
            {
                TextBox box = this.Controls.Find("txtSideContent" + choosen.Substring(0, choosen.IndexOf(" ")), true).FirstOrDefault() as TextBox;
                box.Visible = true;
                box.BringToFront();
            }
            else
            {
                TextBox box = this.Controls.Find("txtContent" + choosen, true).FirstOrDefault() as TextBox;
                box.Visible = true;
                box.BringToFront();
            }
        }

        private void TxtPageNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            string initPages = txtPageNames.Text;
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (checkBoxCreateSideBar.Checked)
                    CheckBoxCreateSideBar_CheckedChanged(sender, e);

                int countComas = Regex.Matches(txtPageNames.Text, ",").Count;
                string[] pageNames = new string[countComas + 1];
                comboBoxPagesCollection.Enabled = true;

                //Uklanjanje praznih karaktera 
                for (int i = 0; i < pageNames.Length; i++)
                {
                    string pageName = string.Empty;
                    if (txtPageNames.Text.Contains(",") == true)
                    {
                        pageName = txtPageNames.Text.Substring(0, txtPageNames.Text.IndexOf(","));
                        txtPageNames.Text = txtPageNames.Text.Substring(txtPageNames.Text.IndexOf(",") + 1);

                        if (comboBoxPagesCollection.Items.Contains(pageName.Trim()) == false)
                            comboBoxPagesCollection.Items.Add(pageName.Trim());

                        TextBox txtBox = new TextBox();
                        txtBox.Name = "txtContent" + pageName.Trim();
                        txtBox.Multiline = true;
                        txtBox.Text = pageName.Trim();
                        txtBox.Location = new Point(12, 65);
                        txtBox.Size = new Size(378, 178);
                        txtBox.Visible = false;
                        this.Controls.Add(txtBox);
                    }
                    else
                    {
                        pageName = txtPageNames.Text;

                        if (comboBoxPagesCollection.Items.Contains(pageName.Trim()) == false)
                            comboBoxPagesCollection.Items.Add(pageName.Trim());

                        TextBox txtBox = new TextBox();
                        txtBox.Name = "txtContent" + pageName.Trim();
                        txtBox.Multiline = true;
                        txtBox.Text = pageName.Trim();
                        txtBox.Location = new Point(12, 65);
                        txtBox.Size = new Size(378, 178);
                        txtBox.Visible = false;
                        this.Controls.Add(txtBox);
                    }
                }
                txtPageNames.Text = initPages;
            }
        }

        private void CheckBoxCreateSideBar_CheckedChanged(object sender, EventArgs e)
        {
            string initPages = txtPageNames.Text;
            int countComas = Regex.Matches(txtPageNames.Text, ",").Count;
            string[] pageNames = new string[countComas + 1];
            comboBoxPagesCollection.Enabled = true;

            //Uklanjanje praznih karaktera 
            for (int i = 0; i < pageNames.Length; i++)
            {
                string pageName = string.Empty;
                if (txtPageNames.Text.Contains(",") == true)
                {
                    pageName = txtPageNames.Text.Substring(0, txtPageNames.Text.IndexOf(","));
                    txtPageNames.Text = txtPageNames.Text.Substring(txtPageNames.Text.IndexOf(",") + 1);

                    if (comboBoxPagesCollection.Items.Contains(pageName.Trim() + " side bar") == false)
                        comboBoxPagesCollection.Items.Add(pageName.Trim() + " side bar");

                    TextBox txtBox = new TextBox();
                    txtBox.Name = "txtSideContent" + pageName.Trim();
                    txtBox.Multiline = true;
                    txtBox.Text = pageName.Trim() + " side bar content";
                    txtBox.Location = new Point(12, 65);
                    txtBox.Size = new Size(378, 178);
                    txtBox.Visible = false;
                    this.Controls.Add(txtBox);
                }
                else
                {
                    pageName = txtPageNames.Text;

                    if (comboBoxPagesCollection.Items.Contains(pageName.Trim() + " side bar") == false)
                        comboBoxPagesCollection.Items.Add(pageName.Trim() + " side bar");

                    TextBox txtBox = new TextBox();
                    txtBox.Name = "txtSideContent" + pageName.Trim();
                    txtBox.Multiline = true;
                    txtBox.Text = pageName.Trim() + " side bar content";
                    txtBox.Location = new Point(12, 65);
                    txtBox.Size = new Size(378, 178);
                    txtBox.Visible = false;
                    this.Controls.Add(txtBox);
                }
            }
            txtPageNames.Text = initPages;
        }

        private void ButtonInsertImage_Click(object sender, EventArgs e)
        {
            TextBox box = new TextBox();
            string choosen = comboBoxPagesCollection.SelectedItem.ToString();
            if (choosen.Contains("side bar"))
                box = this.Controls.Find("txtSideContent" + choosen.Substring(0, choosen.IndexOf(" ")), true).FirstOrDefault() as TextBox;
            else
                box = this.Controls.Find("txtContent" + choosen, true).FirstOrDefault() as TextBox;
            string filename = string.Empty;
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = openFile.FileName;
                filename = Path.GetFileName(filename);
            }
            box.Text += "<br> <img src=\"" + filename + "\" height=\"200\" /> <br>";
        }
    }
}
