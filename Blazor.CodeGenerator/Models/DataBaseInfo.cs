using CodeGenerator.Models;
using System.Collections.Generic;

public class DataBaseMapModel
{
    public List<TableModel> Tables { get; set; } = new List<TableModel>();
    public List<ColumnModel> Columns { get; set; } = new List<ColumnModel>();
    public List<InReferencesModel> InReferences { get; set; } = new List<InReferencesModel>();
    public List<OutReferencesModel> OutReferences { get; set; } = new List<OutReferencesModel>();
    public List<IndexModel> Indexes { get; set; } = new List<IndexModel>();
    public int NumberConnection { get; set; }
}