using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace BingWallPaperDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var logFileName = "BingWallPaperDownloaderLog.txt";
            var baseDirectory = "D:\\BingWallPapers\\";
            FileInfo loggerFi = new FileInfo(Path.Combine(baseDirectory, logFileName));
            StringBuilder logText = new StringBuilder();

            logText.AppendLine(new string('+',80));

            logText.AppendLine(String.Concat(DateTime.Now.ToShortDateString(), " ",
                DateTime.Now.ToShortTimeString(), " ", 
                "Wallpaper downloader started"));

            try
            {
                var url = "http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US";

                // Read in the Xml
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(url);

                //Find the file name
                var innerValue = xmlDocument.DocumentElement.SelectSingleNode("/images/image/url").InnerText;

                var newUrl = string.Concat("https://bing.com", innerValue);
                Uri myUri = new Uri(newUrl);
                string fileName = HttpUtility.ParseQueryString(myUri.Query).Get("id");
                string path = String.Concat("D:\\BingWallPapers\\", fileName);
                FileInfo fi = new FileInfo(path);

                logText.AppendLine(String.Concat(DateTime.Now.ToShortDateString(), " ",
    DateTime.Now.ToShortTimeString(), " ",
    "Download link: " + myUri.ToString()));

                logText.AppendLine(String.Concat(DateTime.Now.ToShortDateString(), " ",
    DateTime.Now.ToShortTimeString(), " ",
    "Save to: " + fi.FullName.ToString()));

                innerValue = xmlDocument.DocumentElement.SelectSingleNode("/images/image/copyright").InnerText;

                logText.AppendLine(String.Concat(DateTime.Now.ToShortDateString(), " ",
    DateTime.Now.ToShortTimeString(), " ",
    "Copyright info: " + innerValue.ToString()));

                if (!File.Exists(fi.FullName))
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(new Uri(myUri.ToString()), fi.FullName);

                        logText.AppendLine(String.Concat(DateTime.Now.ToShortDateString(), " ",
DateTime.Now.ToShortTimeString(), " ",
"INFO: File successfully downloaded"));
                    }
                }
                else
                {
                    logText.AppendLine(String.Concat(DateTime.Now.ToShortDateString(), " ",
DateTime.Now.ToShortTimeString(), " ",
"WARNING: File already exists locally so file is NOT downloaded again"));
                }
                
            }
            catch (Exception ex)
            {
                logText.AppendLine(String.Concat(DateTime.Now.ToShortDateString(), " ",
                DateTime.Now.ToShortTimeString(), " ",
                "ERROR: Could not download image of the day. Message: " + ex.Message));
            }
            logText.AppendLine(String.Concat(DateTime.Now.ToShortDateString(), " ",
                DateTime.Now.ToShortTimeString(), " ",
                "Wallpaper downloader finished"));
            logText.AppendLine(new string('+', 80));
            try
            {
                // Write the string array to a new file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(loggerFi.FullName, append: true))
                {
                        outputFile.Write(logText.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Critical error writing to log! " + ex.Message);
            }

            Console.Write(logText.ToString());
        }
    }
}
