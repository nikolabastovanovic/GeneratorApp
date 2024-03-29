﻿using System;
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
using System.Xml;

namespace AppGenerator
{
    public partial class StaticAppGenerator : Form
    {
        public StaticAppGenerator()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        public string filename = string.Empty;
        public string grammer = string.Empty;
        private void BtnBrowseGrammer_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML Files (*.xml)|*.xml";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                grammer = openFile.FileName;
            }
            txtGrammer.Text = grammer;
            btnBrowse.Enabled = true;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = folder.SelectedPath;
            }
            txtPath.Text = filename;
            button1.Enabled = true;
        }

        string myAppName = string.Empty;
        private void GenerateButton(object sender, EventArgs e)
        {
            List<string> pageList = new List<string>();
            XmlDocument xml = new XmlDocument();
            xml.Load(grammer);
            XmlNode myAppNameNode = xml.SelectSingleNode(@"gramer/app_name");
            myAppName = myAppNameNode.InnerText; //Naziv aplikacije
            foreach (XmlNode node in xml.GetElementsByTagName("pageName"))
            {
                pageList.Add(node.InnerText); //Nazivi stranica pokupljen iz gramatike
            }

            string generatedHtmlString = string.Empty;

            for (int y = 0; y < pageList.Count; y++)
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
                    writer.Write(pageList[y]);
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
                    writer.Write(myAppName);
                    writer.RenderEndTag(); //end div
                    writer.RenderEndTag(); //end h1
                    writer.WriteLine();

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "navbar");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    for (int menuTabs = 0; menuTabs < pageList.Count; menuTabs++)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, pageList[menuTabs] + ".html");
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write(pageList[menuTabs]);
                        writer.RenderEndTag(); //end a
                        writer.WriteLine();
                    }
                    writer.RenderEndTag(); //end div
                    writer.WriteLine();

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    foreach (XmlNode xmlNodePageSideBar in xml.GetElementsByTagName("sideBarDesc"))
                    {
                        if (xmlNodePageSideBar.ParentNode.ParentNode.FirstChild.InnerText == pageList[y])
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "side");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            writer.RenderBeginTag(HtmlTextWriterTag.H2);
                            writer.Write(pageList[y]);
                            writer.RenderEndTag();
                            writer.WriteLine();
                            writer.RenderBeginTag(HtmlTextWriterTag.H5);
                            writer.Write("<i>Place your favorite image here</i>");
                            writer.RenderEndTag();
                            writer.WriteLine();
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "fakeimg");
                            writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:200px;");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            writer.Write("Image");
                            writer.RenderEndTag();
                            writer.WriteLine();
                            writer.RenderBeginTag(HtmlTextWriterTag.P);
                            writer.Write(xmlNodePageSideBar.InnerText);

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
                    }
                    
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "main");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    foreach (XmlNode xmlNodePageTitle in xml.GetElementsByTagName("pageName"))
                    {
                        if (xmlNodePageTitle.InnerText == pageList[y])
                        {
                            if (xmlNodePageTitle.Attributes["title"] != null)
                            {
                                writer.RenderBeginTag(HtmlTextWriterTag.H2);
                                writer.Write(xmlNodePageTitle.Attributes["title"].Value);
                                writer.RenderEndTag();
                                writer.WriteLine();
                            }
                        }
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "fakeimg");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "height:200px;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("Bunner");
                    writer.RenderEndTag();
                    writer.WriteLine();

                    foreach (XmlNode xmlNodePageDescription in xml.GetElementsByTagName("description"))
                    {
                        if (xmlNodePageDescription.ParentNode.ParentNode.FirstChild.InnerText == pageList[y])
                        {
                            string imageSource = string.Empty;
                            writer.RenderBeginTag(HtmlTextWriterTag.P);
                            if (xmlNodePageDescription.InnerText.Contains("ImageSource"))
                            {
                                imageSource = xmlNodePageDescription.InnerText.Substring(xmlNodePageDescription.InnerText.IndexOf("ImageSource(") + 13, xmlNodePageDescription.InnerText.IndexOf(")") - (xmlNodePageDescription.InnerText.IndexOf("ImageSource(") + 14));
                                xmlNodePageDescription.InnerText = xmlNodePageDescription.InnerText.Replace("\"" + imageSource + "\"", " ");
                                xmlNodePageDescription.InnerText = xmlNodePageDescription.InnerText.Insert(xmlNodePageDescription.InnerText.IndexOf("ImageSource(") + 13, "<img src=" + "\"" + imageSource + "\"" + "/>");
                                xmlNodePageDescription.InnerText = xmlNodePageDescription.InnerText.Replace("ImageSource(", " ");
                                xmlNodePageDescription.InnerText = xmlNodePageDescription.InnerText.Replace(")", " ");
                            }
                            
                            writer.Write(xmlNodePageDescription.InnerText);
                            writer.RenderEndTag();
                            writer.WriteLine();
                        }
                    }

                    
                    writer.RenderEndTag(); //end div class main

                    writer.RenderEndTag(); //end div class row
                    writer.WriteLine();

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "footer");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderBeginTag(HtmlTextWriterTag.H2);
                    writer.Write(xml.SelectSingleNode("gramer/footer").InnerText);
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
                //Provera da li postoji .csproj fajl
                if (File.Exists(filename + @"\" + myAppName + ".csproj") == true)
                    {
                    string csprojPath = filename + @"\" + myAppName + ".csproj";
                    string csprojEdited = File.ReadAllText(csprojPath);
                    int positionToIncludeHTML = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
                    if (csprojEdited.Contains(@"<Content Include=""" + pageList[y] + ".html\"") == false) //zakucan base.html
                    {
                        csprojEdited = csprojEdited.Insert(positionToIncludeHTML, @"<Content Include=""" + pageList[y] + ".html\"" + "" + " />" + Environment.NewLine + "\t"); //zakucan base.html
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
                }
                #endregion

                #region Kreiranje generisanih fajlova
                string path = filename + @"\" + pageList[y] + ".html";
                string javaScriptPath = filename + @"\JavaScript.js";
                string cssPath = filename + @"\HelpCSS.css";

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
}

/*Position in center*/
.center{
    display: block;
    margin-left: auto;
    margin-right: auto;
    width: 30%;
}";

        //private void ComboBoxPagesCollection_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string choosen = comboBoxPagesCollection.SelectedItem.ToString();
        //    buttonInsertImage.Visible = true;
        //    //KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.K);
        //    //TxtPageNames_KeyPress(null, key);
        //    if (choosen.Contains("side bar"))
        //    {
        //        TextBox box = this.Controls.Find("txtSideContent" + choosen.Substring(0, choosen.IndexOf(" ")), true).FirstOrDefault() as TextBox;
        //        box.Visible = true;
        //        box.BringToFront();
        //    }
        //    else
        //    {
        //        TextBox box = this.Controls.Find("txtContent" + choosen, true).FirstOrDefault() as TextBox;
        //        box.Visible = true;
        //        box.BringToFront();
        //    }
        //}

        //private void TxtPageNames_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    string initPages = txtPageNames.Text;
        //    if (e.KeyChar == (char)Keys.Enter)
        //    {
        //        if (checkBoxCreateSideBar.Checked)
        //            CheckBoxCreateSideBar_CheckedChanged(sender, e);

        //        int countComas = Regex.Matches(txtPageNames.Text, ",").Count;
        //        string[] pageNames = new string[countComas + 1];
        //        comboBoxPagesCollection.Enabled = true;

        //        //Uklanjanje praznih karaktera 
        //        for (int i = 0; i < pageNames.Length; i++)
        //        {
        //            string pageName = string.Empty;
        //            if (txtPageNames.Text.Contains(",") == true)
        //            {
        //                pageName = txtPageNames.Text.Substring(0, txtPageNames.Text.IndexOf(","));
        //                txtPageNames.Text = txtPageNames.Text.Substring(txtPageNames.Text.IndexOf(",") + 1);

        //                if (comboBoxPagesCollection.Items.Contains(pageName.Trim()) == false)
        //                    comboBoxPagesCollection.Items.Add(pageName.Trim());

        //                TextBox txtBox = new TextBox();
        //                txtBox.Name = "txtContent" + pageName.Trim();
        //                txtBox.Multiline = true;
        //                txtBox.Text = pageName.Trim();
        //                txtBox.Location = new Point(12, 65);
        //                txtBox.Size = new Size(378, 178);
        //                txtBox.Visible = false;
        //                this.Controls.Add(txtBox);
        //            }
        //            else
        //            {
        //                pageName = txtPageNames.Text;

        //                if (comboBoxPagesCollection.Items.Contains(pageName.Trim()) == false)
        //                    comboBoxPagesCollection.Items.Add(pageName.Trim());

        //                TextBox txtBox = new TextBox();
        //                txtBox.Name = "txtContent" + pageName.Trim();
        //                txtBox.Multiline = true;
        //                txtBox.Text = pageName.Trim();
        //                txtBox.Location = new Point(12, 65);
        //                txtBox.Size = new Size(378, 178);
        //                txtBox.Visible = false;
        //                this.Controls.Add(txtBox);
        //            }
        //        }
        //        txtPageNames.Text = initPages;
        //    }
        //}

        //private void CheckBoxCreateSideBar_CheckedChanged(object sender, EventArgs e)
        //{
        //    string initPages = txtPageNames.Text;
        //    int countComas = Regex.Matches(txtPageNames.Text, ",").Count;
        //    string[] pageNames = new string[countComas + 1];
        //    comboBoxPagesCollection.Enabled = true;

        //    //Uklanjanje praznih karaktera 
        //    for (int i = 0; i < pageNames.Length; i++)
        //    {
        //        string pageName = string.Empty;
        //        if (txtPageNames.Text.Contains(",") == true)
        //        {
        //            pageName = txtPageNames.Text.Substring(0, txtPageNames.Text.IndexOf(","));
        //            txtPageNames.Text = txtPageNames.Text.Substring(txtPageNames.Text.IndexOf(",") + 1);

        //            if (comboBoxPagesCollection.Items.Contains(pageName.Trim() + " side bar") == false)
        //                comboBoxPagesCollection.Items.Add(pageName.Trim() + " side bar");

        //            TextBox txtBox = new TextBox();
        //            txtBox.Name = "txtSideContent" + pageName.Trim();
        //            txtBox.Multiline = true;
        //            txtBox.Text = pageName.Trim() + " side bar content";
        //            txtBox.Location = new Point(12, 65);
        //            txtBox.Size = new Size(378, 178);
        //            txtBox.Visible = false;
        //            this.Controls.Add(txtBox);
        //        }
        //        else
        //        {
        //            pageName = txtPageNames.Text;

        //            if (comboBoxPagesCollection.Items.Contains(pageName.Trim() + " side bar") == false)
        //                comboBoxPagesCollection.Items.Add(pageName.Trim() + " side bar");

        //            TextBox txtBox = new TextBox();
        //            txtBox.Name = "txtSideContent" + pageName.Trim();
        //            txtBox.Multiline = true;
        //            txtBox.Text = pageName.Trim() + " side bar content";
        //            txtBox.Location = new Point(12, 65);
        //            txtBox.Size = new Size(378, 178);
        //            txtBox.Visible = false;
        //            this.Controls.Add(txtBox);
        //        }
        //    }
        //    txtPageNames.Text = initPages;
        //}

        //private void ButtonInsertImage_Click(object sender, EventArgs e)
        //{
        //    TextBox box = new TextBox();
        //    string choosen = comboBoxPagesCollection.SelectedItem.ToString();
        //    if (choosen.Contains("side bar"))
        //        box = this.Controls.Find("txtSideContent" + choosen.Substring(0, choosen.IndexOf(" ")), true).FirstOrDefault() as TextBox;
        //    else
        //        box = this.Controls.Find("txtContent" + choosen, true).FirstOrDefault() as TextBox;
        //    string filename = string.Empty;
        //    OpenFileDialog openFile = new OpenFileDialog();
        //    if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        filename = openFile.FileName;
        //        //filename = Path.GetFileName(filename);
        //    }
        //    box.Text += "<br> <img src=\"" + filename + "\" class=\"center\" /> <br>";
        //}

        private void FormGenerator_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
