
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc.Factories;
using Dominus.Backend.Application;
using System;
using System.Text;

namespace Dominus.Frontend.Mvc
{

    public class ToolbarDetailConfiguration
    {
        private ToolbarBuilder Toolbar { get; set; }
        public string Prefix { get; set; }
        public string UrlDelete { get; set; }

        private bool ActionSave { get; set; }
        private bool DisableSave { get; set; }
        private string preFunctionSave { get; set; }
        private bool IsByAjaxSave { get; set; }


        private bool ActionCancel { get; set; }
        private bool DisableCancel { get; set; }
        private string preFunctionCancel { get; set; }

        private bool ActionDelete { get; set; }
        private bool DisableDelete { get; set; }
        private string preFunctionDelete { get; set; }
        private bool IsByAjaxDelete { get; set; }

        public ToolbarDetailConfiguration(ToolbarBuilder toolbar)
        {
            this.Toolbar = toolbar;
        }

        public ToolbarDetailConfiguration(string Prefix)
        {
            this.Prefix = Prefix;
            this.ActionSave = false;
            this.ActionCancel = false;
            this.ActionDelete = false;
            this.DisableCancel = false;
            this.DisableDelete = false;
            this.DisableSave = false;
            this.preFunctionSave = null;
            this.preFunctionCancel = null;
            this.preFunctionDelete = null;
            this.IsByAjaxSave = false;
            this.IsByAjaxDelete = false;
        }

        public ToolbarDetailConfiguration Save()
        {
            this.ActionSave = true;
            return this;
        }

        public ToolbarDetailConfiguration Save(bool isByAjax = false)
        {
            this.ActionSave = true;
            this.IsByAjaxSave = isByAjax;
            return this;
        }

        public ToolbarDetailConfiguration Save(bool disable = false, bool isByAjax = false)
        {
            this.ActionSave = true;
            this.DisableSave = disable;
            this.IsByAjaxSave = isByAjax;
            return this;
        }

        public ToolbarDetailConfiguration Save(bool disable = false, string preFunctionSave = null, bool isByAjax = false)
        {
            this.ActionSave = true;
            this.preFunctionSave = preFunctionSave;
            this.DisableSave = disable;
            this.IsByAjaxSave = isByAjax;
            return this;
        }

        public ToolbarDetailConfiguration Save(string preFunctionSave = null, bool isByAjax = false)
        {
            this.ActionSave = true;
            this.preFunctionSave = preFunctionSave;
            this.IsByAjaxSave = isByAjax;
            return this;
        }

        public ToolbarDetailConfiguration Cancel()
        {
            this.ActionCancel = true;
            return this;
        }

        public ToolbarDetailConfiguration Cancel(bool disable = true)
        {
            this.ActionCancel = true;
            this.DisableCancel = disable;
            return this;
        }

        public ToolbarDetailConfiguration Cancel(bool disable = true, string preFunctionCancel = null)
        {
            this.ActionCancel = true;
            this.DisableCancel = disable;
            this.preFunctionCancel = preFunctionCancel;
            return this;
        }

        public ToolbarDetailConfiguration Cancel(string preFunctionCancel)
        {
            this.ActionCancel = true;
            this.preFunctionCancel = preFunctionCancel;
            return this;
        }

        public ToolbarDetailConfiguration Delete(string urlDelete)
        {
            this.ActionDelete = true;
            this.UrlDelete = urlDelete;
            return this;
        }

        public ToolbarDetailConfiguration Delete(string urlDelete, bool isByAjax = false)
        {
            this.ActionDelete = true;
            this.UrlDelete = urlDelete;
            this.IsByAjaxDelete = isByAjax;
            return this;
        }

        public ToolbarDetailConfiguration Delete(string urlDelete, bool disable = true, bool isByAjax = false)
        {
            this.ActionDelete = true;
            this.DisableDelete = disable;
            this.UrlDelete = urlDelete;
            this.IsByAjaxDelete = isByAjax;
            return this;
        }

        public ToolbarDetailConfiguration Delete(string urlDelete, bool disable = true, string preFunctionDelete = null, bool isByAjax = false)
        {
            this.ActionDelete = true;
            this.DisableDelete = disable;
            this.preFunctionDelete = preFunctionDelete;
            this.UrlDelete = urlDelete;
            this.IsByAjaxDelete = isByAjax;
            return this;
        }

        public ToolbarDetailConfiguration Delete(string urlDelete, string preFunctionDelete = null, bool isByAjax = false)
        {
            this.ActionDelete = true;
            this.preFunctionDelete = preFunctionDelete;
            this.UrlDelete = urlDelete;
            this.IsByAjaxDelete = isByAjax;
            return this;
        }

        public ToolbarBuilder BuilderToWidget(ToolbarDetailConfiguration config)
        {
            var control = this.Toolbar;
            control.ID(config.Prefix + "ToolbarDetail");

            //Boton Guardar
            if (config.ActionSave)
            {
                if (!string.IsNullOrWhiteSpace(config.preFunctionSave))
                    config.preFunctionSave += "()";
                else
                    config.preFunctionSave += "true";

                JS OnSaveClick = new JS(@"function(params){
                                                params.prefixForm = '" + config.Prefix + @"'; 
                                                params.preFunctionSave = " + config.preFunctionSave + @"; 
                                                BtnSaveValidationGroup_click(params);
                                            }");

                if (config.IsByAjaxSave)
                {
                    OnSaveClick = new JS(@"function(){ SaveFormAjaxDetail('" + config.Prefix + "'," + config.preFunctionSave + "); }");
                }

                control.Items(item => item.Add().Location(ToolbarItemLocation.Before).Widget(w => w.Button().ID(config.Prefix + "SaveDetail").ValidationGroup(config.Prefix + "ValidationGroupDetail")
                .Icon("save").Text("Guardar").UseSubmitBehavior(!config.IsByAjaxSave)
                .OnClick(OnSaveClick.Value)));
            }

            //Boton Cancelar
            if (config.ActionCancel)
            {
                if (!string.IsNullOrWhiteSpace(config.preFunctionCancel))
                    config.preFunctionCancel += "()";
                else
                    config.preFunctionCancel += "true";

                JS onCancelclick = new JS(@"function(){ RemoveContainer('"+config.Prefix+"',"+config.preFunctionCancel+"); }");

                control.Items(item => item.Add().Widget(w => w.Button().ID(config.Prefix + "CancelDetail").Icon("clear").Text("Cancelar").OnClick(onCancelclick.Value)).Location(ToolbarItemLocation.Before));
            }

            //Boton Borrar
            if (config.ActionDelete)
            {
                if (!string.IsNullOrWhiteSpace(config.preFunctionDelete))
                    config.preFunctionDelete += "()";
                else
                    config.preFunctionDelete += "true";

                JS onDeleteClick = new JS(@"function(){
                                                if(" + config.preFunctionDelete + @") {
                                                    var result = DevExpress.ui.dialog.confirm('¿Desea eliminar este registro?','Eliminación');
                                                    result.done(function(dialogResult) {
                                                        if (dialogResult)
                                                        {
                                                            DeleteFormAction('" + config.Prefix + @"','" + config.UrlDelete + @"');
                                                        }
                                                    }); 
                                                }
                                            }");

                if (config.IsByAjaxDelete)
                {
                    onDeleteClick = new JS(@"function(){
                                                if(" + config.preFunctionDelete + @") {
                                                    var result = DevExpress.ui.dialog.confirm('¿Desea eliminar este registro?','Eliminación');
                                                    result.done(function(dialogResult) {
                                                        if (dialogResult)
                                                        {
                                                            DeleteFormAjaxDetail('" + config.Prefix + @"','" + config.UrlDelete + @"');
                                                        }
                                                    }); 
                                                }
                                            }");
                }

                control.Items(item => item.Add().Location(ToolbarItemLocation.Before).Widget(w => w.Button().ID(config.Prefix + "DeleteDetail")
                .Icon("trash").Text("Borrar").Disabled(config.DisableDelete).UseSubmitBehavior(!config.IsByAjaxDelete)
                .OnClick(onDeleteClick.Value)));
            }

            control.ElementAttr("class", "toolbarSectionDetail");
            return control;
        }

        //public Action<FormItemsFactory<T>> GetButtonsDetail<T>(FormItemsFactory<T> data)
        //{
        //    Action<FormItemsFactory<T>> messageTarget;
        //    messageTarget = BuilderButtonsDetail;
        //    messageTarget(data);
        //    return messageTarget;
        //}
        //private void BuilderButtonsDetail<T>(FormItemsFactory<T> data)
        //{
        //    data.AddButton().HorizontalAlignment(HorizontalAlignment.Left)
        //            .ButtonOptions(b => b.Text(DApp.GetResource("ButtonSave")).Type(ButtonType.Success).UseSubmitBehavior(true));
        //    data.AddButton().HorizontalAlignment(HorizontalAlignment.Left)
        //            .ButtonOptions(b => b.Text(DApp.GetResource("ButtonCancel"))
        //            .OnClick("function(){RemoveContainer('"+Prefix+"')}")
        //            .Type(ButtonType.Normal));
        //    data.AddButton().ColSpan(10).HorizontalAlignment(HorizontalAlignment.Right)
        //            .ButtonOptions(b => b.Text(DApp.GetResource("ButtonDelete"))
        //            .OnClick("function(){DeleteFormAction('" + Prefix + "','" + UrlDelete + "');}")
        //            .Type(ButtonType.Danger));

        //}

    }

    //public class ButtonsDetail<T> 
    //{

    //    public Action<FormItemsFactory<T>> GetButtonsDetail(FormItemsFactory<T> data,string Prefix,string UrlDelete)
    //    {
    //        Action<FormItemsFactory<T>> messageTarget;
    //        messageTarget = ShowWindowsMessage;
    //        messageTarget(data);
    //        return messageTarget;
    //    }
    //    private static void ShowWindowsMessage(FormItemsFactory<T> data)
    //    {
    //        data.AddButton().HorizontalAlignment(HorizontalAlignment.Left)
    //                .ButtonOptions(b => b.Text(DApp.GetResource("APP.BtnSave")).Type(ButtonType.Success).UseSubmitBehavior(true));
    //        data.AddButton().HorizontalAlignment(HorizontalAlignment.Left)
    //                .ButtonOptions(b => b.Text(DApp.GetResource("APP.BtnCancel")).Type(ButtonType.Normal));
    //        data.AddButton().ColSpan(10).HorizontalAlignment(HorizontalAlignment.Right)
    //                .ButtonOptions(b => b.Text(DApp.GetResource("APP.BtnDelete"))
    //                .OnClick("function (){DeleteFormAction('" + Prefix + "','" + UrlDelete + "');}")
    //                .Type(ButtonType.Danger));

    //    }

    //}

}
