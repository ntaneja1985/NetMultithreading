namespace OffloadingTasks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => ShowMessage("First Message", 3000));
           thread.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => ShowMessage("Second Message", 5000));
            thread.Start();
        }

        private void ShowMessage(string message, int delay)
        {
            Monitor.Enter(lblMessage);
            Thread.Sleep(delay);
            lblMessage.Text = message;
            Monitor.Exit(lblMessage);
        }
    }
}
