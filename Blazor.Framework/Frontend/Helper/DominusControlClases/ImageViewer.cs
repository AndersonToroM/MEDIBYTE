
using DevExtreme.AspNet.Mvc.Builders;
using DevExtreme.AspNet.Mvc.Factories;
using DevExtreme.AspNet.Mvc.Internals;
using Dominus.Backend.Application;
using Microsoft.AspNetCore.Html;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace Dominus.Frontend.Mvc
{
    public class ImageViewer<TModel> : IHtmlContent
    {

        private DominusWidgetFactory<TModel> Factory { get; set; }
        private Dictionary<int, string> Base64Files { get; set; }
        private string Prefix { get; set; }
        private string[] ImageFormats { get; set; }
        private string TextBoxName { get; set; }

        private string DefaultImg { get; set; }
        private int index { get; set; }

        public ImageViewer(DominusWidgetFactory<TModel> factory, string Prefix)
        {
            this.Factory = factory;
            this.Prefix = Prefix;
        }

        public ImageViewer<TModel> Data(Dictionary<int, string> data, int index = 0) {
            this.Base64Files = data;
            this.index = index;
            return this;
        }
        public ImageViewer<TModel> Formats(params string[] values) {
            this.ImageFormats = values;
            return this;
        }
        public ImageViewer<TModel> Name(string value) {
            this.TextBoxName = value;
            return this;
        }
        public ImageViewer<TModel> DefaultImage(string path) {
            this.DefaultImg = path;
            return this;
        }

        public new void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {

            HtmlContentBuilder builder = new HtmlContentBuilder();

            builder.AppendHtml($"<div class='card image-viewer-control'>");
            builder.AppendHtml($"   <div class='card-header'>");
            builder.AppendHtml($"       <div class='row justify-content-end'>");
            builder.AppendHtml($"           <button onclick='SE.ImageViewer.imageDelete(\"{Prefix}\", \"{DefaultImg}\")' type='button' class='close btn btn-danger bnt-circle'> ");
            builder.AppendHtml($"               <p style='font-size: 1.25rem;color: white;'> &times; </p>");
            builder.AppendHtml($"           </button>");
            builder.AppendHtml($"       </div>");
            builder.AppendHtml($"       <div class='row justify-content-center align-items-center' style='height: 8rem;margin-left: -15px;margin-right: -15px;'>");
            builder.AppendHtml($"            <div onclick='SE.ImageViewer.imageClick(\"{Prefix}\")' style='cursor: pointer;'>");
            builder.AppendHtml($"                <img id='{Prefix}ImageViewerImg' src='{DApp.GetImageSource(GetValue(), DefaultImg)}' class='img-fluid rounded' alt='No Photo'>");
            builder.AppendHtml($"            </div>");
            builder.AppendHtml($"        </div>");
            builder.AppendHtml($"    </div>");
            builder.AppendHtml($"    <div class='card-body' style='padding: 5px;'>");
            builder.AppendHtml($"    {GenerateFileUploader()}");
            builder.AppendHtml($"    </div>");
            //builder.AppendHtml($"    <input id='{Prefix}ImageViewerInput' name='{TextBoxName}'>");
            //builder.AppendHtml($"    <script></script>");
            builder.AppendHtml($"</div>");


            builder.WriteTo(writer, encoder);
        }


        private FileUploaderBuilder GenerateFileUploader()
        {

            var fileUploader = Factory.FileUploader().ID($"{Prefix}ImageViewer")
                .Visible(true).ShowFileList(false)
                .OnUploadError($"function(item){{console.log(item)}}")
                .UploadFile($"function(item){{ SE.ImageViewer.imageChanged(item,'{Prefix}'); }}")
                .Multiple(false).OnContentReady($"function(item){{ SE.ImageViewer.imageInitialized(item, '{Prefix}' , '{TextBoxName}', '{GetValue()}') }}")
                .MaxFileSize(5242880);


            if (ImageFormats != null && ImageFormats.Length > 0)
            {
                fileUploader.Accept(string.Join(",", ImageFormats));
                fileUploader.AllowedFileExtensions(ImageFormats);
            }
            else
            {
                fileUploader.Accept(".jpg,.png,.jpeg");
                fileUploader.AllowedFileExtensions(new[] { ".jpg", ".png", ".jpeg" });
            }

            return fileUploader;
        }

        private string GetValue() {
            return (Base64Files != null && Base64Files.ContainsKey(index)) ? Base64Files[index] : "";
        }

      
    }

}