using EZ_B;

namespace FollowMe.EzRobot
{
    public class EzbCameraProvider
    {
        private static Camera instance;

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Camera(UcezbConnectProvider.Instance.EZB);
                }
                return instance;
            }
        }
    }
}