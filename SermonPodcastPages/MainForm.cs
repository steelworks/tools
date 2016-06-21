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
            InitializeComponent();

            string driveLocation = Properties.Settings.Default.DriveLocation;
            string spreadsheet = Properties.Settings.Default.SpreadsheetLocation;
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
                string html = Path.Combine( driveLocation, sheet.TableName + ".html" );
                using ( TextWriter htmlWriter = File.CreateText( html ) )
                {
                    htmlWriter.WriteLine( "<!DOCTYPE html>" );
                    htmlWriter.WriteLine( "<HTML>" );

                    htmlWriter.WriteLine( "<HEAD>" );
                    htmlWriter.WriteLine( "<TITLE>Bethel Podcasts {0}</TITLE>", sheet.TableName );
                    htmlWriter.WriteLine( "<STYLE>" );
                    htmlWriter.WriteLine( "P.SERIF{font-family:\"Times New Roman\",Times,serif;}" );
                    htmlWriter.WriteLine( "P.SANSSERIF{font-family:Tahoma,Helvetica,sans-serif;}" );
                    htmlWriter.WriteLine( "TABLE,TH,TD" );
                    htmlWriter.WriteLine( "{" );
                    htmlWriter.WriteLine( "    border:1px solid black;" );
                    htmlWriter.WriteLine( "}" );
                    htmlWriter.WriteLine( "TD" );
                    htmlWriter.WriteLine( "{" );
                    htmlWriter.WriteLine( "padding:15px;" );
                    htmlWriter.WriteLine( "}" );
                    htmlWriter.WriteLine( "</STYLE>" );
                    htmlWriter.WriteLine( "</HEAD>" );

                    htmlWriter.WriteLine( "<BODY>" );
                    string year = sheet.TableName;
                    htmlWriter.WriteLine( "<H1>Bethel Macclesfield sermons {0}</H1>", year );

                    WriteYearIndex( htmlWriter, year, result.Tables );

                    WriteLinkToLatest( htmlWriter, sheet );

                    htmlWriter.WriteLine( "<BR>" );
                    htmlWriter.WriteLine( @"<TABLE style=""width:800px"">" );

                    foreach ( DataRow row in sheet.Rows )
                    {
                        WriteSermon( htmlWriter, sheet.TableName, row );
                    }

                    htmlWriter.WriteLine( "</TABLE>" );
                    htmlWriter.WriteLine( @"<A id=""latest""> </A>" );
                    htmlWriter.WriteLine( "</BODY>" );
                    htmlWriter.WriteLine( "</HTML>" );
                }
            }

            iTextBoxStatus.Text += "Done";
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
            aWriter.WriteLine( @"Show sermons for year:" );
            aWriter.WriteLine( @"<TABLE>" );
            aWriter.WriteLine( @"<TR>" );

            foreach ( DataTable year in aYears )
            {
                if ( year.TableName == aThisYear )
                {
                    aWriter.WriteLine( @"<TD style=""background-color:#FFFF00"">" );
                }
                else
                {
                    aWriter.WriteLine( @"<TD style=""background-color:#00FFFF"">" );
                }

                aWriter.WriteLine( @"<A href=""{0}.html"">{0}</A>", year.TableName );
                aWriter.WriteLine( @"</TD>" );
            }

            aWriter.WriteLine( "</TR>" );
            aWriter.WriteLine( "</TABLE>" );
        }

        /// <summary>
        /// Write details for a single sermon (single row in the spreadsheet/DataTable)
        /// </summary>
        void WriteSermon( TextWriter aWriter, string aYear, DataRow aSermonDetails )
        {
            // If we fail to parse the date, we assume that the row is empty and write no details at all
            DateTime sermonDate;
            if ( DateTime.TryParse( aSermonDetails.ItemArray[kDateColumn].ToString(), out sermonDate ) )
            {
                aWriter.WriteLine( "<TR>" );

                aWriter.WriteLine( @"<TD style=""width:100px"">" );
                aWriter.WriteLine( sermonDate.ToString( "d MMM yyyy" ) );
                aWriter.WriteLine( "</TD>" );

                aWriter.WriteLine( "<TD>" );
                aWriter.WriteLine( "<P class=\"SANSSERIF\">" );
                aWriter.WriteLine( "<B>" + aSermonDetails.ItemArray[kTitleColumn].ToString() + "</B>" );
                string series = aSermonDetails.ItemArray[kSeriesColumn].ToString();
                if ( series.Length > 0 )
                {
                    aWriter.WriteLine( " (" + series + ")" );
                }
                aWriter.WriteLine( "</P>" );
                string scripture = aSermonDetails.ItemArray[kScriptureColumn].ToString();
                if ( scripture.Length > 0 )
                {
                    scripture += ", ";
                }
                aWriter.WriteLine( scripture + aSermonDetails.ItemArray[kSpeakerColumn].ToString() );
                aWriter.WriteLine( "<BR>" );
                aWriter.WriteLine( "<I>" + aSermonDetails.ItemArray[kDescriptionColumn].ToString() + "</I>" );
                aWriter.WriteLine( "<P class=\"SANSSERIF\">" );
                DateTime duration;
                string durationString = string.Empty;
                if ( DateTime.TryParse( aSermonDetails.ItemArray[kDurationColumn].ToString(), out duration ) )
                {
                    durationString = " duration: " + duration.ToLongTimeString();
                }
                string image = @"<IMG src=""speaker.png"" alt=""play"" height=""16"" width=""16"">";
                string audio = aSermonDetails.ItemArray[kFileColumn].ToString();
                ValidateAudio( audio, aYear );
                string link = string.Format( @"<A href=""{0}/{1}"" target=""_blank"">{2} {1}</A>",
                                             aYear, audio, image );
                aWriter.WriteLine( link + durationString );
                aWriter.WriteLine( "</P>" );
                aWriter.WriteLine( "</TD>" );

                aWriter.WriteLine( "</TR>" );
            }
        }

        void ValidateAudio( string aAudio, string aYear )
        {
            string fullPath = Path.Combine( Properties.Settings.Default.DriveLocation, aYear, aAudio );
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
