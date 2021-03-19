using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Toyota.Common.Database;
using Toyota.Common.Web.Platform;
using Toyota.Common.Utilities;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace PakAnas.Models
{
    public class SimpleMasterCustomModel
    {
        public string TABLE_SCHEMA { get; set; }
        public string TABLE_CATALOG { get; set; }
        public string TABLE_NAME { get; set; }
        public string TABLE_TYPE { get; set; }
        public string COLUMN_NAME { get; set; }
        public string ORDINAL_POSITION { get; set; }
        public string IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
        public string CHARACTER_MAXIMUM_LENGTH { get; set; }
        public string CONSTRAINT_NAME { get; set; }
        public int cnt { get; set; }
        public int cntColumn { get; set; }
        public string RECORD_COLUMN { get; set; }
        public string MODE { get; set; }
        public string RECORD_EDIT { get; set; }
        public string VALUE { get; set; }
        public string DESCRIPTION { get; set; }
        public string PK { get; set; }

        public string tampung;

        public List<List<string>> getDataGridTables(string sc_TABLE_NAME, string sc_DEFINITION, string sc_TABLE_COLUMN,
            string sc_OPERATION, string sc_CONDITION, string sh_DEFINITION, string sh_TABLE_COLUMN, string sh_OPERATION, string sh_CONDITION, int START, int END)
        {
            
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic test = new List<ExpandoObject>();

            var result = db.Query<dynamic>("Master/SimpleMasterCustom_getDataGrid", new
            {
                TABLE_NAME = sc_TABLE_NAME,
                sc_DEFINITION = sc_DEFINITION,
                sc_TABLE_COLUMN = sc_TABLE_COLUMN,
                sc_OPERATION = sc_OPERATION,
                sc_CONDITION = sc_CONDITION,
                sh_DEFINITION = sh_DEFINITION,
                sh_TABLE_COLUMN = sh_TABLE_COLUMN,
                sh_OPERATION = sh_OPERATION,
                sh_CONDITION = sh_CONDITION,
                START = START,
                END = END

            });
            IDictionary<string, object> propMap;
            List<List<string>> value = new List<List<string>>();
            List<string> field = new List<string>();
            
            foreach (object m in result)
            {
                
                if (m.GetType() == typeof(ExpandoObject))
                {
                    propMap = (IDictionary<string, object>)m;
                }
                else
                {
                    propMap = m.GetPropertyMap();
                }
                
                if (field.Count < 1)//list field
                {
                    //field.Clear();
                    foreach (string propName in propMap.Keys)
                    {
                        field.Add(propName);
                    }
                    value.Add(field);
                    
                }

                List<string> list = new List<string>();
                foreach (var propVal in propMap.Values) // list value
                {
                    if (propVal != null)
                    {
                        list.Add(propVal.ToString());
                    }
                    else
                    {
                        list.Add("");
                    }
                }
                value.Add(list);
                
            }

            
            db.Close();
            return value;
        }

        public List<List<string>> downloadDataGridTables(string sc_TABLE_NAME, string sc_DEFINITION, string sc_TABLE_COLUMN,
            string sc_OPERATION, string sc_CONDITION, string sh_DEFINITION, string sh_TABLE_COLUMN, string sh_OPERATION, string sh_CONDITION)
        {

            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic test = new List<ExpandoObject>();

            var result = db.Query<dynamic>("Master/SimpleMasterCustom_DownloadData", new
            {
                TABLE_NAME = sc_TABLE_NAME,
                sc_DEFINITION = sc_DEFINITION,
                sc_TABLE_COLUMN = sc_TABLE_COLUMN,
                sc_OPERATION = sc_OPERATION,
                sc_CONDITION = sc_CONDITION,
                sh_DEFINITION = sh_DEFINITION,
                sh_TABLE_COLUMN = sh_TABLE_COLUMN,
                sh_OPERATION = sh_OPERATION,
                sh_CONDITION = sh_CONDITION

            });
            IDictionary<string, object> propMap;
            List<List<string>> value = new List<List<string>>();
            List<string> field = new List<string>();

            foreach (object m in result)
            {

                if (m.GetType() == typeof(ExpandoObject))
                {
                    propMap = (IDictionary<string, object>)m;
                }
                else
                {
                    propMap = m.GetPropertyMap();
                }

                if (field.Count < 1)//list field
                {
                    //field.Clear();
                    foreach (string propName in propMap.Keys)
                    {
                        field.Add(propName);
                    }
                    value.Add(field);

                }

                List<string> list = new List<string>();
                foreach (var propVal in propMap.Values) // list value
                {
                    if (propVal != null)
                    {
                        list.Add(propVal.ToString());
                    }
                    else
                    {
                        list.Add("");
                    }
                }
                value.Add(list);

            }


            db.Close();
            return value;
        }

        public List<SimpleMasterCustomModel> data_Table(string sc_TABLE_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_dataTablesName", new {
                TABLE_NAME = sc_TABLE_NAME
            });
            db.Close();
            return res.ToList();
        }

        public List<SimpleMasterCustomModel> getTables()
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_getTablesName");
            db.Close();
            return res.ToList();
        }

        public List<SimpleMasterCustomModel> getDescription(string sc_TABLE_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_getDescription",
                new
                {
                    TABLE_NAME = sc_TABLE_NAME
                });
            db.Close();
            return res.ToList();
        }

        public List<SimpleMasterCustomModel> getColumn(string sc_TABLE_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_getColumnName",
                new
                {
                    TABLE_NAME = sc_TABLE_NAME
                });
            db.Close();
            return res.ToList();
        }

        public int getColumnCount(string sc_TABLE_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_getColumnNameCount",
                new
                {
                    TABLE_NAME = sc_TABLE_NAME
                });
            db.Close();
            return res.ToList().First().cntColumn;
        }

        public List<SimpleMasterCustomModel> getPK(string sc_TABLE_NAME)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_GetPK",
                new
                {
                    TABLE_NAME = sc_TABLE_NAME
                });
            db.Close();
            return res.ToList();
        }

        public List<SimpleMasterCustomModel> getDataGrid(string sc_TABLE_NAME, string sc_DEFINITION, string sc_TABLE_COLUMN,
            string sc_OPERATION, string sc_CONDITION, string sh_DEFINITION, string sh_TABLE_COLUMN, string sh_OPERATION, string sh_CONDITION)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_getDataGrid",
                new
                {
                    TABLE_NAME = sc_TABLE_NAME,
                    sc_DEFINITION = sc_DEFINITION,
                    sc_TABLE_COLUMN = sc_TABLE_COLUMN,
                    sc_OPERATION = sc_OPERATION,
                    sc_CONDITION = sc_CONDITION,
                    sh_DEFINITION = sh_DEFINITION,
                    sh_TABLE_COLUMN = sh_TABLE_COLUMN,
                    sh_OPERATION = sh_OPERATION,
                    sh_CONDITION = sh_CONDITION

                });
            db.Close();
            return res.ToList();
        }

        public int getDataGridCount(string sc_TABLE_NAME, string sc_DEFINITION, string sc_TABLE_COLUMN,
            string sc_OPERATION, string sc_CONDITION, string sh_DEFINITION, string sh_TABLE_COLUMN, string sh_OPERATION, string sh_CONDITION)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_getDataGridCount",
                new
                {
                    TABLE_NAME = sc_TABLE_NAME,
                    sc_DEFINITION = sc_DEFINITION,
                    sc_TABLE_COLUMN = sc_TABLE_COLUMN,
                    sc_OPERATION = sc_OPERATION,
                    sc_CONDITION = sc_CONDITION,
                    sh_DEFINITION = sh_DEFINITION,
                    sh_TABLE_COLUMN = sh_TABLE_COLUMN,
                    sh_OPERATION = sh_OPERATION,
                    sh_CONDITION = sh_CONDITION
                });
            db.Close();
            return res.ToList().First().cnt;
        }

        public List<SimpleMasterCustomModel> geValueEdit(string PK, string table, string RECORD)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var res = db.Query<SimpleMasterCustomModel>("Master/SimpleMasterCustom_GetRecordPK",
                new
                {
                    PK = PK,
                    RECORD = RECORD,
                    TABLE_NAME = table
                });
            db.Close();
            return res.ToList();
        }

        public int InsertData(string xxx, string table)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var tambah = xxx.Replace("+", " ");
            var ubah = tambah.Replace("%3A", ":");
            var split = ubah.Split('&');
            var tampung = "";
            string[] split_tampungnya;
            var field = "";
            var value = "";
            int res = 0;

            db.BeginTransaction();
            try
            {
                for (int i = 0; i < split.Length; i++)
                {
                    tampung = split[i];
                    split_tampungnya = tampung.Split('=');
                    if (field == "")
                    {
                        field = split_tampungnya[0].ToString();
                        value = "'" + split_tampungnya[1].ToString() + "'";
                    }
                    else
                    {
                        field = field + "," + split_tampungnya[0].ToString();
                        value = value + "," + "'" + split_tampungnya[1].ToString() + "'";
                    }
                }

                res = db.Execute("Master/SimpleMasterCustom_InsertData",
                    new
                    {
                        P_FIELD = field,
                        P_VALUE = value,
                        P_TABLE = table
                    });
                db.Close();

                db.CommitTransaction();
            }
            catch (Exception E)
            {
                db.AbortTransaction();
            }
            return res;
        }

        public int UpdateData(string record, string table, string PK)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            var tambah = record.Replace("+", " ");
            var ubah = tambah.Replace("%3A", ":");
            var split = ubah.Split('&');
            var tampung = "";
            string[] split_tampungnya;
            var field = "";
            var value = "";
            int res = 0;

            db.BeginTransaction();
            try
            {
                for (int i = 0; i < split.Length; i++)
                {
                    tampung = split[i];
                    split_tampungnya = tampung.Split('=');
                    if (field == "")
                    {
                        field = split_tampungnya[0].ToString();
                        value = split_tampungnya[1].ToString();
                    }
                    else
                    {
                        field = field + "," + split_tampungnya[0].ToString();
                        value = value + "," + split_tampungnya[1].ToString();
                    }
                }

                res = db.Execute("Master/SimpleMasterCustom_UpdateData",
                    new
                    {
                        P_FIELD = field,
                        P_VALUE = value,
                        P_TABLE = table,
                        PK = PK,
                    });
                db.Close();

                db.CommitTransaction();
            }
            catch (Exception E)
            {
                db.AbortTransaction();
            }
            return res;
        }

        public int DeleteData(string PK, string table)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();
            int res = 0;

            db.BeginTransaction();
            try
            {
                res = db.Execute("Master/SimpleMasterCustom_DeleteData",
                new
                {
                    PK = PK,
                    TABLE_NAME = table
                });

                db.Close();
                db.CommitTransaction();
            }
            catch (Exception E)
            {
                db.AbortTransaction();
            }
            return res;
        }

        #region upload data
        public string ValidationThenInsert(string path, string user)
        {
            HSSFWorkbook hssfwb;
            string result = "";
            IDBContext db = DatabaseManager.Instance.GetContext();

            FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            hssfwb = new HSSFWorkbook(file);
            ISheet sheet = hssfwb.GetSheet("Application");

            string ID;
            string NAME;
            string TYPE;
            string RUNTIME;
            string DESCRIPTION;

            bool chk = false;

            //db.BeginTransaction();
            try
            {
                file.Close();
                if (chk != true)
                {
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        sheet.GetRow(row).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        sheet.GetRow(row).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        sheet.GetRow(row).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        sheet.GetRow(row).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        sheet.GetRow(row).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK);

                        sheet.GetRow(row).GetCell(0).SetCellType(CellType.STRING);
                        sheet.GetRow(row).GetCell(1).SetCellType(CellType.STRING);
                        sheet.GetRow(row).GetCell(2).SetCellType(CellType.STRING);
                        sheet.GetRow(row).GetCell(3).SetCellType(CellType.STRING);
                        sheet.GetRow(row).GetCell(4).SetCellType(CellType.STRING);

                        NAME = sheet.GetRow(row).GetCell(0).StringCellValue.ToString().Trim();
                        ID = sheet.GetRow(row).GetCell(1).StringCellValue.ToString().Trim();
                        DESCRIPTION = sheet.GetRow(row).GetCell(2).StringCellValue.ToString().Trim();
                        TYPE = sheet.GetRow(row).GetCell(3).StringCellValue.ToString().Trim();
                        RUNTIME = sheet.GetRow(row).GetCell(4).StringCellValue.ToString().Trim();

                        int affectedRow = db.Execute("application/saveApplication", new
                        {
                            AppId = ID,
                            AppName = NAME,
                            AppType = TYPE,
                            AppRuntime = RUNTIME,
                            AppDesc = DESCRIPTION,
                            AppActor = user
                        });

                    }//End
                }
            }
            catch (Exception err)
            {
                result = "error|catch:" + Convert.ToString(err.Message) + ":\n" + path;
            }

            db.Close();
            return result;
        }
        #endregion

        public string get_description(string table_name)
        {
            IDBContext db = DatabaseManager.Instance.GetContext();

            dynamic test = new List<ExpandoObject>();

            var result = db.Fetch<SimpleMasterCustomModel>("Master/SimpleMasterCustom_getDescription", new
            {
                TABLE_NAME = table_name
            });
            return result.First().DESCRIPTION;
        }
    }
}