using System;
using System.Windows.Forms;

namespace SkyRunner
{
    public class MessageHandler : Control
    {
        public event EventHandler<Message> MessageReceived;

        protected override void WndProc(ref Message m) {

            Console.WriteLine($"WndProc msg: 0x{m.Msg:X4}");

            MessageReceived?.Invoke(this, m);
            base.WndProc(ref m);
        }
    }
}
