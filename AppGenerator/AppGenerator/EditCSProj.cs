using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGenerator
{
    class EditCSProj
    {
        public static string IncludePages(string pagesPath, string generatedPagesString, string csprojPath, int insertStartPosition, string stringToInsert)
        {
            string checkIfContains = stringToInsert.Substring(0, stringToInsert.IndexOf(">") + 1);
            if (File.Exists(pagesPath))
            {
                File.Delete(pagesPath);
                using (StreamWriter sw = File.CreateText(pagesPath))
                {
                    sw.Write(generatedPagesString);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(pagesPath))
                {
                    sw.Write(generatedPagesString);
                }
            }

            //Dodavanje aspx fajla u .csproj
            string csprojEdited = File.ReadAllText(csprojPath);
            if (csprojEdited.Contains(checkIfContains) == false) //Proveri da li aspx postoji u .csproj
            {
                csprojEdited = csprojEdited.Insert(insertStartPosition, stringToInsert + Environment.NewLine + "\t");
                File.WriteAllText(csprojPath, csprojEdited);
            }

            return csprojEdited;
        }
    }
}
