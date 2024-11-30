namespace AsyncAwaitSynchronizationContext
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await ShowMessage("First Message", 3000);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await ShowMessage("Second Message", 4000);
        }

        private async Task ShowMessage(string message, int delay)
        {
            await Task.Delay(delay);
            lblMessage.Text = message;

        }
    }
}
