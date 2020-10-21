using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SermonPodcastPages
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            const string bootstrapIncludes = @"
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">
<script src=""https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js""></script>
<script src=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js""></script>
                ";

            InitializeComponent();

            string localDriveLocation = Properties.Settings.Default.LocalDriveLocation;     // Where we are writing the output
            string spreadsheet = Properties.Settings.Default.SpreadsheetLocation;
            localDriveLocation = @"C:\Users\graha\Documents\Sermon podcasts\play";
            FileStream spreadsheetStream = null;
            try
            {
                spreadsheetStream = File.Open( spreadsheet, FileMode.Open, FileAccess.Read );
            }
            catch
            {
                iTextBoxStatus.Text = "Unable to open " + spreadsheet;
                return;
            }

            // Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader( spreadsheetStream );

            // DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            foreach ( DataTable sheet in result.Tables )
            {
                iTextBoxStatus.Text += ( sheet.TableName + Environment.NewLine );
                string html = Path.Combine( localDriveLocation, sheet.TableName + ".html" );
                using ( TextWriter htmlWriter = File.CreateText( html ) )
                {
                    htmlWriter.WriteLine( "<!DOCTYPE html>" );
                    htmlWriter.WriteLine( "<HTML>" );

                    htmlWriter.WriteLine( "<HEAD>" );
                    htmlWriter.WriteLine( "<TITLE>Bethel Podcasts {0}</TITLE>", sheet.TableName );
                    htmlWriter.WriteLine(bootstrapIncludes);

                    //AddOriginalStyle(htmlWriter);
                    //AddColourfulStyle(htmlWriter);

                    htmlWriter.WriteLine( "</HEAD>" );

                    htmlWriter.WriteLine("<BODY>");
                    string year = sheet.TableName;
                    WriteYearIndex(htmlWriter, year, result.Tables);
                    htmlWriter.WriteLine(@"<div class=""container"">");
                    htmlWriter.WriteLine("<BR><BR>");
                    htmlWriter.WriteLine( "<H1>Bethel Macclesfield sermons {0}</H1>", year );

                    htmlWriter.WriteLine( "<BR>" );
                    //htmlWriter.WriteLine( @"<TABLE style=""width:800px"">" );

                    foreach ( DataRow row in sheet.Rows )
                    {
                        WriteSermon( htmlWriter, sheet.TableName, row );
                    }

                    //htmlWriter.WriteLine( "</TABLE>" );
                    htmlWriter.WriteLine( @"<A id=""latest""> </A>" );
                    htmlWriter.WriteLine(@"</div>");
                    htmlWriter.WriteLine( "</BODY>" );
                    htmlWriter.WriteLine( "</HTML>" );
                }
            }

            iTextBoxStatus.Text += "Done";
        }

        /// <summary>
        /// CSS styling for a simple HTML table
        /// </summary>
        static void AddOriginalStyle(TextWriter aWriter)
        {
            aWriter.WriteLine("<STYLE>");
            aWriter.WriteLine("P.SERIF{font-family:\"Times New Roman\",Times,serif;}");
            aWriter.WriteLine("P.SANSSERIF{font-family:Tahoma,Helvetica,sans-serif;}");
            aWriter.WriteLine("TABLE,TH,TD");
            aWriter.WriteLine("{");
            aWriter.WriteLine("    border:1px solid black;");
            aWriter.WriteLine("}");
            aWriter.WriteLine("TD");
            aWriter.WriteLine("{");
            aWriter.WriteLine("padding:15px;");
            aWriter.WriteLine("}");
            aWriter.WriteLine("</STYLE>");
        }

        /// <summary>
        /// CSS styling for a colourful HTML table
        /// </summary>
        static void AddColourfulStyle(TextWriter aWriter)
        {
            aWriter.WriteLine("<STYLE>");
            //aWriter.WriteLine("P.SERIF{font-family:\"Times New Roman\",Times,serif;}");
            //aWriter.WriteLine("P.SANSSERIF{font-family:Tahoma,Helvetica,sans-serif;}");

            aWriter.WriteLine("table {");
            aWriter.WriteLine("       border-collapse: collapse;");
            aWriter.WriteLine("       width: 100 %;");
            aWriter.WriteLine("}");

            aWriter.WriteLine("th, td {");
            aWriter.WriteLine("       text-align: left;");
            aWriter.WriteLine("       padding: 8px;");
            aWriter.WriteLine("       border-bottom: 1px solid #ddd;");
            aWriter.WriteLine("}");

            aWriter.WriteLine("tr:nth-child(even){background-color: #f2f2f2}");
            aWriter.WriteLine("tr:hover {background-color: #fff5f5}");

            aWriter.WriteLine("th {");
            aWriter.WriteLine("       background-color: #b2f2b2;");
            aWriter.WriteLine("       color: green;");
            aWriter.WriteLine("}");
            aWriter.WriteLine("</STYLE>");
        }

        /// <summary>
        /// If we are writing this year's sheet, then show a link to the latest sermon at the bottom
        /// of the page
        /// </summary>
        static void WriteLinkToLatest( TextWriter aWriter, DataTable aSheet )
        {
            if ( aSheet.TableName == DateTime.Now.Year.ToString() )
            {
                aWriter.WriteLine( @"<BR>" );
                aWriter.WriteLine( @"<DIV style=""background-color:#FFA500;clear:both;text-align:center;width:800px;"">" +
                                      @"<A href=""#latest"">Go to latest sermon</A></DIV>" );
            }
        }

        void WriteYearIndex( TextWriter aWriter, string aThisYear, DataTableCollection aYears  )
        {
            const string navbarBegin = @"
<nav class=""navbar navbar-inverse navbar-fixed-top"">
   <div class=""container-fluid"">
    <div class=""navbar-header"">
      <button type=""button"" class=""navbar-toggle"" data-toggle=""collapse"" data-target=""#myNavbar"">
        <span class=""icon-bar""></span>
        <span class=""icon-bar""></span>
        <span class=""icon-bar""></span>
      </button>
      <a class=""navbar-brand"" href=""http://www.bethelmacclesfield.org.uk"">Bethel Macclesfield</a>
    </div>
    <div class=""collapse navbar-collapse"" id=""myNavbar"">
      <ul class=""nav navbar-nav"">
                ";

            const string navbarEnd = @"
      </ul>
      <ul class=""nav navbar-nav navbar-right"">
        <li><a href=""{0}.html#latest"">Latest sermon</a></li>
      </ul>
    </div>
  </div>
</nav>
                ";

            aWriter.WriteLine(navbarBegin);

            foreach ( DataTable year in aYears )
            {
                if ( year.TableName == aThisYear )
                {
                    aWriter.WriteLine(@"<li class=""active""><a href=""{0}.html"">{0}</a></li>", year.TableName);
                }
                else
                {
                    aWriter.WriteLine(@"<li><a href=""{0}.html"">{0}</a></li>", year.TableName );
                }
            }

            var latestYear = DateTime.Now.Year.ToString();
            aWriter.WriteLine(navbarEnd, latestYear);
        }

        /// <summary>
        /// Write details for a single sermon (single row in the spreadsheet/DataTable)
        /// </summary>
        //void WriteSermon(TextWriter aWriter, string aYear, DataRow aSermonDetails)
        //{
        //    // If we fail to parse the date, we assume that the row is empty and write no details at all
        //    DateTime sermonDate;
        //    if (DateTime.TryParse(aSermonDetails.ItemArray[kDateColumn].ToString(), out sermonDate))
        //    {
        //        aWriter.WriteLine("<TR>");

        //        aWriter.WriteLine(@"<TD style=""width:100px"">");
        //        aWriter.WriteLine(sermonDate.ToString("d MMM yyyy"));
        //        aWriter.WriteLine("</TD>");

        //        aWriter.WriteLine("<TD>");
        //        aWriter.WriteLine("<P class=\"SANSSERIF\">");
        //        aWriter.WriteLine("<B>" + aSermonDetails.ItemArray[kTitleColumn].ToString() + "</B>");
        //        string series = aSermonDetails.ItemArray[kSeriesColumn].ToString();
        //        if (series.Length > 0)
        //        {
        //            aWriter.WriteLine(" (" + series + ")");
        //        }
        //        aWriter.WriteLine("</P>");
        //        string scripture = aSermonDetails.ItemArray[kScriptureColumn].ToString();
        //        if (scripture.Length > 0)
        //        {
        //            scripture += ", ";
        //        }
        //        aWriter.WriteLine(scripture + aSermonDetails.ItemArray[kSpeakerColumn].ToString());
        //        aWriter.WriteLine("<BR>");
        //        aWriter.WriteLine("<I>" + aSermonDetails.ItemArray[kDescriptionColumn].ToString() + "</I>");
        //        aWriter.WriteLine("<P class=\"SANSSERIF\">");
        //        DateTime duration;
        //        string durationString = string.Empty;
        //        if (DateTime.TryParse(aSermonDetails.ItemArray[kDurationColumn].ToString(), out duration))
        //        {
        //            durationString = " duration: " + duration.ToLongTimeString();
        //        }
        //        string image = @"<IMG src=""speaker.png"" alt=""play"" height=""16"" width=""16"">";
        //        string audio = aSermonDetails.ItemArray[kFileColumn].ToString();
        //        ValidateAudio(audio, aYear);
        //        string networkDriveLocation = Properties.Settings.Default.NetworkDriveLocation; // Where the MP3 resources are located on the internet
        //        string link = string.Format(@"<A href=""{0}/{1}/{2}"" target=""_blank"">{3} {2}</A>",
        //                                     networkDriveLocation, aYear, audio, image);
        //        //link = string.Format(@"<A href=""{1}/{2}"" target=""_blank"">{3} {2}</A>",
        //        //                             networkDriveLocation, aYear, audio, image);
        //        aWriter.WriteLine(link + durationString);
        //        aWriter.WriteLine("</P>");
        //        aWriter.WriteLine("</TD>");

        //        aWriter.WriteLine("</TR>");
        //    }
        //}

        /// <summary>
        /// Write details for a single sermon (single row in the spreadsheet/DataTable)
        /// </summary>
        void WriteSermon(TextWriter aWriter, string aYear, DataRow aSermonDetails)
        {
            // If we fail to parse the date, we assume that the row is empty and write no details at all
            DateTime sermonDate;
            if (DateTime.TryParse(aSermonDetails.ItemArray[kDateColumn].ToString(), out sermonDate))
            {
                aWriter.WriteLine(@"<div class=""well"">");

                aWriter.WriteLine(@"<div class=""row"">");
                string series = aSermonDetails.ItemArray[kSeriesColumn].ToString();
                if (series.Length > 0)
                {
                    aWriter.WriteLine(@"<H3>{0} <SMALL>({1})</SMALL></H3>", aSermonDetails.ItemArray[kTitleColumn].ToString(), series);
                }
                else
                {
                    aWriter.WriteLine(@"<H3>{0}</H3>", aSermonDetails.ItemArray[kTitleColumn].ToString());
                }
                aWriter.WriteLine(@"</div>");

                aWriter.WriteLine(@"  <div class=""row"">");
                aWriter.WriteLine(@"    <div class=""col-sm-3"" style=""background-color:lavender;"">");
                aWriter.WriteLine(@"       <p class=""text-primary"">{0}</p>", sermonDate.ToString("d MMM yyyy"));
                string scripture = aSermonDetails.ItemArray[kScriptureColumn].ToString();
                if (scripture.Length > 0)
                {
                    aWriter.WriteLine(@"       <p>{0}</p>", scripture);
                }
                aWriter.WriteLine(@"       <p>{0}</p>", aSermonDetails.ItemArray[kSpeakerColumn].ToString());
                aWriter.WriteLine(@"    </div>");
                aWriter.WriteLine(@"    <div class=""col-sm-1"">");
                aWriter.WriteLine(@"    </div>");
                aWriter.WriteLine(@"    <div class=""col-sm-8"">");
                aWriter.WriteLine(@"      <p class=""text-info"">");
                aWriter.WriteLine(@"        {0}", aSermonDetails.ItemArray[kDescriptionColumn].ToString());
                aWriter.WriteLine(@"      </p>");
                aWriter.WriteLine(@"      <p>");

                string audio = aSermonDetails.ItemArray[kFileColumn].ToString();
                ValidateAudio(audio, aYear);
                string networkDriveLocation = Properties.Settings.Default.NetworkDriveLocation; // Where the MP3 resources are located on the internet
                aWriter.WriteLine(@"    <a href=""{0}/{1}/{2}"" target=""_blank"" class=""btn btn-info"">", networkDriveLocation, aYear, audio);
                aWriter.WriteLine(@"      <span class=""glyphicon glyphicon-play""></span> Play");
                aWriter.WriteLine(@"    </a>");

                aWriter.WriteLine(@"      </p>");
                DateTime duration;
                string durationString = string.Empty;
                if (DateTime.TryParse(aSermonDetails.ItemArray[kDurationColumn].ToString(), out duration))
                {
                    aWriter.WriteLine(@"      <p class=""text-info"">");
                    aWriter.WriteLine(@"        duration: " + duration.ToLongTimeString());
                    aWriter.WriteLine(@"      </p>");
                }
                aWriter.WriteLine(@"    </div>");
                aWriter.WriteLine(@"</div>");

                aWriter.WriteLine(@"</div>");
            }
        }

        void ValidateAudio( string aAudio, string aYear )
        {
            string fullPath = Path.Combine( Properties.Settings.Default.LocalDriveLocation, aYear, aAudio );
            if( !File.Exists( fullPath ) )
            {
                iTextBoxStatus.Text += ( "File " + aAudio + " not present" + Environment.NewLine );
            }
        }

        void iButtonOK_Click( object sender, EventArgs e )
        {
            Close();
        }

        const int kFileColumn = 0;
        const int kDateColumn = 1;
        const int kAmPmColumn = 2;
        const int kTitleColumn = 3;
        const int kSpeakerColumn = 4;
        const int kScriptureColumn = 5;
        const int kSeriesColumn = 6;
        const int kDurationColumn = 7;
        const int kDescriptionColumn = 8;
    }
}
