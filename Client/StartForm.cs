using SimpleSockets.Client;
using System;
using System.Windows.Forms;

namespace ExtChat.Client
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string ip = ipBox.Text;
            int port = (int)portBox.Value;

            SimpleSocketClient client = new SimpleSocketTcpClient();
            client.StartClient(ip, port);
            client.ConnectedToServer += Client_ConnectedToServer;
        }

        private void Client_ConnectedToServer(SimpleSocketClient client)
        {
            client.SendMessageAsync("JOIN|" + nameBox.Text);
            Form chatform = new ChatForm(client);
            chatform.ShowDialog();
        }
    }
}
