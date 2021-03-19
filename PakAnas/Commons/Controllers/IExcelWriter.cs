using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace PakAnas.Commons.Controllers
{
    public interface IExcelWriter
    {
        byte[] Write<T>(IEnumerable<T> dataSource, string sheetName);
        byte[] Write<T>(IEnumerable<T> dataSource, string sheetName, string[] footer);
        byte[] Write(DataTable dataSource, string sheetName);
        byte[] WriteXLSx(DataTable dataSource, string sheetName);
    }
}