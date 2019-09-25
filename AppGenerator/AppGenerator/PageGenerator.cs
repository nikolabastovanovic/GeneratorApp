using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml;

namespace AppGenerator
{
    class PageGenerator
    {
        public static string PagesDirPath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages";
        public static void GeneratePagesByModel(string myAppName, XmlDocument xmlDocument, string csprojPath)
        {

            if (Directory.Exists(PagesDirPath) == false)
            {
                Directory.CreateDirectory(PagesDirPath);
            }
            XmlNode dbNameNode = xmlDocument.SelectSingleNode(@"gramer/db_name");
            string dbName = dbNameNode.InnerText; //Naziv baze

            string generatedPagesString = string.Empty;
            string aspxDesignerString = string.Empty;

            foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
            {
                string modelName = xmlNodeTableName.InnerText;

                aspxDesignerString = $"namespace {myAppName}.Pages" +
"{" +
    $"public partial class Manage{modelName}s" +
        "{\n";

                generatedPagesString = $"<%@ Page Title=\"\" Language=\"C#\" MasterPageFile=\"~/MasterPage.Master\" AutoEventWireup=\"true\" CodeBehind=\"Manage{modelName}s.aspx.cs\" Inherits=\"{myAppName}.Pages.Manage{modelName}s\" %>" +
"<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"head\" runat=\"server\">" +
     "</asp:Content>" +
      "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"ContentPlaceHolder1\" runat=\"server\">";

                foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                {
                    string modelColumnName = xmlNodeTableColumns.InnerText;
                    if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                    {
                        generatedPagesString = generatedPagesString + "\n<p>\n" + modelColumnName + ":\n" + "</p>\n";

                        if (xmlNodeTableColumns.Attributes["fk"] != null && xmlNodeTableColumns.Attributes["fk"].Value != "")
                        {
                            generatedPagesString = generatedPagesString + "<p>\n" +
                            $@"<asp:DropDownList ID=""ddl{modelColumnName}"" runat=""server"" DataSourceID=""SqlDataSource1"" DataTextField=""ID"" DataValueField = ""ID"" >
             </asp:DropDownList>
              <asp:SqlDataSource ID=""SqlDataSource{modelColumnName}"" runat=""server"" ConnectionString=""<%$ ConnectionStrings:{dbName}ConnectionString %>"" SelectCommand = ""SELECT * FROM [{xmlNodeTableColumns.Attributes["fk"].Value}] ORDER BY [ID]""></asp:SqlDataSource>" + "\n</p>";

                            aspxDesignerString += $@"protected global::System.Web.UI.WebControls.DropDownList ddl{modelColumnName};" + "\n";
                            aspxDesignerString += $@"protected global::System.Web.UI.WebControls.SqlDataSource SqlDataSource{modelColumnName};" + "\n";
                        }
                        else if (xmlNodeTableColumns.Attributes["type"].Value == "int" ||
                                 xmlNodeTableColumns.Attributes["type"].Value == "string" ||
                                 xmlNodeTableColumns.Attributes["type"].Value == "decimal")
                        {
                            if (xmlNodeTableColumns.Attributes["type"].Value == "string" && xmlNodeTableColumns.Attributes["multiline"] != null)
                            {
                                if (xmlNodeTableColumns.Attributes["multiline"].Value == "true")
                                {
                                    generatedPagesString = generatedPagesString + "<p>\n" +
                                        $@"<asp:TextBox ID=""txt{modelColumnName}"" runat=""server"" Height=""60px"" TextMode=""MultiLine"" Width=""260px""></asp:TextBox>" + "\n</p>";

                                    aspxDesignerString += $@"protected global::System.Web.UI.WebControls.TextBox txt{modelColumnName};" + "\n";
                                }
                            }
                            else
                            {
                                generatedPagesString = generatedPagesString + "<p>\n" +
                                    $@"<asp:TextBox ID=""txt{modelColumnName}"" runat=""server""></asp:TextBox>" + "\n</p>";

                                aspxDesignerString += $@"protected global::System.Web.UI.WebControls.TextBox txt{modelColumnName};" + "\n";
                            }
                        }
                        else if (xmlNodeTableColumns.Attributes["type"].Value == "image")
                        {
                            generatedPagesString = generatedPagesString + "<p>\n" +
                            $@"<asp:DropDownList ID=""ddl{modelColumnName}"" runat=""server"" >
       </asp:DropDownList>" + "\n</p>";

                            aspxDesignerString += $@"protected global::System.Web.UI.WebControls.DropDownList ddl{modelColumnName};" + "\n";
                        }
                    }
                }
                generatedPagesString = generatedPagesString +
                    @"<p>
    <asp:Button ID=""btnSubmit"" runat=""server"" Text=""Submit"" OnClick=""btnSubmit_Click""/>
       </p>
       <asp:Label ID=""lblResult"" runat=""server""></asp:Label>
              </asp:Content>";

                aspxDesignerString += $@"protected global::System.Web.UI.WebControls.Button btnSubmit;" + "\n";
                aspxDesignerString += $@"protected global::System.Web.UI.WebControls.Label lblResult;" + "\n}\n}";

                //Dodavanje aspx
                string pagesPath = PagesDirPath + @"\Manage" + modelName + "s.aspx";
                string csprojEdited = File.ReadAllText(csprojPath);
                int positionToIncludeAspx = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
                csprojEdited = EditCSProj.IncludePages(pagesPath, generatedPagesString, csprojPath, positionToIncludeAspx, $@"<Content Include=""Pages\Manage{modelName}s.aspx"" />");

                //Dodavanje aspx.designer.cs
                string pagesDesignerPath = PagesDirPath + @"\Manage" + modelName + "s.aspx.designer.cs";
                int positionToIncludePageDesigner = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
                string pageDesigner = $@"<Compile Include=""Pages\Manage{modelName}s.aspx.designer.cs"">" + "\n" +
      $@"<DependentUpon>Manage{modelName}s.aspx</DependentUpon>" + "\n" +
    @"</Compile> " + Environment.NewLine + "\n\t";
                csprojEdited = EditCSProj.IncludePages(pagesDesignerPath, aspxDesignerString, csprojPath, positionToIncludePageDesigner, pageDesigner);
            }
        }

        public static void GenrateClassByModel(string myAppName, XmlDocument xmlDocument, string csprojPath)
        {
            string PagesDirPath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages";
            XmlNode dbNameNode = xmlDocument.SelectSingleNode(@"gramer/db_name");
            string dbName = dbNameNode.InnerText; //Naziv baze

            string generateClassModelString = string.Empty;
            foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
            {
                string modelName = xmlNodeTableName.InnerText;
                generateClassModelString = $@"
using {myAppName}.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace {myAppName}.Pages
{{
    public partial class Manage{modelName}s : System.Web.UI.Page
    {{
        protected void Page_Load(object sender, EventArgs e)
        {{
            
        }}
        private {modelName} Create{modelName}()
        {{
            {modelName} {modelName.ToLower()} = new {modelName}();
            ";

                foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                {
                    string modelColumnName = xmlNodeTableColumns.InnerText;
                    if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                    {
                        if (xmlNodeTableColumns.Attributes["fk"] != null && xmlNodeTableColumns.Attributes["fk"].Value != "")
                        {
                            generateClassModelString += $@"{modelName.ToLower()}.{modelColumnName} = Convert.ToInt32(ddl{modelColumnName}.SelectedValue);" + "\n\t\t\t";
                        }
                        else if (xmlNodeTableColumns.Attributes["type"].Value == "decimal")
                        {
                            generateClassModelString += $@"{modelName.ToLower()}.{modelColumnName} = Convert.ToDecimal(txt{modelColumnName}.Text);" + "\n\t\t\t";
                        }
                        else if (xmlNodeTableColumns.Attributes["type"].Value == "string")
                        {
                            generateClassModelString += $@"{modelName.ToLower()}.{modelColumnName} = txt{modelColumnName}.Text;" + "\n\t\t\t";
                        }
                        else if (xmlNodeTableColumns.Attributes["type"].Value == "int")
                        {
                            generateClassModelString += $@"{modelName.ToLower()}.{modelColumnName} = Convert.ToInt32(txt{modelColumnName}.Text);" + "\n\t\t\t";
                        }
                    }
                }

                generateClassModelString += $@"
            return {modelName.ToLower()};
        }}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {{
            {modelName}Model model = new {modelName}Model();
            {modelName} {modelName.ToLower()} = Create{modelName}();

            lblResult.Text = model.Insert{modelName}({modelName.ToLower()});
        }}
    }}
}}";
                //Dodavanje aspx.cs
                string pagesClassModelPath = PagesDirPath + @"\Manage" + modelName + "s.aspx.cs";
                string csprojEdited = File.ReadAllText(csprojPath);
                int positionToIncludePageClassModel = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
                string stringToInsert = $@"<Compile Include=""Pages\Manage{modelName}s.aspx.cs"">" + "\n" +
  $@"<DependentUpon>Manage{modelName}s.aspx</DependentUpon>
       <SubType>ASPXCodeBehind</SubType>
     </Compile>" + Environment.NewLine + "\n\t";
                csprojEdited = EditCSProj.IncludePages(pagesClassModelPath, generateClassModelString, csprojPath, positionToIncludePageClassModel, stringToInsert);
            }
        }

        public static void GenerateAdminPage(string myAppName, XmlDocument xmlDocument, string csprojPath)
        {
            string adminPageString = string.Empty;
            string adminPageClassString = string.Empty;
            string adminPageDesignerString = string.Empty;

            XmlNode dbNameNode = xmlDocument.SelectSingleNode(@"gramer/db_name");
            string dbName = dbNameNode.InnerText; //Naziv baze

            StringBuilder sb = new StringBuilder("<%@ Page Title=\"\" Language=\"C#\" MasterPageFile=\"~/MasterPage.Master\" AutoEventWireup=\"true\" CodeBehind=\"Admin.aspx.cs\" Inherits=\"" + myAppName + ".Pages.Admin\" %>" + Environment.NewLine +
"<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"head\" runat=\"server\">" + Environment.NewLine +
     "</asp:Content>" + Environment.NewLine);
            StringWriter stringWriter = new StringWriter(sb);

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute("ID", "Content2");
                writer.AddAttribute("ContentPlaceHolderID", "ContentPlaceHolder1");
                writer.AddAttribute("runat", "server");
                writer.RenderBeginTag("asp:Content");

                int index = 0;
                foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
                {
                    string modelName = xmlNodeTableName.InnerText;

                    writer.AddAttribute("ID", "LinkButton" + index++.ToString());
                    writer.AddAttribute("runat", "server");
                    writer.AddAttribute("PostBackUrl", "~/Pages/Manage" + modelName + "s.aspx");
                    writer.RenderBeginTag("asp:LinkButton");
                    writer.Write("Add new " + modelName);
                    writer.RenderEndTag(); //End asp:LinkButton tag
                    writer.WriteLine();
                    writer.Write("<br/>");
                    writer.WriteLine();
                    writer.AddAttribute("ID", "grd" + modelName);
                    writer.AddAttribute("runat", "server");
                    writer.AddAttribute("AllowPaging", "True");
                    writer.AddAttribute("AllowSorting", "True");
                    writer.AddAttribute("AutoGenerateColumns", "False");
                    writer.AddAttribute("DataKeyNames", "ID");
                    writer.AddAttribute("DataSourceID", "sds" + modelName);
                    writer.AddAttribute("Width", "100%");
                    writer.AddAttribute("CellPadding", "4");
                    writer.AddAttribute("ForeColor", "#333333");
                    writer.AddAttribute("GridLines", "None");
                    writer.RenderBeginTag("asp:GridView");
                    writer.WriteLine();

                    writer.WriteBeginTag("AlternatingRowStyle");
                    writer.WriteAttribute("BackColor", "White");
                    writer.WriteAttribute("ForeColor", "#284775");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd); //End AlternatingRowStyle tag
                    writer.WriteLine();
                    writer.RenderBeginTag("Columns");

                    writer.WriteBeginTag("asp:CommandField");
                    writer.WriteAttribute("ShowDeleteButton", "True");
                    writer.WriteAttribute("ShowEditButton", "True");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("asp:BoundField");
                    writer.WriteAttribute("DataField", "ID");
                    writer.WriteAttribute("HeaderText", "ID");
                    writer.WriteAttribute("InsertVisible", "False");
                    writer.WriteAttribute("ReadOnly", "True");
                    writer.WriteAttribute("SortExpression", "ID");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();


                    int counter = 0;
                    foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                    {
                        string modelColumnName = xmlNodeTableColumns.InnerText;
                        if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                        {
                            writer.WriteBeginTag("asp:BoundField");
                            writer.WriteAttribute("DataField", modelColumnName);
                            writer.WriteAttribute("HeaderText", modelColumnName);
                            writer.WriteAttribute("SortExpression", modelColumnName);
                            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                            writer.WriteLine();
                            counter++;
                        }
                    }
                    writer.RenderEndTag(); //End Column tag
                    writer.WriteLine();

                    writer.WriteBeginTag("EditRowStyle");
                    writer.WriteAttribute("BackColor", "#999999");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("FooterStyle");
                    writer.WriteAttribute("BackColor", "#5D7B9D");
                    writer.WriteAttribute("Font-Bold", "True");
                    writer.WriteAttribute("ForeColor", "White");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("HeaderStyle");
                    writer.WriteAttribute("BackColor", "#5D7B9D");
                    writer.WriteAttribute("Font-Bold", "True");
                    writer.WriteAttribute("ForeColor", "White");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("PagerStyle");
                    writer.WriteAttribute("BackColor", "#284775");
                    writer.WriteAttribute("ForeColor", "White");
                    writer.WriteAttribute("HorizontalAlign", "Center");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("RowStyle");
                    writer.WriteAttribute("BackColor", "#F7F6F3");
                    writer.WriteAttribute("ForeColor", "#333333");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("SelectedRowStyle");
                    writer.WriteAttribute("BackColor", "#E2DED6");
                    writer.WriteAttribute("Font-Bold", "True");
                    writer.WriteAttribute("ForeColor", "#333333");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("SortedAscendingCellStyle");
                    writer.WriteAttribute("BackColor", "#E9E7E2");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("SortedAscendingHeaderStyle");
                    writer.WriteAttribute("BackColor", "#506C8C");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("SortedDescendingCellStyle");
                    writer.WriteAttribute("BackColor", "#FFFDF8");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.WriteBeginTag("SortedDescendingHeaderStyle");
                    writer.WriteAttribute("BackColor", "#6F8DAE");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();

                    writer.RenderEndTag(); //End asp:GridView tag
                    writer.WriteLine();

                    writer.AddAttribute("ID", "sds" + modelName);
                    writer.AddAttribute("runat", "server");
                    writer.AddAttribute("ConnectionString", "<%$ ConnectionStrings: " + dbName + "ConnectionString %>");
                    writer.AddAttribute("DeleteCommand", "DELETE FROM [" + modelName + "] WHERE [ID] = @ID");

                    int modelColumnsCounter = 0;
                    string insertQuery = "INSERT INTO [" + modelName + "] () VALUES ()";
                    string updateQuery = "UPDATE [" + modelName + "] SET ";
                    foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                    {
                        string modelColumnName = xmlNodeTableColumns.InnerText;
                        if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                        {
                            if (modelColumnsCounter < counter - 1)
                            {
                                modelColumnsCounter++;
                                insertQuery = insertQuery.Insert(insertQuery.IndexOf(")"), "[" + modelColumnName + "],");
                                insertQuery = insertQuery.Insert(insertQuery.Length - 1, "@" + modelColumnName + ",");
                                updateQuery = updateQuery.Insert(updateQuery.Length, "[" + modelColumnName + "] = @" + modelColumnName + ", ");
                            }
                            else
                            {
                                insertQuery = insertQuery.Insert(insertQuery.IndexOf(")"), "[" + modelColumnName + "]");
                                insertQuery = insertQuery.Insert(insertQuery.Length - 1, "@" + modelColumnName);
                                updateQuery = updateQuery.Insert(updateQuery.Length, "[" + modelColumnName + "] = @" + modelColumnName + " WHERE [ID] = @ID");
                            }
                        }
                    }
                    writer.AddAttribute("InsertCommand", insertQuery);
                    writer.AddAttribute("SelectCommand", "SELECT * FROM [" + modelName + "]");
                    writer.AddAttribute("UpdateCommand", updateQuery);
                    writer.RenderBeginTag("asp:SqlDataSource");

                    writer.RenderBeginTag("DeleteParameters");
                    writer.WriteBeginTag("asp:Parameter");
                    writer.WriteAttribute("Name", "ID");
                    writer.WriteAttribute("Type", "Int32");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.RenderEndTag(); //End DeleteParameters tag
                    writer.WriteLine();

                    writer.RenderBeginTag("InsertParameters");
                    foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                    {
                        string modelColumnName = xmlNodeTableColumns.InnerText;
                        if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                        {
                            string columnType = xmlNodeTableColumns.Attributes["type"].Value;
                            if (columnType == "int")
                                columnType = "int32";
                            else if (columnType == "image")
                                columnType = "string";
                            writer.WriteBeginTag("asp:Parameter");
                            writer.WriteAttribute("Name", modelColumnName);
                            writer.WriteAttribute("Type", columnType);
                            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                            writer.WriteLine();
                        }
                    }
                    writer.RenderEndTag(); //End InsertParameters tag
                    writer.WriteLine();

                    writer.RenderBeginTag("UpdateParameters");
                    foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                    {
                        string modelColumnName = xmlNodeTableColumns.InnerText;
                        if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                        {
                            string columnType = xmlNodeTableColumns.Attributes["type"].Value;
                            if (columnType == "int")
                                columnType = "int32";
                            else if (columnType == "image")
                                columnType = "string";
                            writer.WriteBeginTag("asp:Parameter");
                            writer.WriteAttribute("Name", modelColumnName);
                            writer.WriteAttribute("Type", columnType);
                            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                            writer.WriteLine();
                        }
                    }
                    writer.WriteBeginTag("asp:Parameter");
                    writer.WriteAttribute("Name", "ID");
                    writer.WriteAttribute("Type", "Int32");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.WriteLine();
                    writer.RenderEndTag(); //End UpdateParameters tag
                    writer.WriteLine();

                    writer.RenderEndTag(); //End asp:SqlDataSource tag
                    writer.WriteLine();
                }

                writer.RenderEndTag(); //End asp:Content tag

            }
            adminPageString = stringWriter.ToString();
            adminPageString = adminPageString.Replace("&lt;", "<");

            #region Generate Admin.aspx page
            string adminPagePath = PagesDirPath + @"\Admin.aspx";
            if (File.Exists(adminPagePath))
            {
                File.Delete(adminPagePath);
                using (StreamWriter sw = File.CreateText(adminPagePath))
                {
                    sw.Write(adminPageString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(adminPagePath))
                {
                    sw.Write(adminPageString);
                }
            }

            string csprojEdited = File.ReadAllText(csprojPath);
            int positionToIncludeAdminPage = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
            if (csprojEdited.Contains($@"<Content Include=""Pages\Admin.aspx"" />") == false) //Proveri da li aspx postoji u .csproj
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeAdminPage, $@"<Content Include=""Pages\Admin.aspx"" />" + Environment.NewLine + "\t");
                File.WriteAllText(csprojPath, csprojEdited);
            }
            #endregion

            adminPageClassString = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace {myAppName}.Pages
{{
    public partial class Admin : System.Web.UI.Page
    {{
        protected void Page_Load(object sender, EventArgs e)
        {{

        }}
    }}
}}";

            #region Generate Admin.aspx.cs page
            string adminPageClassPath = PagesDirPath + @"\Admin.aspx.cs";
            if (File.Exists(adminPageClassPath))
            {
                File.Delete(adminPageClassPath);
                using (StreamWriter sw = File.CreateText(adminPageClassPath))
                {
                    sw.Write(adminPageClassString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(adminPageClassPath))
                {
                    sw.Write(adminPageClassString);
                }
            }

            int positionToIncludeAdminPageClass = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            if (csprojEdited.Contains($@"<Compile Include=""Pages\Admin.aspx.cs"">") == false)
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeAdminPageClass, $@"<Compile Include=""Pages\Admin.aspx.cs"">" + "\n" +
  $@"<DependentUpon>Admin.aspx</DependentUpon>
       <SubType>ASPXCodeBehind</SubType>
     </Compile>" + Environment.NewLine + "\n\t");
                File.WriteAllText(csprojPath, csprojEdited);
            }
            #endregion

            #region Generate Admin.aspx.designer.cs
            string adminPageDesignerPath = PagesDirPath + @"\Admin.aspx.designer.cs";
            if (File.Exists(adminPageDesignerPath))
            {
                File.Delete(adminPageDesignerPath);
                using (StreamWriter sw = File.CreateText(adminPageDesignerPath))
                {
                    sw.Write(adminPageDesignerString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(adminPageDesignerPath))
                {
                    sw.Write(adminPageDesignerString);
                }
            }

            int positionToIncludeAdminPageDesigner = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            if (csprojEdited.Contains($@"<Compile Include=""Pages\Admin.aspx.designer.cs") == false)
            {
                csprojEdited = csprojEdited.Insert(positionToIncludeAdminPageDesigner, $@"<Compile Include=""Pages\Admin.aspx.designer.cs"">" + "\n" +
  $@"<DependentUpon>Admin.aspx</DependentUpon>" + "\n" +
@"</Compile> " + Environment.NewLine + "\n\t");
                File.WriteAllText(csprojPath, csprojEdited);
            }
            #endregion
        }

        public static void GenerateIndexPage(string myApp, XmlDocument xmlDocument, string csprojPath)
        {
            string generatedIndexPageString = string.Empty;
            string generatedIndexPageClassString = string.Empty;
            string generatedIndexPageDesignerString = string.Empty;

            XmlNode dbNameNode = xmlDocument.SelectSingleNode(@"gramer/db_name");
            string dbName = dbNameNode.InnerText; //Naziv baze

            #region Generate Index.aspx
            StringBuilder sb = new StringBuilder($@"<%@ Page Title="""" Language=""C#"" MasterPageFile=""~/MasterPage.Master"" AutoEventWireup=""true"" CodeBehind=""Index.aspx.cs"" Inherits=""{myApp}.Index"" %>" + "\n");
            StringWriter stringWriter = new StringWriter(sb);

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute("ID", "Content1");
                writer.AddAttribute("ContentPlaceHolderID", "head");
                writer.AddAttribute("runat", "server");
                writer.RenderBeginTag("asp:Content");
                writer.RenderEndTag(); //End asp:Content tag
                writer.WriteLine();

                writer.AddAttribute("ID", "Content2");
                writer.AddAttribute("ContentPlaceHolderID", "ContentPlaceHolder1");
                writer.AddAttribute("runat", "server");
                writer.RenderBeginTag("asp:Content");

                foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
                {
                    if (xmlNodeTableName.Attributes["index"] != null)
                    {
                        if (xmlNodeTableName.Attributes["index"].Value == "true")
                        {
                            writer.AddAttribute("ID", "pnl" + xmlNodeTableName.InnerText);
                            writer.AddAttribute("runat", "server");
                            writer.RenderBeginTag("asp:Panel");
                            writer.RenderEndTag(); //End asp:Panel tag
                            writer.WriteLine();
                        }
                    }
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Style, "clear:both");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag(); //End div tag
                writer.WriteLine();

                writer.RenderEndTag(); //End asp:Content tag
                writer.WriteLine();
            }
            generatedIndexPageString += stringWriter.ToString();
            string indexPagePath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Index.aspx";
            string csprojEdited = File.ReadAllText(csprojPath);
            int insertStartPosition = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
            string stringToInsert = $@"<Content Include=""Index.aspx"" />";

            csprojEdited = EditCSProj.IncludePages(indexPagePath, generatedIndexPageString, csprojPath, insertStartPosition, stringToInsert);
            #endregion

            #region Generate Index.aspx.cs
            generatedIndexPageClassString = $@"
using {myApp}.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace {myApp}
{{
    public partial class Index : System.Web.UI.Page
    {{
        protected void Page_Load(object sender, EventArgs e)
        {{
            FillPage();
        }}

        private void FillPage()
        {{";

            foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
            {
                string modelName = xmlNodeTableName.InnerText;
                if (xmlNodeTableName.Attributes["index"] != null)
                {
                    if (xmlNodeTableName.Attributes["index"].Value == "true")
                    {
                        generatedIndexPageClassString += $@"
            {modelName}Model {modelName.ToLower()}Model = new {modelName}Model();
            List<{modelName}> {modelName.ToLower()}s = {modelName.ToLower()}Model.GetAll{modelName}s();

            if ({modelName.ToLower()}s != null)
            {{
                foreach({modelName} {modelName.ToLower()} in {modelName.ToLower()}s)
                {{
                    Panel {modelName.ToLower()}Panel = new Panel();
                    ImageButton imageButton = new ImageButton();";

                        foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                        {
                            string modelColumnName = xmlNodeTableColumns.InnerText;
                            if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                            {
                                if (xmlNodeTableColumns.Attributes["index"] != null && xmlNodeTableColumns.Attributes["index"].Value == "true")
                                {
                                    if (xmlNodeTableColumns.Attributes["type"].Value == "string")
                                    {
                                        generatedIndexPageClassString += $@"
                    Label lbl{modelColumnName} = new Label();
                    lbl{modelColumnName}.Text = {modelName.ToLower()}.{modelColumnName};
                    lbl{modelColumnName}.CssClass = ""productName"";
                    { modelName.ToLower()}Panel.Controls.Add(lbl{modelColumnName});
                    {modelName.ToLower()}Panel.Controls.Add(new Literal {{ Text = ""<br/>"" }});
";
                                    }

                                    else if (xmlNodeTableColumns.Attributes["type"].Value == "decimal" || xmlNodeTableColumns.Attributes["type"].Value == "int")
                                    {
                                        generatedIndexPageClassString += $@"
                    Label lbl{modelColumnName} = new Label();
                    lbl{modelColumnName}.Text = {modelName.ToLower()}.{modelColumnName}.ToString();
                    lbl{modelColumnName}.CssClass = ""productPrice"";
                    {modelName.ToLower()}Panel.Controls.Add(lbl{modelColumnName});
                    {modelName.ToLower()}Panel.Controls.Add(new Literal {{ Text = ""<br/>"" }});
";
                                    }

                                    else if (xmlNodeTableColumns.Attributes["type"].Value == "image")
                                    {
                                        generatedIndexPageClassString += $@"
                    imageButton.ImageUrl = ""~/Images/Products/"" + {modelName.ToLower()}.Image;
                    imageButton.CssClass = ""productImage"";
                    imageButton.PostBackUrl = ""~/Pages/{modelName}Item.aspx?id="" + {modelName.ToLower()}.ID;
                    {modelName.ToLower()}Panel.Controls.Add(imageButton);
                    {modelName.ToLower()}Panel.Controls.Add(new Literal {{ Text = ""<br/>"" }});
";

                                    }
                                }
                            }
                        }
                        generatedIndexPageClassString += $@"

                    pnl{modelName}.Controls.Add({modelName.ToLower()}Panel);
                }}
            }}
            else
            {{
                pnl{modelName}.Controls.Add(new Literal {{ Text = ""No products found!"" }});
            }}
        }}
    }}
}}";
                    }
                }
            }

            string indexPageClassPath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Index.aspx.cs";
            insertStartPosition = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            stringToInsert = $@"<Compile Include=""Index.aspx.cs"">" + "\n" +
  $@"<DependentUpon>Index.aspx</DependentUpon>
       <SubType>ASPXCodeBehind</SubType>
     </Compile>" + Environment.NewLine + "\n\t";
            csprojEdited = EditCSProj.IncludePages(indexPageClassPath, generatedIndexPageClassString, csprojPath, insertStartPosition, stringToInsert);

            #endregion

            #region Generate Index.aspx.designer.cs
            foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
            {
                string modelName = xmlNodeTableName.InnerText;
                if (xmlNodeTableName.Attributes["index"] != null)
                {
                    if (xmlNodeTableName.Attributes["index"].Value == "true")
                    {
                        generatedIndexPageDesignerString = $@"
namespace {myApp} {{
    
    public partial class Index {{

        protected global::System.Web.UI.WebControls.Panel pnl{modelName};
    }}
}}";
                    }
                }
            }
            string indexPageDesignerPath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Index.aspx.designer.cs";
            stringToInsert = $@"<Compile Include=""Index.aspx.designer.cs"">" + "\n" +
  $@"<DependentUpon>Index.aspx</DependentUpon>" + "\n" +
@"</Compile> " + Environment.NewLine + "\n\t";
            csprojEdited = EditCSProj.IncludePages(indexPageDesignerPath, generatedIndexPageDesignerString, csprojPath, insertStartPosition, stringToInsert);
            #endregion
        }

        public static void GeneratePageItem(string myApp, XmlDocument xmlDocument, string csprojPath)
        {
            string generatedItemPageString = string.Empty;
            string generatedItemPageClassString = string.Empty;
            string generatedItemPageDesignerString = string.Empty;

            XmlNode dbNameNode = xmlDocument.SelectSingleNode(@"gramer/db_name");
            string dbName = dbNameNode.InnerText; //Naziv baze

            string itemModelName = string.Empty;
            string itemPagePath = string.Empty;
            string stringToInsert = string.Empty;
            foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
            {
                if (xmlNodeTableName.Attributes["index"] != null)
                {
                    if (xmlNodeTableName.Attributes["index"].Value == "true")
                    {
                        itemModelName = xmlNodeTableName.InnerText;
                        itemPagePath = $@"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\{itemModelName}Item.aspx";
                        stringToInsert = $@"<Content Include=""Pages\{itemModelName}Item.aspx"" />";
                    }
                }
            }

            #region Generate Item.aspx
            StringBuilder sb = new StringBuilder($@"<%@ Page Title="""" Language=""C#"" MasterPageFile=""~/MasterPage.Master"" AutoEventWireup=""true"" CodeBehind=""{itemModelName}Item.aspx.cs"" Inherits=""{myApp}.{itemModelName}Item"" %>" + "\n");
            StringWriter stringWriter = new StringWriter(sb);

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.AddAttribute("ID", "Content1");
                writer.AddAttribute("ContentPlaceHolderID", "head");
                writer.AddAttribute("runat", "server");
                writer.RenderBeginTag("asp:Content");
                writer.RenderEndTag(); //End asp:Content tag
                writer.WriteLine();

                writer.AddAttribute("ID", "Content2");
                writer.AddAttribute("ContentPlaceHolderID", "ContentPlaceHolder1");
                writer.AddAttribute("runat", "server");
                writer.RenderBeginTag("asp:Content");

                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
                {
                    string modelName = xmlNodeTableName.InnerText;
                    if (xmlNodeTableName.Attributes["index"] != null)
                    {
                        if (xmlNodeTableName.Attributes["index"].Value == "true")
                        {
                            foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                            {
                                string modelColumnName = xmlNodeTableColumns.InnerText;
                                if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                                {
                                    if (xmlNodeTableColumns.Attributes["type"] != null && xmlNodeTableColumns.Attributes["type"].Value == "image")
                                    {
                                        writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, "4");
                                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 400px");
                                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                        writer.WriteLine();
                                        writer.AddAttribute("ID", "img" + modelName);
                                        writer.AddAttribute("runat", "server");
                                        writer.AddAttribute("CssClass", "detailsImage");
                                        writer.RenderBeginTag("asp:Image");
                                        writer.RenderEndTag(); //End asp:Image tag
                                        writer.WriteLine();
                                        writer.RenderEndTag(); //End td tag
                                        writer.WriteLine();
                                    }
                                }
                            }

                            foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                            {
                                string modelColumnName = xmlNodeTableColumns.InnerText;
                                if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                                {
                                    if (xmlNodeTableColumns.Attributes["tittle"] != null && xmlNodeTableColumns.Attributes["tittle"].Value == "true")
                                    {
                                        writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 400px");
                                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                        writer.WriteLine();
                                        writer.RenderBeginTag(HtmlTextWriterTag.H2);
                                        writer.AddAttribute("ID", "lblTittle");
                                        writer.AddAttribute("runat", "server");
                                        writer.RenderBeginTag("asp:Label");
                                        writer.RenderEndTag(); //End asp:Image tag
                                        writer.WriteLine();
                                        writer.RenderEndTag(); //End h2 tag
                                        writer.WriteLine();
                                        writer.WriteLine("<hr/>");
                                        writer.RenderEndTag(); //End td tag
                                        writer.WriteLine();
                                    }
                                }
                            }
                        }
                    }
                }
                writer.RenderEndTag(); //End tr tag
                writer.WriteLine();

                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
                {
                    string modelName = xmlNodeTableName.InnerText;
                    if (xmlNodeTableName.Attributes["index"] != null)
                    {
                        if (xmlNodeTableName.Attributes["index"].Value == "true")
                        {
                            foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                            {
                                string modelColumnName = xmlNodeTableColumns.InnerText;
                                if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                                {
                                    if (xmlNodeTableColumns.Attributes["multiline"] != null && xmlNodeTableColumns.Attributes["multiline"].Value == "true")
                                    {
                                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                        writer.WriteLine();
                                        writer.AddAttribute("ID", "lbl" + modelColumnName);
                                        writer.AddAttribute("runat", "server");
                                        writer.AddAttribute("CssClass", "detailsDescription");
                                        writer.RenderBeginTag("asp:Label");
                                        writer.RenderEndTag(); //End asp:Label tag
                                        writer.WriteLine();
                                        writer.RenderEndTag(); //End td tag
                                        writer.WriteLine();
                                    }
                                }
                            }

                            foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                            {
                                string modelColumnName = xmlNodeTableColumns.InnerText;
                                if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                                {
                                    if (xmlNodeTableColumns.Attributes["multiline"] == null && xmlNodeTableColumns.Attributes["tittle"] == null
                                        && xmlNodeTableColumns.Attributes["type"].Value != "image" && xmlNodeTableColumns.Attributes["fk"] == null
                                        && (xmlNodeTableColumns.Attributes["type"].Value == "string" || xmlNodeTableColumns.Attributes["type"].Value == "decimal" || xmlNodeTableColumns.Attributes["type"].Value == "int"))
                                    {
                                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                                        writer.WriteLine();
                                        writer.AddAttribute("ID", "lbl" + modelColumnName);
                                        writer.AddAttribute("runat", "server");
                                        writer.AddAttribute("CssClass", "detailsPrice");
                                        writer.RenderBeginTag("asp:Label");
                                        writer.RenderEndTag(); //End asp:Label tag
                                        writer.WriteLine();
                                        writer.RenderEndTag(); //End td tag
                                        writer.WriteLine();
                                    }
                                }
                            }
                        }
                    }
                }
                writer.RenderEndTag(); //End tr tag
                writer.WriteLine();

                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.WriteLine();
                writer.WriteLine(itemModelName + " No: <br/>");
                writer.WriteLine();
                writer.AddAttribute("ID", "lblItemNumber");
                writer.AddAttribute("runat", "server");
                writer.AddAttribute("style", "font-style: italic");
                writer.RenderBeginTag("asp:Label");
                writer.RenderEndTag(); //End asp:Label tag
                writer.WriteLine();
                writer.RenderEndTag(); //End td tag
                writer.WriteLine();
                writer.RenderEndTag(); //End tr tag
                writer.WriteLine();

                writer.RenderEndTag(); //End table tag
                writer.WriteLine();
                writer.RenderEndTag(); //End asp:Content tag
                writer.WriteLine();
            }

            generatedItemPageString = stringWriter.ToString();
            string csprojEdited = File.ReadAllText(csprojPath);
            int insertStartPosition = csprojEdited.IndexOf(@"<Content Include=""Web.config""");

            csprojEdited = EditCSProj.IncludePages(itemPagePath, generatedItemPageString, csprojPath, insertStartPosition, stringToInsert);
            #endregion

            #region Generate Item.aspx.cs and Item.aspx.designer.cs
            generatedItemPageDesignerString = $@"
namespace {myApp} {{
    
    public partial class {itemModelName}Item {{";

            generatedItemPageClassString = $@"
using {myApp}.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace {myApp}
{{
    public partial class {itemModelName}Item : System.Web.UI.Page
    {{
        protected void Page_Load(object sender, EventArgs e)
        {{
            FillPage();
        }}

        private void FillPage()
        {{
            if(!String.IsNullOrWhiteSpace(Request.QueryString[""id""]))
            {{
                int id = Convert.ToInt32(Request.QueryString[""id""]);
                {itemModelName}Model {itemModelName.ToLower()}Model = new {itemModelName}Model();
                {itemModelName} {itemModelName.ToLower()} = {itemModelName.ToLower()}Model.Get{itemModelName}(id);
";
            foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
            {
                string modelName = xmlNodeTableName.InnerText;
                if (xmlNodeTableName.Attributes["index"] != null)
                {
                    if (xmlNodeTableName.Attributes["index"].Value == "true")
                    {
                        foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                        {
                            string modelColumnName = xmlNodeTableColumns.InnerText;
                            if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                            {
                                if (xmlNodeTableColumns.Attributes["type"] != null && xmlNodeTableColumns.Attributes["type"].Value == "image")
                                {
                                    generatedItemPageClassString += $@"
                img{modelName}.ImageUrl = ""~/Images/Products/"" + {modelName.ToLower()}.{modelColumnName};";
                                    generatedItemPageDesignerString += $@"
        protected global::System.Web.UI.WebControls.Image img{itemModelName};";
                                }
                                else if (xmlNodeTableColumns.Attributes["tittle"] != null && xmlNodeTableColumns.Attributes["tittle"].Value == "true")
                                {
                                    generatedItemPageClassString += $@"
                lblTittle.Text = {modelName.ToLower()}.{modelColumnName};";
                                    generatedItemPageDesignerString += $@"
        protected global::System.Web.UI.WebControls.Label lblTittle;";
                                }
                                else if (xmlNodeTableColumns.Attributes["multiline"] != null && xmlNodeTableColumns.Attributes["multiline"].Value == "true")
                                {
                                    generatedItemPageClassString += $@"
                lbl{modelColumnName}.Text = {modelName.ToLower()}.{modelColumnName};";
                                    generatedItemPageDesignerString += $@"
        protected global::System.Web.UI.WebControls.Label lbl{modelColumnName};";
                                }
                                else if (xmlNodeTableColumns.Attributes["multiline"] == null && xmlNodeTableColumns.Attributes["tittle"] == null
                                        && xmlNodeTableColumns.Attributes["type"].Value != "image" && xmlNodeTableColumns.Attributes["fk"] == null
                                        && (xmlNodeTableColumns.Attributes["type"].Value == "string" || xmlNodeTableColumns.Attributes["type"].Value == "decimal" || xmlNodeTableColumns.Attributes["type"].Value == "int"))
                                {
                                    generatedItemPageClassString += $@"
                lbl{modelColumnName}.Text = ""{modelColumnName}: "" + {modelName.ToLower()}.{modelColumnName}.ToString();";
                                    generatedItemPageDesignerString += $@"
        protected global::System.Web.UI.WebControls.Label lbl{modelColumnName};";
                                }
                            }
                        }
                    }
                }
            }
            generatedItemPageClassString += $@"
                lblItemNumber.Text = id.ToString();
            }}
        }}
    }}
}}
";
            generatedItemPageDesignerString += $@"
        protected global::System.Web.UI.WebControls.Label lblItemNumber;
    }}
}}";

            itemPagePath = $@"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\{itemModelName}Item.aspx.cs";
            insertStartPosition = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            stringToInsert = $@"<Compile Include=""Pages\{itemModelName}Item.aspx.cs"">" + "\n" +
  $@"<DependentUpon>{itemModelName}Item.aspx</DependentUpon>
       <SubType>ASPXCodeBehind</SubType>
     </Compile>" + Environment.NewLine + "\n\t";
            csprojEdited = EditCSProj.IncludePages(itemPagePath, generatedItemPageClassString, csprojPath, insertStartPosition, stringToInsert);

            stringToInsert = $@"<Compile Include=""Pages\{itemModelName}Item.aspx.designer.cs"">" + "\n" +
  $@"<DependentUpon>{itemModelName}Item.aspx</DependentUpon>" + "\n" +
@"</Compile> " + Environment.NewLine + "\n\t";
            itemPagePath = $@"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\{itemModelName}Item.aspx.designer.cs";
            csprojEdited = EditCSProj.IncludePages(itemPagePath, generatedItemPageDesignerString, csprojPath, insertStartPosition, stringToInsert);
            #endregion
        }

        public static void GenerateRegisterPage(string myApp, XmlDocument xmlDocument, string csprojPath)
        {
            string generatedRegisterString = string.Empty;
            string generatedRegisterClassString = string.Empty;
            string generatedRegisterDesignerString = string.Empty;

            string pagePath = string.Empty;
            string stringToInsert = string.Empty;

            #region Generate Register.aspx
            StringBuilder sb = new StringBuilder($@"<%@ Page Title="""" Language=""C#"" MasterPageFile=""~/MasterPage.Master"" AutoEventWireup=""true"" CodeBehind=""Register.aspx.cs"" Inherits=""{myApp}.Pages.Account.Register"" %>" + "\n");
            StringWriter stringWriter = new StringWriter(sb);

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {//<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
                writer.AddAttribute("ID", "Content1");
                writer.AddAttribute("ContentPlaceHolderID", "head");
                writer.AddAttribute("runat", "server");
                writer.RenderBeginTag("asp:Content");
                writer.RenderEndTag(); //End asp:Content tag
            }
            generatedRegisterString = stringWriter.ToString();
            pagePath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\Account\Register.aspx";
            string csprojEdited = File.ReadAllText(csprojPath);
            int insertStartPosition = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
            stringToInsert = $@"<Content Include=""Pages\Account\Register.aspx"" />";

            csprojEdited = EditCSProj.IncludePages(pagePath, generatedRegisterString, csprojPath, insertStartPosition, stringToInsert);
            #endregion

            #region Generate Register.aspx.designer.cs
            generatedRegisterDesignerString = $@"
namespace {myApp}.Pages.Account {{
    
    public partial class Register {{

    }}
}}";
            pagePath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\Account\Register.aspx.designer.cs";
            insertStartPosition = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            stringToInsert = $@"<Compile Include=""Pages\Account\Register.aspx.designer.cs"">" + "\n" +
  $@"<DependentUpon>Register.aspx</DependentUpon>" + "\n" +
@"</Compile> " + Environment.NewLine + "\n\t";

            csprojEdited = EditCSProj.IncludePages(pagePath, generatedRegisterDesignerString, csprojPath, insertStartPosition, stringToInsert);
            #endregion

            #region Generate Register.aspx.cs
            generatedRegisterClassString = $@"
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace {myApp}.Pages.Account
{{
    public partial class Register : System.Web.UI.Page
    {{

    }}
}}";
            pagePath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\Account\Register.aspx.cs";
            insertStartPosition = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            stringToInsert = $@"<Compile Include=""Pages\Account\Register.aspx.cs"">" + "\n" +
  $@"<DependentUpon>Register.aspx</DependentUpon>
       <SubType>ASPXCodeBehind</SubType>
     </Compile>" + Environment.NewLine + "\n\t";

            csprojEdited = EditCSProj.IncludePages(pagePath, generatedRegisterClassString, csprojPath, insertStartPosition, stringToInsert);
            #endregion
        }

        public static void GenerateLogInPage(string myApp, XmlDocument xmlDocument, string csprojPath)
        {
            string generatedRegisterString = string.Empty;
            string generatedRegisterClassString = string.Empty;
            string generatedRegisterDesignerString = string.Empty;

            string pagePath = string.Empty;
            string stringToInsert = string.Empty;

            #region Generate Register.aspx
            StringBuilder sb = new StringBuilder($@"<%@ Page Title="""" Language=""C#"" MasterPageFile=""~/MasterPage.Master"" AutoEventWireup=""true"" CodeBehind=""Login.aspx.cs"" Inherits=""{myApp}.Pages.Account.Login"" %>" + "\n");
            StringWriter stringWriter = new StringWriter(sb);

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {//<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
                writer.AddAttribute("ID", "Content1");
                writer.AddAttribute("ContentPlaceHolderID", "head");
                writer.AddAttribute("runat", "server");
                writer.RenderBeginTag("asp:Content");
                writer.RenderEndTag(); //End asp:Content tag
            }
            generatedRegisterString = stringWriter.ToString();
            pagePath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\Account\Login.aspx";
            string csprojEdited = File.ReadAllText(csprojPath);
            int insertStartPosition = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
            stringToInsert = $@"<Content Include=""Pages\Account\Login.aspx"" />";

            csprojEdited = EditCSProj.IncludePages(pagePath, generatedRegisterString, csprojPath, insertStartPosition, stringToInsert);
            #endregion

            #region Generate Register.aspx.designer.cs
            generatedRegisterDesignerString = $@"
namespace {myApp}.Pages.Account {{
    
    public partial class Login {{

    }}
}}";
            pagePath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\Account\Login.aspx.designer.cs";
            insertStartPosition = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            stringToInsert = $@"<Compile Include=""Pages\Account\Login.aspx.designer.cs"">" + "\n" +
  $@"<DependentUpon>Login.aspx</DependentUpon>" + "\n" +
@"</Compile> " + Environment.NewLine + "\n\t";

            csprojEdited = EditCSProj.IncludePages(pagePath, generatedRegisterDesignerString, csprojPath, insertStartPosition, stringToInsert);
            #endregion

            #region Generate Register.aspx.cs
            generatedRegisterClassString = $@"
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace {myApp}.Pages.Account
{{
    public partial class Login : System.Web.UI.Page
    {{

    }}
}}";
            pagePath = @"C:\Users\nikola.bastovanovic\source\repos\MyGeneratedApp\MyGeneratedApp\Pages\Account\Login.aspx.cs";
            insertStartPosition = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
            stringToInsert = $@"<Compile Include=""Pages\Account\Login.aspx.cs"">" + "\n" +
  $@"<DependentUpon>Login.aspx</DependentUpon>
       <SubType>ASPXCodeBehind</SubType>
     </Compile>" + Environment.NewLine + "\n\t";

            csprojEdited = EditCSProj.IncludePages(pagePath, generatedRegisterClassString, csprojPath, insertStartPosition, stringToInsert);
            #endregion
        }
    }
}