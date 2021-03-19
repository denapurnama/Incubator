using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace PakAnas.Commons.Helpers
{
    public static class Util
    {
        public static string getPathValue = "";
        public static DateTime lastGetPath = new DateTime(1900, 1, 1, 0, 0, 0);
        public static DateTime lastSay = DateTime.Now.Date;

        public static string CleanFilename(string f)
        {
            string unwise = "{}|\\^[]`";
            string reserved = ";/?:@&=+$,";
            string delims = "<>#%\"";
            string na = unwise + reserved + delims;
            char[] NA = na.ToCharArray();
            string x = f;
            if (!string.IsNullOrEmpty(x))
            {
                string fn = "";
                for (int i = 0; i < x.Length; i++)
                {
                    char xx = x.Substring(i, 1).ToCharArray()[0];
                    if (!NA.Contains(xx))
                        fn = fn + x.Substring(i, 1);
                }
                x = fn;
                if (!string.IsNullOrEmpty(x))
                    x = x.Trim();
            }

            return x;
        }

        readonly static string DirtyFileNamePattern = "[\\+/\\\\\\#%&*{}/:<>?|\"-]";

        public static string SanitizeFilename(string insane)
        {
            //string pattern = "[\\+/\\\\\\~#%&*{}/:<>?|\"-]";
            string replacement = "_";

            Regex regEx = new Regex(DirtyFileNamePattern);
            return Regex.Replace(regEx.Replace(insane, replacement), @"\s+", " ");
        }

        public static bool isDirty(string s)
        {
            return Regex.IsMatch(s, DirtyFileNamePattern);
        }

        public static void ReMoveFile(string fn, string code = null)
        {
            if (System.IO.File.Exists(fn))
            {
                int dotpos = fn.LastIndexOf(".");
                if (dotpos < 1) dotpos = fn.Length;
                if (string.IsNullOrEmpty(code))
                    code = DateTime.Now.ToString("yyyy-MM-dd");

                string fn1 =
                        Path.Combine(Path.GetDirectoryName(fn),
                            code + "_" +
                            Path.GetFileNameWithoutExtension(fn)
                            + "_" + DateTime.Now.ToString("HHmmss")
                            + Path.GetExtension(fn));
                System.IO.File.Move(fn, fn1);
            }
        }


 


        public static string SaveUpload(string operation, HttpPostedFileBase file, string p, string saveFileName)
        {
            int code = 1;
            string m = "";
            try
            {
                if (file != null)
                {
                    string fx = file.FileName;

                    if (!Directory.Exists(p))
                        Directory.CreateDirectory(p);
                    string fn = Path.Combine(p, saveFileName + Path.GetExtension(fx));
                    Util.ReMoveFile(fn);
                    file.SaveAs(fn);
                }
            }
            catch (Exception e)
            {
                m = e.Message;
                code = 0;
            }
            string r = operation + " " + ((code > 0) ? "Berhasil" : (" Gagal." + m));
            return r;
        }

        public static bool ForceDirectories(string d)
        {
            bool r = false;
            try
            {
                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }
                r = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return r;
        }


        public static string tick(string s)
        {
            return DateTime.Now.ToString("HH:mm:ss") + "\t" + s;
        }

        public static string GetPath(string workingDir = null)
        {
            DateTime n = DateTime.Now;
            if (((n.Date - lastGetPath.Date).TotalDays < 1) && !string.IsNullOrEmpty(getPathValue) && string.IsNullOrEmpty(workingDir))
                return getPathValue;

            Assembly ass = Assembly.GetEntryAssembly();
            if (ass == null)
                ass = Assembly.GetCallingAssembly();
            if (ass == null)
                ass = Assembly.GetExecutingAssembly();
            if (ass == null)
                return null;
            string assLocation = ass.Location;
            FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(assLocation);
            Debug.WriteLine("log: " + assLocation);

            string company = fv.CompanyName;
            string app = fv.ProductName;

            string pathe = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            if (!string.IsNullOrEmpty(company)) pathe = Path.Combine(pathe, company);
            if (!string.IsNullOrEmpty(app)) pathe = Path.Combine(pathe, app);
            if (!string.IsNullOrEmpty(workingDir))
                pathe = Path.Combine(pathe, workingDir);
            else
                pathe = Path.Combine(pathe, "log");
            pathe = Path.Combine(pathe, n.Year.ToString(), n.Month.ToString(), n.Day.ToString());

            if (string.IsNullOrEmpty(workingDir))
            {
                getPathValue = pathe;
                lastGetPath = n;
                if (!Directory.Exists(pathe))
                {
                    Util.ForceDirectories(pathe);
                }
            }

            return pathe;

        }

        public static int Say(string loc, string w, params object[] x)
        {
            string path = GetPath();
            string logExt = ".txt";
            string dateFmt = "HH:mm:ss";
            string logfile = Path.Combine(path, ((string.IsNullOrEmpty(loc)) ? "0" + logExt : Util.SanitizeFilename(loc) + logExt));
            string ts = "        ";
            if (Math.Abs(Math.Round((DateTime.Now - lastSay).TotalSeconds)) >= 1.0)
            {
                ts = DateTime.Now.ToString(dateFmt);
                lastSay = DateTime.Now;
            }

            try
            {
                File.AppendAllText(logfile, Environment.NewLine + ts + " "
                    + ((x != null) ? String.Format(w.Replace("|", Environment.NewLine + "\t"), x) : w));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return -2;
            }
            return 0;
        }

        public static void TraceCall(string filename, string info)
        {
            if (info.Contains("{"))
            {
                Util.Say(filename, info, Util.SourceLocation(new System.Diagnostics.StackTrace(1)));
            }
            else
            {
                Util.Say(filename, info, null);
            }
        }

        public static string SourceLocation(this StackTrace s)
        {
            if (s == null) return null;
            StackFrame F = s.GetFrame(1);
            if (F == null) return null;
            MethodBase M = F.GetMethod();
            if (M == null) return null;

            int lin = F.GetFileLineNumber();
            int col = F.GetFileColumnNumber();

            return string.Format("{0}.{1}"
                                  + ((lin > 0)
                                      ? " @({2}, {3})"
                                      : "")
                                  , M.ReflectedType.Name, M.Name
                                  , lin, col);
        }

    }

}