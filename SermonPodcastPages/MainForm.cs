using Excel;
using System;
using System.Data;
using System.IO;
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
                    htmlWriter.WriteLine( $"<TITLE>Bethel Podcasts {sheet.TableName}</TITLE>");
                    htmlWriter.WriteLine(bootstrapIncludes);

                    htmlWriter.WriteLine( "</HEAD>" );

                    htmlWriter.WriteLine("<BODY>");
                    string year = sheet.TableName;
                    WriteYearIndex(htmlWriter, year, result.Tables);
                    htmlWriter.WriteLine("<div class=\"container\">");
                    htmlWriter.WriteLine("<BR><BR>");
                    htmlWriter.WriteLine( $"<H1>Bethel Macclesfield sermons {year}</H1>" );

                    htmlWriter.WriteLine( "<BR>" );

                    if (sheet.TableName == DateTime.Now.Year.ToString())
                    {
                        RedirectToPodbean(htmlWriter);
                    }
                    else
                    {
                        foreach (DataRow row in sheet.Rows)
                        {
                            WriteSermon(htmlWriter, sheet.TableName, row);
                        }
                    }

                    htmlWriter.WriteLine( "<A id=\"latest\"> </A>" );
                    htmlWriter.WriteLine("</div>");
                    htmlWriter.WriteLine( "</BODY>" );
                    htmlWriter.WriteLine( "</HTML>" );
                }
            }

            iTextBoxStatus.Text += "Done";
        }

        /// <summary>
        /// Avoid excessive traffic to the web host: for the current year, redirect the user to Podbean
        /// </summary>
        /// <param name="htmlWriter"></param>
        static void RedirectToPodbean(TextWriter htmlWriter)
        {
            htmlWriter.WriteLine("<div class=\"well\">");
            htmlWriter.WriteLine("<H3>For the latest sermons, please visit our Podbean site</H3>");
            htmlWriter.WriteLine("    <a href=\"https://bethelmacc.podbean.com/\" target=\"_blank\" class=\"btn btn-info\">");
            htmlWriter.WriteLine("      <span class=\"glyphicon glyphicon-share-alt\"></span> Bethel Macclesfield on Podbean");
            htmlWriter.WriteLine("    </a>");
            htmlWriter.WriteLine("</div>");
        }

        /// <summary>
        /// If we are writing this year's sheet, then show a link to the latest sermon at the bottom
        /// of the page
        /// </summary>
        static void WriteLinkToLatest( TextWriter htmlWriter, DataTable sheet )
        {
            if ( sheet.TableName == DateTime.Now.Year.ToString() )
            {
                htmlWriter.WriteLine( "<BR>" );
                htmlWriter.WriteLine( "<DIV style=\"background-color:#FFA500;clear:both;text-align:center;width:800px;\">" +
                                      "<A href=\"#latest\">Go to latest sermon</A></DIV>" );
            }
        }

        void WriteYearIndex( TextWriter htmlWriter, string thisYear, DataTableCollection years  )
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
      <a class=""navbar-brand"" href=""https://www.bethelmacclesfield.org.uk"">Bethel Macclesfield</a>
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

            htmlWriter.WriteLine(navbarBegin);

            foreach ( DataTable year in years )
            {
                if ( year.TableName == thisYear )
                {
                    htmlWriter.WriteLine($"<li class=\"active\"><a href=\"{year.TableName}.html\">{year.TableName}</a></li>");
                }
                else
                {
                    htmlWriter.WriteLine($"<li><a href=\"{year.TableName}.html\">{year.TableName}</a></li>" );
                }
            }

            var latestYear = DateTime.Now.Year.ToString();
            htmlWriter.WriteLine(navbarEnd, latestYear);
        }

        /// <summary>
        /// Write details for a single sermon (single row in the spreadsheet/DataTable)
        /// </summary>
        void WriteSermon(TextWriter htmlWriter, string year, DataRow sermonDetails)
        {
            // If we fail to parse the date, we assume that the row is empty and write no details at all
            DateTime sermonDate;
            if (!DateTime.TryParse(sermonDetails.ItemArray[kDateColumn].ToString(), out sermonDate))
            {
                return;
            }

            // Skip the entry if the audio is not present, eg, no recording, or if we recorded but agreed not to publish the message
            string audio = sermonDetails.ItemArray[kFileColumn].ToString();
            if (!ValidateAudio(audio, year))
            {
                return;
            }

            htmlWriter.WriteLine("<div class=\"well\">");

            htmlWriter.WriteLine("<div class=\"row\">");
            string series = sermonDetails.ItemArray[kSeriesColumn].ToString();
            if (series.Length > 0)
            {
                htmlWriter.WriteLine($"<H3>{sermonDetails.ItemArray[kTitleColumn].ToString()} <SMALL>({series})</SMALL></H3>" );
            }
            else
            {
                htmlWriter.WriteLine($"<H3>{sermonDetails.ItemArray[kTitleColumn].ToString()}</H3>");
            }
            htmlWriter.WriteLine("</div>");

            htmlWriter.WriteLine("  <div class=\"row\">");
            htmlWriter.WriteLine("    <div class=\"col-sm-3\" style=\"background-color:lavender;\">");
            htmlWriter.WriteLine($"       <p class=\"text-primary\">{sermonDate.ToString("d MMM yyyy")}</p>" );
            string scripture = sermonDetails.ItemArray[kScriptureColumn].ToString();
            if (scripture.Length > 0)
            {
                htmlWriter.WriteLine($"       <p>{scripture}</p>");
            }
            htmlWriter.WriteLine($"       <p>{sermonDetails.ItemArray[kSpeakerColumn].ToString()}</p>");
            htmlWriter.WriteLine("    </div>");
            htmlWriter.WriteLine("    <div class=\"col-sm-1\">");
            htmlWriter.WriteLine("    </div>");
            htmlWriter.WriteLine("    <div class=\"col-sm-8\">");
            htmlWriter.WriteLine("      <p class=\"text-info\">");
            htmlWriter.WriteLine($"        {sermonDetails.ItemArray[kDescriptionColumn].ToString()}");
            htmlWriter.WriteLine("      </p>");
            htmlWriter.WriteLine("      <p>");

            htmlWriter.WriteLine($"    <a href=\"{year}/{audio}\" target=\"_blank\" class=\"btn btn-info\">");
            htmlWriter.WriteLine("      <span class=\"glyphicon glyphicon-play\"></span> Play");
            htmlWriter.WriteLine("    </a>");

            htmlWriter.WriteLine("      </p>");
            DateTime duration;
            string durationString = string.Empty;
            if (DateTime.TryParse(sermonDetails.ItemArray[kDurationColumn].ToString(), out duration))
            {
                htmlWriter.WriteLine("      <p class=\"text-info\">");
                htmlWriter.WriteLine($"        duration: {duration.ToLongTimeString()}");
                htmlWriter.WriteLine("      </p>");
            }
            htmlWriter.WriteLine("    </div>");
            htmlWriter.WriteLine("</div>");

            htmlWriter.WriteLine("</div>");
        }

        bool ValidateAudio( string audio, string year )
        {
            string fullPath = Path.Combine( Properties.Settings.Default.LocalDriveLocation, year, audio );
            var valid = File.Exists(fullPath);
            if ( !valid )
            {
                iTextBoxStatus.Text += ( $"File {audio} not present" + Environment.NewLine );
            }

            return valid;
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
