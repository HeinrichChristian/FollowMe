using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FollowMe.Interfaces
{
    public interface IFlyingRobot
    {
        void TakeOff();

        void Land();

        void Emergency();

        
        void Roll(float value);

        void Yaw(float value);

        void Pitch(float value);

        void Nick(float value);
    }
}
