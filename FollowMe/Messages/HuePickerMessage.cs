namespace FollowMe.Messages
{
    public class HuePickerMessage
    {
        public int HueMin { get; private set; }
        public int HueMax { get; private set; }
        

        public HuePickerMessage(int hueMin, int hueMax)
        {
            HueMin = hueMin;
            HueMax = hueMax;
        }
    }
}
