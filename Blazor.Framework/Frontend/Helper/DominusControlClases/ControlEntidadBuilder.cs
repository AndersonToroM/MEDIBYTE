
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc.Factories;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;

namespace Dominus.Frontend.Mvc
{
    public class ControlEntidadBuilder<TModel, TProperty, TExternalModel> : IHtmlContent
    { 
        private string _Prefix { get; set; }
        private string IdComponent { get; set; }
        private string Complement { get; set; }
        public string ModelAttribute { get; set; }

        private string _ValueExpr { get; set; }
        private string ReferenceAttribute_1 { get; set; }
        private string ReferenceAttribute_2 { get; set; }

        public string[] IdComponets { get; set; }
        public string[] refAttr { get; set; }

        private TextBoxBuilder TextBox { get; set; }
        private SelectBoxBuilder _SelectBoxInput { get; set; }
        private ButtonBuilder Button { get; set; }
        private DataGridBuilder<TExternalModel> DataGrid { get; set; }

        private string TitleModal { get; set; }
        private string WidthModal { get; set; }
        
        private Action<CollectionFactory<DataGridColumnBuilder<TExternalModel>>> ColumnsGrid { get; set; }
        private Func<DataSourceFactory, OptionsOwnerBuilder> configurator { get; set; }

        private string onValueChanged{ get; set; }

        private DominusWidgetFactory<TModel> Factory { get; set; }
        private Expression<Func<TModel, TProperty>> expression { get; set; }


        public ControlEntidadBuilder(DominusWidgetFactory<TModel> factory)
        {
            this.Factory = factory;
        }


        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> Expression(Expression<Func<TModel, TProperty>> expression)
        {
            this.expression = expression;
            return this;
        }

        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> Prefix(string value)
        {
            this._Prefix = value;
            return this;
        }
        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> ComplementId(string value)
        {
            this.Complement = value;
            return this;
        }

        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> ValueExpr(string value)
        {
            this._ValueExpr = value;
            return this;
        }

        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> DisplayExpr(string value1, string value2 = null)
        {
            this.ReferenceAttribute_1 = value1;
            this.ReferenceAttribute_2 = value2;
            return this;
        }


        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> Modal(string titleModal, string widthModal = "600")
        {
            this.TitleModal = titleModal;
            this.WidthModal = widthModal;
            return this;
        }

        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> CompositeKey(string[] IdComponets, string[] refAttr) {

            this.IdComponets = IdComponets;
            this.refAttr = refAttr;
            return this;
        }

        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> DataSource(Func<DataSourceFactory, OptionsOwnerBuilder> configurator)
        {
            this.configurator = configurator;
            return this;
        }

        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> Columns(Action<CollectionFactory<DataGridColumnBuilder<TExternalModel>>> columns)
        {
            ColumnsGrid = columns;
            return this;
        }


        public ControlEntidadBuilder<TModel, TProperty, TExternalModel> OnValueChanged(string jsFunc)
        {
            onValueChanged = jsFunc;
            return this;
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            IdComponent = _Prefix;           
            this.TextBox = Factory.TextBox();
            this.Button = Factory.Button();
            this.DataGrid = Factory.DataGridSimple<TExternalModel>();

            if (expression != null)
            {
                ModelAttribute = (expression.Body as MemberExpression).Member.Name;
                this._SelectBoxInput = Factory.SelectBoxFor(expression).Visible(false).Disabled(true);
                IdComponent += ModelAttribute;
            }
            else
            {
                this._SelectBoxInput = Factory.SelectBox().Visible(false).Disabled(true);
            }

            IdComponent += (Complement != null) ? Complement : "";

            HtmlContentBuilder builder = new HtmlContentBuilder();
            bool complete = ReferenceAttribute_2 != null;
            string filterTxt_2 = "";

            string filterTxt_1 = $@"window.SE.ControlEntidad.getFilter('{IdComponent}','txt1','{ReferenceAttribute_1}')";
            string val = "'" + ReferenceAttribute_1 + "',";

            if (complete)
            {
                filterTxt_2 = $@"window.SE.ControlEntidad.getFilter('{IdComponent}','txt2','{ReferenceAttribute_2}')";
                val += "'" + ReferenceAttribute_2 + "'";
            }


            string ButtonClick = $@"function(){{
                                    window.SE.ControlEntidad.onClickBuscar('{IdComponent}'," + (complete ? "true" : "false") + $@");
                                 }}";

            string ButtonReady = $@"function(ev){{
                                    window.SE.ControlEntidad.onButtonReady(ev);
                                 }}";

            string SelectBoxChanged = $@"function(){{
                                                window.SE.ControlEntidad.onChangedSelectBox('{IdComponent}',[{val}]," + (complete ? "true" : "false") + $@");
                                             }}";

            Button.ID($"{IdComponent}_buttonSearch")
                  .Icon("search")
                  .OnClick(ButtonClick)
                  .OnContentReady(ButtonReady);

            TextBox.ShowClearButton(!complete);

            builder.AppendHtml($"<div id='{IdComponent}_container' class='row'>");
            builder.AppendHtml($"<div class='col{(complete ? "-3" : "")}' style='padding: 0;'>");
            builder.AppendHtml(GenerateTextBox($"{IdComponent}_txt1", TextBox, filterTxt_1, DataGrid));
            ColumnsGrid = null;
            builder.AppendHtml($"</div>");

            builder.AppendHtml($"<div class='{(complete ? "col-2 col-md-1" : "col-2")} text-center' style='padding: 0px;height: 25px;'>");
            builder.AppendHtml(Button.ToString());
            builder.AppendHtml($"</div>");

            if (complete)
            {
                builder.AppendHtml($"<div class='col' style='padding: 0;'>");
                builder.AppendHtml(GenerateTextBox($"{IdComponent}_txt2", TextBox, filterTxt_2, DataGrid));
                builder.AppendHtml($"</div>");

                ButtonClick = $@"function(){{
                                        window.SE.ControlEntidad.ClearComponent('{IdComponent}'," + (complete ? "true" : "false") + $@");
                                     }}";

                Button.ID($"{IdComponent}_buttonClear")
                  .Icon("clear")
                  .OnClick(ButtonClick);

                builder.AppendHtml($"<div id='{IdComponent}_buttonClearContainer' class='col-2 col-md-1 text-center' style='padding: 0px;height: 25px;display:none;'>");
                builder.AppendHtml(Button.ToString());
                builder.AppendHtml($"</div>");
            }

            builder.AppendHtml($"</div>");

            _SelectBoxInput.ID($"{IdComponent}_input")
                           .DataSource(configurator)
                           .OnSelectionChanged(SelectBoxChanged);

            if (onValueChanged != null)
                _SelectBoxInput.OnValueChanged(onValueChanged);

            builder.AppendHtml(_SelectBoxInput);

            builder.WriteTo(writer,encoder);
        }
        

        private IHtmlContent GenerateTextBox(string idTxt, TextBoxBuilder txt, string filter, DataGridBuilder<TExternalModel> DataGrid)
        {
            string containerId = "controlEntidadContainer";
            string modalId = "controlEntidadModal";
            bool complete = ReferenceAttribute_2 != null;
            var grid = GridControlEntidad(IdComponent, filter,complete, DataGrid, new List<string[]>(){ IdComponets, refAttr });


            txt.ID(idTxt);
            txt.OnValueChanged($@"function(item){{ 
                                    window.SE.ControlEntidad.onValueChangedTxt(item,'{IdComponent}','{idTxt}',"+ (complete ? "true" : "false") + $@");
                                }}");
            txt.OnInput($@"function(){{ 
                            window.SE.ControlEntidad.onInputTxt('{IdComponent}','{idTxt}',"+ (complete ? "true" : "false") + $@");
                        }}");

            string val = "'"+ ReferenceAttribute_1+"',";

            if (ReferenceAttribute_2 != null)
                val += "'"+ ReferenceAttribute_2 + "'";

            string openModal = $@"function(){{ 
                window.SE.ControlEntidad.OpenModal('{IdComponent}','{modalId}','{containerId}','{_ValueExpr}',[{val}],{(complete?"true":"false")});
            }}";

            grid.OnContentReady(openModal);
            var code = Convert.ToBase64String(Encoding.Unicode.GetBytes(grid.ToString()));

            string TextBoxEnterKey = $@"function(){{
                                         window.SE.ControlEntidad.CreateModal('{IdComponent}','{containerId}','{modalId}','{TitleModal}','{WidthModal}','{code}',{(complete ? "true" : "false")});
                                        }}";

            string TextBoxKeyDown = $@"function(ev){{
                                        window.SE.ControlEntidad.onKeyDownTxt(ev,'{idTxt}');
                                       }}";

            txt.InputAttr("class", "form-control")
                .OnEnterKey(TextBoxEnterKey)
                .OnKeyDown(TextBoxKeyDown);

            HtmlString element = new HtmlString(txt.ToString());
            return element;
        }

        private DataGridBuilder<TExternalModel> GridControlEntidad(string id, string filter,bool complete, DataGridBuilder<TExternalModel> DataGrid, List<string[]> attrs)
        {
            string val = $@"String(grid.data['{ReferenceAttribute_1}']),";

            if (ReferenceAttribute_2 != null)
                val += $@"String(grid.data['{ReferenceAttribute_2}'])";

            string onRowClickDataGrid = $@"function(grid){{
                                               window.SE.ControlEntidad.onRowClickDataGrid(grid,'{id}',grid.data['{_ValueExpr}'],[{val}],{(complete ? "true" : "false")});";
            
            if ( (attrs[0] != null && attrs[1] != null) && attrs[0].Length == attrs[1].Length)
            {
                string[] list = new string[2];
                for (int i = 0; i < attrs[0].Length; i++)
                {
                    list[0] += $"'{_Prefix + attrs[0][i]}',";
                    list[1] += $"grid.data['{attrs[1][i]}'],";
                }
                onRowClickDataGrid += $@"window.SE.ControlEntidad.CompositeKey([{list[0]}],[{list[1]}]);";
            }

            onRowClickDataGrid += "}";

            string OnToolbarPreparing = $@"function(grid){{ 
                                            window.SE.ControlEntidad.OnToolbarPreparing(grid);
                                           }}";

            DataGrid.ID(id + "DataGrid")
                .DataSource(configurator).DataSourceOptions(op => op.Filter(filter))
                .Export(x => x.Enabled(false))
                .Selection(s => s.Mode(SelectionMode.None).ShowCheckBoxesMode(GridSelectionShowCheckBoxesMode.None))
                .Columns(ColumnsGrid)
                .OnRowClick(onRowClickDataGrid)
                .OnToolbarPreparing(OnToolbarPreparing)
                .SearchPanel(sp => sp.Visible(true).Placeholder("Busqueda avanzada"))
                .HeaderFilter(headerFilter => headerFilter.Visible(false))
                .FilterRow(p => p.Visible(true))
                .ElementAttr("class", "gridToolbar");
            return DataGrid;
        }

    }

}