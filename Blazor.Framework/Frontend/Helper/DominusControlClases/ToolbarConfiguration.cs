
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using System.Collections.Generic;

namespace Dominus.Frontend.Mvc
{

    public class ToolbarConfiguration
    {
        private string Prefix { get; set; }
        private ToolbarBuilder Toolbar { get; set; }

        private string UrlBack { get; set; }
        private string ContainerBack { get; set; }
        private string FunctionBack { get; set; }

        private string TitleToolbar { get; set; }

        private bool ActionSave { get; set; }
        private string FunctionSave { get; set; }
        private bool DisabledSave { get; set; }

        private string UrlNew { get; set; }
        private string ContainerNew { get; set; }
        private string FunctionNew { get; set; }
        private bool DisabledNew { get; set; }

        private string UrlDelete { get; set; }
        private string UrlDeleteResult { get; set; }
        private bool NuevoDelete { get; set; }
        private string ContainerDelete { get; set; }
        private string FunctionDelete { get; set; }
        private bool DisabledDelete { get; set; }
        private ButtonToolbar BtnDelete { get; set; }

        private string UrlDuplicate { get; set; }
        private bool NuevoDuplicate { get; set; }
        private string ContainerDuplicate { get; set; }
        private string FunctionDuplicate { get; set; }
        private bool DisabledDuplicate { get; set; }

        private bool LinkNavigationToolbar { get; set; }

        private List<ButtonToolbar> Buttons { get; set; }
        private bool FirstOfAll { get; set; }

        private bool Focus { get; set; }
        
        private bool GoManual { get; set; }
        private string UrlManual { get; set; }

        public ToolbarConfiguration(string prefix)
        {
            this.Prefix = prefix;
            BtnDelete = new ButtonToolbar();
            Focus = true;
        }

        public ToolbarConfiguration(ToolbarBuilder toolbar)
        {
            this.Toolbar = toolbar;
        }

        public ToolbarConfiguration Back(string url, string preFunction = null, string container = "mainPanel")
        {
            UrlBack = url;
            FunctionBack = preFunction;
            ContainerBack = container;
            return this;
        }

        public ToolbarConfiguration Title(string title)
        {
            TitleToolbar = title;
            return this;
        }

        public ToolbarConfiguration Save(bool disabled, bool nuevo, string preFunction = null)
        {
            ActionSave = true;
            FunctionSave = preFunction;
            if(nuevo)
                DisabledSave = false;
            else
                DisabledSave = disabled;
            return this;
        }

        public bool GetDisabledSave()
        {
            return this.DisabledSave;
        }

        public ToolbarConfiguration New(string url, string container, string preFunction = null)
        {
            UrlNew = url;
            ContainerNew = container;
            FunctionNew = preFunction;
            return this;
        }

        public ToolbarConfiguration New(string url, bool disabled, string preFunction = null, string container = "mainPanel")
        {
            UrlNew = url;
            DisabledNew = disabled;
            FunctionNew = preFunction;
            ContainerNew = container;
            return this;
        }

        public ToolbarConfiguration Delete(string urlDelete, string urlResult, bool nuevo, string container, string preFunction = null)
        {
            UrlDelete = urlDelete;
            UrlDeleteResult = urlResult;
            NuevoDelete = nuevo;
            ContainerDelete = container;
            FunctionDelete = preFunction;
            return this;
        }
        public ToolbarConfiguration ButtonDelete(ButtonToolbar config)
        {
            BtnDelete = config;
            return this;
        }

        public ToolbarConfiguration Delete(string urlDelete, bool disabled, string urlResult, bool nuevo, string preFunction = null, string container = "mainPanel")
        {
            UrlDelete = urlDelete;
            DisabledDelete = disabled;
            UrlDeleteResult = urlResult;
            NuevoDelete = nuevo;
            FunctionDelete = preFunction;
            ContainerDelete = container;
            return this;
        }

        public ToolbarConfiguration Duplicate(string url, bool nuevo, string container, string preFunction = null)
        {
            UrlDuplicate = url;
            NuevoDuplicate = nuevo;
            ContainerDuplicate = container;
            FunctionDuplicate = preFunction;
            return this;
        }

        public ToolbarConfiguration Duplicate(string url, bool disabled, bool nuevo, string preFunction = null, string container = "mainPanel")
        {
            UrlDuplicate = url;
            DisabledDuplicate = disabled;
            NuevoDuplicate = nuevo;
            FunctionDuplicate = preFunction;
            ContainerDuplicate = container;
            return this;
        }

        public ToolbarConfiguration LinkNavigation()
        {
            LinkNavigationToolbar = true;
            return this;
        }

        public ToolbarConfiguration AddButtons(List<ButtonToolbar> buttons, bool firstOfAll = true)
        {
            FirstOfAll = firstOfAll;
            Buttons = buttons;
            return this;
        }

        public ToolbarConfiguration FocusFirstInput(bool enabled)
        {
            Focus = enabled;
            return this;
        }

        public ToolbarConfiguration GoInstructionManual(string urlManual)
        {
            GoManual = true;
            UrlManual = urlManual;
            return this;
        }

        //Función Generadora del Componente

        public ToolbarBuilder BuilderToWidget(ToolbarConfiguration config)
        {
            var control = this.Toolbar;
            string Focus = config.Focus ? "true" : "false";

            //ID del Componente

            control.ID(config.Prefix + "Toolbar");

            //Back

            if (config.UrlBack != null && config.ContainerBack != null)
            {
                if (config.FunctionBack != null)
                    config.FunctionBack += "()";
                else
                    config.FunctionBack += "true";

                JS OnBackClick = new JS(@"function(){
                                                if(" + config.FunctionBack + @") {
                                                    GetViewOnContainer('" + config.UrlBack + @"','" + config.ContainerBack + @"'); 
                                                }
                                            }");

                control.Items(item => item.Add().Widget(w => w.Button().ID(config.Prefix + "Back").Icon("back").OnClick(OnBackClick.Value)).Location(ToolbarItemLocation.Before));
            }

            //Title

            if (config.TitleToolbar != null)
            {
                control.Items(item => item.Add().Text(config.TitleToolbar).Location(ToolbarItemLocation.Before));
            }

            //Add Button Before

            if (config.FirstOfAll && config.Buttons != null)
            {
                foreach (var btn in config.Buttons)
                {
                    control.Items(item => item.Add().Widget(
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

            //Save

            if (config.ActionSave)
            {
                if (config.FunctionSave != null)
                    config.FunctionSave += "()";
                else
                    config.FunctionSave += "true";

                JS OnSaveClick = new JS(@"function(params){
                                                params.prefixForm = '" + config.Prefix + @"'; 
                                                params.preFunctionSave = " + config.FunctionSave + @"; 
                                                BtnSaveValidationGroup_click(params);
                                            }");

                control.Items(item => item.Add().Widget(w => w.Button().ID(config.Prefix + "Save").ValidationGroup(config.Prefix + "ValidationGroup").Icon("save").Text("Guardar").OnClick(OnSaveClick.Value).Disabled(config.DisabledSave)).Location(ToolbarItemLocation.After));
            }

            //New

            if (config.UrlNew != null && config.ContainerNew != null)
            {
                if (config.FunctionNew != null)
                    config.FunctionNew += "()";
                else
                    config.FunctionNew += "true";

                JS OnNewClick = new JS(@"function(){
                                            $('input').blur();
                                            if(" + config.FunctionNew + @") {
                                                saveLastTab('" + config.Prefix + @"');
                                                GetViewOnContainer('" + config.UrlNew + @"','" + config.ContainerNew + @"');
                                            }
                                        }");

                control.Items(item => item.Add().Widget(w => w.Button().ID(config.Prefix + "New").Icon("add").Text("Nuevo").OnClick(OnNewClick.Value).Disabled(config.DisabledNew)).Location(ToolbarItemLocation.After));
            }

            //Delete

            if (config.UrlDelete != null && config.UrlDeleteResult != null && config.ContainerDelete != null)
            {
                if (config.FunctionDelete != null)
                    config.FunctionDelete += "()";
                else
                    config.FunctionDelete += "true";

                JS OnDeleteClick = new JS(@"function(){
                                                if(" + config.FunctionDelete + @") {
                                                    var result = DevExpress.ui.dialog.confirm('¿Desea eliminar este registro?','Eliminación');
                                                    result.done(function(dialogResult) {
                                                        if (dialogResult)
                                                        {
                                                            DeleteFormAction('" + config.Prefix + @"','" + config.UrlDelete + @"');
                                                        }
                                                    }); 
                                                }
                                            }");

                control.Items(item => item.Add().Widget(w => w.Button().ID(config.Prefix + "Delete")
                    .Icon(config.BtnDelete.Icon ?? "trash")
                    .Text(config.BtnDelete.Text ?? "Borrar")
                    .OnClick(OnDeleteClick.Value)
                    .Disabled(config.NuevoDelete || config.DisabledDelete))
                    .Location(ToolbarItemLocation.After));
            }

            //Duplicate

            if (config.UrlDuplicate != null && config.ContainerDuplicate != null)
            {
                if (config.FunctionDuplicate != null)
                    config.FunctionDuplicate += "()";
                else
                    config.FunctionDuplicate += "true";

                JS OnDuplicateClick = new JS(@"function(){
                                                if(" + config.FunctionDuplicate + @") {
                                                    saveLastTab('" + config.Prefix + @"');
                                                    GetViewOnContainer('" + config.UrlDuplicate + @"','" + config.ContainerDuplicate + @"'); 
                                                }
                                            }");

                control.Items(item => item.Add().Widget(w => w.Button().ID(config.Prefix + "Duplicate").Icon("repeat").Text("Duplicar").OnClick(OnDuplicateClick.Value).Disabled(config.NuevoDuplicate || config.DisabledDuplicate)).Location(ToolbarItemLocation.After));
            }


            //Add Buttons After

            if (!config.FirstOfAll && config.Buttons != null)
            {
                foreach (var btn in config.Buttons)
                {
                    control.Items(item => item.Add().Widget(
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

            string ContentReady = $@"function(){{
                                        var btnDuplicate = $('#{config.Prefix}Duplicate .dx-button-content i');                 
                                        btnDuplicate.removeClass();
                                        btnDuplicate.addClass('dx-icon material-icons');
                                        btnDuplicate.html('file_copy');
                                        
                                        $(document).ready(function(){{
                                            if({Focus}){{
                                                setFocusFirstElement();         
                                            }}
                                            //ValidatioChangeForm = $('#{config.Prefix}Form').serialize();
                                            focusSequence('{config.Prefix}');
                                            keyNavigationPane();";

            //Save

            if (config.GoManual)
            {
                JS OnGoManual = new JS(@"function(){window.open('"+ config.UrlManual + "', '_blank').focus();}");

                control.Items(item => item.Add().Widget(w => w.Button().ID(config.Prefix + "GoManual").Icon("file").Text("Ver Manual").OnClick(OnGoManual.Value)).Location(ToolbarItemLocation.Before).LocateInMenu(ToolbarItemLocateInMenuMode.Always));
            }

            //LinkNavigation

            if (config.LinkNavigationToolbar)
            {

                ContentReady += $@"$('#{config.Prefix}DetailBread').append(RouteNavigation);
                                    $.each(TooltipNavigation, function (index, value) {{
                                        $('#{config.Prefix}DetailBread').after(value[1]);
                                    }});";
            }

            control.OnContentReady(ContentReady + $"setTab('{config.Prefix}');}});}}");

            control.ElementAttr("class", "toolbarSection sticky-top");

            return control;
        }

    }

    public class ButtonToolbar
    {

        public string Id { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Action { get; set; }
        public ToolbarItemLocateInMenuMode LocateInMenu { get; set; }
        public bool Disabled { get; set; }
        public bool Visible { get; set; }
        public ToolbarItemLocation Location { get; set; }

        public ButtonToolbar()
        {
            Visible = true;
            LocateInMenu = ToolbarItemLocateInMenuMode.Auto;
        }
    }
}
