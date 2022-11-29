using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc.Factories;
using DevExtreme.AspNet.Mvc.Internals;
using Dominus.Backend.Application;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Dominus.Frontend.Mvc
{
    public class DominusWidgetFactory<TModel> : WidgetFactory<TModel>
    {
        public DominusWidgetFactory(OptionsOwnerBuilder owner, IHtmlHelperAdapter html) : base(owner, html)
        {
        }

        #region Controles Siesa

        public DataGridBuilder<object> DataGridBase()
        {
            var control = this.DataGrid();
            control
            .AllowColumnReordering(true)
            .ShowRowLines(true)
            .RowAlternationEnabled(true)
            .ShowBorders(true)
            .Pager(p => p
               .AllowedPageSizes(new int[] { 10, 15, 20 })
               .ShowInfo(true)
               .ShowNavigationButtons(true)
               .ShowPageSizeSelector(true)
               .Visible(true))
            .Paging(p => p.PageSize(10))
            .Export(e => e.Enabled(true).FileName("file" + DateTime.Now.ToLongDateString()).AllowExportSelectedData(true))
            .GroupPanel(groupPanel => groupPanel.Visible(true))
            .SearchPanel(searchPanel => searchPanel
               .Visible(true)
               .Width(240)
               .Placeholder("Buscar")
            )
            .HeaderFilter(headerFilter => headerFilter.Visible(true))
            .FilterRow(p => p.Visible(true))
            .ColumnAutoWidth(true);
            
            return control;
        }

        public DataGridBuilder<object> DataGridSimple()
        {
            var control = this.DataGrid();
            control
            .ShowColumnLines(true)
            .Selection(s => s.Mode(SelectionMode.Multiple).ShowCheckBoxesMode(GridSelectionShowCheckBoxesMode.Always))
            .ShowRowLines(true)
            .RemoteOperations(x => x.GroupPaging(true).GroupPaging(true).Paging(true).Sorting(true).Filtering(true).Summary(true))
            .RowAlternationEnabled(true)
            .ShowBorders(true)
            .Pager(p => p
               .AllowedPageSizes(new int[] { 10, 50, 100 })
               .ShowInfo(true)
               .ShowNavigationButtons(true)
               .ShowPageSizeSelector(false)
               .Visible(true))
            .Paging(p => p.PageSize(10))
            .Export(e => e.Enabled(true).FileName("file" + DateTime.Now.ToLongDateString()).AllowExportSelectedData(true))
            .GroupPanel(p => p.Visible(false))
            .SearchPanel(searchPanel => searchPanel
               .Visible(true)
               .Width(240)
               .Placeholder("Buscar")
            )
            .FocusedRowEnabled(true)
            .HeaderFilter(headerFilter => headerFilter.Visible(true))
            .FilterRow(p => p.Visible(true))
            .ColumnAutoWidth(true);
            return control;
        }

        public DataGridBuilder<T> DataGridSimple<T>()
        {

            var control = this.DataGrid<T>();
            control
            .ShowColumnLines(true)
            .Selection(s => s.Mode(SelectionMode.Multiple).ShowCheckBoxesMode(GridSelectionShowCheckBoxesMode.Always))
            .ShowRowLines(true)
            .RemoteOperations(true)
            .RowAlternationEnabled(true)
            .ShowBorders(true)
            .Pager(p => p
               .AllowedPageSizes(new int[] { 10, 50, 100 })
               .ShowInfo(true)
               .ShowNavigationButtons(true)
               .ShowPageSizeSelector(false)
               .Visible(true))
            .Paging(p => p.PageSize(10))
            .Export(e => e.Enabled(true).FileName("file" + typeof(T).GetType().Name + DateTime.Now.ToLongDateString()).AllowExportSelectedData(true))
            .GroupPanel(p => p.Visible(false))
            .SearchPanel(searchPanel => searchPanel
               .Visible(true)
               .Width(240)
               .Placeholder("Buscar")
            )
            .HeaderFilter(headerFilter => headerFilter.Visible(true))
            .FilterRow(p => p.Visible(true))
            .ColumnAutoWidth(true);
            return control;
        }

        public DataGridBuilder<T> DataGridGroup<T>()
        {
            var control = this.DataGrid<T>();
            control
            .ShowColumnLines(true)
            .ShowRowLines(true)
            .RowAlternationEnabled(true)
            .ShowBorders(true)
            .Pager(p => p
               .AllowedPageSizes(new int[] { 10, 50, 100 })
               .ShowInfo(true)
               .ShowNavigationButtons(true)
               .ShowPageSizeSelector(false)
               .Visible(true))
            .Paging(p => p.PageSize(8))
            .Export(e => e.Enabled(true).FileName("file" + DateTime.Now.ToLongDateString()).AllowExportSelectedData(true))
            .GroupPanel(p => p.Visible(true))
            .SearchPanel(searchPanel => searchPanel
               .Visible(true)
               .Width(240)
               .Placeholder("Buscar")
            )
            .HeaderFilter(headerFilter => headerFilter.Visible(true))
            .FilterRow(p => p.Visible(false))
            .ColumnAutoWidth(true);
            return control;
        }

        //public SelectBoxBuilder Conexiones<TProperty>(Expression<Func<TModel, TProperty>> expression)
        //{
        //    var control = this.SelectBoxFor(expression);
        //    control
        //       .DataSource(DApp.DataBaseSettings, "NroConexion")
        //       .DisplayExpr("Nombre")
        //       .ValueExpr("NroConexion")
        //       .SearchEnabled(true)
        //       .Placeholder("Selecione una conexion")
        //       .ShowClearButton(true);
        //    return control;
        //}


        #region Control Entidad

        #region General

        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> ControlEntidad<TProperty, TExternalModel>(string Prefix, Expression<Func<TModel, TProperty>> expression = null)
        {
            var control = new ControlEntidadBuilder<TModel, TProperty, TExternalModel>(this).Prefix(Prefix);
            return control;
        }

        #endregion General

        #region Dictionary

        //    public SelectBoxBuilder ControlEntidadFor<TProperty>(Expression<Func<TModel, TProperty>> expression, Dictionary<string, string[]> configuration,
        //                                                      Action<CollectionFactory<DataGridColumnBuilder<object>>> columns)
        //    {
        //        var control = this.SelectBoxFor(expression);
        //        return selectBoxControlEntidad(configuration, columns, control);
        //    }

        //    public SelectBoxBuilder ControlEntidad(Dictionary<string, string[]> configuration,
        //                                           Action<CollectionFactory<DataGridColumnBuilder<object>>> columns)
        //    {
        //        var control = this.SelectBox();
        //        return selectBoxControlEntidad(configuration, columns, control);
        //    }

        //    private SelectBoxBuilder selectBoxControlEntidad(Dictionary<string, string[]> configuration,
        //                                                        Action<CollectionFactory<DataGridColumnBuilder<object>>> columns,
        //                                                        SelectBoxBuilder control)
        //    {
        //        string containerId = "controlEntidadContainer";
        //        string modalId = "controlEntidadModal";
        //        string[] configure = { configuration["Component"].GetValue(0).ToString(),
        //                               configuration["Component"].GetValue(1).ToString(),
        //                               configuration["Modal"].GetValue(0).ToString(),
        //                               configuration["Modal"].GetValue(1).ToString()};

        //        var grid = gridControlEntidad(columns, configure[0]);

        //        JS onRowClickDataGrid = new JS(@"function DataGridModalRowClik(grid){

        //                                            var itemsControl = $('#" + configure[0] + @"').dxSelectBox('getDataSource').items();
        //                                            itemsControl.splice(0, itemsControl.length);
        //                                            itemsControl.unshift(grid.data);

        //                                            $('#" + configure[0] + @"').dxSelectBox('instance').option('value', grid.data['" + configure[1] + @"']);
        //                                            $.when($('#" + modalId + @"').modal('hide')).then(function(){
        //                                                $('.modal-backdrop').remove();
        //                                                $('#" + containerId + @"').remove();
        //                                            });
        //                                         }");

        //        grid.OnRowClick(onRowClickDataGrid.Value);

        //        string contentModal = TemplateControlModal().Replace("IdModal", modalId).Replace("TitleModal", configure[2]).Replace("ContentModal", grid.ToString())
        //                                                    .Replace('\"', '\'').Replace(Environment.NewLine, "").Replace("</script>", "</scr\"+\"ipt>");

        //        JS openModal = new JS(@"function(data){ 
        //                                                $('<div>').attr('id','" + containerId + @"').appendTo('body');
        //                                                var closeDisplay = function(){
        //                                                    $('#" + configure[0] + @"').dxSelectBox('instance').option('disabled',true);
        //                                                    $('.dx-overlay-wrapper').css('display', 'none');
        //                                                    $('#" + configure[0] + @"').dxSelectBox('instance').option('opened',false);
        //                                                }; 
        //                                                $.when(closeDisplay()).then(function(){
        //                                                    $('#" + containerId + @"').html(""" + contentModal + @""");
        //                                                    $('#" + modalId + @" .modal-dialog').css({'max-width':'1000px','min-width':'500px','width':" + configure[3] + @" + 'px'});
        //                                                    $('.dx-overlay-wrapper').css('display','block');
        //                                                    $('#" + modalId + @"').on('hide.bs.modal', function (e) {
        //                                                        $('#" + configure[0] + @"').dxSelectBox('instance').option('disabled',false);
        //                                                    });
        //                                                    $('#" + modalId + @"').modal('show')});
        //                                              }");

        //        control.ID(configure[0])
        //            .ValueExpr(configure[1])
        //            .DataSourceOptions(o => o.Paginate(true).PageSize(10))
        //            .OnOpened(openModal.Value)
        //            .InputAttr("class", "form-control");

        //        return control;
        //    }

        //    private DataGridBuilder<object> gridControlEntidad(Action<CollectionFactory<DataGridColumnBuilder<object>>> columns, string controlId)
        //{
        //    var control = this.DataGridSimple();
        //    control.DataSource(new JS(@"$('#" + controlId + @"').dxSelectBox('instance').option('dataSource')"))
        //        .Export(x => x.Enabled(false))
        //        .Selection(s => s.Mode(SelectionMode.None).ShowCheckBoxesMode(GridSelectionShowCheckBoxesMode.None))
        //        .Columns(columns)
        //        .ElementAttr("class", "gridToolbar");
        //    return control;
        //}

        #endregion Dictionary

        #region Sin y Con Modelo

        public SelectBoxBuilder ControlEntidad(ControlEntidadConfiguration config)
            {
                var control = this.SelectBox();
                var dataGrid = this.DataGridSimple();

                control = new ControlEntidadConfiguration(control, dataGrid).BuilderToWidget(config);
                return control;
            }

        public SelectBoxBuilder ControlEntidadFor<TProperty>(Expression<Func<TModel, TProperty>> expression, ControlEntidadConfiguration config)
        {
            var control = this.SelectBoxFor(expression);
            var dataGrid = this.DataGridSimple();

            control = new ControlEntidadConfiguration(control, dataGrid).BuilderToWidget(config);
            return control;
        }

        public SelectBoxBuilder ControlEntidad<T>(ControlEntidadConfiguration<T> config)
        {
            var control = this.SelectBox();
            var dataGrid = this.DataGridSimple<T>();

            control = new ControlEntidadConfiguration<T>(control, dataGrid).BuilderToWidget(config);
            return control;
        }

        public SelectBoxBuilder ControlEntidadFor<TProperty, T>(Expression<Func<TModel, TProperty>> expression, ControlEntidadConfiguration<T> config)
            {
                var control = this.SelectBoxFor(expression);
                var dataGrid = this.DataGridSimple<T>();

                control = new ControlEntidadConfiguration<T>(control, dataGrid).BuilderToWidget(config);
                return control;
            }

        #endregion Sin y Con Modelo


        #endregion

        #region Grid with JavaScript functions

        public DataGridBuilder<T> DataGridSimple<T>(Dictionary<string, string[]> actions)
        {
            var control = this.DataGridSimple<T>();
            var toolbar = this.Toolbar();
            JS OnClick = new JS("null");
            JS OnToolbarPreparing = new JS("null");
            JS OnInitialized = new JS("null");

            foreach (var item in actions)
            {
                if (item.Key == "Prefix")
                {
                    control.ID(item.Value[0] + "Grid");

                }
                else if (item.Key == "OnClick")
                {
                    JObject parameters = JObject.Parse(item.Value[1]);

                    string values = "{";

                    foreach (var each in parameters)
                    {
                        values += each.Key + ":";
                        values += "grid.data['" + each.Value.ToString() + "'],";
                    }

                    values += "}";

                    if (actions.ContainsKey("LinkNavigation"))
                    {
                        string link = "getLinkComponet('" + actions["Prefix"].GetValue(0) + @"DetailBread', '" + item.Value[2] + @"',
                                                """ + actions["LinkNavigation"].GetValue(0) + @""".replace(/ /g,'&nbsp;'),
                                                '" + actions["LinkNavigation"].GetValue(1) + @"',
                                                """ + actions["LinkNavigation"].GetValue(2) + @""".replace(/ /g,'&nbsp;') 
                                               )";

                        OnClick = new JS(@"function(grid){
                                        showLoadIndicator();
                                        var url = '" + item.Value[0] + @"';
                                        GetViewNavigation(url," + link + @"," + values + @");
                                       }");

                    }
                    else
                    {
                        OnClick = new JS(@"function(grid){
                                        showLoadIndicator();
                                        var url = '" + item.Value[0] + @"';
                                        var idParameters = " + values + @";
                                        var idContainer = '" + item.Value[2] + @"';

                                        GetViewOnContainer(url, idContainer, idParameters);
                                       }");
                    }



                }
                else if (item.Key == "Actions")
                {
                    if (actions.ContainsKey("ToolbarTop"))
                    {

                        JS ToolbarOnInitialized = new JS(
                                                    @"function(toolbar){

                                                        var " + actions["Prefix"].GetValue(0) + @"GridDeleteList = function(){
                                                        var urlDeleteList = '" + item.Value[0] + @"';
                                                        var models = $('#" + actions["Prefix"].GetValue(0) + @"Grid').dxDataGrid('instance').getSelectedRowsData();

                                                        var result = DevExpress.ui.dialog.confirm('¿Desea eliminar registros seleccionados?','Eliminación');
                                                            result.done(function(dialogResult) {
                                                                if (dialogResult)
                                                                {
                                                                    DeleteList(urlDeleteList, models, '" + actions["Prefix"].GetValue(0) + @"Grid');
                                                                }
                                                            });
                                                        
                                                        };
  
                                                        var url = '" + item.Value[1] + @"';
                                                        var idContainer = '" + item.Value[2] + @"';
                                                        var toolbarConfig = {ButtonNew:true,DeleteList:true,ExportSelected:true,ExportAll:true};

                                                        GetToolbar('" + actions["Prefix"].GetValue(0) + @"Grid', url, idContainer, toolbarConfig, " + actions["Prefix"].GetValue(0) + @"GridDeleteList,'" + actions["ToolbarTop"].GetValue(0) + @"');
                                                    }");


                        toolbar.ID(actions["ToolbarTop"].GetValue(0).ToString())
                               .Items(i => i.Add()
                                .Text(actions["ToolbarTop"].GetValue(1).ToString())
                                .Location(ToolbarItemLocation.Before))
                                .ElementAttr("class", "toolbarSection sticky-top")
                                .OnInitialized(ToolbarOnInitialized.Value);


                        string toolbarComponent = toolbar.ToString().Replace('\"', '\'').Replace(Environment.NewLine, "").Replace("</script>", "</scr\"+\"ipt>");

                        OnInitialized = new JS(@"function(){
                                                              $('#" + actions["ToolbarTop"].GetValue(2) + @"').prepend(""" + toolbarComponent + @""");
                                                            }");

                        OnToolbarPreparing = new JS(@"function(grid){
                                                        setToolbarTop('" + actions["ToolbarTop"].GetValue(0) + @"', grid);
                                                    }");



                    }

                    else if (actions.ContainsKey("LinkNavigation"))
                    {

                        string link = "getLinkComponet('" + actions["Prefix"].GetValue(0) + @"DetailBread', '" + item.Value[2] + @"',
                                                """ + actions["LinkNavigation"].GetValue(0) + @""".replace(/ /g,'&nbsp;'),
                                                '" + actions["LinkNavigation"].GetValue(1) + @"',
                                                """ + actions["LinkNavigation"].GetValue(2) + @""".replace(/ /g,'&nbsp;') 
                                               )";

                        OnToolbarPreparing = new JS(@"
                                                function(grid){
                                                    var " + actions["Prefix"].GetValue(0) + @"GridDeleteList = function(){
                                                    var urlDeleteList = '" + item.Value[0] + @"';
                                                    var models = $('#" + actions["Prefix"].GetValue(0) + @"Grid').dxDataGrid('instance').getSelectedRowsData();

                                                    var result = DevExpress.ui.dialog.confirm('¿Desea eliminar registros seleccionados?','Eliminación');
                                                        result.done(function(dialogResult) {
                                                            if (dialogResult)
                                                            {
                                                                DeleteList(urlDeleteList, models, '" + actions["Prefix"].GetValue(0) + @"Grid');
                                                            }
                                                    });
                                                };
  
                                                var url = '" + item.Value[1] + @"';
                                                var idContainer = '" + item.Value[2] + @"';
                                                var toolbarConfig = {ButtonNew:true,DeleteList:true,ExportSelected:true,ExportAll:true};
                                                var link = " + link + @";

                                                GetToolbar(grid, url, idContainer, toolbarConfig, " + actions["Prefix"].GetValue(0) + @"GridDeleteList,'',link);
                                                }");
                        control.ElementAttr("class", "gridToolbar");

                    }

                }

            }


            control.OnRowClick(OnClick.Value)
                 .OnInitialized(OnInitialized.Value)
                 .OnToolbarPreparing(OnToolbarPreparing.Value)
                 .Export(x => x.Enabled(false).FileName(actions["Prefix"].GetValue(0) + "Export_" + DateTime.Now.ToString()));
            return control;
        }

        public DataGridBuilder<T> DataGridSimple<T>(DataGridConfiguration<T> config)
        {
            var control = this.DataGridSimple<T>();
            var toolbar = this.Toolbar();
            control = new DataGridConfiguration<T>(control, toolbar).BuilderToWidget(config);

            return control;
        }

        #endregion

        # region ToolbarTop

        public ToolbarBuilder Toolbar(Dictionary<string, string[]> actions)
        {
            var control = this.Toolbar();

            if (actions.ContainsKey("Back"))
            {
                string url = actions["Back"].GetValue(0).ToString();
                string container = actions["Back"].GetValue(1).ToString();

                JS OnBackClick = new JS("function(){GetViewOnContainer('" + url + "','" + container + "')}");

                control.Items(item => item.Add().Widget(w => w.Button().ID(actions["Prefix"].GetValue(0).ToString() + "Back").Icon("back").OnClick(OnBackClick.Value)).Location(ToolbarItemLocation.Before));
            }

            if (actions.ContainsKey("Title"))
            {
                control.Items(item => item.Add().Text(actions["Title"].GetValue(0).ToString()).Location(ToolbarItemLocation.Before));
            }

            if (actions.ContainsKey("Save"))
            {
                string form = actions["Save"].GetValue(0).ToString();

                JS OnSaveClick = new JS("function(){BtnSave_click('" + form + "')}");

                control.Items(item => item.Add().Widget(w => w.Button().ID(actions["Prefix"].GetValue(0).ToString() + "Save").Icon("save").Text("Guardar").OnClick(OnSaveClick.Value)).Location(ToolbarItemLocation.After));
            }

            if (actions.ContainsKey("Add"))
            {
                string url = actions["Add"].GetValue(0).ToString();
                string container = actions["Add"].GetValue(1).ToString();

                JS OnAddClick = new JS("function(){GetViewOnContainer('" + url + "','" + container + "')}");

                control.Items(item => item.Add().Widget(w => w.Button().ID(actions["Prefix"].GetValue(0).ToString() + "Add").Icon("add").Text("Nuevo").OnClick(OnAddClick.Value)).Location(ToolbarItemLocation.After));
            }

            if (actions.ContainsKey("Remove"))
            {
                string urlDelete = actions["Remove"].GetValue(0).ToString();
                string urlResult = actions["Remove"].GetValue(1).ToString();
                string model = actions["Remove"].GetValue(2).ToString();
                string visible = actions["Remove"].GetValue(3).ToString();
                string container = actions["Remove"].GetValue(4).ToString();

                JS OnRemoveClick = new JS(@"function(){

                                                var result = DevExpress.ui.dialog.confirm('¿Desea eliminar este registro?','Eliminación');
                                                result.done(function(dialogResult) {
                                                    if (dialogResult)
                                                    {
                                                        GetDeleteById('" + urlDelete + @"',{},'" + urlResult + @"','" + container + @"','" + model + @"');
                                                    }
                                                });
                                            }");

                control.Items(item => item.Add().Widget(w => w.Button().ID(actions["Prefix"].GetValue(0).ToString() + "Remove").Icon("remove").Text("Borrar").OnClick(OnRemoveClick.Value).Visible(!visible.Equals("True"))).Location(ToolbarItemLocation.After));
            }

            if (actions.ContainsKey("Repeat"))
            {
                string url = actions["Repeat"].GetValue(0).ToString();
                string visible = actions["Repeat"].GetValue(1).ToString();
                string container = actions["Repeat"].GetValue(2).ToString();

                JS OnRepeatClick = new JS("function(){GetViewOnContainer('" + url + "','" + container + "')}");

                control.Items(item => item.Add().Widget(w => w.Button().ID(actions["Prefix"].GetValue(0).ToString() + "Repeat").Icon("repeat").Text("Duplicar").OnClick(OnRepeatClick.Value).Visible(!visible.Equals("True"))).Location(ToolbarItemLocation.After));

            }

            if (actions.ContainsKey("LinkNavigation"))
            {
                control.OnInitialized("function(){ $('#" + actions["LinkNavigation"].GetValue(0).ToString() + "DetailBread').append(RouteNavigation); }");
            }

            control.ElementAttr("class", "toolbarSection sticky-top");

            return control;

        }

        public ToolbarBuilder Toolbar(ToolbarConfiguration config)
        {
            var toolbar = this.Toolbar();
            toolbar = new ToolbarConfiguration(toolbar).BuilderToWidget(config);

            return toolbar;
        }

        public ToolbarBuilder Toolbar(ToolbarDetailConfiguration config)
        {
            var toolbar = this.Toolbar();
            toolbar = new ToolbarDetailConfiguration(toolbar).BuilderToWidget(config);

            return toolbar;
        }

        //public string ToolbarDetail(ToolbarDetailConfiguration config)
        //{
        //    StringBuilder ToolbarDetail = new ToolbarDetailConfiguration(config.Prefix, config.UrlDelete).ToolbarDetailBuilder(config);
        //    return ToolbarDetail.ToString();
        //}

        #endregion


        #region ImageViewer

        public ImageViewer<TModel> ImageViewer(string Prefix)
        {
            var control = new ImageViewer<TModel>(this, Prefix);
            return control;
        }

        #endregion

        #region Templetes

        private string TemplateControlModal()
        {
            StringBuilder ControlModal = new StringBuilder();

            ControlModal.Append("<!-- The Modal -->" + Environment.NewLine);
            ControlModal.Append("<div class='modal fade' id='IdModal'>" + Environment.NewLine);
            ControlModal.Append("    <div class='modal-dialog'>" + Environment.NewLine);
            ControlModal.Append("        <div class='modal-content'>" + Environment.NewLine);

            ControlModal.Append("            <!-- Modal Header -->" + Environment.NewLine);
            ControlModal.Append("            <div class='modal-header'>" + Environment.NewLine);
            ControlModal.Append("                <h4 class='modal-title text-bold' id='controlTitle'>TitleModal</h4>" + Environment.NewLine);
            ControlModal.Append("                <button type='button' class='close' data-dismiss='modal'>&times;</button>" + Environment.NewLine);
            ControlModal.Append("            </div>" + Environment.NewLine);

            ControlModal.Append("            <!-- Modal body -->" + Environment.NewLine);
            ControlModal.Append("            <div class='modal-body' style='padding: 8px;'>" + Environment.NewLine);
            ControlModal.Append("                <div id='controlContent'>ContentModal</div>" + Environment.NewLine);
            ControlModal.Append("            </div>" + Environment.NewLine);

            ControlModal.Append("            <!-- Modal footer -->" + Environment.NewLine);
            ControlModal.Append("            <!--<div class='modal-footer'>-->" + Environment.NewLine);
            ControlModal.Append("            <!--    <button type='button' class='btn btn-primary' data-dismiss='modal'>Close</button>-->" + Environment.NewLine);
            ControlModal.Append("            <!--</div>-->" + Environment.NewLine);

            ControlModal.Append("        </div>" + Environment.NewLine);
            ControlModal.Append("    </div>" + Environment.NewLine);
            ControlModal.Append("</div>");

            return ControlModal.ToString();
        }

        #endregion


        #endregion
    }

}