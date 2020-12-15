using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleSockets.Client;
using System.Windows.Forms;

namespace ExtChat.Client
{
    public partial class ChatForm : Form
    {
        private SimpleSocketClient _client;
        public ChatForm(SimpleSocketClient client)
        {
            _client = client;
            _client.MessageReceived += _client_MessageReceived;
            InitializeComponent();
        }

        public void AppendText(string text, Color color)
        {
            chatBox.SelectionStart = chatBox.TextLength;
            chatBox.SelectionLength = 0;

            chatBox.SelectionColor = color;
            chatBox.AppendText("\n"+text);
            chatBox.SelectionColor = chatBox.ForeColor;
        }

        private void _client_MessageReceived(SimpleSocketClient client, string msg)
        {
            var tokens = msg.Split('|');
            switch(tokens[0])
            {
                case "JOIN":
                    AppendText(tokens[1] + " joined", Color.Blue);
                    break;
                case "MSG":
                    Color c = Color.Black;
                    if (tokens[2].StartsWith(">"))
                    {
                        c = Color.DarkGreen;
                    }
                    AppendText(tokens[1] + ": " + tokens[2], c);
                    break;
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            _client.SendMessageAsync("MSG|"+inputBox.Text);
        }

        private void inputBox_TextChanged(object sender, EventArgs e)
        {
            if (inputBox.Text.StartsWith(">"))
            {
                inputBox.ForeColor = Color.DarkGreen;
            } else
            {
                inputBox.ForeColor = ForeColor;
            }
        }
    }
}
