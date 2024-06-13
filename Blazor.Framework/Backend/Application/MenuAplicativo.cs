using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Dominus.Backend.Application
{

    public class MenuModel
    {
        public string Module { get; set; }
        public string Icon { get; set; }

        public List<Option> Options { get; set; }

    }

    public class Option
    {

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Resource { get; set; }
        public bool Havepermission { get; set; } = true;

    }

    public static class MenuAplicativo
    {
        public static List<MenuModel> Menus { get; set; } = new List<MenuModel>();

        public static void GetMenu(string pathMenu)
        {
            List<MenuModel> menu = new List<MenuModel>();
            if (File.Exists(pathMenu))
            {
                Menus = JsonConvert.DeserializeObject<List<MenuModel>>(File.ReadAllText(pathMenu));
            }
            else
            {
                DApp.LogToFile(LogType.Error, $"El archivo para el menu {pathMenu} no existe.");
            }
        }

    }

    //public class Menu
    //{
    //    public string Id { get; set; }

    //    public string Name { get; set; }

    //    public string Url { get; set; }

    //    public string Area { get; set; }

    //    public string Controller { get; set; }

    //    public string Action { get; set; }

    //    public string Parameters { get; set; }

    //    public string MainMenuId { get; set; }

    //    public bool Expanded { get; set; }

    //    public Dictionary<string, bool> Securities { get; set; }

    //    public List<Menu> Childs { get; set; }
    //}
}
