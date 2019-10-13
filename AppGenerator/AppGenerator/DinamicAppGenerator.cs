using Microsoft.Build.BuildEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TextTemplating;
using System.Web;

namespace AppGenerator
{
    public partial class DinamicAppGenerator : Form
    {
        public DinamicAppGenerator()
        {
            InitializeComponent();
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
            btnBrowsePath.Enabled = true;
        }

        private void BtnBrowsePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                filename = folder.SelectedPath;
            }
            txtSourceCodeDest.Text = filename;
            btnGenerate.Enabled = true;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            var proces = new Process
            {
                StartInfo =
                {
                    FileName = "TextTransform.exe",
                    Arguments = "TextTemplate1.tt"
                }
            };

            proces.Start();
            proces.WaitForExit();


            #region Update csproj fajla
            string csprojPath = filename + @"\MyGeneratedApp.csproj";
            string csprojEdited = File.ReadAllText(csprojPath);

            //Dodavanje MasterPage.Master strane u .csproj
            int positionToIncludeMasterPage = csprojEdited.IndexOf(@"<None Include=""Web.Debug.config""");
            if (csprojEdited.Contains(@"<Content Include=""" + "MasterPage.Master" + "\"") == false) //Proveri da li vec postoji MasterPage.Master u .csproj
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeMasterPage, @"<Content Include=""" + "MasterPage.Master" + "\"" + " />" + Environment.NewLine + "\t");
                File.WriteAllText(csprojPath, csprojEdited);
            }

            //Dodavanje MasterPage.Master.designer.cs klase u .csproj
            int positionToIncludeMasterPageDesigner = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            if (csprojEdited.Contains(@"<Compile Include=""" + "MasterPage.Master.designer.cs" + "\"") == false) //Proveri da li vec postoji MasterPage.Master.designer.cs u .csproj
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeMasterPageDesigner, @"<Compile Include=""" + "MasterPage.Master.designer.cs" + "\"" + @">
      <DependentUpon>MasterPage.Master</DependentUpon>
    </Compile> " + Environment.NewLine + "\t");
                File.WriteAllText(csprojPath, csprojEdited);
            }

            //Dodavanje MasterPage.Master.cs klase u .csproj
            int positionToIncludeMasterPageCs = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            if (csprojEdited.Contains(@"<Compile Include=""" + "MasterPage.Master.cs" + "\"") == false) //Proveri da li vec postoji MasterPage.Master.cs u .csproj
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeMasterPageCs, @"<Compile Include=""" + "MasterPage.Master.cs" + "\"" + @">
      <DependentUpon>MasterPage.Master</DependentUpon>
      <SubType>ASPXCodeBehind</SubType>
    </Compile> " + Environment.NewLine + "\t");
                File.WriteAllText(csprojPath, csprojEdited);
            }
            #endregion

            List<string> pageList = new List<string>();
            XmlDocument xml = new XmlDocument();
            xml.Load(grammer);
            foreach (XmlNode node in xml.GetElementsByTagName("pageName"))
            {
                pageList.Add(node.InnerText); //Nazivi stranica pokupljen iz gramatike
            }
            string myAppName = string.Empty;
            string bannerImagePath = string.Empty;
            string indexPage = string.Empty;
            XmlNode myAppNameNode = xml.SelectSingleNode(@"gramer/app_name");
            myAppName = myAppNameNode.InnerText; //Naziv aplikacije
            XmlNode bannerImagePathNode = xml.SelectSingleNode(@"gramer/banner_image");
            bannerImagePath = bannerImagePathNode.InnerText;

            foreach (XmlNode xmlNodeIndexPage in xml.GetElementsByTagName("pageName"))
            {
                if (xmlNodeIndexPage.Attributes["index"] != null && xmlNodeIndexPage.Attributes["index"].Value == "true")
                    indexPage = xmlNodeIndexPage.InnerText;
            }

            #region Generisanje MasterPage.Master strane
            string pathMaster = filename + @"\MasterPage.Master";

            string generatedMasterPageString = $@"<%@ Master Language=""C#"" AutoEventWireup=""true"" CodeBehind=""MasterPage.master.cs"" Inherits=""{myAppName}.Site1"" %>
<!DOCTYPE html>

<html>
  <head runat=""server"">
    <title>" + myAppName + @"</title>
    <link rel=""stylesheet"" href=""Styles/StyleSheet.css"" type=""text/css""/>
    <asp:ContentPlaceHolder ID = ""head"" runat = ""server"">
    </asp:ContentPlaceHolder>
  </head>
    <body>
        <form id = ""form1"" runat = ""server"">
           <div>
               <div id=""wrapper"">
                   <div id = ""banner"">
                       <asp:Image ID = ""Image1"" runat = ""server"" ImageUrl = """ + bannerImagePath + @""" Width = ""100%"" />
                           </div>
                               <div id = ""navigation"">
                                   <ul id = ""nav"">" + Environment.NewLine + "\t\t\t\t\t\t\t\t\t\t";

            for (int i = 0; i < pageList.Count; i++)
            {
                if (pageList[i] == indexPage)
                    generatedMasterPageString = generatedMasterPageString + @"<li><asp:HyperLink ID=""HyperLink" + i.ToString() + "\"" + $@" NavigateUrl=""~ /Index.aspx"" runat=""server"">" + pageList[i] + "</asp:HyperLink></li>" + Environment.NewLine + "\t\t\t\t\t\t\t\t\t\t";
                else
                    generatedMasterPageString = generatedMasterPageString + @"<li><asp:HyperLink ID=""HyperLink" + i.ToString() + "\"" + $@" NavigateUrl=""~/Pages/{pageList[i].Replace(" ", string.Empty)}.aspx"" runat=""server"">" + pageList[i] + "</asp:HyperLink></li>" + Environment.NewLine + "\t\t\t\t\t\t\t\t\t\t";
            }
            generatedMasterPageString = generatedMasterPageString +
                                      @"<li style=""float:right"">
                                            <asp:HyperLink ID=""lnkRegister"" NavigateUrl=""~/Pages/Account/Register.aspx"" runat=""server"">Register</asp:HyperLink>
                                            <asp:HyperLink ID=""litStatus"" runat=""server"">Login</asp:HyperLink>
                                        </li>
                                        <li style=""float:right"">
                                            <asp:HyperLink ID=""lnkLogin"" NavigateUrl=""~/Pages/Account/Login.aspx"" runat=""server"">LogIn</asp:HyperLink>
                                            <asp:LinkButton ID=""lnkLogout"" runat=""server"" OnClick=""lnkLogout_Click"">LogOut</asp:LinkButton>
                                        </li>
                                    </ul>
                               </div>
                           <div id = ""content"">
                       <asp:ContentPlaceHolder ID = ""ContentPlaceHolder1"" runat = ""server"">
                       </asp:ContentPlaceHolder>
                    </div>
                </div>
             </div>
        </form>
    </body>
 </html>";

            if (File.Exists(pathMaster))
            {
                File.Delete(pathMaster);
                using (StreamWriter sw = File.CreateText(pathMaster))
                {
                    sw.Write(generatedMasterPageString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(pathMaster))
                {
                    sw.Write(generatedMasterPageString);
                }
            }
            #endregion

            #region Generisanje MasterPage.Master.designer.cs klase
            string pathMasterDesinger = filename + @"\MasterPage.Master.designer.cs";

            string generatedMasterDesignerString = @"namespace MyGeneratedApp {
    
    public partial class Site1 {
        
        protected global::System.Web.UI.WebControls.ContentPlaceHolder head;
        protected global::System.Web.UI.HtmlControls.HtmlForm form1;
        protected global::System.Web.UI.WebControls.ContentPlaceHolder ContentPlaceHolder1;
        protected global::System.Web.UI.WebControls.HyperLink lnkRegister;
        protected global::System.Web.UI.WebControls.HyperLink litStatus;
        protected global::System.Web.UI.WebControls.HyperLink lnkLogin;
        protected global::System.Web.UI.WebControls.LinkButton lnkLogout;
    }
}
";

            if (File.Exists(pathMasterDesinger))
            {
                File.Delete(pathMasterDesinger);
                using (StreamWriter sw = File.CreateText(pathMasterDesinger))
                {
                    sw.Write(generatedMasterDesignerString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(pathMasterDesinger))
                {
                    sw.Write(generatedMasterDesignerString);
                }
            }
            #endregion

            #region Generisanje MasterPage.Master.cs klase
            string pathMasterCs = filename + @"\MasterPage.Master.cs";

            string generatedMasterCsString = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyGeneratedApp
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Context.User.Identity;
            if (user.IsAuthenticated)
            {
                litStatus.Text = Context.User.Identity.Name;

                lnkLogin.Visible = false;
                lnkRegister.Visible = false;

                lnkLogout.Visible = true;
                litStatus.Visible = true;
            }
            else
            {
                lnkLogin.Visible = true;
                lnkRegister.Visible = true;

                lnkLogout.Visible = false;
                litStatus.Visible = false;
            }
        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();

            Response.Redirect(""~/Index.aspx"");
        }
    }
}";

            if (File.Exists(pathMasterCs))
            {
                File.Delete(pathMasterCs);
                using (StreamWriter sw = File.CreateText(pathMasterCs))
                {
                    sw.Write(generatedMasterCsString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(pathMasterCs))
                {
                    sw.Write(generatedMasterCsString);
                }
            }
            #endregion

            #region Generisanje CSS-a
            string cssString = @"
body {
    color: #574c3f;
    font-family: Corbel, Arial, Helvetica, Verdana, sans-serif;
    font-size: 16px;
    background-color: lavender;
}

#wrapper {
    width: 1100px;
    margin: auto;
    padding-bottom: 20px;
    background-color: white;
}

#navigation {
    height: 45px;
    background: linear-gradient(to top, #c2bcb5, #ffffff);
}

#nav {
    display: block;
    list-style: none;
    text-align: center;
}

    #nav ul {
        display: none;
        list-style: none;
    }

    #nav li {
        float: left;
        display: block;
        margin-top: 15px;
        text-transform: uppercase;
        padding: 0px 20px 0px 20px;
        border-left: 1px solid #FFFFFF;
    }

    #nav a:link, #nav a :visited, #nav a:active {
        display: block;
        color: #574c3f;
        text-decoration: none;
    }

    #nav a:hover {
        background-color: white;
    }

a:visited {
    display: block;
    color: #574c3f;
    text-decoration: none;
}

#content {
    padding: 20px;
    min-height: 400px;
}

/*Generic Controls*/
.button {
    background: linear-gradient(to bottom, darkgreen, green);
    padding: 10px;
    margin: 10px;
    color: white;
    -moz-border-radius: 4px;
    -webkit-border-radius: 4px;
    border-radius: 4px;
    border: 0;
    clear: both;
    width: 150px;
}

/*Webshop */
#ContentPlaceHolder1_pnlProduct div {
    float: left;
    width: 250px;
    height: 250px;
    text-align: center;
}

.productName {
    font-weight: bold;
    font-size: 18px;
}

.productPrice {
    font-family: arial,helvetica,sans-serif;
    color: #FFB600;
}

.productImage {
    width: 150px;
    height: 150px;
    vertical-align: central;
    padding: 10px;
    -moz-border-radius: 50px;
    -webkit-border-radius: 50px;
    border-radius: 50px;
}

/*Detail page*/
.detailsImage {
    height: 465px;
    width: 400px;
    margin-right: 30px;
}

.detailsDescription {
    font-style: italic;
    width: 300px;
}

.detailsPrice {
    font-weight: bold;
    font-size: 24px;
    width: 200px;
}

.inputs {
    -moz-border-radius: 5px;
    -webkit-border-radius: 5px;
    background-color: #EAEAEA;
    background: -moz-linear-gradient(top, #FFF, #EAEAEA);
    background: -webkit-gradient(linear, left top, left bottom, color-stop(0.0, #FFF), color-stop(1.0, #EAEAEA));
    border: 1px solid #CACACA;
    color: #444;
    margin: 0 0 25px;
    padding: 5px 9px;
    width: 260px;
}

    .inputs:focus {
        background: #FFF;
        -webkit-box-shadow: 0 0 25px #CCC;
        -moz-box-shadow: 0 0 25px #ccc;
        box-shadow: 0 0 25px #CCC;
    }

/*Shopping Cart Page */
.CartTable {
    text-align: center;
    border: 15px solid #f5f4f2;
    margin-bottom: 10px;
    width: 100%;
    padding: 10px;
}

    .CartTable input {
        width: 65px;
        height: 65px;
    }

    .CartTable h4 {
        padding: 0;
        margin: 0;
    }

    .CartTable tr td select {
        width: 100px;
    }

";

            string styleDirPath = filename + @"\Styles";
            string cssPath = filename + @"\Styles\StyleSheet.css";

            //Kreiranje foldera Styles za smestanje css fajla
            if (!Directory.Exists(styleDirPath))
            {
                Directory.CreateDirectory(styleDirPath);
            }

            if (File.Exists(cssPath))
            {
                File.Delete(cssPath);
                using (StreamWriter sw = File.CreateText(cssPath))
                {
                    sw.Write(cssString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(cssPath))
                {
                    sw.Write(cssString);
                }
            }
            //Dodavanje css fajla u .csproj
            int positionToIncludeCSS = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
            if (csprojEdited.Contains(@"<Content Include=""Styles\StyleSheet.css"" />") == false) //Proveri da li css postoji u .csproj
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeCSS, @"<Content Include=""Styles\StyleSheet.css""" + " />" + Environment.NewLine + "\t"); //zakucan HelpCSS.css
                File.WriteAllText(csprojPath, csprojEdited);
            }
            #endregion


            string imagesDirPath = filename + @"\Images";
            if (!Directory.Exists(imagesDirPath))
            {
                Directory.CreateDirectory(imagesDirPath);
            }

            int positionToIncludeImages = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
            if (csprojEdited.Contains(@"<Content Include=""Styles\StyleSheet.css"" />") == false) //Proveri da li css postoji u .csproj
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeImages, @"<Content Include=""Styles\StyleSheet.css""" + " />" + Environment.NewLine + "\t"); //zakucan HelpCSS.css
                File.WriteAllText(csprojPath, csprojEdited);
            }

            string modelsDirPath = filename + @"\Models";
            if (!Directory.Exists(modelsDirPath))
            {
                Directory.CreateDirectory(modelsDirPath);
            }

            CRUDOperations.DbOperations(myAppName, xml, modelsDirPath, csprojPath);

            PageGenerator.GeneratePagesByModel(myAppName, xml, csprojPath, filename);

            PageGenerator.GenrateClassByModel(myAppName, xml, csprojPath, filename);

            PageGenerator.GenerateAdminPage(myAppName, xml, csprojPath, filename);

            PageGenerator.GenerateIndexPage(myAppName, xml, csprojPath, filename);

            PageGenerator.GeneratePageItem(myAppName, xml, csprojPath, filename);

            string acountDirPath = filename + @"\Pages\Account";
            if (!Directory.Exists(acountDirPath))
            {
                Directory.CreateDirectory(acountDirPath);
            }

            PageGenerator.GenerateRegisterPage(myAppName, xml, csprojPath, filename);

            PageGenerator.GenerateLogInPage(myAppName, xml, csprojPath, filename);

            PageGenerator.GenerateNavigationPages(myAppName, xml, csprojPath, filename);
        }
    }
}
