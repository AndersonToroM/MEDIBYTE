
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Text;

namespace Dominus.Frontend.Mvc
{
    public class ControlEntidadConfiguration<T> 
    {
        private string IdComponent { get; set; }
        private string ReferenceAttribute { get; set; }
        private SelectBoxBuilder SelectBox { get; set; }

        private string TitleModal { get; set; }
        private string WidthModal { get; set; }


        private DataGridBuilder<T> DataGrid { get; set; }
        private Action<CollectionFactory<DataGridColumnBuilder<T>>> ColumnsGrid { get; set; }


        public ControlEntidadConfiguration(string idComponent, string referenceAttribute)
        {
            this.IdComponent = idComponent;
            this.ReferenceAttribute = referenceAttribute;
        }

        public ControlEntidadConfiguration(SelectBoxBuilder selectBox, DataGridBuilder<T> dataGrid)
        {
            this.SelectBox = selectBox;
            this.DataGrid = dataGrid;
        }


        public ControlEntidadConfiguration<T> Modal(string titleModal, string widthModal)
        {
            this.TitleModal = titleModal;
            this.WidthModal = widthModal;
            return this;
        }

        public ControlEntidadConfiguration<T> Columns(Action<CollectionFactory<DataGridColumnBuilder<T>>> columns)
        {
            ColumnsGrid = columns;
            return this;
        }

        public SelectBoxBuilder BuilderToWidget(ControlEntidadConfiguration<T> config)
        {
            var control = SelectBox;
            control.ID(config.IdComponent);

            if (config.TitleModal != null && config.WidthModal != null)
            {

                string containerId = "controlEntidadContainer";
                string modalId = "controlEntidadModal";


                var grid = GridControlEntidad(config.IdComponent, config.ColumnsGrid);

                JS onRowClickDataGrid = new JS(@"function DataGridModalRowClik(grid){
                                                
                                                var itemsControl = $('#" + config.IdComponent + @"').dxSelectBox('getDataSource').items();
                                                itemsControl.splice(0, itemsControl.length);
                                                itemsControl.unshift(grid.data);

                                                $('#" + config.IdComponent + @"').dxSelectBox('instance').option('value', grid.data['" + config.ReferenceAttribute + @"']);
                                                $('#" + modalId + @" .modal-header button.close').trigger('click');
                                                setTimeout(function(){ $('#" + containerId + @"').remove(); },500);
                                             }");

                grid.OnRowClick(onRowClickDataGrid.Value);


                string contentModal = TemplateControlModal().Replace("IdModal", modalId).Replace("TitleModal", config.TitleModal).Replace("ContentModal", grid.ToString())
                                                            .Replace('\"', '\'').Replace('\"', '\'').Replace("\r\n", "").Replace("\r", "").Replace("\r", "");


                string[] script = contentModal.Split("</script>");
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

                JS openModal = new JS(@"function(data){ 
                                                    $('<div>').attr('id','" + containerId + @"').appendTo('body');
                                                    var closeDisplay = function(){
                                                        $('#" + config.IdComponent + @"').dxSelectBox('instance').option('disabled',true);
                                                        $('.dx-overlay-wrapper').css('display', 'none');
                                                        $('#" + config.IdComponent + @"').dxSelectBox('instance').option('opened',false);
                                                    }; 
                                                    $.when(closeDisplay()).then(function(){
                                                        $('#" + containerId + @"').html(""" + content + @");
                                                        $('#" + modalId + @" .modal-dialog').css({'max-width':'1000px','min-width':'500px','width':" + config.WidthModal + @" + 'px'});
                                                        $('.dx-overlay-wrapper').css('display','block');
                                                        $('#" + modalId + @"').on('hide.bs.modal', function (e) {
                                                            $('#" + config.IdComponent + @"').dxSelectBox('instance').option('disabled',false);
                                                            setTimeout(function(){$('#" + containerId + @"').remove()},500);
                                                        });
                                                        $('#" + modalId + @"').modal('show')});
                                                  }");


                control.ValueExpr(config.ReferenceAttribute)
                    .DataSourceOptions(o => o.Paginate(true).PageSize(10))
                    .OnOpened(openModal.Value)
                    .InputAttr("class", "form-control");
            }


            return control;
        }

        #region Templetes

        private DataGridBuilder<T> GridControlEntidad(string idComponent, Action<CollectionFactory<DataGridColumnBuilder<T>>> columns)
        {
            var control = this.DataGrid;
            control.ID(idComponent + "DataGrid")
                .DataSource(new JS(@"$('#" + idComponent + @"').dxSelectBox('instance').option('dataSource')"))
                .Export(x => x.Enabled(false))
                .Selection(s => s.Mode(SelectionMode.None).ShowCheckBoxesMode(GridSelectionShowCheckBoxesMode.None))
                .Columns(columns)
                .ElementAttr("class", "gridToolbar");
            return control;
        }

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


    }

}