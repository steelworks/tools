using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpeechLib;

namespace Speak
{
    public partial class Speak : Form
    {
        public Speak()
        {
            InitializeComponent();
        }

        private void buttonSpeak_Click(object sender, EventArgs e)
        {
            if (speechBox.Text.Length == 0)
            {
                string errorMessage = "Please enter some text";
                MessageBox.Show(errorMessage);
                SpVoice errorVoice = new SpVoiceClass();
                errorVoice.Speak(errorMessage, SpeechVoiceSpeakFlags.SVSFDefault);
            }
            else
            {
                SpVoice realVoice = new SpVoiceClass();
                realVoice.Speak(speechBox.Text, SpeechVoiceSpeakFlags.SVSFDefault);
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonVary_Click(object sender, EventArgs e)
        {
            if (speechBox.Text.Length == 0)
            {
                string errorMessage = "Please enter some text";
                MessageBox.Show(errorMessage);
                SpVoice errorVoice = new SpVoiceClass();
                errorVoice.Speak(errorMessage, SpeechVoiceSpeakFlags.SVSFDefault);
            }
            else
            {
                SpVoice realVoice = new SpVoiceClass();
                for (int i = 0; i < 10; i++)
                {
                    string message =
                        string.Format("{0} at volume {1}", speechBox.Text, realVoice.Volume);
                    realVoice.Speak(message, SpeechVoiceSpeakFlags.SVSFDefault);
                    realVoice.Volume = realVoice.Volume * 9 / 10;
                }
            }
        }
    }
}
