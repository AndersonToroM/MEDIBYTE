
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominus.Frontend.Mvc
{

    public class DataGridConfiguration<T>
    {

        private string Prefix { get; set; }
        DataGridBuilder<T> DataGrid { get; set; }
        ToolbarBuilder Toolbar { get; set; }

        private string UrlClick { get; set; }
        private object ParametersClick { get; set; }
        private string ContainerClick { get; set; }
        private object ParametersAdicionalClick { get; set; }
        private bool DisabledClick { get; set; }

        private string RecursoPadre { get; set; }
        private string UrlLink { get; set; }
        private string RecursoHijo { get; set; }
        private string PreFunctionLink { get; set; }
        private string PrefixPadreLink { get; set; }

        private string TooltipString { get; set; }
        private JS TooltipJs { get; set; }

        private string UrlNew { get; set; }
        private string ContainerNew { get; set; }

        private bool ModalNew { get; set; }
        private string TitleNew { get; set; }
        private string SizeNew { get; set; }
        private bool DisabledNew { get; set; }

        private string UrlDeleteList { get; set; }
        private bool DisabledDeleteList { get; set; }

        private bool ExportSelected { get; set; }
        private bool ExportAll { get; set; }

        private string TitleToolbar { get; set; }
        private string ContainerToolbar { get; set; }

        private List<ButtonToolbar> Buttons { get; set; }

        private bool PutStorage { get; set; } = false;

        private bool GoManual { get; set; }
        private string UrlManual { get; set; }

        public DataGridConfiguration(string prefix)
        {
            this.Prefix = prefix;
            ExportSelected = true;
            ExportAll = true;
        }

        public DataGridConfiguration(string prefix, string prefixPadre)
        {
            this.Prefix = prefix;
            PrefixPadreLink = prefixPadre;
            ExportSelected = true;
            ExportAll = true;
        }

        public DataGridConfiguration(DataGridBuilder<T> control, ToolbarBuilder toolbar)
        {
            this.DataGrid = control;
            this.Toolbar = toolbar;
        }


        public DataGridConfiguration<T> OnClick(string url, object parameters, string container = "mainPanel", object parametersAdicional = null)
        {
            UrlClick = url;
            ParametersClick = parameters;
            ContainerClick = container;
            ParametersAdicionalClick = parametersAdicional;
            return this;
        }

        public DataGridConfiguration<T> OnClick(string url, bool disabled, object parameters, string container = "mainPanel", object parametersAdicional = null)
        {
            UrlClick = url;
            ParametersClick = parameters;
            ContainerClick = container;
            ParametersAdicionalClick = parametersAdicional;
            DisabledClick = disabled;
            return this;
        }

        public DataGridConfiguration<T> New(string url, string container)
        {
            UrlNew = url;
            ContainerNew = container;
            ModalNew = false;
            return this;
        }

        public DataGridConfiguration<T> New(string url, bool disabled = false, string container = "mainPanel")
        {
            UrlNew = url;
            ContainerNew = container;
            ModalNew = false;
            DisabledNew = disabled;
            return this;
        }

        public DataGridConfiguration<T> NewModal(string url, string title, string size = "")
        {
            UrlNew = url;
            TitleNew = title;
            ModalNew = true;
            SizeNew = size;
            return this;
        }

        public DataGridConfiguration<T> DeleteList(string url)
        {
            UrlDeleteList = url;
            return this;
        }

        public DataGridConfiguration<T> DeleteList(string url, bool disabled = false)
        {
            UrlDeleteList = url;
            DisabledDeleteList = disabled;
            return this;
        }

        public DataGridConfiguration<T> Exports(bool exportSelected, bool exportAll)
        {
            ExportSelected = exportSelected;
            ExportAll = exportAll;
            return this;
        }

        public DataGridConfiguration<T> ToolbarTop(string title, string container = "mainPanel")
        {
            TitleToolbar = title;
            ContainerToolbar = container;
            return this;
        }

        public DataGridConfiguration<T> LinkNavigation(string recursoPadre, string url, string recursoHijo, string preFunction = null)
        {
            RecursoPadre = recursoPadre;
            UrlLink = url;
            RecursoHijo = recursoHijo;
            PreFunctionLink = preFunction;
            return this;
        }

        public DataGridConfiguration<T> TooltipText(string text)
        {
            TooltipString = text;
            TooltipJs = new JS();
            return this;
        }

        public DataGridConfiguration<T> TooltipText(JS action)
        {
            TooltipJs = action;
            TooltipString = null;
            return this;
        }


        public DataGridConfiguration<T> AddButtons(List<ButtonToolbar> buttons)
        {
            Buttons = buttons;
            return this;
        }

        public DataGridConfiguration<T> SetStorage()
        {
            this.PutStorage = true;
            return this;
        }

        public DataGridConfiguration<T> GoInstructionManual(string urlManual)
        {
            GoManual = true;
            UrlManual = urlManual;
            return this;
        }

        //Función Generadora del Componente

        public DataGridBuilder<T> BuilderToWidget(DataGridConfiguration<T> config)
        {
            var control = this.DataGrid;

            string Actions = @"var " + config.Prefix + @"GridDeleteList = '';
                               var urlNew = '';
                               var idContainer = '';";
            string ToolbarConfig = "{ ";
            JS OnToolbarPreparing = new JS("null");


            //ID del Componente

            control.ID(config.Prefix + "DataGrid");

            control.Paging(x => x.PageSize(15));

            //Click
            //if (!config.DisabledClick)
            //{
            if (config.UrlClick != null && config.ParametersClick != null && config.ContainerClick != null)
            {
                string OnClick;
                JObject parameters = JObject.Parse(JsonConvert.SerializeObject(config.ParametersClick));

                string values = "{";

                foreach (var each in parameters)
                {
                    values += each.Key + ":";
                    values += "grid.data." + each.Value.ToString() + ",";
                }

                if (config.ParametersAdicionalClick != null)
                {
                    JObject parametersAdicional = JObject.Parse(JsonConvert.SerializeObject(config.ParametersAdicionalClick));

                    foreach (var each in parametersAdicional)
                    {
                        values += each.Key + ":";
                        values += "'" + each.Value.ToString() + "',";
                    }
                }

                values += "}";

                //LinkNavigation

                if (config.RecursoPadre != null && config.UrlLink != null && config.RecursoHijo != null)
                {
                    OnClick = @"function(grid){";
                    string tooltipText = "";

                    tooltipText = config.TooltipString != null ? "'" + config.TooltipString + "'" : config.TooltipJs.Value + "()";

                    string link = "getLinkComponet('" + config.Prefix + @"DetailBread', '" + config.ContainerClick + @"',
                                                    """ + config.RecursoPadre + @""".replace(/ /g,'&nbsp;'),
                                                    '" + config.UrlLink + @"',
                                                    """ + config.RecursoHijo + @""".replace(/ /g,'&nbsp;')";

                    link += (config.PreFunctionLink != null) ? ",'" + config.PreFunctionLink + "'" : ",undefined";

                    if (config.PrefixPadreLink != null)
                    {
                        link += ",'" + config.PrefixPadreLink + "'";
                        link += ",'" + config.Prefix + "DataGrid" + "'";

                        OnClick += @"saveLastTab('" + config.PrefixPadreLink + @"');
                                     /*clearCurrentStorage('" + config.Prefix + "DataGridStorage" + @"');*/";


                        control.StateStoring(s => s
                            .Enabled(true)
                            .Type(StateStoringType.SessionStorage)
                            .SavingTimeout(0)
                            .StorageKey(config.Prefix + "DataGridStorage"));
                    }
                    else
                    {
                        link += ",undefined,undefined";
                    }

                    link += (config.TooltipString != null || config.TooltipJs.ToString() != null) ? "," + tooltipText + ".replace(/ /g,'&nbsp;')" : ",undefined";

                    link += ")";

                    OnClick += @"var url = '" + config.UrlClick + @"';
                                    GetViewNavigation(url," + link + @"," + values + @");
                                }";

                }
                else
                {
                    OnClick = @"function(grid){
                                            var url = '" + config.UrlClick + @"';
                                            var idParameters = " + values + @";
                                            var idContainer = '" + config.ContainerClick + @"';

                                            GetViewOnContainer(url, idContainer, idParameters);
                                           }";
                }

                control.OnRowClick(OnClick);

            }
            //}
            //else
            //{
            //    control.OnRowClick("function(){alert('No tiene permiso para Editar');}");
            //}

            //DeleteList

            if (config.UrlDeleteList != null)
            {
                Actions += @"var " + config.Prefix + @"GridDeleteList = function(){
                                    var urlDeleteList = '" + config.UrlDeleteList + @"';
                                    var models = $('#" + config.Prefix + @"DataGrid').dxDataGrid('instance').getSelectedRowsData();

                                    var result = DevExpress.ui.dialog.confirm('¿Desea eliminar los registros seleccionados?','Eliminación');
                                        result.done(function(dialogResult) {
                                            if (dialogResult)
                                            {
                                                DeleteList(urlDeleteList, models, '" + config.Prefix + @"DataGrid');
                                            }
                                        });
                                                        
                              };";

                ToolbarConfig += "DeleteList:true,";
            }
            else
                ToolbarConfig += "DeleteList:false,";

            //New

            if (config.UrlNew != null)
            {
                Actions += "var urlNew = '" + config.UrlNew + @"';
                            var idContainer = '" + config.ContainerNew + @"';";
                if (config.ModalNew)
                {
                    ToolbarConfig += "ButtonNewModal:true,";
                    ToolbarConfig += "TitleNewModal:'" + config.TitleNew + "',";
                    ToolbarConfig += "SizeNewModal:'" + config.SizeNew + "',";
                    ToolbarConfig += "Prefix:'" + config.Prefix + "',";
                }
                else
                    ToolbarConfig += "ButtonNew:true,";
            }
            else
            {
                ToolbarConfig += "ButtonNew:false,";
                ToolbarConfig += "ButtonNewModal:false,";
            }

            //Exports

            ToolbarConfig += config.ExportSelected ? "ExportSelected:true," : "ExportSelected:false,";

            ToolbarConfig += config.ExportAll ? "ExportAll:true," : "ExportAll:false,";


            //Disabled Buttons

            ToolbarConfig += config.DisabledClick ? "DisabledClick:true," : "DisabledClick:false,";
            ToolbarConfig += config.DisabledNew ? "DisabledNew:true," : "DisabledNew:false,";
            ToolbarConfig += config.DisabledDeleteList ? "DisabledDeleteList:true," : "DisabledDeleteList:false,";



            if (config.TitleToolbar != null && config.ContainerToolbar != null)
            {
                JS ToolbarOnInitialized =
                new JS(
                    @"function(toolbar){ 
                        " + Actions + @"
                        GetToolbar('" + config.Prefix + @"DataGrid', urlNew, idContainer, " + ToolbarConfig + @"}, " + config.Prefix + @"GridDeleteList,'" + config.Prefix + @"Toolbar');
                        }");

                Toolbar.ID(config.Prefix + "Toolbar")
                    .Items(i => i.Add()
                        .Text(config.TitleToolbar)
                        .Location(ToolbarItemLocation.Before))
                        .ElementAttr("class", "toolbarSection sticky-top")
                        .OnInitialized(ToolbarOnInitialized.Value);

                if (config.Buttons != null)
                {
                    foreach (var btn in config.Buttons)
                    {
                        Toolbar.Items(item => item.Add().Widget(
                            w => w.Button().ID(btn.Id)
                                           .Icon(btn.Icon)
                                           .Text(btn.Text)
                                           .OnClick(btn.Action)
                                           .Disabled(btn.Disabled)
                                           .Visible(btn.Visible))
                                            .Location(btn.Location)
                                            .LocateInMenu(btn.LocateInMenu));
                    }
                }

                if (config.GoManual)
                {
                    JS OnGoManual = new JS(@"function(){window.open('" + config.UrlManual + "', '_blank').focus();}");
                    Toolbar.Items(item => item.Add().Widget(w => w.Button().ID(config.Prefix + "GoManual").Icon("file").Text("Ver Manual").OnClick(OnGoManual.Value)).Location(ToolbarItemLocation.Before).LocateInMenu(ToolbarItemLocateInMenuMode.Always));
                }

                string toolbarComponent = Toolbar.ToString().Replace('\"', '\'').Replace("\r\n", "").Replace("\r", "").Replace("\n", "");

                string[] script = toolbarComponent.Split("</script>");
                string content = "";

                for (int i = 0; i < script.Length; i++)
                {
                    if (i == 0)
                    {
                        content += script[i] + "\".concat(\"</scr\".concat(\"ipt>";
                    }
                    else if (i == (script.Length - 1))
                    {
                        content += script[i] + "\"))";
                    }
                    else
                    {
                        content += script[i] + "\")).concat(\"</scr\".concat(\"ipt>";
                    }
                }

                string OnInitialized = @"function(){
                                                $('#" + config.ContainerToolbar + @"').prepend(""" + content + @");
                                                TabNavigation = [];
                                                RouteNavigation = """";
                                                $('#" + config.Prefix + @"DataGrid').dxDataGrid('instance').option('loadPanel.enabled',true);
                                                clearStorage('" + config.Prefix + @"');
                                          }";


                OnToolbarPreparing = new JS(@"function(grid){
                                                    showLoadIndicator();
                                                    setToolbarTop('" + config.Prefix + @"Toolbar', grid, '" + config.TitleToolbar + @"');
                                                }");

                control.OnInitialized(OnInitialized);
                control.LoadPanel(lp => lp.Enabled(false));
                control.OnContentReady("function(){ hideLoadIndicator(); }");
            }

            else
            {

                //Toolbar.Items(item => item.Add().Widget(
                //            w => w.Button().ID(btn.Id)
                //                           .Icon(btn.Icon)
                //                           .Text(btn.Text)
                //                           .OnClick(btn.Action)
                //                           .Disabled(btn.Disabled)
                //                           .Visible(btn.Visible))
                //                            .Location(btn.Location)
                //                            .LocateInMenu(btn.LocateInMenu));

                if (config.Buttons != null && config.Buttons.Count > 0)
                {
                    ToolbarConfig += "ListNewButtons : [";
                    foreach (var btn in config.Buttons)
                    {
                        ToolbarConfig += $@"{{Id : '{btn.Id ?? "null"}' , 
                                            Icon : '{btn.Icon ?? "null"}', 
                                            Text : '{btn.Text ?? "null"}', 
                                            Action : {btn.Action ?? "null"}, 
                                            Disabled : {btn.Disabled.ToString().ToLower()}, 
                                            Visible : {btn.Visible.ToString().ToLower()}, 
                                            Location : '{btn.Location.ToString().ToLower()}', 
                                            LocateInMenu : '{btn.LocateInMenu.ToString().ToLower()}'}},";
                    }
                    ToolbarConfig += "],";
                }

                string tooltipText = config.TooltipString != null ? "'" + config.TooltipString + "'.replace(/ /g,'&nbsp;')" : config.TooltipJs.Value;
                string container = config.ContainerClick != null ? config.ContainerClick : config.ContainerNew;

                string link = "getLinkComponet('" + config.Prefix + @"DetailBread', '" + container + @"',
                                                    """ + config.RecursoPadre + @""".replace(/ /g,'&nbsp;'),
                                                    '" + config.UrlLink + @"',
                                                    """ + config.RecursoHijo + @""".replace(/ /g,'&nbsp;')";

                link += (config.PreFunctionLink != null) ? ",'" + config.PreFunctionLink + "'" : ",undefined";

                if (config.PrefixPadreLink != null)
                {
                    link += ",'" + config.PrefixPadreLink + "'";
                    link += ",'" + config.Prefix + "DataGrid" + "'";

                    control.StateStoring(s => s
                        .Enabled(true)
                        .Type(StateStoringType.SessionStorage)
                        .SavingTimeout(0)
                        .StorageKey(config.Prefix + "DataGridStorage"));
                }
                else
                {
                    link += ",undefined,undefined";
                }

                link += (config.TooltipString != null || config.TooltipJs.ToString() != null) ? "," + tooltipText : ",undefined";

                link += ")";

                OnToolbarPreparing =
                new JS(
                    @"function(grid){ 
                        " + Actions + @"
                        GetToolbar(grid, urlNew, idContainer, " + ToolbarConfig + @"}, " + config.Prefix + @"GridDeleteList,''," + link + @");
                        }");

                control.ElementAttr("class", config.PrefixPadreLink != null ? "gridToolbar PersistentDataGrid" : "gridToolbar");
            }

            control.OnToolbarPreparing(OnToolbarPreparing.Value);

            control.Export(x => x.Enabled(false).FileName(config.Prefix + "Export_" + DateTime.Now.ToString()));

            control.StateStoring(s => s
                .Enabled(config.PutStorage)
                .Type(StateStoringType.LocalStorage)
                .StorageKey($"SiisoGridStorage{config.Prefix}")
                .SavingTimeout(0));



            return control;
        }

    }

}