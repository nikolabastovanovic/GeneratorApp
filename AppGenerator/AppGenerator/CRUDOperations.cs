using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AppGenerator
{
    class CRUDOperations
    {
        public static void DbOperations(string myAppName, XmlDocument xmlDocument, string modelsDirPath, string csprojPath)
        {
            //string generatedModelsString = string.Empty;

            XmlNode dbNameNode = xmlDocument.SelectSingleNode(@"gramer/db_name");
            string dbName = dbNameNode.InnerText; //Naziv baze

            foreach (XmlNode xmlNodeTableName in xmlDocument.GetElementsByTagName("name"))
            {
                string modelName = xmlNodeTableName.InnerText;
                string generatedModelsString = $@"
using {myAppName};
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace {myAppName}.Models
{{
    public class {modelName}Model
    {{
        public string Insert{modelName}({modelName} {modelName.ToLower()})
        {{
            try
            {{
                {dbName}Entities db = new {dbName}Entities();
                db.{modelName}s.Add({modelName.ToLower()});
                db.SaveChanges();

                return {modelName.ToLower()}.ID + "" was succesufully inserted."";
            }}
            catch (Exception e)
            {{
                return ""Error: "" + e;
            }}
        }}

        public string Update{modelName}(int id, {modelName} {modelName.ToLower()})
        {{
            try
            {{
                {dbName}Entities db = new {dbName}Entities();
                {modelName} tmp = db.{modelName}s.Find(id);" + "\n";

                foreach (XmlNode xmlNodeTableColumns in xmlDocument.GetElementsByTagName("column"))
                {
                    string a = xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText;
                    string modelColumnName = xmlNodeTableColumns.InnerText;
                    //p.Name = { modelName.ToLower()}.Name;
                    if (xmlNodeTableColumns.ParentNode.ParentNode.FirstChild.InnerText == modelName)
                    {
                        generatedModelsString = generatedModelsString + "\t\t\t\ttmp." + modelColumnName + " = " + modelName.ToLower() + "." + modelColumnName + ";\n";
                    }
                }
generatedModelsString = generatedModelsString + "\t\t\t\t" +
                $@"db.SaveChanges();
                return {modelName.ToLower()}.ID + "" was succesufully updated."";
            }}
            catch (Exception e)
            {{
                return ""Error: "" + e;
            }}
        }}

        public string Delete{modelName}(int id)
        {{
            try
            {{
                {dbName}Entities db = new {dbName}Entities();
                {modelName} {modelName.ToLower()} = db.{modelName}s.Find(id);

                db.{modelName}s.Attach({modelName.ToLower()});
                db.{modelName}s.Remove({modelName.ToLower()});
                db.SaveChanges();

                return {modelName.ToLower()}.ID + "" was succesufully deleted."";
            }}
            catch (Exception e)
            {{
                return ""Error: "" + e;
            }}
        }}

        public {modelName} Get{modelName}(int id)
        {{
            try
            {{
                using ({dbName}Entities db = new {dbName}Entities())
                {{
                    {modelName} {modelName.ToLower()} = db.{modelName}s.Find(id);
                    return {modelName.ToLower()};
                }}
            }}
            catch (Exception)
            {{
                return null;
            }}
        }}

        public List<{modelName}> GetAll{modelName}s()
        {{
            try
            {{
                using({dbName}Entities db = new {dbName}Entities())
                {{
                    List<{modelName}> {modelName.ToLower()}s = (from x in db.{modelName}s select x).ToList();
                    return {modelName.ToLower()}s;
                }}
            }}
            catch (Exception)
            {{
                return null;
            }}
        }}
    }}
}}
";
                string modelsPath = modelsDirPath + @"\" + modelName + "Model.cs";
                if (File.Exists(modelsPath))
                {
                    File.Delete(modelsPath);
                    using (StreamWriter sw = File.CreateText(modelsPath))
                    {
                        sw.Write(generatedModelsString);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(modelsPath))
                    {
                        sw.Write(generatedModelsString);
                    }
                }

                //Dodavanje Models u .csproj
                string csprojEdited = File.ReadAllText(csprojPath);
                int positionToIncludeModeles = csprojEdited.IndexOf(@"<Compile Include=""Properties\AssemblyInfo.cs""");
                if (csprojEdited.Contains($"<Compile Include=\"Models\\{modelName}Model.cs\" />") == false) //Proveri da li vec postoji Models u .csproj
                {
                    csprojEdited = csprojEdited.Insert(positionToIncludeModeles, $"<Compile Include=\"Models\\{modelName}Model.cs\" />" + Environment.NewLine + "\t");
                    File.WriteAllText(csprojPath, csprojEdited);
                }

            }
        }
    }
}
