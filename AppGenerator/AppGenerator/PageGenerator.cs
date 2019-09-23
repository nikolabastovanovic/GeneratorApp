﻿using System;
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
              <asp:SqlDataSource ID=""SqlDataSource1"" runat=""server"" ConnectionString=""<%$ ConnectionStrings:{dbName}ConnectionString %>"" SelectCommand = ""SELECT * FROM [{xmlNodeTableColumns.Attributes["fk"].Value}] ORDER BY [ID]""></asp:SqlDataSource>" + "\n</p>";

                            aspxDesignerString += $@"protected global::System.Web.UI.WebControls.DropDownList ddl{modelColumnName};" + "\n";
                            aspxDesignerString += @"protected global::System.Web.UI.WebControls.SqlDataSource SqlDataSource1;" + "\n";
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

                string pagesPath = PagesDirPath + @"\Manage" + modelName + "s.aspx";
                string csprojEdited = File.ReadAllText(csprojPath);
                int positionToIncludeAspx = csprojEdited.IndexOf(@"<Content Include=""Web.config""");
                csprojEdited = EditCSProj.IncludePages(pagesPath, generatedPagesString, csprojPath, positionToIncludeAspx, $@"<Content Include=""Pages\Manage{modelName}s.aspx"" />");

                string pagesDesignerPath = PagesDirPath + @"\Manage" + modelName + "s.aspx.designer.cs";


                //if (File.Exists(pagesDesignerPath))
                //{
                //    File.Delete(pagesDesignerPath);
                //    using (StreamWriter sw = File.CreateText(pagesDesignerPath))
                //    {
                //        sw.Write(aspxDesignerString);
                //    }
                //}
                //else
                //{
                //    using (StreamWriter sw = File.CreateText(pagesDesignerPath))
                //    {
                //        sw.Write(aspxDesignerString);
                //    }
                //}

                int positionToIncludePageDesigner = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
                //            if (csprojEdited.Contains($@"<Compile Include=""Pages\Manage{modelName}s.aspx.designer.cs"">") == false) //Proveri da li vec postoji MasterPage.Master.designer.cs u .csproj
                //            {
                //                csprojEdited = csprojEdited.Insert(positionToIncludePageDesigner, $@"<Compile Include=""Pages\Manage{modelName}s.aspx.designer.cs"">" + "\n" +
                //  $@"<DependentUpon>Manage{modelName}s.aspx</DependentUpon>" + "\n" +
                //@"</Compile> " + Environment.NewLine + "\n\t");
                //                File.WriteAllText(csprojPath, csprojEdited);
                //            }
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
                        else if (xmlNodeTableColumns.Attributes["type"].Value == "string")
                        {
                            generateClassModelString += $@"{modelName.ToLower()}.{modelColumnName} = ddl{modelColumnName}.SelectedValue;" + "\n\t\t\t";
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

                string pagesClassModelPath = PagesDirPath + @"\Manage" + modelName + "s.aspx.cs";
                //if (File.Exists(pagesClassModelPath))
                //{
                //    File.Delete(pagesClassModelPath);
                //    using (StreamWriter sw = File.CreateText(pagesClassModelPath))
                //    {
                //        sw.Write(generateClassModelString);
                //    }
                //}
                //else
                //{
                //    using (StreamWriter sw = File.CreateText(pagesClassModelPath))
                //    {
                //        sw.Write(generateClassModelString);
                //    }
                //}

                string csprojEdited = File.ReadAllText(csprojPath);
                int positionToIncludePageClassModel = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
                //           if (csprojEdited.Contains($@"<Compile Include=""Pages\Manage{modelName}s.aspx.cs"">") == false) //Proveri da li vec postoji MasterPage.Master.designer.cs u .csproj
                //           {
                //               csprojEdited = csprojEdited.Insert(positionToIncludePageClassModel, $@"<Compile Include=""Pages\Manage{modelName}s.aspx.cs"">" + "\n" +
                // $@"<DependentUpon>Manage{modelName}s.aspx</DependentUpon>
                //  <SubType>ASPXCodeBehind</SubType>
                //</Compile>" + Environment.NewLine + "\n\t");
                //               File.WriteAllText(csprojPath, csprojEdited);

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
    }
}