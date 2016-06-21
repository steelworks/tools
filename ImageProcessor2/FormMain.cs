using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessor2
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            ExtractMetaData();
        }

        private void ExtractMetaData()
        {
            try
            {
                // Create an Image object.
                string imageFilename = "C:\\Users\\Public\\Pictures\\photoalbum\\2013\\Holidays\\Cornwall\\Autumn\\Rock\\2013-11-10_14-24-58.JPG";
                Image theImage = new Bitmap( imageFilename );

                // Get the PropertyItems property from image.
                PropertyItem[] propItems = theImage.PropertyItems;

                // For each PropertyItem in the array, display the id,  
                // type, and length. 
                int count = 0;
                foreach ( PropertyItem propItem in propItems )
                {
                    var descriptor = iTagNames.FirstOrDefault( x => x.Value == propItem.Id ).Key ?? string.Empty;
                    iImageText.Text += ( "Property Item " + count.ToString() + Environment.NewLine );
                    iImageText.Text += ("   ID: 0x" + propItem.Id.ToString( "x" ) + " " + descriptor );
                    iImageText.Text += ("   TYPE: " + propItem.Type.ToString() + " (" + DecodeType(propItem.Type) + ")" );
                    iImageText.Text += ("   LENGTH: " + propItem.Len.ToString() + Environment.NewLine);

                    if (propItem.Type == 2)
                    {
                        int dataLength = propItem.Value.Length;
                        string data = Encoding.ASCII.GetString( propItem.Value, 0, dataLength - 1 );
                        iImageText.Text += ( "   VALUE: \"" + data + "\"" + Environment.NewLine );
                    }

                    iImageText.Text += Environment.NewLine;
                    count += 1;
                }

                // Need additional character in title - will replace with zero terminator
                string newTitle = "This is a replacement title for the imageX";
                var encodedTitle = System.Text.Encoding.UTF8.GetBytes( newTitle );
                encodedTitle[encodedTitle.Length - 1] = 0;
                PropertyItem title = GetPropertyItem( propItems, "ImageDescription" );
                if ( title != null )
                {
                    title.Id = iTagNames["ImageDescription"];
                    title.Type = 2;             // ascii string
                    title.Len = encodedTitle.Length;
                    title.Value = encodedTitle;
                    theImage.SetPropertyItem( title );
                }

                string newComment = "This is a replacement comment for the imageX";
                var encodedComment = System.Text.Encoding.UTF8.GetBytes( newComment );
                encodedComment[encodedComment.Length - 1] = 0;
                PropertyItem userComment = GetPropertyItem( propItems, "ExifUserComment" );
                if ( userComment != null )
                {
                    userComment.Id = iTagNames["ExifUserComment"];
                    userComment.Type = 2;             // ascii string
                    userComment.Len = encodedComment.Length;
                    userComment.Value = encodedComment;
                    theImage.SetPropertyItem( userComment );
                }

                string newFilename = imageFilename.Replace( "2013-", "NEW-" );
                theImage.Save( newFilename, System.Drawing.Imaging.ImageFormat.Jpeg );

                iImageText.SelectionLength = 0;
            }
            catch ( Exception ex )
            {
                MessageBox.Show( "There was an error. Make sure the path to the image file is valid.", ex.Message );
            }
        }

        PropertyItem GetPropertyItem( PropertyItem[] aExistingItems, string aName )
        {
            int id;
            if ( iTagNames.TryGetValue( aName, out id ) )
            {
                var existingItem = aExistingItems.FirstOrDefault( x => x.Id == id );
                if ( existingItem == null )
                {
                    // Property does not exist
                    return aExistingItems[0];
                }
                else
                {
                    // Found existing property
                    return existingItem;
                }
            }
            else
            {
                // Property name not recognised
                return null;
            }
        }

        string DecodeType(int aType)
        {
            switch (aType)
            {
                case 1:
                    return "byte[]";
                case 2:
                    return "ascii string";
                case 3:
                    return "unsigned short[]";
                case 4:
                    return "unsigned long[]";
                case 5:
                    return "unsigned long pair [] (enumerator/denominator)";
                case 6:
                    return "byte[] (generic types)";
                case 7:
                    return "signed long[]";
                case 10:
                    return "signed long pair [] (enumerator/denominator)";
                default:
                    return "undefined";
            }
        }

        TagId iTagNames = new TagId();
    }
}
