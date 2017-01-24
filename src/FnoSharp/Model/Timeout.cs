namespace FnoSharp.Model
{
    public class Timeout
    {
        public static int DefaultTimeout = 100000;

        public Timeout()
        {
            Seconds = DefaultTimeout;
        }

        public int Seconds { get; set; }

        public int Milliseconds
        {
            get { return Seconds == -1 ? -1 : Seconds * 1000; }
        }
    }
}
